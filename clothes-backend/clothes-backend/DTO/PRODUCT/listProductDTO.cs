namespace clothes_backend.DTO.PRODUCT_DTO
{
    public class listProductDTO
    {
        public int id { get; set; }
        public string title { get; set; }
        public List<imageDTO> images { get; set; }
        public List<variantsDTO> variants { get; set; }    
        public List<optionImageDTO> options_value { get; set; }

    }
}
