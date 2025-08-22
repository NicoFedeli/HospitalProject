using System;
using System.ComponentModel.DataAnnotations;

namespace Hospital.Models.ValidationAttributes
{
    public class FutureDateAttribute : ValidationAttribute
    {
        // Verifico la validità della data inserita dall'utente
        public override bool IsValid(object value)
        {
            if (value is DateTime dateTime)
            {
                return dateTime >= DateTime.Now;
            }
            return false;
        }

    }
}
