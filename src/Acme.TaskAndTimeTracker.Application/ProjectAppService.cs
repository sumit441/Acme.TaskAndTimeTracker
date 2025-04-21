using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Acme.TaskAndTimeTracker.DTOs;
using Acme.TaskAndTimeTracker.Permissions;
using Acme.TaskAndTimeTracker.Projects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;

namespace Acme.TaskAndTimeTracker;

[Authorize]
public class ProjectAppService : ApplicationService
{
    private readonly IRepository<Project, Guid> _projectRepository;
    private readonly IGuidGenerator _guidGenerator;

    public ProjectAppService(IRepository<Project, Guid> projectRepository, IGuidGenerator guidGenerator)
    {
        _projectRepository = projectRepository;
        _guidGenerator = guidGenerator;
    }

    [Authorize(TaskAndTimeTrackerPermissions.Projects.Create)]
    [HttpPost("api/projects")]
    public async Task<ProjectDto> CreateAsync(CreateProjectDto input)
    {
        try
        {
            var project = new Project(GuidGenerator.Create(), input.Name, input.Description);
            await _projectRepository.InsertAsync(project);

            return new ProjectDto
            {
                Id = project.Id,
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
            var project = await _projectRepository.GetAsync(id);
            if (project == null)
            {
                throw new UserFriendlyException("Project not found.");
            }
            await _projectRepository.DeleteAsync(project);
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
                Id = project.Id,
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
    public async Task<List<ProjectDto>> GetListAsync()
    {
        try
        {
            var projects = await _projectRepository.GetListAsync();

            var projectDtos = new List<ProjectDto>();
            foreach (var project in projects)
            {
                projectDtos.Add(new ProjectDto
                {
                    Id = project.Id,
                    Name = project.Name,
                    Description = project.Description
                });
            }

            return projectDtos;
        }
        catch (UserFriendlyException)
        {
            throw;
        }
        catch (Exception ex)
        {
            Logger.LogException(ex);
            throw new UserFriendlyException("An error occurred while retrieving the project list.");
        }
    }

    [Authorize(TaskAndTimeTrackerPermissions.Projects.Update)]
    [HttpPut("api/projects/{id}")]
    public async Task UpdateAsync(Guid id, CreateProjectDto input)
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