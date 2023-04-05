using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LBT_Api.Entities
{
    public class Company
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Address")]
        public int AddressId { get; set; }
        public virtual Address Address { get; set; }

        [ForeignKey("ContactInfo")]
        public int ContactInfoId { get; set; }
        public virtual ContactInfo ContactInfo { get; set; }


        [Required]
        public string Name { get; set; }


        public virtual List<Company> Companies { get; set; }
    }
}
