using System.Reflection;
using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Serialization;
using Amazon.S3;
using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SafeguardSystem.BLL.IServices;
using SafeguardSystem.BLL.Services;
using SafeguardSystem.Common.AWSSettings;
using SafeguardSystem.DAL;
using SafeguardSystem.DAL.IRepositories;
using SafeguardSystem.DAL.UnitOfWork;

namespace SafeguardSystem;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Dependency Injection cho các dịch vụ
        builder.Services.AddHttpClient();
        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
        builder.Services.AddScoped<IAttendenceService, AttendenceService>();
        builder.Services.AddScoped<IAuthService, AuthService>();
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<IEmailService, EmailService>();
        builder.Services.AddScoped<EmailService>();
        builder.Services.AddScoped<IBusinessService, BusinessService>();
        builder.Services.AddScoped<ICheckpointService, CheckpointService>();
        builder.Services.AddScoped<IShiftTypeService, ShiftTypeService>();
        builder.Services.AddScoped<IGuardService, GuardService>();
        builder.Services.AddScoped<ILocationService, LocationService>();
        builder.Services.AddScoped<ISecurityshiftService, SecurityshiftService>();
        builder.Services.AddScoped<ITeamService, TeamService>();
        builder.Services.AddScoped<ILoginRequestService, LoginRequestService>();
        builder.Services.AddScoped<IReportService, ReportService>();
        builder.Services.AddScoped<LoginRequestService>();

        //Cấu hình dịch vụ AWS S3
        builder.Services.Configure<AwsS3Setting>(builder.Configuration.GetSection("AWS"));
        builder.Services.AddScoped<IAWSS3Service, AWSS3Service>();
        builder.Services.AddDefaultAWSOptions(builder.Configuration.GetAWSOptions());
        builder.Services.AddAWSService<IAmazonS3>();

        // Cấu hình context database
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        builder.Services.AddDbContext<SafeguardDbContext>(options =>
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

        // Cấu hình Firebase
        var firebaseConfig = builder.Configuration.GetSection("FIREBASE_CONFIG").Get<Dictionary<string, string>>();
        var jsonConfig = JsonSerializer.Serialize(firebaseConfig);
        var credential = GoogleCredential.FromJson(jsonConfig);

        FirebaseApp.Create(new AppOptions
        {
            Credential = credential
        });


        // Cấu hình Swagger API
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "SafeguardSystem API",
                Version = "v1",
                Description = "API for Safeguard System",
                Contact = new OpenApiContact
                {
                    Name = "Safeguard System",
                    Url = new Uri("https://github.com/Vietanh2703/SafeguardSystemBE.git")
                }
            });
            //Cấu hình note cho Swagger
            var xmlFile = Path.Combine(AppContext.BaseDirectory,
                $"{Assembly.GetExecutingAssembly().GetName().Name}.xml");
            c.IncludeXmlComments(xmlFile);
            c.EnableAnnotations();
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter your JWT token without 'Bearer' prefix.",
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                BearerFormat = "JWT"
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
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

        // Cấu hình xác thực
        builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = async context =>
                    {
                        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                        if (token != null)
                            try
                            {
                                var decodedToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(token);
                                context.Principal = new ClaimsPrincipal(new ClaimsIdentity(
                                    decodedToken.Claims.Select(c => new Claim(c.Key, c.Value.ToString())), "firebase"));
                                context.Success();
                            }
                            catch (Exception)
                            {
                                context.Fail("Unauthorized");
                            }
                    }
                };
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = "https://securetoken.google.com/safeguard-authen",
                    ValidateAudience = true,
                    ValidAudience = "safeguard-authen",
                    ValidateLifetime = true
                };
            });

        // Cấu hình CORS cho frontend
        var allowedOrigins = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>();
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowFrontend",
                policy =>
                {
                    //Thay đổi địa chỉ cấu hình frontend để backend kết nối được tới frontend
                    policy.WithOrigins(allowedOrigins)
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                });
        });


        // Add controllers
        builder.Services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(
                    new JsonStringEnumConverter()); // Convert all enums to strings
            });

        //Add IHttpContextAccessor
        builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();


        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseRouting();
        app.UseCors();
        app.UseHttpsRedirection();
        app.UseCors("AllowFrontend");
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}