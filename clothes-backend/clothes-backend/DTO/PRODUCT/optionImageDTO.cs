namespace clothes_backend.DTO.PRODUCT_DTO
{
    public class optionImageDTO
    {
        public string title { get; set; }
        public string option_id { get; set; }
        public List<optionValueDTO> options { get; set; }
    }
}
