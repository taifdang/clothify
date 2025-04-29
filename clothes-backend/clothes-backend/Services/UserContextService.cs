using clothes_backend.Interfaces.Service;

namespace clothes_backend.Service
{
    public class UserContextService : IUserContextService
    {
        public readonly IHttpContextAccessor _context;
        public UserContextService(IHttpContextAccessor context)
        {
            _context = context;
        }

        public int convertToInt(string input, int default_value = 0)
        {
            return int.TryParse(input, out int value) ? value : default_value;
        }

        public string getValueAuth()
        {
            var context = _context.HttpContext;
            if(context != null && context.Items.TryGetValue("IsUser",out var user))
            {
               return user?.ToString() ?? "";
            }
            return "";
        }      
    }
}
