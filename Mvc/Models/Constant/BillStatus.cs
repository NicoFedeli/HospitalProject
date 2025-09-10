using System.ComponentModel.DataAnnotations;

namespace Hospital.Models.Constant
{
    public class BillStatus
    {
        public const string Paid = "PAID";
        public const string UnPaid = "UNPAID";
    }

    public class BillStatusValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var status = value as string;
            var validStatuses = new[] { BillStatus.Paid, BillStatus.UnPaid };

            if (status != null && validStatuses.Contains(status))
            {
                return ValidationResult.Success;
            }

            return new ValidationResult(ErrorMessage ?? "Invalid status");
        }
    }
}
