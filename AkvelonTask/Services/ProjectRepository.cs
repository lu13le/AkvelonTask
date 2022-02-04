using AkvelonTask.Data;
using AkvelonTask.Models;
using System.Collections.Generic;
using System.Linq;

namespace AkvelonTask.Services
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly DataContext _projectDataContext;

        public ProjectRepository(DataContext projectDataContext)
        {
            _projectDataContext = projectDataContext;
        }

        public bool CreateProject(Project project)
        {
            _projectDataContext.Projects.Add(project);
            return Save();
        }

        public bool DeleteProject(Project project)
        {
            _projectDataContext.Projects.Remove(project);
            return Save();
        }

        public Project GetProject(int projectId)
        {
            return _projectDataContext.Projects.Where(p=> p.Id == projectId).FirstOrDefault();
        }

        public ICollection<Project> GetProjects()
        {
            return _projectDataContext.Projects.OrderBy(c => c.ProjectName).ToList();
        }

        public ICollection<Project> GetSortedProjectsByPriority()
        {
            return _projectDataContext.Projects.OrderByDescending(p=>p.ProjectPriority).ToList();
        }

        public ICollection<ProjectTask> GetTasksFromProject(int projectId)
        {
            return _projectDataContext.Tasks.Where(t => t.Project.Id == projectId).ToList();
        }

        public ICollection<ProjectTask> GetTasksFromProjectSortedByPriority(int projectId)
        {
            return _projectDataContext.Tasks.OrderByDescending(p => p.TaskPriority).Where(t=>t.Project.Id==projectId).ToList();
        }

        public bool ProjectExists(int projectId)
        {
            return _projectDataContext.Projects.Any(p => p.Id == projectId);
        }

        public bool Save()
        {
            var saved = _projectDataContext.SaveChanges();
            return saved >= 0 ? true : false;
        }

        public bool UpdateProject(Project project)
        {
            _projectDataContext.Update(project);
            return Save();
        }
    }
}
