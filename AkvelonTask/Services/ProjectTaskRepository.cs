using AkvelonTask.Data;
using AkvelonTask.Models;
using System.Collections.Generic;
using System.Linq;

namespace AkvelonTask.Services
{
    public class ProjectTaskRepository : IProjectTaskRepository
    {
        private readonly DataContext _projectTaskDataContext;
        public ProjectTaskRepository(DataContext projectTaskDataContext)
        {
            _projectTaskDataContext=projectTaskDataContext;
        }


        public bool CreateTask(ProjectTask task)
        {
            _projectTaskDataContext.Tasks.Add(task);
            return Save();
        }

        public bool DeleteTask(ProjectTask task)
        {
            _projectTaskDataContext.Tasks.Remove(task);
            return Save();
        }

        public ProjectTask GetTask(int taskId)
        {
            return _projectTaskDataContext.Tasks.Where(p => p.Id == taskId).FirstOrDefault();
        }

        public ICollection<ProjectTask> GetTasks()
        {
            return _projectTaskDataContext.Tasks.OrderBy(c => c.TaskPriority).ToList();
        }

        public ICollection<ProjectTask> GetTasksSortedByName()
        {
            return _projectTaskDataContext.Tasks.OrderBy(c => c.TaskName).ToList();
        }

        public bool Save()
        {
            var saved = _projectTaskDataContext.SaveChanges();
            return saved >= 0 ? true : false;
        }

        public bool TaskExists(int taskId)
        {
            return _projectTaskDataContext.Tasks.Any(p => p.Id == taskId);
        }

        public bool UpdateTask(ProjectTask task)
        {
            _projectTaskDataContext.Update(task);
            return Save();
        }
    }
}
