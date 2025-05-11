using AutoMapper;
using clothes_backend.DTO.Product;
using clothes_backend.DTO.PRODUCT_DTO;
using clothes_backend.Models;

namespace clothes_backend.AutoMapper
{
    public class ProductProfile:Profile
    {
       public ProductProfile()
       {
            #region
            ////variants
            //CreateMap<ProductVariants, VariantDTO>();

            //CreateMap<Products, productListDTO>()
                
            //    .ForMember(
            //        x => x.variants,
            //        y => y.MapFrom(src => src.product_variants)) //variants
            //    .ForMember(
            //        x => x.options_value,
            //        y => y.MapFrom(src =>
            //            src.product_option_images
            //                .GroupBy(x => x.options_values.options.title)
            //                .Select(group_option => new OptionImageDTO
            //                {
            //                    title = group_option.Key,
            //                    option_id = group_option.Select(k => k.options_values.option_id).FirstOrDefault() ?? null!,
            //                    options = group_option
            //                        .GroupBy(v => v.options_values.value)
            //                        .Select(group_option_value => new optionValueDTO
            //                        {
            //                            //image =  valueGroup.Select(i => i.src).ToList()
            //                            title = group_option_value.Key,
            //                            image = group_option_value.Select(i => new ImageDTO { id = i.id, src = i.src }).ToList()
            //                        })
            //                        .ToList()
            //                })
            //                .ToList()
            //    ))//options_value
            //    .ForMember(
            //        x => x.options,
            //        y => y.MapFrom(src =>
            //            src.product_variants
            //                .SelectMany(v => v.variants)
            //                .GroupBy(ovid => ovid.option_values.option_id)
            //                .Select(x => new OptionDTO
            //                {
            //                    option_id = x.Key,
            //                    title = x.Select(x => x.option_values.options.title).FirstOrDefault(),
            //                    values = x.Select(a => a.option_values.value).Distinct().ToList()
            //                })
            //                .ToList()
            //    ))
            //   ;
            #endregion

        }
    }
}
