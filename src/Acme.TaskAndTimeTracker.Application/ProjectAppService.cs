using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Acme.TaskTracker.Projects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Acme.TaskTracker;

[Authorize]
public class ProjectAppService : ApplicationService
{
    private readonly IRepository<Project, Guid> _projectRepository;

    public ProjectAppService(IRepository<Project, Guid> projectRepository)
    {
        _projectRepository = projectRepository;
    }

    [HttpPost("api/projects")]
    public async Task<ProjectDto> CreateAsync(CreateUpdateProjectDto input)
    {
        try
        {
            var project = new Project(GuidGenerator.Create(), input.Name, input.Description);
            await _projectRepository.InsertAsync(project);
            return ObjectMapper.Map<Project, ProjectDto>(project);
        }
        catch (Exception ex)
        {
            Logger.LogException(ex);
            throw new UserFriendlyException("An error occurred while creating the project.");
        }
    }

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
            return ObjectMapper.Map<Project, ProjectDto>(project);
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
            return ObjectMapper.Map<List<Project>, List<ProjectDto>>(projects);
        }
        catch (Exception ex)
        {
            Logger.LogException(ex);
            throw new UserFriendlyException("An error occurred while retrieving the project list.");
        }
    }

    [HttpPut("api/projects/{id}")]
    public async Task UpdateAsync(Guid id, CreateUpdateProjectDto input)
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
        catch (Exception ex)
        {
            Logger.LogException(ex);
            throw new UserFriendlyException("An error occurred while updating the project.");
        }
    }
}