using BankingSystem.Application.Behaviours;
using BankingSystem.Domain.Interfaces;
using BankingSystem.Domain.Services;
using BankingSystem.Infrastructure.DataAccess;
using BankingSystem.WebAPI.Middlewares;
using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BankingSystem.WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));

            var applicationAssembly = Assembly.Load("BankingSystem.Application");
            builder.Services.AddMediatR(x => x.RegisterServicesFromAssembly(applicationAssembly));

            builder.Services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();
            builder.Services.AddValidatorsFromAssembly(applicationAssembly);

            builder.Services.AddDbContext(builder.Configuration);
            builder.Services.AddHttpContextAccessor();

            builder.Services.AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.Converters.Add(new StringEnumConverter(typeof(CamelCaseNamingStrategy)));
                }).AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
                });

            #region Domain services

            builder.Services.AddScoped<IAccountDomainService, AccountDomainService>();
            builder.Services.AddScoped<ITransactionDomainService, TransactionDomainService>();
            builder.Services.AddScoped<IUserDomainService, UserDomainService>();

            #endregion

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseExceptionMiddleware();

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            MigrateDatabase(app);

            app.Run();
        }

        public static void MigrateDatabase(IHost host)
        {
            using var scope = host.Services.CreateScope();

            var db = scope.ServiceProvider.GetRequiredService<BankingDbContext>();
            db.Database.Migrate();
        }
    }
}
