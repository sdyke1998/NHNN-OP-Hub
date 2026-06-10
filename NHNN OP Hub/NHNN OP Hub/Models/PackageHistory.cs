

using Microsoft.IdentityModel.Tokens;

namespace NHNN_OP_Hub.Models
{
    //The lowercase d is a term borrowed from maths. 'dPackage' represents a change in a package.
    public class dPackage
    {
        public string ChangeString { get; private set; } //What was changed
        public DateTime Timestamp { get; set; } //When did it change

        private dPackage() { }

        public dPackage(PatientPackage package1, PatientPackage package2)
        {
            if(package1 is PostingPackage && package2 is PostingPackage)
            {

            }
            else if(package1 is OutpatientPackage && package2 is OutpatientPackage)
            {

            }
            else
            {
                throw new Exception("");
            }
        }

        public static dPackage ChangedToPosting(OutpatientPackage oldPackage, PostingPackage newPackage)
        {
            return new dPackage
            {
                ChangeString = "changed", //Unique string
                Timestamp = DateTime.Now
            };
        }
        public static dPackage ChangedToOutpatient(PostingPackage oldPackage, OutpatientPackage newPackage)
        {
            return new dPackage
            {
                ChangeString = "changed", //Different unique string
                Timestamp = DateTime.Now
            };
        }
    }

    public class PackageHx
    {
        public List<dPackage> History { get; set; }

        public static PackageHx operator +(PackageHx packageHx, (PatientPackage, PatientPackage) packages)
        {
            (var package1, var package2) = packages;
            dPackage change = new dPackage(package1, package2);

            List<dPackage> newHistory = packageHx.History;
            newHistory.Add(change);

            return new PackageHx
            {
                History = newHistory
            };
        }
    }
}