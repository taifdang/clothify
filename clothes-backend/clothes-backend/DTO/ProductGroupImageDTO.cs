using clothes_backend.Models;

namespace clothes_backend.DTO
{
    public class ProductGroupImageDTO
    {
        public string title { get; set; }
        public List<productImageDTO> image { get; set; }
    }
}
