using System.Text;
using CityInfo.API.Data;
using CityInfo.API.Services.CityRepository;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

/*
    Support for the xml input and output
    insted of Json we add the xml input and output
    in the builder.Services.AddController()
    but the entities has to have parameterless constructor
*/


builder.Services.AddControllers(op=>{
    op.ReturnHttpNotAcceptable = true;
})
.AddNewtonsoftJson()
.AddXmlDataContractSerializerFormatters();




builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

builder.Services.AddSwaggerGen(c =>
{
    //c.SwaggerDoc("v1", new OpenApiInfo { Title = "TangWeb_Api", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please Bearer and then token in the field",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                   {
                     new OpenApiSecurityScheme
                     {
                       Reference = new OpenApiReference
                       {
                         Type = ReferenceType.SecurityScheme,
                         Id = "Bearer"
                       }
                      },
                      new string[] { }
                    }
                });
});







var dbConnection = builder.Configuration["ConnectionStrings:CityInfoConnectonString"];
builder.Services.AddDbContext<CityInfoContext>(opt=>opt.UseSqlite(dbConnection));
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddScoped<ICityInfoRepository,CityInfoRepository>();
builder.Services.AddSingleton<FileExtensionContentTypeProvider>(); // now you can inject this


builder.Services.AddAuthentication("Bearer")
.AddJwtBearer(opt=>{
            opt.TokenValidationParameters = new(){
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = builder.Configuration["Authentication:Issuer"],
                ValidAudience = builder.Configuration["Authentication:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Authentication:SecretKey"]))
            };

            }
);






var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseRouting();

// here you need this authentiaction
// before authorization
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
