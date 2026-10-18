using Microsoft.EntityFrameworkCore;
using Navira.Shop.Core.Bus;
using Navira.Shop.Core.Mapper;
using Navira.Shop.Core.Persistence;
using Navira.Shop.Core.Results;
using Navira.Shop.Domain.Catalog;

namespace Navira.Shop.Application.Catalog
{
    public class ProductAllAttributeEditCommandHandler : CommandHandler, ICommandHandler<ProductAllAttributeEditCommand>
    {
        private readonly IProductWriteRepository _productWriteRepository;
        private readonly IProductAttributeValueWriteRepository _productAttributeValueWriteRepository;
        private readonly IProductAttributeWriteRepository _productAttributeWriteRepository;
        private readonly IProductAttributeOptionWriteRepository _productAttributeOptionWriteRepository;
        private readonly IProductAttributeValuePolicy _productAttributeValuePolicy;
        public ProductAllAttributeEditCommandHandler(IUnitOfWork uow, IProductWriteRepository productWriteRepository, IProductAttributeValueWriteRepository productAttributeValueWriteRepository, IProductAttributeWriteRepository productAttributeWriteRepository, IProductAttributeOptionWriteRepository productAttributeOptionWriteRepository, IProductAttributeValuePolicy productAttributeValuePolicy) : base(uow)
        {
            _productWriteRepository = productWriteRepository;
            _productAttributeValueWriteRepository = productAttributeValueWriteRepository;
            _productAttributeWriteRepository = productAttributeWriteRepository;
            _productAttributeOptionWriteRepository = productAttributeOptionWriteRepository;
            _productAttributeValuePolicy = productAttributeValuePolicy;
        }

        public async Task<IResult> Handle(ProductAllAttributeEditCommand command, CancellationToken cancellationToken = default)
        {
            try
            {
                if (command.ProductId <= 0)
                    return Result.Fail("شناسه کالا معتبر نیست.");

                var product = await _productWriteRepository.Get(command.ProductId);

                if (product == null)
                    return Result.Fail("کالا با این شناسه وجد ندرد");


                var currentValues = await _productAttributeValueWriteRepository.GetAll(x => x.Where(i => i.ProductId == product.Id));

                var attributeIds = command.Attributes
                   .Select(x => x.ProductAttributeId)
                   .Distinct()
                   .ToArray();

                var definitions = await _productAttributeWriteRepository.GetAll(x => x.Where(i => attributeIds.Contains(i.Id)));

                var definitionsById = definitions.ToDictionary(x => x.Id);

                var optionIds = command.Attributes
                                  .SelectMany(x =>
                                      (x.ProductAttributeOptionIds ?? new List<int>())
                                      .Concat(
                                          x.ProductAttributeOptionId.HasValue
                                              ? new[] { x.ProductAttributeOptionId.Value }
                                              : Array.Empty<int>()))
                                  .Distinct()
                                  .ToArray();


                var options = await _productAttributeOptionWriteRepository.GetAll(x => x.Where(i => optionIds.Contains(i.Id)));

                var optionsById = options.ToDictionary(x => x.Id);

                var currentByAttribute = currentValues.ToDictionary(x => x.ProductAttributeId);


                foreach (var item in command.Attributes)
                {
                    if (!definitionsById.TryGetValue(item.ProductAttributeId, out var definition) || definition.IsDeleted)
                        return Result.Fail($"ویژگی با شناسه {item.ProductAttributeId} وجود ندارد.");

                    if (definition.IsVariantAttribute)
                        return Result.Fail($"ویژگی «{definition.Name}» مربوط به تنوع کالا است.");

                    currentByAttribute.TryGetValue(item.ProductAttributeId, out var current);


                    // بررسی:
                    // - نوع و تعداد مقدارها
                    // - محدوده و دقت عدد
                    // - تعلق گزینه به ویژگی
                    // - گزینه‌های تکراری
                    // - حفظ انتخاب غیرفعال قبلی و ممنوعیت انتخاب غیرفعال جدید
                    var validation = _productAttributeValuePolicy.Validate(
                        definition,
                        item,
                        current,
                        optionsById);

                    if (!validation.IsSuccess)
                        return Result.Fail(validation.Error);



                }

                var incomingAttributeIds = attributeIds.ToHashSet();

                // حذف ارتباط‌هایی که دیگر در فرم وجود ندارند.
                foreach (var current in currentValues)
                {
                    if (!incomingAttributeIds.Contains(current.ProductAttributeId))
                    {
                        // حذف نرم والد و فرزندان؛ با ثبت Audit
                        //current.Remove();
                        await _productAttributeValueWriteRepository.Delete(current);
                    }
                }

                foreach (var item in command.Attributes)
                {
                    var definition = definitionsById[item.ProductAttributeId];

                    if (!currentByAttribute.TryGetValue(
                            item.ProductAttributeId,
                            out var entity))
                    {
                        // Factory جدید باید انواع مقدار را پشتیبانی کند.
                        entity = ProductAttributeValue.Create(
                            product.Id,
                            definition.Id,
                            item.ProductAttributeOptionId,
                            item.TextValue,
                            item.NumberValue,
                            item.BooleanValue,
                            item.DateValue);
                        //,
                        //item.ProductAttributeOptionIds ?? new List<int>()); **********************

                        // Add فقط Entity را به ChangeTracker اضافه کند.
                        // اینجا SaveChanges انجام نشود.
                        await _productAttributeValueWriteRepository.Insert(entity);
                    }
                    else
                    {
                        entity.Map(item);
                    }
                }

                // نسخه ارسالی باید به عنوان OriginalValue برای کنترل هم‌زمانی تنظیم شود.
                //_productWriteRepository.SetOriginalRowVersion(
                //    product,
                //    command.RowVersion);

                // والد واقعاً Modified شود تا RowVersion جدید تولید شود.
                // این متد باید ModifiedDate و ModifiedByUserId را با زیرساخت Audit
                // پروژه هماهنگ کند.
                //product.MarkAttributesChanged();

                return Result.Success("اطلاعات با موفقیت ذخیره شد");
            }

            catch (DbUpdateConcurrencyException)
            {
                // در لایه API به HTTP 409 نگاشت شود.
                return Result.Fail(
                    "اطلاعات کالا توسط کاربر دیگری تغییر کرده است؛ دوباره بارگذاری کنید.");
            }

            catch (Exception ex)
            {
                return Result.Fail($"Failed to save {ex.Message}");
            }
        }
    }
}
