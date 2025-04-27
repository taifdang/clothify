using System.ComponentModel.DataAnnotations;

namespace clothes_backend.Utils.Validate
{
    public class NonEmptyListIntAttribute: ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            var list = value as IList<int>;    
            return !list.Any(x => x == 0);
        }
    }
}
