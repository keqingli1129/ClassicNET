using System.Collections.Generic;

namespace Shared.Contracts.DTOs
{
    public class CategoryDto
    {
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public byte[] Picture { get; set; }
        public List<ProductDto> Products { get; set; }
    }
}
