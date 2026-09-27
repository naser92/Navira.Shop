namespace Navira.Shop.Application.Media
{
    public sealed class MediaValidationException : Exception
    {
        public string Code { get; }

        public MediaValidationException(string code, string message)
            : base(message)
        {
            Code = code;
        }
    }
}
