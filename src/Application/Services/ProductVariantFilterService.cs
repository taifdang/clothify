using Application.Common.Interface;
using Infrastructure.Enitites;

namespace Application.Services;

public class ProductVariantFilterService : BaseFilterService<ProductVariant>, IProductVariantFilterService
{
    public void FilterByOptionValues(Dictionary<string, string>? options)
    {
        if (options != null && options.Count > 0)
        {
            foreach (var opt in options)
            {
                AddFilter(x => x.Variants.Any(v =>
                    v.OptionValues.Options.Title == opt.Key &&
                    v.OptionValues.Value == opt.Value));
            }
            //AddFilter(x =>
            //    options.All(opt =>
            //        x.Variants.Any(y =>
            //            y.OptionValues.Options.Title == opt.Key &&
            //            y.OptionValues.Value == opt.Value)));
        }
    }

    public void FilterByProductId(int ProductId)
    {
        AddFilter(x => x.ProductId == ProductId);
    }

    public void FilterByExactOptionValues(List<int> items)
    {
        //AddFilter(x => x.Variants.All(x => items.Contains(x.OptionValueId)));

        AddFilter(x => x.Variants
            .Select(v => v.OptionValueId)
            .OrderBy(v => v)
            .SequenceEqual(items.OrderBy(i => i)));
    }

    public void FilterByOptionCount(List<int> items)
    {
        AddFilter(x => x.Variants.Count == items.Count);
    }
}
