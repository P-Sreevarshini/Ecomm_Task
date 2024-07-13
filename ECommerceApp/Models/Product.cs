using System.ComponentModel.DataAnnotations;

namespace ECommerceApp.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        [Required(ErrorMessage="Product name is mandatory")]
        public string Name { get; set; }

        [Required(ErrorMessage="Product Description is mandatory")]
        public string Description { get; set; }

        [Required(ErrorMessage="Product Price is mandatory")]
        public decimal Price { get; set; }

        [Required(ErrorMessage="Product Category is mandatory")]
        public string Category { get; set; }

    }
}
