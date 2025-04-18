using clothes_backend.Inteface.User;

namespace clothes_backend.Service
{
    public class AuthService : IAuthService
    {
        public readonly IHttpContextAccessor _context;
        public AuthService(IHttpContextAccessor context)
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
