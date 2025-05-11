using clothes_backend.DTO.Product;
using clothes_backend.DTO.PRODUCT_DTO;

namespace clothes_backend.DTO.Test
{
    public class ListPDTO
    {
        public int id { get; set; }
        public string title { get; set; }
        public List<ImageDTO> images { get; set; }
        public List<VariantDTO> variants { get; set; }
        public List<OptionImageDTO> options_value { get; set; }
        public List<OptionDTO> options { get; set; }
    }
}
