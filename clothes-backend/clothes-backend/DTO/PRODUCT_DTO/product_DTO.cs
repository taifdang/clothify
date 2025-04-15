namespace clothes_backend.DTO.PRODUCT_DTO
{
    public class product_DTO
    {
        public int id { get; set; }
        public string title { get; set; }
        public List<image_DTO> images { get; set; }
        public List<variants_DTO> variants { get; set; }    
        public List<OptionValueImageGroupDTO> options_value { get; set; }

    }
}
