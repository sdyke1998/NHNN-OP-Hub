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
        OUTPATIENT,
        POSTING
    };

    [Authorize]
    public class ViewRecordModel : PageModel
    {
        /*
         * Variables required for interacting with webpage
         */
        private int workRequestID;
        public PatientPackage PackageToView;
        public string ErrorMessage;
        public EditType editType;
        public bool IsStagingChanges;// This may be used to display something of a 'stage changes' view down the line. For now it has no use.

        /*
         * Dependency injection
         */
        public PatientPackageDbContext dbContext;

        public ViewRecordModel(PatientPackageDbContext _dbContext)
        {
            this.dbContext = _dbContext;
        }

        /*
         * Form data entry variables
         * Apologies for the excessive use of variables. It's a heuristic solution.
         */
        [BindProperty]
        public string ptName { get; set; }
        [BindProperty]
        public bool stage_ptName { get; set; } = false;
        [BindProperty]
        public string MRN { get; set; }
        [BindProperty]
        public bool stage_MRN { get; set; } = false;
        [BindProperty]
        public int? ticketNum { get; set; } = null;
        [BindProperty]
        public bool stage_ticketNum { get; set; } = false;
        [BindProperty]
        public bool isPaying { get; set; } = false;
        [BindProperty]
        public bool stage_isPaying { get; set; } = false;
        [BindProperty]
        public bool isPrivate { get; set; } = false;
        [BindProperty]
        public bool stage_isPrivate { get; set; } = false;
        [BindProperty]
        public float? rxCost { get; set; } = null;
        [BindProperty]
        public bool stage_rxCost { get; set; } = false;
        [BindProperty]
        public int? reciptNum { get; set; } = null;
        [BindProperty]
        public bool stage_reciptNum { get; set; } = false;
        [BindProperty]
        public bool hasCollected { get; set; }
        [BindProperty]
        public bool stage_hasCollected { get; set; } = false;
        [BindProperty]
        public DateOnly collectionDate { get; set; }
        [BindProperty]
        public bool stage_collectionDate { get; set; } = false;
        [BindProperty]
        public string? trackingNumber { get; set; }
        [BindProperty]
        public bool stage_trackingNumber { get; set; } = false;
        [BindProperty]
        public bool hasReturned { get; set; }
        [BindProperty]
        public bool stage_hasReturned { get; set; } = false;
        [BindProperty]
        public string deliveryAddress { get; set; }
        [BindProperty]
        public bool stage_deliveryAddress { get; set; } = false;


        public void OnPostSwitchToPosting()
        {
            if (PackageToView is PostingPackage)
            {
                ErrorMessage = "This package is already for posting!";
                return;
            }

            PostingPackage newPostingPackage = new PostingPackage
            {
                WorkRequestID = PackageToView.WorkRequestID,
                Name = PackageToView.Name,
                MRN = PackageToView.MRN,
                DateDispensed = PackageToView.DateDispensed,
                IsPaying = PackageToView.IsPaying,
                IsPrivate = PackageToView.IsPrivate,
                RxCost = PackageToView.RxCost,
                ReciptNumber = PackageToView.ReciptNumber,
                TrackingNumber = null,
                HasReturned = false,
                DeliveryAddress = ""
            };
            
            newPostingPackage.History += PackageChange.PackageChangedTo(EditType.POSTING);

            dbContext.PatientPackages.Remove(PackageToView);
            dbContext.PatientPackages.Add(newPostingPackage);

            dbContext.SaveChanges();
            PackageToView = dbContext.PatientPackages.Find(workRequestID);
        }

        public void OnPostSwitchToOutpatient()
        {
            if (PackageToView is OutpatientPackage)
            {
                ErrorMessage = "This package is already for outpatients!";
                return;
            }

            OutpatientPackage newOutpatientPackage = new OutpatientPackage
            {
                WorkRequestID = PackageToView.WorkRequestID,
                Name = PackageToView.Name,
                MRN = PackageToView.MRN,
                DateDispensed = PackageToView.DateDispensed,
                IsPaying = PackageToView.IsPaying,
                IsPrivate = PackageToView.IsPrivate,
                RxCost = PackageToView.RxCost,
                ReciptNumber = PackageToView.ReciptNumber,
                TicketNumber = null,
                CollectionDate = DateOnly.FromDateTime(DateTime.Now.AddDays(7)),
                HasCollected = false
            };
            
            newOutpatientPackage.History += PackageChange.PackageChangedTo(EditType.OUTPATIENT);

            dbContext.PatientPackages.Remove(PackageToView);
            dbContext.PatientPackages.Add(newOutpatientPackage);

            dbContext.SaveChanges();
            PackageToView = dbContext.PatientPackages.Find(workRequestID);
        }

        public void OnPostEdit()
        {

            if (stage_ptName) PackageToView.Name = ptName;
            if (stage_MRN) PackageToView.MRN = MRN;
            if (stage_isPaying) PackageToView.IsPaying = isPaying;
            if (stage_isPrivate) PackageToView.IsPrivate = isPrivate;
            if (stage_rxCost) PackageToView.RxCost = rxCost ?? 0;
            if (stage_reciptNum) PackageToView.ReciptNumber = reciptNum;


            if (PackageToView is OutpatientPackage)
            {
                
                if (stage_ticketNum) ((OutpatientPackage)PackageToView).TicketNumber = ticketNum;
                if (stage_hasCollected) ((OutpatientPackage)PackageToView).HasCollected = hasCollected;
                if (stage_collectionDate) ((OutpatientPackage)PackageToView).CollectionDate = collectionDate;
            }
            else
            {

                if (stage_trackingNumber) ((PostingPackage)PackageToView).TrackingNumber = trackingNumber;
                if (stage_hasReturned) ((PostingPackage)PackageToView).HasReturned = hasReturned;
                if (stage_deliveryAddress) ((PostingPackage)PackageToView).DeliveryAddress = deliveryAddress;
            }
            
            PackageToView.History += PackageChange.GetPackageChange(PackageToView, dbContext);
            dbContext.SaveChanges();
        }

        public void OnGet(int id)
        {
            PackageToView = dbContext.PatientPackages.Find(id);
            workRequestID = id;

            if (PackageToView is OutpatientPackage) editType = EditType.OUTPATIENT;
            else editType = EditType.POSTING;
        }
    }
}
