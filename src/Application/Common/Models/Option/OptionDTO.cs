using Application.Common.Models.OptionValue;

namespace Application.Common.Models.Option;

public class OptionDTO
{
    public string Id { get; set; }
    public string Title { get; set; }
    public List<OptionValueDTO> Values { get; set; }
}
