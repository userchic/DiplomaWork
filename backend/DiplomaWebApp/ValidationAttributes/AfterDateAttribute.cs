using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Reflection;

namespace DiplomaWebApp.ValidationAttributes
{
    public class AfterDateAttribute : CompareAttribute
    {
        public AfterDateAttribute(string otherProperty) : base(otherProperty)
        {
        }

        public string? OtherPropertyDisplayName { get; internal set; }


        public override string FormatErrorMessage(string name) =>
            string.Format(
                CultureInfo.CurrentCulture, ErrorMessageString, name, OtherPropertyDisplayName ?? OtherProperty);

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var otherPropertyInfo = validationContext.ObjectType.GetRuntimeProperty(OtherProperty);

            object? otherPropertyValue = otherPropertyInfo.GetValue(validationContext.ObjectInstance, null);
            if (value is null || otherPropertyValue is null) return null;
            if ((DateTime)value >(DateTime)otherPropertyValue)
            {
                OtherPropertyDisplayName ??= GetDisplayNameForProperty(otherPropertyInfo);

                string[]? memberNames = validationContext.MemberName != null
                   ? new[] { validationContext.MemberName }
                   : null;
                return new ValidationResult(FormatErrorMessage(validationContext.DisplayName), memberNames);
            }

            return null;
        }

        private string? GetDisplayNameForProperty(PropertyInfo property)
        {
            IEnumerable<Attribute> attributes = CustomAttributeExtensions.GetCustomAttributes(property, true);
            foreach (Attribute attribute in attributes)
            {
                if (attribute is DisplayAttribute display)
                {
                    return display.GetName();
                }
            }
            return OtherProperty;
        }
    }
}
