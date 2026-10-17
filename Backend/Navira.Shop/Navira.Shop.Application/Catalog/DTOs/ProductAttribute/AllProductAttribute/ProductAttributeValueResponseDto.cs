using Navira.Shop.Core.ViewModels;
using Navira.Shop.Domain.Catalog;

namespace Navira.Shop.Application.Catalog
{
    public class ProductAttributeValueResponseDto : BaseDto<long>
    {
        // شناسه رکورد ProductAttributeValue

        public int ProductAttributeId { get; set; }

        public string Name { get; set; } = default!;

        public AttributeValueType ValueType { get; set; }

        // وضعیت تعریف ویژگی
        public bool IsActive { get; set; }

        public int? ProductAttributeOptionId { get; set; }

        public List<int> ProductAttributeOptionIds { get; set; } = [];

        public string? TextValue { get; set; }

        public decimal? NumberValue { get; set; }

        public bool? BooleanValue { get; set; }

        public DateTime? DateValue { get; set; }

        public List<ProductAttributeSelectedOptionDto> SelectedOptions { get; set; } = [];
    }
}
