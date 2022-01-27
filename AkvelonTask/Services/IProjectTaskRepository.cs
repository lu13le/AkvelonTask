using AkvelonTask.Models;
using System.Collections.Generic;

namespace AkvelonTask.Services
{
    public interface IProjectTaskRepository
    {
        //methods to be imlepented
        ICollection<ProjectTask> GetTasks();
        ProjectTask GetTask(int taskId);
        public ICollection<ProjectTask> GetTasksSortedByName();
        bool TaskExists(int taskId);

        //Crud methods
        bool CreateTask(ProjectTask task);
        bool UpdateTask(ProjectTask task);
        bool DeleteTask(ProjectTask task);
        bool Save();
    }
}
