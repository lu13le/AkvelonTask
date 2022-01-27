using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace AkvelonTask.Models
{
    public class Project
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(100, ErrorMessage = "Project name must be up to 100 characters in lenght")]
        public string ProjectName { get; set; }

        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime CompletionDate { get; set; }

        [Required]
        [Range(0, 2)]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public  EProjectStatus ProjectStatus  { get; set; }
        public enum EProjectStatus { NotStarted = 0, Active = 1, Completed = 2 }

        [Required]
        [Range(1,10)]
        public int ProjectPriority { get; set; }

        //Navigational property / relationship property
        public virtual ICollection<ProjectTask> Tasks { get; set; }
    }
}
