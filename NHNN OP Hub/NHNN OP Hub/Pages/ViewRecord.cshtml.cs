using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NHNN_OP_Hub.Models;
using Microsoft.EntityFrameworkCore;
using NHNN_OP_Hub.Data;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;

namespace NHNN_OP_Hub.Pages
{
    public enum EditType
    {
        [Display(Name = "Switch to package for posting")]
        TO_POSTING,
        [Display(Name = "Switch to outpatient package")]
        TO_OUTPATIENT,
        [Display(Name = "Update package")]
        UPDATE
    };

    [Authorize]
    public class ViewRecordModel : PageModel
    {

        private int WorkRequestID;
        public PatientPackage PackageToView;

        public PatientPackageDbContext dbContext;
        public Notifications notifications;

        public ViewRecordModel(PatientPackageDbContext _dbContext, Notifications _notifications)
        {
            this.notifications = _notifications;
            this.dbContext = _dbContext;
        }

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
        public float? rxCost { get; set; } = null;
        [BindProperty]
        public int? reciptNum { get; set; } = null;
        [BindProperty]
        public bool hasCollected { get; set; }
        [BindProperty]
        public DateOnly collectionDate { get; set; } = DateOnly.FromDateTime(DateTime.Now.AddDays(7));
        [BindProperty]
        public string? trackingNumber { get; set; }
        [BindProperty]
        public bool hasReturned { get; set; }
        [BindProperty]
        public string deliveryAddress { get; set; }
        [BindProperty]
        public EditType editType { get; set; }
        

        public void SwitchToPosting() { }
        public void SwitchToOutpatient() { }
        public void UpdatePackageRecord() { }

        public void OnGet()
        {
            PackageToView = dbContext.PatientPackages.Find(WorkRequestID);
        }
    }
}
