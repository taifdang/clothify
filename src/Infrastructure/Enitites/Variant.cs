namespace Infrastructure.Enitites;

public class Variant
{
    public int ProductVariantId { get; set; }
    public int OptionValueId { get; set; }
    public ProductVariant ProductVariants { get; set; }
    public OptionValue OptionValues { get; set; }
}
