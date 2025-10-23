namespace Shared.Models.ProductVariant;

public class GenerateVariantsRequest
{
    public Dictionary<string, List<int>> OptionValues { get; set; } = new();
}
