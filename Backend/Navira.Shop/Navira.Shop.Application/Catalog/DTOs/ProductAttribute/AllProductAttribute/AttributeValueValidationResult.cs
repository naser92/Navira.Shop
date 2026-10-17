namespace Navira.Shop.Application.Catalog
{
    public sealed record AttributeValueValidationResult(
     bool IsSuccess,
     string? Error)
    {
        public static AttributeValueValidationResult Success()
            => new(true, null);

        public static AttributeValueValidationResult Fail(string error)
            => new(false, error);
    }
}
