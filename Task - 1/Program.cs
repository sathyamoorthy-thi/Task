using System.Net.Http;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PaymentClaimApi.Data;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options => 
{
  options.JsonSerializerOptions.PropertyNamingPolicy = null;
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddCors(options => {
  options.AddPolicy("MyPolicy",a => a.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

#region Configure JwtAuthentication
 builder.Services.AddSwaggerGen(c=>{

  c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo {Title = "Reimbursement" , Version = "v1"});
  c.AddSecurityDefinition("Bearer",new OpenApiSecurityScheme 
  {
    Description = "Jwt Authorization",
    Name = "Authorization",
    In = ParameterLocation.Header,
    Type = SecuritySchemeType.ApiKey,
    Scheme = "Bearer"
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
                              new string[]{}
                            }
  });

});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>{
options.TokenValidationParameters=new Microsoft.IdentityModel.Tokens.TokenValidationParameters
{
    ValidateIssuer = true,
    ValidateAudience = true,
    ValidateLifetime = true,
    ValidateIssuerSigningKey = true,
    ValidIssuer = builder.Configuration["Jwt:Issuer"],
    ValidAudience = builder.Configuration["Jwt:Audience"],
    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:key"]))

};
});

#endregion


#region Configure Database

  var DefaultConnection = builder.Configuration.GetConnectionString("DefaultConnection");
  builder.Services.AddDbContext<ApplicationDbContext>(options=>options.UseSqlServer(DefaultConnection));

#endregion

#region  Configure Serilog

builder.Host.UseSerilog((context, config)=> {

  config.WriteTo.File("Logs/log.txt",rollingInterval:RollingInterval.Day);

  if(context.HostingEnvironment.IsProduction() == false)
  {
    config.WriteTo.Console();
  }

 });

#endregion



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "PaymentClaimApiToken v1"));
}

 /*app.UseEndpoints(endpoints =>
    {
        endpoints.MapControllerRoute(
            name: "Create",
            pattern: "CreateId",
            defaults: new { controller = "Details", action = "Create" }
        );

        // Other conventional routes...

        endpoints.MapDefaultControllerRoute();
    });*/

app.UseCors(options =>
{
 options.AllowAnyOrigin().
 AllowAnyMethod().
 AllowAnyHeader();
});   

app.UseCors("MyPolicy");
app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();


