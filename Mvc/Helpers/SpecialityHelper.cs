using Microsoft.AspNetCore.Mvc.Rendering;
using Hospital.Models.Constant;
namespace Hospital.Helpers
{
    public static class SpecialityHelper
    {
        public static IEnumerable<SelectListItem> GetSpecialityList()
        {
            return new List<SelectListItem>
        {
            new SelectListItem { Text = "Surgeon", Value = Speciality.Surgeon },
            new SelectListItem { Text = "Cardiologist", Value = Speciality.Cardiologist },
            new SelectListItem { Text = "Neurologist", Value = Speciality.Neurologist },
            new SelectListItem { Text = "Pediatrician", Value = Speciality.Pediatrician },
            new SelectListItem { Text = "General Practitioner", Value = Speciality.GeneralPractitioner }
        };
        }
    }
}
