using System.ComponentModel.DataAnnotations;

namespace IncidentAPI.Models
{
    public class Incident
    {
        
      public int Id { get; set; }

       [Required]
       [StringLength(30,MinimumLength =5,ErrorMessage ="Titre Invalide.")]
       public string Title { get; set; } = null!;

        [Required]
        [StringLength(200,ErrorMessage="Description Invalide.")]
        public string Description { get; set; } = string.Empty;
        [Required]
        public string Severity { get; set; } = string.Empty;
   
        
        public string Status { get; set; } = string.Empty;
        
     
        public DateTime CreatedAt { get; set; } = System.DateTime.UtcNow;   
       
     
    }   
}

