using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoppingCart.Models
{
    public class Sale
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public AppUser User { get; set; }
        public DateTime Datetime { get; set; }

        [Column(TypeName = "decimal(8, 2)")]
        public decimal Total { get; set; }

        public ICollection<DetailSale> Detail { get; set; }
    }
}
