using Newtonsoft.Json;
using Serilog;
using VisaLib;
using VisaLib.Helpers;
using VisaLib.Models;
using COFDS = VisaLib.Models.COFDS;

var builder = WebApplication.CreateBuilder(args);
#region Services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IVisaService, VisaService>();
builder.Services.AddScoped<IHelpers, Helpers>();
// remove default logging providers
builder.Logging.ClearProviders();
// Serilog configuration		
var logger = new LoggerConfiguration()
    .WriteTo.Console()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();
// Register Serilog
builder.Logging.AddSerilog(logger);
// Get the current configuration file.

#endregion Services
var app = builder.Build();
/*if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}*/
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
#region API Endpoints
app.MapGet("/TestVisa", async (ILoggerFactory loggerFactory, IVisaService visaService) =>
{
    var logger = loggerFactory.CreateLogger("TestVisa");
    logger.LogError("Visa Test Request Init");
    var result = await visaService.HelloWordVisaAsync();
    logger.LogError("Res: {0}", JsonConvert.SerializeObject(result));
    logger.LogError("Visa Test Request Ended");
    return result;
})
.WithName("TestVisa");
app.MapPost("/COFR", async (COFDS.ApiRequest request, ILoggerFactory loggerFactory, IVisaService visaService) =>
{
    var logger = loggerFactory.CreateLogger("COFR");
    logger.LogError("COFR Request Init");
    logger.LogError("Req: {0}", JsonConvert.SerializeObject(request));
    var result = await visaService.getVisaCOFR(request);
    logger.LogError("Res: {0}", JsonConvert.SerializeObject(result));
    logger.LogError("COFR Request Ended");
    return result;
})
.WithName("COFR");
#endregion API Endpoints
app.Run();