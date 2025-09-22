using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace MedLinkDashboard.ViewModels
{
    public class RoleVM
    {
        [ValidateNever]
        public string Id { get; set; }

        [Required]
        [MinLength(3, ErrorMessage = "Min Length is 3 char")]
        public string Name { get; set; }
    }
}
