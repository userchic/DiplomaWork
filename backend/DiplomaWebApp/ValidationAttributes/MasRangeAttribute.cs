using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace DiplomaWebApp.ValidationAttributes
{
    public class MasRangeAttribute:ValidationAttribute
    {
        int Min,Max;
        public MasRangeAttribute(int min,int max)
        {
            Min = min; Max = max;
        }
        public override bool IsValid(object? value)
        {
            if (value is null) return false;
            return ((ICollection)value).Count<Max && ((ICollection)value).Count > Min;
        }
    }
}
