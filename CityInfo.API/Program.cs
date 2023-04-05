using CityInfo.API.Data;
using CityInfo.API.Services.CityRepository;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

/*
    Support for the xml input and output
    insted of Json we add the xml input and output
    in the builder.Services.AddController()
    but the entities has to have parameterless constructor
*/


builder.Services.AddControllers(op=>{
    op.ReturnHttpNotAcceptable = true;
}).AddXmlDataContractSerializerFormatters();




builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var dbConnection = builder.Configuration["ConnectionStrings:CityInfoConnectonString"];
builder.Services.AddDbContext<CityInfoContext>(opt=>opt.UseSqlite(dbConnection));
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddScoped<ICityInfoRepository,CityInfoRepository>();
builder.Services.AddSingleton<FileExtensionContentTypeProvider>(); // now you can inject this
var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.MapControllers();
app.Run();
