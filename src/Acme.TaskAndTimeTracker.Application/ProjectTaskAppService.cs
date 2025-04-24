using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Acme.TaskAndTimeTracker.DTOs;
using Acme.TaskAndTimeTracker.Permissions;
using Acme.TaskAndTimeTracker.Projects;
using Acme.TaskAndTimeTracker.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Volo.Abp.Identity;

namespace Acme.TaskAndTimeTracker;

[Authorize]
public class ProjectTaskAppService : ApplicationService, IProjectTaskAppService
{
    private readonly IRepository<Project, Guid> _projectRepository;
    private readonly IRepository<ProjectTask, Guid> _taskRepository;
    private readonly IGuidGenerator _guidGenerator;
    private readonly IRepository<IdentityUser, Guid> _abpUsers;

    public ProjectTaskAppService(IRepository<ProjectTask, Guid> taskRepository, IGuidGenerator guidGenerator, IRepository<IdentityUser, Guid> abpUsers, IRepository<Project, Guid> projectRepository)
    {
        _taskRepository = taskRepository;
        _guidGenerator = guidGenerator;
        _abpUsers = abpUsers;
        _projectRepository = projectRepository;
    }

    [Authorize(TaskAndTimeTrackerPermissions.ProjectTasks.Create)]
    [HttpPost("api/tasks")]
    public async Task<ProjectTaskDto> CreateAsync(CreateUpdateProjectTaskDto input)
    {
        try
        {
            var task = new ProjectTask(
                     GuidGenerator.Create(),
                     input.Title,
                     input.Description,
                     input.DueDate,
                     input.Status,
                     input.Priority,
                     input.ProjectId,
                     input.AssignedUserId
            );

            await _taskRepository.InsertAsync(task);

            return new ProjectTaskDto
            {
                Title = task.Title,
                Description = task.Description,
                DueDate = task.DueDate,
                Status = task.Status,
                Priority = task.Priority,
                ProjectId = task.ProjectId,
                AssignedUserId = task.AssignedUserId
            };
        }
        catch (UserFriendlyException)
        {
            throw;
        }
        catch (Exception ex)
        {
            Logger.LogException(ex);
            throw new UserFriendlyException("An error occurred while creating the task.");
        }
    }

    [Authorize(TaskAndTimeTrackerPermissions.ProjectTasks.Delete)]
    [HttpDelete("api/tasks/{id}")]
    public async Task DeleteAsync(Guid id)
    {
        try
        {
            var task = await _taskRepository.GetAsync(id);
            if (task == null)
            {
                throw new UserFriendlyException("Task not found.");
            }
            await _taskRepository.DeleteAsync(task);
        }
        catch (UserFriendlyException)
        {
            throw;
        }
        catch (Exception ex)
        {
            Logger.LogException(ex);
            throw new UserFriendlyException("An error occurred while deleting the task.");
        }
    }

    [Authorize(TaskAndTimeTrackerPermissions.ProjectTasks.Update)]
    [HttpPut("api/tasks/{id}")]
    public async Task<ProjectTaskDto> UpdateAsync(Guid id, CreateUpdateProjectTaskDto input)
    {
        try
        {
            var task = await _taskRepository.GetAsync(id);
            if (task == null)
            {
                throw new UserFriendlyException("Task not found.");
            }
            task.Title = input.Title;
            task.Description = input.Description;
            task.DueDate = input.DueDate;
            task.Status = input.Status;
            task.Priority = input.Priority;
            task.ProjectId = input.ProjectId;
            task.AssignedUserId = input.AssignedUserId;
            await _taskRepository.UpdateAsync(task);

            return new ProjectTaskDto
            {
                Title = task.Title,
                Description = task.Description,
                DueDate = task.DueDate,
                Status = task.Status,
                Priority = task.Priority,
                ProjectId = task.ProjectId,
                AssignedUserId = task.AssignedUserId
            };
        }
        catch (UserFriendlyException)
        {
            throw;
        }
        catch (Exception ex)
        {
            Logger.LogException(ex);
            throw new UserFriendlyException("An error occurred while updating the task.");
        }
    }

    [HttpGet("api/tasks/{id}")]
    public async Task<ProjectTaskDto> GetAsync(Guid id)
    {
        try
        {
            var task = await _taskRepository.GetAsync(id);
            if (task == null)
            {
                throw new UserFriendlyException("Task not found.");
            }

            return new ProjectTaskDto
            {
                Title = task.Title,
                Description = task.Description,
                DueDate = task.DueDate,
                Status = task.Status,
                Priority = task.Priority,
                ProjectId = task.ProjectId,
                AssignedUserId = task.AssignedUserId
            };
        }
        catch (UserFriendlyException)
        {
            throw;
        }
        catch (Exception ex)
        {
            Logger.LogException(ex);
            throw new UserFriendlyException("An error occurred while retrieving the task.");
        }
    }

    [HttpGet("api/tasks")]
    public async Task<PagedResultDto<ProjectTaskDto>> GetListAsync(ProjectTaskFilterDto input)
    {
        if (string.IsNullOrWhiteSpace(input.Sorting))
            input.Sorting = "Title";

        var tasks = await _taskRepository.GetQueryableAsync();
        var users = await _abpUsers.GetQueryableAsync();
        var projects = await _projectRepository.GetQueryableAsync();

        var query = (from t in tasks
                     join u in users on t.AssignedUserId equals u.Id into userGroup
                     from user in userGroup.DefaultIfEmpty()
                     join p in projects on t.ProjectId equals p.Id into projectGroup
                     from proj in projectGroup.DefaultIfEmpty()
                     select new ProjectTaskDto
                     {
                         Title = t.Title,
                         Description = t.Description,
                         Status = t.Status,
                         Priority = t.Priority,
                         DueDate = t.DueDate,
                         ProjectId = t.ProjectId,
                         ProjectName = proj != null ? proj.Name : "",
                         AssignedUserId = t.AssignedUserId,
                         AssignedUserName = user != null ? user.UserName : "",
                         CreationTime = t.CreationTime
                     })
                    .WhereIf(!input.Filter.IsNullOrWhiteSpace(),
                        t => t.Title.Contains(input.Filter) ||
                             t.Description.Contains(input.Filter) ||
                             t.ProjectName.Contains(input.Filter) ||
                             t.AssignedUserName.Contains(input.Filter));

        var totalCount = await AsyncExecuter.CountAsync(query);

        var items = await AsyncExecuter.ToListAsync(
            query
            .Skip(input.SkipCount)
            .Take(input.MaxResultCount)
        );

        return new PagedResultDto<ProjectTaskDto>(totalCount, items);
    }
}