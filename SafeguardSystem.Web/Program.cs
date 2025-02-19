using Microsoft.EntityFrameworkCore;
using SafeguardSystem.DAL.DBContext;
using SafeguardSystem.DAL.UnitOfWork;
using SafeguardSystem.BLL.IServices;
using SafeguardSystem.BLL.Services;

namespace SafeguardSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Dependency Injection cho các dịch vụ
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IUnitOfWorkFactory, UnitOfWorkFactory>();
            builder.Services.AddScoped<IBusinessService, BusinessService>();

            // Cấu hình context database
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<SafeguardDbContext>(options =>
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString), b => b.MigrationsAssembly("DAL")));

            builder.Services.AddHttpContextAccessor();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Add authorization services
            builder.Services.AddAuthorization();

            // Add controllers
            builder.Services.AddControllers();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
