namespace clothes_backend.DTO.Product
{
    public class OptionDTO
    {
        public string title { get; set; }
        public string option_id { get; set; }
        public List<string> values { get; set; }
        //public List<ValueMapDTO> values_map { get; set; }
    }
}
