namespace Navira.Shop.Core.Dapper
{
    public class DapperServiceOptions
    {
        public DapperServiceOptions(string connectionString, int? commandTimeOut)
        {
            ConnectionString = connectionString;
            CommandTimeOut = commandTimeOut;
        }

        public string ConnectionString { get; set; }


        public int? CommandTimeOut { get; set; }
    }
}
