using Microsoft.EntityFrameworkCore;
using NHNN_OP_Hub.Data;
using System.Collections;
using System.Diagnostics.Contracts;
using static NHNN_OP_Hub.Pages.EditType;

namespace NHNN_OP_Hub.Models
{
    public class PackageChange
    {

        private readonly DateTime Timestamp;
        private readonly List<string> FieldNames;
        private readonly List<string> FieldValues;
        private readonly int n_FieldsChanged;

        private PackageChange(DateTime _Timestamp, List<string> _FieldNames, List<string> _FieldValues)
        {
            this.n_FieldsChanged = _FieldNames.Count;
            if (this.n_FieldsChanged != _FieldValues.Count) throw new Exception("Error: The number of fields changed is discrepant with the number of values.");

            this.Timestamp = _Timestamp;
            this.FieldNames = _FieldNames;
            this.FieldValues = _FieldValues;
        }

        //This may be called after DbContext.SaveChanges() is called
        public static PackageChange PackageChangedTo(NHNN_OP_Hub.Pages.EditType packageType)
        {
            string _FieldName = "Package changed to ";
            string _FieldValue;

            if (packageType == NHNN_OP_Hub.Pages.EditType.OUTPATIENT) _FieldValue = "outpatient package.";
            else _FieldValue = "posting package.";

            return new PackageChange(DateTime.Now, new List<string> { _FieldName }, new List<string> { _FieldValue });
        }

        //This must be called before DbContext.SaveChanges() is called
        public static PackageChange GetPackageChange(PatientPackage package, PatientPackageDbContext dbContext)
        {
            List<string> _FieldNames = new List<string>();
            List<string> _FieldValues = new List<string>();

            var properties = dbContext.Entry(package).Properties.Where(pkg => pkg.IsModified);

            foreach(var property in properties)
            {
                _FieldNames.Add(property.Metadata.Name);
                _FieldValues.Add(property.CurrentValue.ToString());
            }
            
            if (_FieldNames.Count != _FieldValues.Count) throw new Exception("Error: The number of fields changed is discrepant with the number of values.");

            return new PackageChange(DateTime.Now, _FieldNames, _FieldValues);
        }

        public static PackageChange Initial()
        {
            return new PackageChange(
                DateTime.Now,
                new List<string> {"Package created."},
                new List<string> {""}
            );
        }

        public override string ToString()
        {
            if (n_FieldsChanged == 1) return $"{Timestamp} : Package Created.";

            string output = $"{Timestamp} : ";

            for(int i = 0; i < n_FieldsChanged; i++)
            {
                output += $"{FieldNames[i]} was changed to {FieldValues[i]}.";
                if (i - 1 != n_FieldsChanged) output += " ";
            }

            Console.WriteLine(FieldNames);
            return output;
        }
    }

    [Owned] //This annotator lets EF know that this class will be owned by each PatientPackage and will not create a primary key for objects of this type.
    public class PackageHistory : IEnumerable
    {
        private List<PackageChange> Changes { get; set; } = new List<PackageChange>();
        public int Length => Changes.Count; //Lambda notation used as a shorthand for a getter with no setter.

        public IEnumerator GetEnumerator()
        {
            return Changes.GetEnumerator();
        }

        public static PackageHistory operator + (PackageHistory thisPackageChange, PackageChange changeToAdd)
        {
            thisPackageChange.Changes.Add(changeToAdd);
            return thisPackageChange;
        }
    }
}