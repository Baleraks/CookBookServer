using System.Security.Claims;

namespace CookBookBase.Models
{
    public class ValidValue <T>
    {
        public bool isSuccess;
        public T value;
        public string exeption;
    }
}
