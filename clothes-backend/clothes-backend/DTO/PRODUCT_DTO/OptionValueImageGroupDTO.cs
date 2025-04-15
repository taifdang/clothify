namespace clothes_backend.DTO.PRODUCT_DTO
{
    public class OptionValueImageGroupDTO
    {
        public string title { get; set; }
        public string option_id { get; set; }
        public List<option_value_DTO> options { get; set; }
    }
}
