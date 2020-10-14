using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ShopBridge.Models
{
    public class Item
    {
        [Key]
        public int Id { get; set; }

        [StringLength(200)]
        [Required(ErrorMessage = "Item Name is Compulsory!")]
        [DisplayName("Item Name")]
        public string Name { get; set; }

        [StringLength(1000)]
        [Required(ErrorMessage = "Item Description is Compulsory!")]
        [DisplayName("Item Description")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Item Price is Compulsory!")]
        [Range(0.01, 999999, ErrorMessage = "Enter Item Price greater then 0!")]
        [DisplayName("Item Price")]
        public decimal Price { get; set; }

        [DisplayName("Item Image")]
        public byte[] Image {get;set;}
    }
}