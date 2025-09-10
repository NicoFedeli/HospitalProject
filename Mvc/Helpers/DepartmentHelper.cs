using Microsoft.AspNetCore.Mvc.Rendering;
using Hospital.Models.Constant;

namespace Hospital.Helpers
{
    public static class DepartmentHelper
    {
        public static IEnumerable<SelectListItem> GetDepartmentList()
        {
            return new List<SelectListItem>
        {
            new SelectListItem { Text = "Cardiology", Value = Department.Cardiology },
            new SelectListItem { Text = "Neurology", Value = Department.Neurology },
            new SelectListItem { Text = "Pediatrics", Value = Department.Pediatrics },
            new SelectListItem { Text = "Emergency", Value = Department.Emergency },
            new SelectListItem { Text = "General", Value = Department.General }
        };
        }

        public static Dictionary<string, string> GetDepartmentColors() => new()
    {
        { Department.Cardiology, "bg-danger" },
        { Department.Neurology, "bg-primary" },
        { Department.Pediatrics, "bg-warning" },
        { Department.Emergency, "bg-success" },
        { Department.General, "bg-info" }
    };
    }
}
