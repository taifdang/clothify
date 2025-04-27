namespace clothes_backend.DTO.PRODUCT_DTO
{
    public class productListDTO
    {
        public int id { get; set; }
        public string title { get; set; }
        public List<imageDTO> images { get; set; }
        public List<VariantDTO> variants { get; set; }    
        public List<optionImageDTO> options_value { get; set; }

    }
}
