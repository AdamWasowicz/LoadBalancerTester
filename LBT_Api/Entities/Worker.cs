using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LBT_Api.Entities
{
    public class Worker
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Company")]
        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }

        [ForeignKey("Address")]
        public int AddressId { get; set; }
        public virtual Address Address { get; set; }

        [ForeignKey("ContactInfo")]
        public int ContactInfoId { get; set; }
        public virtual ContactInfo ContactInfo { get; set; }


        [Required]
        public string Name { get; set; }

        [Required]
        public string Surname { get; set; }


        public virtual List<Worker> Workers { get; set; }
    }
}
