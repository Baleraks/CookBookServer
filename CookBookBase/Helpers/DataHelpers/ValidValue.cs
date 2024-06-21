using System.Security.Claims;

namespace CookBookBase.Helpers.DataHelpers
{
    public class ValidValue<T>
    {
        public bool isSuccess;
        public T value;
        public string exeption;
    }
}
