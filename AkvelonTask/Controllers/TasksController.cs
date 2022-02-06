using AkvelonTask.Models;
using AkvelonTask.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace AkvelonTask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : Controller
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IProjectTaskRepository _taskRepositrory;

        //Injecting needed repos
        public TasksController(IProjectRepository projectRepository, IProjectTaskRepository projectTaskRepository)
        {
            _projectRepository = projectRepository;
            _taskRepositrory = projectTaskRepository;
        }

        //returns list of all tasks
        //api/tasks
        [HttpGet]
        [ProducesResponseType(400)]
        //expected type of response
        [ProducesResponseType(200, Type = typeof(IEnumerable<ProjectTask>))]
        public IActionResult GetTasks()
        {
            //Getting list of all tasks
            var tasks = _taskRepositrory.GetTasks().ToList();

            //Returning badRequest if model state is not valid
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var tasksDto = new List<ProjectTask>();
            foreach (var task in tasks)
            {
                tasksDto.Add(new ProjectTask
                {
                    Id = task.Id,
                    TaskName=task.TaskName,
                    TaskDescription=task.TaskDescription,
                    TaskStatus=task.TaskStatus,
                    TaskPriority=task.TaskPriority

                });
            }


            return Ok(tasksDto);
        }

        //returns list of all tasks sorted by name
        //api/tasks
        [HttpGet("SortedTasksByName",Name ="GetTasks")]
        [ProducesResponseType(400)]
        //expected type of response
        [ProducesResponseType(200, Type = typeof(IEnumerable<ProjectTask>))]
        public IActionResult GetTasksSortedByName()
        {
            //Getting list of all tasks sorted by name
            var tasks = _taskRepositrory.GetTasksSortedByName().ToList();

            //Returning badRequest if model state is not valid
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var tasksDto = new List<ProjectTask>();
            foreach (var task in tasks)
            {
                tasksDto.Add(new ProjectTask
                {
                    Id = task.Id,
                    TaskName = task.TaskName,
                    TaskDescription = task.TaskDescription,
                    TaskStatus = task.TaskStatus,
                    TaskPriority = task.TaskPriority

                });
            }


            return Ok(tasksDto);
        }

        //returns one task by id
        //api/tasks/taskId
        [HttpGet("{taskId}", Name = "GetTask")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        //expected type
        [ProducesResponseType(200, Type = typeof(ProjectTask))]
        public IActionResult GetTask(int taskId)
        {
            //Checking if task exists
            if (!_taskRepositrory.TaskExists(taskId))
                return NotFound();

            //Getting task by taskId
            var task = _taskRepositrory.GetTask(taskId);

            //Returning badRequest if model state is not valid
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var taskDto = new ProjectTask()
            {

                Id = task.Id,
                TaskName = task.TaskName,
                TaskDescription = task.TaskDescription,
                TaskStatus = task.TaskStatus,
                TaskPriority = task.TaskPriority


            };


            return Ok(taskDto);
        }




        //creating task
        //api/tasks
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(ProjectTask))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult CreateTask([FromBody] ProjectTask taskToCreate)
        {
            if (taskToCreate == null)
                return BadRequest(ModelState);

            //checking if assigned project to  task exists
            if (!_projectRepository.ProjectExists(taskToCreate.Project.Id))
            {
                ModelState.AddModelError("", "Project doesn't exist!");
                return StatusCode(404, ModelState);
            }

            //Returning badRequest if model state is not valid
            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            taskToCreate.Project = _projectRepository.GetProject(taskToCreate.Project.Id);


            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            //creating task
            if (!_taskRepositrory.CreateTask(taskToCreate))
            {
                ModelState.AddModelError("", $"Something went wrong saving the task {taskToCreate.TaskName}");
                return StatusCode(500, ModelState);
            }

            //redirecting to an action under ""
            return CreatedAtRoute("GetTask", new { taskId = taskToCreate.Id }, taskToCreate);
        }

        //Updating task
        //api/tasks/taskId
        [HttpPut("{taskId}")]
        [ProducesResponseType(204)] //no content
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        public IActionResult UpdateProject(int taskId, [FromBody] ProjectTask updateTaskInfo)
        {
            //Returning badRequest if model state is not valid
            if (updateTaskInfo == null)
                return BadRequest(ModelState);

            //Returning badRequest if model state is not valid
            if (taskId != updateTaskInfo.Id)
                return BadRequest(ModelState);

            //Checking if task exists
            if (!_taskRepositrory.TaskExists(taskId))
                return NotFound();

            //Returning badRequest if model state is not valid
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_taskRepositrory.UpdateTask(updateTaskInfo))
            {
                ModelState.AddModelError("", $"Something went wrong updating {updateTaskInfo.TaskName}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        //Deleting task
        //api/tasks/taskId
        [HttpDelete("{taskId}")]
        [ProducesResponseType(204)] //no content
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        [ProducesResponseType(409)]
        public IActionResult DeleteTask(int taskId)
        {
            //Checking if task exists
            if (!_taskRepositrory.TaskExists(taskId))
                return NotFound();

            var taskToDelete = _taskRepositrory.GetTask(taskId);


            //Returning badRequest if model state is not valid
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            //Returning friendly error message 
            if (!_taskRepositrory.DeleteTask(taskToDelete))
            {
                ModelState.AddModelError("", $"Something went wrong deleting {taskToDelete.TaskName}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
