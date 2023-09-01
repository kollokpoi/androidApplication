using DiplomApi.Data.Classes;
using DiplomApi.Data.Database;
using DiplomApi.Data.Interfaces;
using DiplomApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using System.Text;
using System.Net.Sockets;
using System.Net;
using DiplomApi.Data.Models;
using System.Text.Json;
using DiplomApi.Data.ViewModels;
using System.Diagnostics;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;
using DiplomApi.Data.TCPModels;
using System.Collections;

internal class Program
{
    public static DatabaseContext context;
    public static IConfiguration Configuration;
    static List<TcpClient> tcpClients = new List<TcpClient>();

    private static void Main(string[] args)
    {

        var builder = WebApplication.CreateBuilder(args);
        Configuration = new ConfigurationBuilder()
           .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
           .Build();

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();


        var key = Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]);
        var validIssuer = Configuration["Jwt:Issuer"];
        var validAudience = Configuration["Jwt:Audience"];


        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    // укзывает, будет ли валидироваться издатель при валидации токена
                    ValidateIssuer = true,
                    // строка, представляющая издателя
                    ValidIssuer = AuthOptions.ISSUER,

                    // будет ли валидироваться потребитель токена
                    ValidateAudience = true,
                    // установка потребителя токена
                    ValidAudience = AuthOptions.AUDIENCE,
                    // будет ли валидироваться время существования
                    ValidateLifetime = true,

                    // установка ключа безопасности
                    IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
                    // валидация ключа безопасности
                    ValidateIssuerSigningKey = true,
                };
            });

        builder.Services.AddAuthorization();
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddHttpContextAccessor();

        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API", Version = "v1" });

            // Добавляем авторизацию в Swagger
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey
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

        builder.Services.AddDbContext<DatabaseContext>(options =>
        {
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
        });
        var app = builder.Build();



        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }


        app.UseHttpsRedirection();


        app.UseAuthentication();
        app.UseAuthorization();
        app.UseMiddleware<UserIdMiddleware>();
        app.MapControllers();

        context = new DatabaseContext(new Microsoft.EntityFrameworkCore.DbContextOptions<DatabaseContext>());
        Reciever();
        app.Run();
    }

    public static async void Reciever()
    {
        IPAddress iPAddress = IPAddress.Parse("192.168.1.4");
        TcpListener tcpListener = new TcpListener(iPAddress,5000);

        tcpListener.Start();

        while (true)
        {
            TcpClient client = await tcpListener.AcceptTcpClientAsync();
            tcpClients.Add(client);
            _ = HandleClientAsync(client);
        }
    }
    static async Task HandleClientAsync(TcpClient client)
    {
        using (NetworkStream stream = client.GetStream())
        {
            byte[] buffer = new byte[1024];


            int bytesRead;
            string receivedString;

            using (MemoryStream memoryStream = new MemoryStream())
            {
                while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    memoryStream.Write(buffer, 0, bytesRead);
                }

                byte[] receivedData = memoryStream.ToArray();
                receivedString = Encoding.UTF8.GetString(receivedData);
            }


            CommandModel? model = new CommandModel();
            model = JsonSerializer.Deserialize<CommandModel>(receivedString);

            if (model!=null)
            {
                if (model.command == "sendMessage")
                {
                    MessageModel? message = null;
                    try
                    {
                        message = JsonConvert.DeserializeObject<MessageModel>(model.jsonData);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex);
                    }
                    

                    Message messageForDb = message.Message;
                    messageForDb.User = context.Users.First(x => x.Id == message.UserId);
                    messageForDb.Chat = context.Chats.First(x => x.Id == message.ChatId);

                    context.Message.Add(messageForDb);
                    try
                    {
                        await context.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex);
                    }
                    
                    CommandModel responce = new CommandModel();
                    responce.command = "recieved sucessfull";
                    byte[] responceBytes = System.Text.UTF8Encoding.UTF8.GetBytes(JsonSerializer.Serialize(responce));
                    await stream.WriteAsync(responceBytes, 0, responceBytes.Length);
                }
            }
        }

        client.Close();
    }
}