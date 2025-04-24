using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Acme.TaskAndTimeTracker.DTOs;
using Acme.TaskAndTimeTracker.Permissions;
using Acme.TaskAndTimeTracker.Projects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Volo.Abp.Identity;
using Volo.Abp.Users;

namespace Acme.TaskAndTimeTracker;

[Authorize]
public class ProjectAppService : ApplicationService, IProjectAppService
{
    private readonly IRepository<Project, Guid> _projectRepository;
    private readonly IGuidGenerator _guidGenerator;
    private readonly IRepository<IdentityUser, Guid> _abpUsers;

    public ProjectAppService(IRepository<Project, Guid> projectRepository, IGuidGenerator guidGenerator, IRepository<IdentityUser, Guid> abpUsers)
    {
        _projectRepository = projectRepository;
        _guidGenerator = guidGenerator;
        _abpUsers = abpUsers;
    }

    [Authorize(TaskAndTimeTrackerPermissions.Projects.Create)]
    [HttpPost("api/projects")]
    public async Task<ProjectDto> CreateAsync(CreateUpdateProjectDto input)
    {
        try
        {
            var project = new Project(GuidGenerator.Create(), input.Name, input.Description);
            await _projectRepository.InsertAsync(project);

            return new ProjectDto
            {
                Name = project.Name,
                Description = project.Description
            };
        }
        catch (UserFriendlyException)
        {
            throw;
        }
        catch (Exception ex)
        {
            Logger.LogException(ex);
            throw new UserFriendlyException("An error occurred while creating the project.");
        }
    }

    [Authorize(TaskAndTimeTrackerPermissions.Projects.Delete)]
    [HttpDelete("api/projects/{id}")]
    public async Task DeleteAsync(Guid id)
    {
        try
        {
            var project = await _projectRepository.FirstOrDefaultAsync(p => p.Id == id);

            if (project == null) 
            {
                throw new UserFriendlyException("Project not found.");
            }

            await _projectRepository.DeleteAsync(project, autoSave: true);
        }
        catch (UserFriendlyException)
        {
            throw;
        }
        catch (Exception ex)
        {
            Logger.LogException(ex);
            throw new UserFriendlyException("An error occurred while deleting the project.");
        }
    }

    [HttpGet("api/projects/{id}")]
    public async Task<ProjectDto> GetAsync(Guid id)
    {
        try
        {
            var project = await _projectRepository.GetAsync(id);
            if (project == null)
            {
                throw new UserFriendlyException("Project not found.");
            }

            return new ProjectDto
            {
                Name = project.Name,
                Description = project.Description
            };
        }
        catch (UserFriendlyException)
        {
            throw;
        }
        catch (Exception ex)
        {
            Logger.LogException(ex);
            throw new UserFriendlyException("An error occurred while retrieving the project.");
        }
    }

    [HttpGet("api/projects")]
    public async Task<PagedResultDto<ProjectDto>> GetListAsync(ProjectFilterDto input)
    {
        if (string.IsNullOrWhiteSpace(input.Sorting))
            input.Sorting = "Name";

        var projects = await _projectRepository.GetQueryableAsync();
        var users = await _abpUsers.GetQueryableAsync();

        var query = (from p in projects
                     join u in users on p.CreatorId equals u.Id into creatorGroup
                     from creator in creatorGroup.DefaultIfEmpty()
                     select new ProjectDto
                     {
                         Name = p.Name,
                         Description = p.Description,
                         CreatorName = creator != null ? creator.UserName : "",
                         CreationTime = p.CreationTime
                     })
                    .WhereIf(!input.Filter.IsNullOrWhiteSpace(),
                        p => p.Name.Contains(input.Filter) || p.Description.Contains(input.Filter) || p.CreatorName.Contains(input.Filter));

        var totalCount = await AsyncExecuter.CountAsync(query);

        var items = await AsyncExecuter.ToListAsync(
            query
            .Skip(input.SkipCount)
            .Take(input.MaxResultCount)
        );

        return new PagedResultDto<ProjectDto>(totalCount, items);
    }

    [Authorize(TaskAndTimeTrackerPermissions.Projects.Update)]
    [HttpPut("api/projects/{id}")]
    public async Task<ProjectDto> UpdateAsync(Guid id, CreateUpdateProjectDto input)
    {
        try
        {
            var project = await _projectRepository.GetAsync(id);
            if (project == null)
            {
                throw new UserFriendlyException("Project not found.");
            }
            project.Name = input.Name;
            project.Description = input.Description;
            await _projectRepository.UpdateAsync(project);

            return new ProjectDto
            {
                Description = project.Description,
                Name = project.Name
            };
        }
        catch (UserFriendlyException)
        {
            throw;
        }
        catch (Exception ex)
        {
            Logger.LogException(ex);
            throw new UserFriendlyException("An error occurred while updating the project.");
        }
    }
}