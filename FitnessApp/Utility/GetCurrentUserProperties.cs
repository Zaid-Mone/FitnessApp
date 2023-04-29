using FitnessApp.Data;
using FitnessApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Threading.Tasks;
using static System.Security.Claims.ClaimsPrincipal;
using Microsoft.AspNetCore.Identity;
namespace FitnessApp.Utility
{
    [Authorize]
    public class GetCurrentUserProperties
    {
        private readonly UserManager<Person> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public GetCurrentUserProperties(UserManager<Person> userManager, 
            IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }


        //get user ID
        public string GetCurrentUserId()
        {
           
            var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user =  _userManager.FindByIdAsync(userId).GetAwaiter().GetResult();
            return user.Id;
        }

        //get user Email
        public string GetCurrentUserEmail()
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user =  _userManager.FindByIdAsync(userId).GetAwaiter().GetResult(); ;
            return user.Email;
        }

    }
}



