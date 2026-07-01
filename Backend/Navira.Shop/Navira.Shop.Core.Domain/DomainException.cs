namespace Navira.Shop.Core.Domain
{
    /// <summary>
    /// Thrown when an aggregate method would violate one of its own invariants
    /// (e.g. reducing stock below zero, issuing an invoice with no lines).
    ///
    /// Distinct from Navira.Shop.Core.Results.ResultException: that one is the bus's
    /// existing flow-control exception (thrown by BusControl on FluentValidation failure).
    /// DomainException signals "the domain model itself refused this operation" and is
    /// caught inside the application-layer command handler's Handle(...) method, then
    /// translated into Result.Fail(ex.Message) — it should never reach BusControl's own
    /// catch block, which exists to roll back the unit of work on unexpected failures.
    /// </summary>
    public class DomainException : Exception
    {
        public DomainException(string message) : base(message) { }
        public DomainException(string message, Exception innerException) : base(message, innerException) { }
    }

}
