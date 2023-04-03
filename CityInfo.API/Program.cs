using CityInfo.API.Data;
using CityInfo.API.Services.CityRepository;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var dbConnection = builder.Configuration["ConnectionStrings:CityInfoConnectonString"];
builder.Services.AddDbContext<CityInfoContext>(opt=>opt.UseSqlite(dbConnection));
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddScoped<ICityInfoRepository,CityInfoRepository>();

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
