using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NHNN_OP_Hub.Data;
using NHNN_OP_Hub.Models;

namespace NHNN_OP_Hub.Pages
{
    [Authorize]
    public class OutpatientsModel : PageModel
    {
        public PatientPackageDbContext dbContext;
        List<OutpatientPackage> PatientsToDisplay;

        public OutpatientsModel(PatientPackageDbContext _dbContext)
        {
            this.dbContext = _dbContext;
        }
        public void OnGet()
        {
            PatientsToDisplay = dbContext.PatientPackages.OfType<OutpatientPackage>()
                .Where(op => op.Name.Length < 5).ToList<OutpatientPackage>();
        }
    }
}