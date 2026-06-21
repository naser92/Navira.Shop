namespace NaviraShop.Core.Mq
{
    public record LogData(string Database = null, string Query = null, TimeSpan? QueryTime = null, string Content = null);
}