using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace AkvelonTask.Models
{
    public class ProjectTask
    {
        
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(100, ErrorMessage = "Task name must be up to 100 characters in lenght")]
        public string TaskName { get; set; }

        [Required]
        [Range(0,2)]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ETaskStatus TaskStatus { get; set; }
        public enum ETaskStatus { NotStarted = 0, Active = 1, Completed = 2 }


        [MaxLength(300, ErrorMessage = "Task description must be up to 300 characters in lenght")]
        public string TaskDescription { get; set; }

        [Required]
        [Range(1,10)]
        public int TaskPriority { get; set; }


        //Navigational property / relationship property
        public virtual Project Project { get; set; }
    }
}
