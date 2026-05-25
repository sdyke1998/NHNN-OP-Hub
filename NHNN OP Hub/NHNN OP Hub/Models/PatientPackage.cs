using System.ComponentModel.DataAnnotations;

namespace NHNN_OP_Hub.Models
{
    public abstract class PatientPackage
    {
        [Key]
        public int WorkRequestID { get; set; }
        public string Name { get; set; }
        public string MRN { get; set; }
        public DateTime DateDispensed { get; set; }
        public bool IsPaying { get; set; }
        public bool IsPrivate { get; set; }
        public float? RxCost { get; set; }
        public int? ReciptNumber { get; set; }
    }

    public class PostingPackage : PatientPackage
    {
        public string? TrackingNumber { get; set; }
        public bool HasReturned { get; set; }
        public string DeliveryAddress { get; set; }
    }

    public class OutpatientPackage : PatientPackage
    {
        public int? TicketNumber { get; set; }
        public DateTime CollectionDate { get; set; }
        public bool HasCollected { get; set; }
    }
}
