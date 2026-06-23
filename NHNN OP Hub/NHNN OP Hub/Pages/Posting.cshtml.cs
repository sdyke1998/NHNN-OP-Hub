using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;
using NHNN_OP_Hub.Data;
using NHNN_OP_Hub.Models;

namespace NHNN_OP_Hub.Pages
{
    [Authorize]
    public class PostingModel : PageModel
    {
        private PatientPackageDbContext dbContext;
        public List<PostingPackage> PackagesToDisplay;
        public string AddPatientErrorMessage;
        public string SearchPatientErrorMessage;

        public PostingModel(PatientPackageDbContext _dbContext)
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
         * Variables used to add a new patient to the posting package record
         */
        [BindProperty]
        public string ptName { get; set; }

        [BindProperty]
        public string MRN { get; set; }

        [BindProperty]
        public string deliveryAddress { get; set; }

        [BindProperty]
        public string? trackingNum { get; set; } = null;

        [BindProperty]
        public bool isPaying { get; set; } = false;

        [BindProperty]
        public bool isPrivate { get; set; } = false;

        [BindProperty]
        public float rxCost { get; set; } = 0;

        [BindProperty]
        public int? reciptNum { get; set; } = null;

        [BindProperty]
        public bool hasReturned { get; set; } = false;

        public void ShowRecentPatients(int days_ago = 7)
        {
            PackagesToDisplay = dbContext.PatientPackages.OfType<PostingPackage>()
                .Where(p => p.DateDispensed >= DateTime.Now.AddDays(-days_ago))
                .OrderBy(p => p.DateDispensed).ToList<PostingPackage>();
        }

        // On start-up, display all records added within the last 7 days
        public void OnGet()
        {
            ShowRecentPatients();
        }

        public async Task<IActionResult> OnPostSearchAsync()
        {
            SearchPatientErrorMessage = "";

            IQueryable<PostingPackage> p_pkg = dbContext.PatientPackages.OfType<PostingPackage>();

            if (ByDate is not null)
            {
                DateTime searchByDate = ByDate.Value.ToDateTime(TimeOnly.MinValue);
                p_pkg = p_pkg.Where(p => p.DateDispensed.Date == searchByDate);
            }

            if (ByMRN is not null)
                p_pkg = p_pkg.Where(p => p.MRN == ByMRN);

            if (ByName is not null)
                p_pkg = p_pkg.Where(p => p.Name == ByName);

            PackagesToDisplay = p_pkg.OrderBy(p => p.DateDispensed).ToList<PostingPackage>();

            if (PackagesToDisplay.Count == 0) SearchPatientErrorMessage = "No patients were found.";

            return Page();
        }

        public async Task<IActionResult> OnPostEnterAsync()
        {
            AddPatientErrorMessage = "";

            PostingPackage p_pkg = new PostingPackage
            {
                Name = ptName,
                MRN = this.MRN,
                DateDispensed = DateTime.Now,
                IsPaying = isPaying,
                IsPrivate = isPrivate,
                RxCost = rxCost,
                ReciptNumber = reciptNum, 
                DeliveryAddress = deliveryAddress,
                TrackingNumber = trackingNum,
                HasReturned = hasReturned,
                History = new PackageHistory()
            };

            p_pkg.History += PackageChange.Initial();

            try
            {
                this.dbContext.Add(p_pkg);
            }
            catch (Exception e)
            {
                AddPatientErrorMessage = "This patient could not be added.";
            }

            try
            {
                await this.dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                AddPatientErrorMessage = "Database error: changes could not be saved. Please retry.";
            }

            ShowRecentPatients();
            return Page();
        }
    }
}