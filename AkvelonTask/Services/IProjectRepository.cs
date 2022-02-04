using AkvelonTask.Models;
using System.Collections.Generic;

namespace AkvelonTask.Services
{
    public interface IProjectRepository
    {
        //methods to be imlepented
        ICollection<Project> GetProjects();
        Project GetProject(int projectId);
        ICollection<ProjectTask> GetTasksFromProject(int projectId);
        ICollection<ProjectTask> GetTasksFromProjectSortedByPriority(int projectId);
        ICollection<Project> GetSortedProjectsByPriority();
        bool ProjectExists(int projectId);



        //Crud methods
        bool CreateProject(Project project);
        bool UpdateProject(Project project);
        bool DeleteProject(Project project);
        bool Save();
    }
}
