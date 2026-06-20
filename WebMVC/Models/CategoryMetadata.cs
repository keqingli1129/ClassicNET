namespace WebMVC.Models
{
    using System.ComponentModel.DataAnnotations;

    [MetadataType(typeof(CategoryMetadata))]
    public partial class Category
    {
    }

    public class CategoryMetadata
    {
        [Required]
        public string CategoryName { get; set; }

        [Required]
        public string Description { get; set; }
    }
}
