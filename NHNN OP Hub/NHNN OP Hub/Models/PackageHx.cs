using Microsoft.EntityFrameworkCore;
using NHNN_OP_Hub.Data;
using System.Collections;
using System.Diagnostics.Contracts;

namespace NHNN_OP_Hub.Models
{
    public class PackageChange
    {
        /*
         * The history of a package should be immutable. If someone needs to make a change to a patient's record, this must be done by creating further changes to undo what was done before.
         * 
         * Every change will be documented in a way not dissimilar to how The Epic EHRS handles audit trails.
         */

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

        public override string ToString()
        {
            string output = $"{Timestamp} : ";

            for(int i = 0; i < n_FieldsChanged; i++)
            {
                output += $"{FieldNames[i]} was changed to {FieldValues[i]}.";
                if (i + 1 != n_FieldsChanged) output += " ";
            }

            return output;
        }
    }

    public class PackageHistory : IEnumerable
    {
        private List<PackageChange> Changes;
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