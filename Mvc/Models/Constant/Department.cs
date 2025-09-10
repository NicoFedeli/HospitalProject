using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Hospital.Models.Constant
{
    public class Department
    {
        public const string Cardiology = "Cardiology";
        public const string Neurology = "Neurology";
        public const string Pediatrics = "Pediatrics";
        public const string Emergency = "Emergency";
        public const string General = "General";
    }

    public class DepartmentValidationAttribute : ValidationAttribute
    {
        private static readonly HashSet<string> ValidDepartments = new()
    {
        Department.Cardiology,
        Department.Neurology,
        Department.Pediatrics,
        Department.Emergency,
        Department.General
    };

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var department = value as string;

            if (!string.IsNullOrEmpty(department) && ValidDepartments.Contains(department))
            {
                return ValidationResult.Success;
            }

            return new ValidationResult(ErrorMessage ?? "Invalid department");
        }
    }
}