using System.ComponentModel.DataAnnotations;

namespace HRManagementSystem.ViewModels
{
    public class AddPositionViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "*")]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required(ErrorMessage = "*")]
        [MaxLength(500)]
        public string Description { get; set; }

        [Required(ErrorMessage = "*")]
        public int DepartmentId { get; set; }

    }
}

