using AkvelonTask.Models;
using AkvelonTask.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AkvelonTask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : Controller
    {
       
        private readonly IProjectRepository _projectRepository;
        

        //Injecting data manipulating repo
        public ProjectsController(IProjectRepository projectRepository, IProjectTaskRepository projectTaskRepository)
        {
            _projectRepository = projectRepository;
            
        }

        //returns list of all projects
        //api/projects
        [HttpGet]
        [ProducesResponseType(400)]
        //expected type of response
        [ProducesResponseType(200, Type = typeof(IEnumerable<Project>))]
        public  IActionResult GetProjects()
        {
            var projects = _projectRepository.GetProjects().ToList();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var projectDto = new List<Project>();
            foreach (var project in projects)
            {
                projectDto.Add(new Project
                {
                    Id= project.Id,
                    ProjectName=project.ProjectName,
                    StartDate=project.StartDate,
                    CompletionDate=project.CompletionDate,
                    ProjectStatus=project.ProjectStatus,
                    ProjectPriority=project.ProjectPriority

                });
            }


            return Ok(projectDto);
        }


        //returns list of all projects sorted by priority
        //api/projects
        [HttpGet("SortedByPriority", Name = "GetSortedProjectsByPriority")]
        [ProducesResponseType(400)]
        //expected type of response
        [ProducesResponseType(200, Type = typeof(IEnumerable<Project>))]
        public IActionResult GetSortedProjectsByPriority()
        {
            var projects = _projectRepository.GetSortedProjectsByPriority().ToList();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var projectDto = new List<Project>();
            foreach (var project in projects)
            {
                projectDto.Add(new Project
                {
                    Id = project.Id,
                    ProjectName = project.ProjectName,
                    StartDate = project.StartDate,
                    CompletionDate = project.CompletionDate,
                    ProjectStatus = project.ProjectStatus,
                    ProjectPriority = project.ProjectPriority

                });
            }


            return Ok(projectDto);
        }


        //returns one project by id
        //api/projects/projectId
        [HttpGet("{projectId}", Name = "GetProject")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        //expected type
        [ProducesResponseType(200, Type = typeof(Project))]
        public IActionResult GetProject(int projectId)
        {
            if (!_projectRepository.ProjectExists(projectId))
                return NotFound();

            var project = _projectRepository.GetProject(projectId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var projectDto = new Project()
            {

                Id = project.Id,
                ProjectName = project.ProjectName, 
                StartDate = project.StartDate,
                CompletionDate =project.CompletionDate,
                ProjectStatus= project.ProjectStatus,
                ProjectPriority = project.ProjectPriority
           

            };


            return Ok(projectDto);
        }

        //returns list of tasks from a project
        //api/projects/projectId/tasks
        [HttpGet("{projectId}/tasks")]
        //expected response type
        [ProducesResponseType(200, Type = typeof(IEnumerable<ProjectTask>))]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public IActionResult GetTasksFromProject(int projectId)
        {
            if (!_projectRepository.ProjectExists(projectId))
                return NotFound();

            var tasks = _projectRepository.GetTasksFromProject(projectId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var taskDto = new List<ProjectTask>();

            foreach (var task in tasks)
            {
                taskDto.Add(new ProjectTask
                {
                   Id=task.Id,
                   TaskName = task.TaskName,
                   TaskDescription = task.TaskDescription,
                   TaskStatus = task.TaskStatus,
                   TaskPriority = task.TaskPriority
                });
            }

            return Ok(taskDto);

        }


        //creating new project
        //api/projects
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(Project))]
        //expected type
        [ProducesResponseType(400)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        public IActionResult CreateProject([FromBody] Project projectToCreate)
        {
            if (projectToCreate == null)
                return BadRequest(ModelState);

            var project = _projectRepository.GetProjects().Where(p => p.ProjectName.Trim().ToUpper() == projectToCreate.ProjectName.Trim().ToUpper()).FirstOrDefault();

            if (project != null)
            {
                ModelState.AddModelError("", $"Project {projectToCreate.ProjectName} already exists.");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_projectRepository.CreateProject(projectToCreate))
            { 
                ModelState.AddModelError("", $"Something went wrong saving {projectToCreate.ProjectName}");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetProject", new { projectId = projectToCreate.Id }, projectToCreate);
        }

        //Updating project
        //api/projects/projectId
        [HttpPut("{projectId}")]
        [ProducesResponseType(204)] //no content
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        public IActionResult UpdateProject(int projectId, [FromBody] Project updatedProjectInfo)
        {
            if (updatedProjectInfo == null)
                return BadRequest(ModelState);

            if (projectId != updatedProjectInfo.Id)
                return BadRequest(ModelState);

            if (!_projectRepository.ProjectExists(projectId))
                return NotFound();


            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_projectRepository.UpdateProject(updatedProjectInfo))
            {
                ModelState.AddModelError("", $"Something went wrong updating {updatedProjectInfo.ProjectName}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        //Deleting project
        //api/projects/projectId
        [HttpDelete("{projectId}")]
        [ProducesResponseType(204)] //no content
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        [ProducesResponseType(409)]
        public IActionResult DeleteProject(int projectId)
        {

            if (!_projectRepository.ProjectExists(projectId))
                return NotFound();

            var projectToDelete = _projectRepository.GetProject(projectId);

            if (_projectRepository.GetTasksFromProject(projectId).Count() > 0)
            {
                ModelState.AddModelError("", $"Project {projectToDelete.ProjectName} cannot be deleted " +
                    "because it has at least one task. ");
                return StatusCode(409, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_projectRepository.DeleteProject(projectToDelete))
            {
                ModelState.AddModelError("", $"Something went wrong deleting {projectToDelete.ProjectName}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

    }
}
