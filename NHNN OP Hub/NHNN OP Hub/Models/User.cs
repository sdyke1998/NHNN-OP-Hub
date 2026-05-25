using System.ComponentModel.DataAnnotations;

namespace NHNN_OP_Hub.Models
{
    public class User
    {
        [Key]
        public int ID { get; set; }
        public string username { get; set; }
        public string password { get; set; }

        public AccessLevel access { get; set; }
    }
}
