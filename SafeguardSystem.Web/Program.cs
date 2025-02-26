using Microsoft.EntityFrameworkCore;
using SafeguardSystem.DAL.UnitOfWork;
using SafeguardSystem.BLL.IServices;
using SafeguardSystem.BLL.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using SafeguardSystem.DAL;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using FirebaseAdmin.Auth;
using System.Security.Claims;
using Microsoft.OpenApi.Models;
using System.Reflection;



namespace SafeguardSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Dependency Injection cho các dịch vụ
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IEmailService, EmailService>();
            builder.Services.AddScoped<IBusinessService, BusinessService>();

            // Cấu hình context database
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<SafeguardDbContext>(options =>
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

            // Cấu hình Firebase
            var firebaseConfig = builder.Configuration.GetSection("FIREBASE_CONFIG").Get<Dictionary<string, string>>();
            var jsonConfig = System.Text.Json.JsonSerializer.Serialize(firebaseConfig);
            var credential = GoogleCredential.FromJson(jsonConfig);

            FirebaseApp.Create(new AppOptions()
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
                var xmlFile = Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml");
                c.IncludeXmlComments(xmlFile);
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter your JWT token without 'Bearer' prefix.",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    BearerFormat = "JWT",
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
                            new string[] {}
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
                        {
                            try
                            {
                                var decodedToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(token);
                                context.Principal = new ClaimsPrincipal(new ClaimsIdentity(decodedToken.Claims.Select(c => new Claim(c.Key, c.Value.ToString())), "firebase"));
                                context.Success();
                            }
                            catch (Exception)
                            {
                                context.Fail("Unauthorized");
                            }
                        }
                    }
                };
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = "https://securetoken.google.com/safeguard-authen",
                    ValidateAudience = true,
                    ValidAudience = "safeguard-authen",
                    ValidateLifetime = true,
                };
            });

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend",
             builder => builder.WithOrigins("http://localhost:5173")
                               .AllowAnyMethod()
                               .AllowAnyHeader()
                               .AllowCredentials());
            });
           

            // Add controllers
            builder.Services.AddControllers();

            //Add IHttpContextAccessor
            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseCors("AllowFrontend");

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
