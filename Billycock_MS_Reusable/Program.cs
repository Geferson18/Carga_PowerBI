using Billycock_MS_Reusable.Service;
using Billycock_MS_Reusable.Repositories.Interfaces;
using Billycock_MS_Reusable.Repositories.Repositories;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Billycock_MS_Reusable.Repositories.Utils.Common;
using Microsoft.AspNetCore.Authentication;
using Billycock_MS_Reusable.Repositories.Utils.TokenSwagger;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
SqlConnectionStringBuilder builder_Billycock = new SqlConnectionStringBuilder();
if (Environment.GetEnvironmentVariable("Server") == "SERVER" || Environment.GetEnvironmentVariable("Server") == null)
{
    //DESARROLLO
    Environment.SetEnvironmentVariable("Server", @"FRIDAY");                          //CASA
    //Environment.SetEnvironmentVariable("Server", @"billycock.database.windows.net");    //AZURE
    Environment.SetEnvironmentVariable("UserId", "sa");                               //CASA
    //Environment.SetEnvironmentVariable("UserId", "vlarosa");                            //AZURE
    Environment.SetEnvironmentVariable("Password", "Nayjuw+29");                        //CASA
    Environment.SetEnvironmentVariable("Database_Billycock", "Billycock_Desarrollo");   //CASA
    //Environment.SetEnvironmentVariable("Database_Billycock", "Billycock_Produccion");   //AZURE
}
builder_Billycock = new SqlConnectionStringBuilder()
{
    DataSource = Environment.GetEnvironmentVariable("Server"),
    InitialCatalog = Environment.GetEnvironmentVariable("Database_Billycock"),
    UserID = Environment.GetEnvironmentVariable("UserId"),
    Password = Environment.GetEnvironmentVariable("Password"),
    ApplicationName = "Billycock",
    Encrypt = true,
    MultipleActiveResultSets = true
};
if (builder_Billycock.UserID != "sa")
{
    builder_Billycock.PersistSecurityInfo = false;
    builder_Billycock.TrustServerCertificate = false;
}
else
{
    builder_Billycock.PersistSecurityInfo = true;
    builder_Billycock.TrustServerCertificate = true;
}
builder.Services.AddDbContext<BillycockServiceContext>(options =>
                {
                    options.UseSqlServer(builder_Billycock.ConnectionString,
                    options => options.EnableRetryOnFailure());
                    options.EnableSensitiveDataLogging();
                    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                });
builder.Services.AddScoped<ILoginRepository, LoginRepository>();

builder.Services.AddScoped<ICommonRepository, CommonRepository>();

builder.Services.AddCors(options => options.AddPolicy("AllowWebApp",
                builder => builder
                .AllowAnyMethod()
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .SetPreflightMaxAge(TimeSpan.FromSeconds(2520))));

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Api Reusable", Version = "v1" });
    c.AddSecurityDefinition("basic", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "basic",
        In = ParameterLocation.Header,
        Description = "Basic Authorization header using the Bearer scheme."
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "basic"
                                }
                            },
                            new string[] {}
                    }
                });
});
builder.Services.AddAuthentication("BasicAuthentication")
.AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowWebApp");

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

#pragma warning disable ASP0014 // Suggest using top level route registrations
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});
#pragma warning restore ASP0014 // Suggest using top level route registrations
app.UseSwagger();

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Billycock");
});

app.Run();
