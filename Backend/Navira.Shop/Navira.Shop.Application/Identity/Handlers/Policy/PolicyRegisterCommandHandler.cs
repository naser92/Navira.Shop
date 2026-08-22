using Navira.Shop.Core.Bus;
using Navira.Shop.Core.Persistence;
using Navira.Shop.Core.Results;
using Navira.Shop.Domain.Identity;
using System.Diagnostics;


namespace Navira.Shop.Application.Identity.Handlers.Policy
{
    public class PolicyRegisterCommandHandler : CommandHandler, ICommandHandler<PolicyRegisterCommand>
    {
        private readonly IPolicyWriteRepository _policyWriteRepository;
        public PolicyRegisterCommandHandler(IUnitOfWork uow, IPolicyWriteRepository policyWriteRepository) : base(uow)
        {
            _policyWriteRepository = policyWriteRepository;
        }

        public async Task<IResult> Handle(PolicyRegisterCommand command, CancellationToken cancellationToken = default)
        {
            try
            {
                var p = await _policyWriteRepository.Get(X => X.Name == command.Name);

                if (p != null)
                    return await Result.FailAsync("نام تکراری است");

                var policy = Domain.Identity.Policy.Create(
                    command.Name,
                    command.Title,
                    command.Description
                    );


                Debug.Assert(policy != null);
                Debug.Assert(!string.IsNullOrWhiteSpace(policy.Name));
                Debug.Assert(!string.IsNullOrWhiteSpace(policy.Title));

                Debug.WriteLine($"Policy.Name = {policy.Name}");
                Debug.WriteLine($"Policy.Title = {policy.Title}");


                await _policyWriteRepository.Insert(policy);

                return await Result.SuccessAsync("اطلاعات با موفقیت ثبت شد");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
