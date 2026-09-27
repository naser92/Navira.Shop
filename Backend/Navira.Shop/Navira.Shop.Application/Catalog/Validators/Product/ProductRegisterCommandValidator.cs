using FluentValidation;

namespace Navira.Shop.Application.Catalog
{
    public class ProductRegisterCommandValidator : AbstractValidator<ProductRegisterCommand>
    {
        public ProductRegisterCommandValidator()
        {


            RuleFor(x => x.Name).Cascade(CascadeMode.Stop).NotEmpty().WithMessage("نام محصول الزامی است.").Length(2, 300)
                 .WithMessage("نام محصول باید بین ۲ تا ۳۰۰ کاراکتر باشد.").WithName("نام محصول");

            RuleFor(x => x.Slug)
                 .Length(2, 350)
                     .WithMessage("اسلاگ باید بین ۲ تا ۳۵۰ کاراکتر باشد.")
                 .When(x => !string.IsNullOrWhiteSpace(x.Slug))
                 .WithName("اسلاگ");

            RuleFor(x => x.ShortDescription)
                 .MaximumLength(1000)
                     .WithMessage("توضیحات کوتاه حداکثر ۱۰۰۰ کاراکتر است.")
                 .WithName("توضیحات کوتاه");

            RuleFor(x => x.CategoryId)
                .Cascade(CascadeMode.Stop)
                .NotNull()
                    .WithMessage("دسته‌بندی الزامی است.")
                .GreaterThan(0)
                    .WithMessage("شناسه دسته‌بندی باید بزرگ‌تر از صفر باشد.")
                .WithName("دسته‌بندی");

            RuleFor(x => x.BrandId)
                .GreaterThan(0)
                    .WithMessage("شناسه برند باید بزرگ‌تر از صفر باشد.")
                .When(x => x.BrandId.HasValue)
                .WithName("برند");

            RuleFor(x => x.TaxCategoryId)
                .GreaterThan(0)
                    .WithMessage("شناسه دسته مالیاتی باید بزرگ‌تر از صفر باشد.")
                .When(x => x.TaxCategoryId.HasValue)
                .WithName("دسته مالیاتی");


        }
    }
}
