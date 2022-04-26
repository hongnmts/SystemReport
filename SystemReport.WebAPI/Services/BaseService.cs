using System.Linq;
using Microsoft.AspNetCore.Http;
using SystemReport.WebAPI.Data;
using SystemReport.WebAPI.Models;

namespace SystemReport.WebAPI.Services
{
    public abstract class BaseService
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly DataContext _context;
        //protected readonly ApplicationDbContext context;
        private HttpContext _httpContext { get { return _contextAccessor.HttpContext; } }
        private User _appUser;
        public User CurrentUser => GetCurrentUser();
        public string CurrentUserName => GetCurrentUser().UserName;

        public BaseService(DataContext context, IHttpContextAccessor contextAccessor)
        {
            this._context = context;
            this._contextAccessor = contextAccessor;
        }
        
        private User GetCurrentUser()
        {
            if (!_httpContext.User.Identity.IsAuthenticated)
                return new User();

            if (_appUser != null)
                return _appUser;

            var userId = _httpContext.User?.Claims.FirstOrDefault(x => x.Type == "id")?.Value;
            // _appUser = _context.Users.FindAsync(x => x.Id == userId).Result.FirstOrDefault();
            return _appUser;
        }
    }
}