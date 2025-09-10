using System.ComponentModel.DataAnnotations;

namespace Hospital.Models.Constant
{
    public class Speciality
    {
        public const string Surgeon = "Surgeon";
        public const string Cardiologist = "Cardiologist";
        public const string Neurologist = "Neurologist";
        public const string Pediatrician = "Pediatrician";
        public const string GeneralPractitioner = "General Practitioner";
    }

    public class SpecialityValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var speciality = value as string;
            var validSpecialities = new[]
            {
            Speciality.Surgeon,
            Speciality.Cardiologist,
            Speciality.Neurologist,
            Speciality.Pediatrician,
            Speciality.GeneralPractitioner
        };

            if (!string.IsNullOrEmpty(speciality) && validSpecialities.Contains(speciality))
            {
                return ValidationResult.Success;
            }

            return new ValidationResult(ErrorMessage ?? "Invalid speciality");
        }
    }
}