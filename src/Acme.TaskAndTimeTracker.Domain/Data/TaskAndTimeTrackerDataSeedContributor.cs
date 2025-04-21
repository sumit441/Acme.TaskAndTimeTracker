using Acme.TaskAndTimeTracker.Projects;
using Acme.TaskAndTimeTracker.Tasks;
using Acme.TaskAndTimeTracker.TimeEntries;
using Volo.Abp.Data;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.DependencyInjection;
using System;
using System.Threading.Tasks;
using Acme.TaskAndTimeTracker.Enum;
using TaskStatus = Acme.TaskAndTimeTracker.Enum.TaskStatus;
using Volo.Abp.Guids; // Add this namespace for IGuidGenerator

public class TaskAndTimeTrackerDataSeeder : IDataSeedContributor, ITransientDependency
{
    private readonly IRepository<Project, Guid> _projectRepository;
    private readonly IRepository<ProjectTask, Guid> _projectTaskRepository;
    private readonly IRepository<TimeEntry, Guid> _timeEntryRepository;
    private readonly IGuidGenerator _guidGenerator;  // Inject IGuidGenerator

    public TaskAndTimeTrackerDataSeeder(
        IRepository<Project, Guid> projectRepository,
        IRepository<ProjectTask, Guid> projectTaskRepository,
        IRepository<TimeEntry, Guid> timeEntryRepository,
        IGuidGenerator guidGenerator) // Inject IGuidGenerator
    {
        _projectRepository = projectRepository;
        _projectTaskRepository = projectTaskRepository;
        _timeEntryRepository = timeEntryRepository;
        _guidGenerator = guidGenerator;  // Assign it
    }

    public async Task SeedAsync(DataSeedContext context)
    {
        if (await _projectRepository.GetCountAsync() <= 0)
        {
            // Seed Projects
            var project1 = await _projectRepository.InsertAsync(new Project(
                _guidGenerator.Create(), "Project Alpha", "This is a sample project."
            ));

            var project2 = await _projectRepository.InsertAsync(new Project(
                _guidGenerator.Create(), "Project Beta", "This is another sample project."
            ));

            // Seed ProjectTasks
            var task1 = await _projectTaskRepository.InsertAsync(new ProjectTask(
                _guidGenerator.Create(), "Task 1", "First task in Project Alpha", DateTime.Now.AddDays(5),
                TaskStatus.Pending, TaskPriority.High, project1.Id, _guidGenerator.Create() // Use _guidGenerator
            ));

            var task2 = await _projectTaskRepository.InsertAsync(new ProjectTask(
                _guidGenerator.Create(), "Task 2", "Second task in Project Alpha", DateTime.Now.AddDays(10),
                TaskStatus.InProgress, TaskPriority.Medium, project1.Id, _guidGenerator.Create() // Use _guidGenerator
            ));

            var task3 = await _projectTaskRepository.InsertAsync(new ProjectTask(
                _guidGenerator.Create(), "Task 1", "First task in Project Beta", DateTime.Now.AddDays(7),
                TaskStatus.Pending, TaskPriority.Low, project2.Id, _guidGenerator.Create() // Use _guidGenerator
            ));

            // Seed TimeEntries
            await _timeEntryRepository.InsertAsync(new TimeEntry(
                _guidGenerator.Create(), task1.Id, _guidGenerator.Create(), DateTime.Now, 2.5M, "Logged hours for Task 1"
            ));

            await _timeEntryRepository.InsertAsync(new TimeEntry(
                _guidGenerator.Create(), task2.Id, _guidGenerator.Create(), DateTime.Now, 3.0M, "Logged hours for Task 2"
            ));
        }
    }
}
