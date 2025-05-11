namespace clothes_backend.DTO.PRODUCT_DTO
{
    public class OptionImageDTO
    {
        public string title { get; set; }
        public string option_id { get; set; }
        public List<optionValueDTO> options { get; set; }
    }
}
