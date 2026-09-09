using Navira.Shop.Core.Bus;
using Navira.Shop.Core.Mapper;
using Navira.Shop.Core.Persistence;
using Navira.Shop.Core.Results;
using Navira.Shop.Domain.Catalog;

namespace Navira.Shop.Application.Catalog
{
    public class BrandRegisterCommandHandler : CommandHandler, ICommandHandler<BrandRegisterCommand>
    {
        private readonly IBrandWriteRepository _repository;
        public BrandRegisterCommandHandler(IUnitOfWork uow, IBrandWriteRepository repository) : base(uow)
        {
            _repository = repository;
        }

        public async Task<IResult> Handle(BrandRegisterCommand command, CancellationToken cancellationToken = default)
        {
            try
            {
                var entiy = command.Map<Brand>();
                await _repository.Insert(entiy);
                return await Result.SuccessAsync("اطلاعات با موفقیت ثبت شد");
            }
            catch (Exception ex)
            {
                return Result.Fail(ex.Message);
            }
        }
    }
}
