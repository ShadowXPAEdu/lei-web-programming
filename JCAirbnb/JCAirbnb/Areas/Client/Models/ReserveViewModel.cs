using JCAirbnb.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace JCAirbnb.Areas.Client.Models
{
    public class ReserveViewModel
    {
        [Display(Name = "Property")]
        public Property Property { get; set; }

        [Required]
        [Display(Name = "Check-in")]
        [DataType(DataType.Date)]
        public DateTime CheckIn { get; set; }

        [Required]
        [Display(Name = "Check-out")]
        [DataType(DataType.Date)]
        public DateTime CheckOut { get; set; }

        [Required]
        [Display(Name = "Credit card number")]
        [DataType(DataType.CreditCard)]
        [StringLength(16, MinimumLength = 16)]
        [CreditCardValidator(ErrorMessage = "Credit card number must be a number")]
        public string CardNumber { get; set; }

        [Required]
        [Display(Name = "Expire date")]
        [DataType(DataType.Date)]
        [ExpireDateValidator]
        public DateTime ExpireDate { get; set; }

        [Required]
        [Display(Name = "CVC")]
        [StringLength(3, MinimumLength = 3)]
        [CVCValidator(ErrorMessage = "CVC needs to be a number")]
        public string CVC { get; set; }
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public sealed class CreditCardValidator : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var inputValue = value as string;
            var isValid = false;

            if (!string.IsNullOrWhiteSpace(inputValue)) isValid = new RegularExpressionAttribute(@"^[0-9]{16}$").IsValid(inputValue);

            return isValid;
        }
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public sealed class CVCValidator : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var inputValue = value as string;
            var isValid = false;

            if (!string.IsNullOrWhiteSpace(inputValue)) isValid = new RegularExpressionAttribute(@"^[0-9]{3}$").IsValid(inputValue);

            return isValid;
        }
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public sealed class ExpireDateValidator : RangeAttribute
    {
        public ExpireDateValidator() : base(typeof(DateTime), DateTime.Today.AddMonths(1).AddDays(-DateTime.Today.Day + 1).ToString(), DateTime.MaxValue.ToString()) { }
    }
}
