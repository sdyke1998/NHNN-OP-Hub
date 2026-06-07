using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NHNN_OP_Hub.Data;

namespace NHNN_OP_Hub.Pages
{
    [Authorize]
    public class HomeModel : PageModel
    {
        public Notifications AppNotifications;
        public PatientPackageDbContext PatientDbContext;

        public HomeModel(Notifications _AppNotifications, PatientPackageDbContext _DbContext)
        {
            this.AppNotifications = _AppNotifications;
            this.PatientDbContext = _DbContext;
        }
        public void OnGet()
        {
            
        }
    }
}
