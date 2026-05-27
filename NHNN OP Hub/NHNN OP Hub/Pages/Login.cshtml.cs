using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using NHNN_OP_Hub.Data;
using NHNN_OP_Hub.Models;
using Microsoft.EntityFrameworkCore;

namespace NHNN_OP_Hub.Pages
{
    public class LoginModel : PageModel
    {
        private readonly UserDbContext dbContext;
        public LoginModel(UserDbContext _dbContext)
        {
            this.dbContext = _dbContext;
        }

        [BindProperty]
        public string Username { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public string ErrorMessage;
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            User user = await dbContext.Users.FirstOrDefaultAsync(u => u.username == Username);
            if(user is null)
            {
                ErrorMessage = "This username was not found";
                return Page();
            }

            if (user.password == Password) //You will need to ammend this to work with user database
            {
                List<Claim> claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, Username),
                    new Claim(ClaimTypes.Role, "Role")//To implement user and admin roles
                };

                ClaimsIdentity claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity));

                return RedirectToPage("/Home");
            }

            ErrorMessage = "Invalid username or password";
            return Page();
        }


    }
}
