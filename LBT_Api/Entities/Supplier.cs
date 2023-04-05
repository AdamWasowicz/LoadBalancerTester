using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LBT_Api.Entities
{
    public class Supplier
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Address")]
        public int AddressId { get; set; }
        public virtual Address Address { get; set; }


        [Required]
        public string Name { get; set; }


        public virtual List<Supplier> Suppliers { get; set; }
    }
}
