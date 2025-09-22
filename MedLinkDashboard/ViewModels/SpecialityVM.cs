using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace MedLinkDashboard.ViewModels
{
    public class SpecialityVM
    {
        public int Id { get; set; }

        [Required]
        [MinLength(3, ErrorMessage = "Min Length is 3 char")]
        [Remote("CheckUnique", "Specialities", AdditionalFields = "Id", ErrorMessage = "Name exist!")]
        public string Name { get; set; }
    }
}
