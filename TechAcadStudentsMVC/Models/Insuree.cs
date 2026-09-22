using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechAcadStudentsMVC.Models
{
    public class Insuree
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [Display(Name = "Email Address")]
        public string EmailAddress { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Date of Birth")]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [Display(Name = "Car Year")]
        public int CarYear { get; set; }

        [Required]
        [Display(Name = "Car Make")]
        public string CarMake { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Car Model")]
        public string CarModel { get; set; } = string.Empty;

        [Display(Name = "DUI")]
        public bool DUI { get; set; }

        [Range(0, int.MaxValue)]
        [Display(Name = "Speeding Tickets")]
        public int SpeedingTickets { get; set; }

        [Display(Name = "Full Coverage")]
        public bool CoverageType { get; set; }

        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Quote { get; set; }
    }
}
