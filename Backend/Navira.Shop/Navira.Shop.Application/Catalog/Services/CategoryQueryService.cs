using Microsoft.EntityFrameworkCore;
using Navira.Shop.Core.Extensions;
using Navira.Shop.Core.Mapper;
using Navira.Shop.Core.Results;
using Navira.Shop.Core.ViewModels;

namespace Navira.Shop.Application.Catalog
{
    public class CategoryQueryService : ICategoryQueryService
    {

        #region variables

        private readonly ICategoryQueryRepository _repository;

        #endregion

        #region constructors

        public CategoryQueryService(ICategoryQueryRepository categoryRepository)
        {
            _repository = categoryRepository;
        }

        #endregion

        #region base methods

        public async Task<IResult<TResult>> Get<TResult>(int id)
        {

            return await _repository.Get<TResult>(x => x.Id.Equals(id)).ResultAsync();

        }

        public async Task<IResult<object>> Get<T>(GridParameters parameters)//SortedGridParameters parameters
        {

            var query = _repository.Table;
            var result = await query.ProjectTo<T>().GridAsync(parameters)
                .ResultAsync();

            return result;
        }

        public async Task<IResult<IList<T>>> Get<T>()
        {

            var query = _repository.Table;//.OrderBy(x => x.Priority);

            var result = await query.ProjectTo<T>().ToListAsync()
                .ResultAsync();

            return result;
        }


        #endregion
        public async Task<bool> IsExisitSlug(string slug) =>
            await _repository.Table.AnyAsync(x => x.Slug == slug);

        public async Task<bool> IsExisitName(string name) =>
            await _repository.Table.AnyAsync(x => x.Slug == name);

        public async Task<List<CategorySortListDto>> GetSort()
        {
            var allCategory = await _repository.GetAll<CategoryListDto>();
            return BuildCategoryTree(allCategory.ToList());
        }

        private static List<CategorySortListDto> BuildCategoryTree(List<CategoryListDto> flatList)
        {
            if (flatList == null || !flatList.Any())
                return new List<CategorySortListDto>();

            // دیکشنری برای دسترسی سریع به نودها با Id
            var lookup = flatList.ToDictionary(
                x => x.Id,
                x => new CategorySortListDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Slug = x.Slug,
                    ParentCategoryId = x.ParentCategoryId,
                    TaxCategoryId = x.TaxCategoryId,
                    IsActive = x.IsActive,
                    Child = new List<CategorySortListDto>()
                });

            var roots = new List<CategorySortListDto>();

            foreach (var item in flatList)
            {
                var node = lookup[item.Id];

                if (item.ParentCategoryId == null)
                {
                    // ریشه
                    roots.Add(node);
                }
                else if (lookup.TryGetValue(item.ParentCategoryId.Value, out var parent))
                {
                    // اضافه کردن به فرزندان پدر
                    parent.Child.Add(node);
                }
                // اگر ParentCategoryId وجود داشته باشد ولی پدر پیدا نشود، می‌توانید تصمیم بگیرید
                // که آن را به عنوان ریشه در نظر بگیرید یا نادیده بگیرید
            }

            return roots;
        }
    }

}

