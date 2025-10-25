using Shared.Models.OptionValue;

namespace Shared.Models.Option;

public class OptionModel
{
    public string Id { get; set; }
    public string Title { get; set; }
    public List<string> Values { get; set; }
}
