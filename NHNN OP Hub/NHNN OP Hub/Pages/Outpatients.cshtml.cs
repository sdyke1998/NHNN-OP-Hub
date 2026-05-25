using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace NHNN_OP_Hub.Pages
{
    [Authorize]
    public class OutpatientsModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
