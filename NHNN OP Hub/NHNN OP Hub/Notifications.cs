using Microsoft.EntityFrameworkCore;
using NHNN_OP_Hub.Data;
using NHNN_OP_Hub.Models;
using System.Linq.Expressions;

namespace NHNN_OP_Hub
{
    public class Notifications
    {

        public int Outpatient = 0;
        public int Posting = 0;

        /*
         * The GrabNotifications function takes two parameters, one of which is the database context: ptpkgContext.
         * The second parameter is a rule which will be used to filter the appropriate items from the database context. The rule is called "predicate".
         * predicate has Expression tree type so that Entity Framework can easily convert what's written in C# into an Sql query.
         * 
         * This is also a generic function, taking the generic type TPackageType so that the same function may be used to grab both
         * outpatient packages or posting packages.
         */
        private int GrabNotifications<TPackageType>(PatientPackageDbContext ptpkgContext, Expression<Func<TPackageType, bool>> predicate)
        {
            int total;
            IQueryable<TPackageType> packages = ptpkgContext
                .PatientPackages
                .OfType<TPackageType>();

            total = packages.Where(predicate).ToList().Count;
            return total;
        }

        public void UpdateNotifications(PatientPackageDbContext ptpkgContext)
        {
            DateOnly today = DateOnly.FromDateTime(DateTime.Now);

            //The gist of the predicate definitions are given after '=>'. Everything before is mostly type safety
            Expression<Func<PostingPackage, bool>> posting_predicate =
                (PostingPackage pkg) => pkg.TrackingNumber == null || pkg.HasReturned == true;

            /*
             * The outpatient predicate is subject to change. Patients often collect later than when they say they will.
             * Staff should be notified to contact patients for any RWR, but this will require a change in how the OutpatientPackage
             * type is described.
             */
            Expression<Func<OutpatientPackage, bool>> outpatient_predicate =
                (OutpatientPackage pkg) => pkg.HasCollected == false && pkg.CollectionDate < today;

            this.Posting = GrabNotifications<PostingPackage>(ptpkgContext, posting_predicate);
            this.Outpatient = GrabNotifications<OutpatientPackage>(ptpkgContext, outpatient_predicate);
        }
    }
}