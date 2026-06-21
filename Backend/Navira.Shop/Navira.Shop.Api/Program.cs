using Navira.Shop.Core.Web;

var builder = WebApplication.CreateBuilder(args);
builder.ConfigureApplicationServices();
var app = builder.ConfigureApplicationBuilder();
app.Run();
