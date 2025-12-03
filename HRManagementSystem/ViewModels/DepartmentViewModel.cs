using System.ComponentModel.DataAnnotations;

namespace HRManagementSystem.ViewModels
{
    public class DepartmentViewModel
    {
        
        public int Id { get; set; }

        [Required(ErrorMessage = "*")]
        public string Name { get; set; }
    }
}
