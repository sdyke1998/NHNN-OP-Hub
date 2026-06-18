using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;
using NHNN_OP_Hub.Data;
using NHNN_OP_Hub.Models;

namespace NHNN_OP_Hub.Pages
{
    [Authorize]
    public class OutpatientsModel : PageModel
    {
        private PatientPackageDbContext dbContext;
        public List<OutpatientPackage> PackagesToDisplay;
        public string AddPatientErrorMessage;
        public string SearchPatientErrorMessage;
        public OutpatientsModel(PatientPackageDbContext _dbContext)
        {
            this.dbContext = _dbContext;
        }

        /*
         * Variables for search box below
         */

        [BindProperty]
        public string? ByName { get; set; }
        [BindProperty]
        public string? ByMRN { get; set; }
        [BindProperty]
        public DateOnly? ByDate { get; set; }

        /*
         * The following are variable used to add a new patient to the outpatient package record
         */
        [BindProperty]
        public string ptName { get; set; }
        [BindProperty]
        public string MRN { get; set; }
        [BindProperty]
        public int? ticketNum { get; set; } = null;
        [BindProperty]
        public bool isPaying { get; set; } = false;
        [BindProperty]
        public bool isPrivate { get; set; } = false;
        [BindProperty]
        public float rxCost { get; set; } = 0; //Nullable type is not necessary since: pt not paying <=> they've spent £0 on the rx
        [BindProperty]
        public int? reciptNum { get; set; } = null;
        [BindProperty]
        public bool hasCollected { get; set; }
        [BindProperty]
        public DateOnly collectionDate { get; set; } = DateOnly.FromDateTime(DateTime.Now.AddDays(7));

        //On start-up, display all records added within the last 24hrs
        public void OnGet()
        {
            PackagesToDisplay = dbContext.PatientPackages.OfType<OutpatientPackage>()
                .Where(op => op.DateDispensed >= DateTime.Now.AddHours(-24))
                    .OrderBy(op => op.DateDispensed).ToList<OutpatientPackage>();
        }

        public async Task<IActionResult> OnPostSearchAsync()
        {
            SearchPatientErrorMessage = "";

            IQueryable<OutpatientPackage> op_pkg
                = dbContext.PatientPackages.OfType<OutpatientPackage>();

            if(ByDate is not null)
            {
                DateTime searchByDate = ByDate.Value.ToDateTime(TimeOnly.MinValue);
                op_pkg = op_pkg.Where(op => op.DateDispensed.Date == searchByDate);
            }  

            if (ByMRN is not null)
                op_pkg = op_pkg.Where(op => op.MRN == ByMRN);

            if (ByName is not null)
                op_pkg = op_pkg.Where(op => op.Name == ByName);

            PackagesToDisplay = op_pkg
                .OrderBy(op => op.DateDispensed).ToList<OutpatientPackage>();

            if (PackagesToDisplay.Count == 0) SearchPatientErrorMessage = "No patients were found.";

            return Page();
        }

        public async Task<IActionResult> OnPostEnterAsync()
        {
            AddPatientErrorMessage = "";
            OutpatientPackage op_pkg = new OutpatientPackage
            {
                Name = ptName,
                MRN = this.MRN,
                DateDispensed = DateTime.Now,
                IsPaying = isPaying,
                IsPrivate = isPrivate,
                RxCost = rxCost,
                ReciptNumber = reciptNum,
                TicketNumber = ticketNum,
                CollectionDate = collectionDate,
                HasCollected = hasCollected,
                History = new PackageHistory()
            };

            try
            {
                this.dbContext.Add(op_pkg);
            }
            catch(Exception e)
            {
                AddPatientErrorMessage = "This patient could not be added.";
            }

            try
            {
                await this.dbContext.SaveChangesAsync();
            }
            catch(Exception e)
            {
                AddPatientErrorMessage = "Database error: changes could not be saved. Please retry.";
            }

            return Page();
        }
    }
}
