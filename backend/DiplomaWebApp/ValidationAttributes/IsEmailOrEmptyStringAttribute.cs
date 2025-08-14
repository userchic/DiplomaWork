using System.ComponentModel.DataAnnotations;

namespace DiplomaWebApp.ValidationAttributes
{
    public class IsEmailOrEmptyStringAttribute:ValidationAttribute
    {
        public override bool IsValid(object?  value)
        {
            if (value is not string line)
            {
                return false;
            }
            else
            {
                if ((line.Contains('\r') || line.Contains('\n')))
                {
                    return false;
                }
                if (line.Length == 0)
                {
                    return true;
                }
                int index = line.IndexOf('@');
                return
                    index > 0 &&
                    index != line.Length - 1 &&
                    index == line.LastIndexOf('@');
            }
        }
    }
}
