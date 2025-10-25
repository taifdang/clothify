namespace Shared.Models.OptionValue;

public class OptionValueReadModel
{
    public string Title { get; set; }
    public string OptionId { get; set; }
    public List<OptionValueImageReadModel> Options { get; set; }
    public List<string> Values { get; set; }
}
