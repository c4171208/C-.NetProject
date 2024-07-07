using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using QueueManagmentApi;
using QueueManagmentApi.Attributes;
using QueueManagmentBL;
using QueueManagmentBL.Interfaces;
using QueueManagmentBL.Servicers;
using QueueManagmentDB.EF.Contexts;
using QueueManagmentDB.Interfaces;
using QueueManagmentDB.Servicers;
using QueueManagmentEntites;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
builder.UseSerilog();

// Add services to the container.
//HttpContext- ����� ������ ������ ��� ������� 
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IUserBl, UserBl>();
builder.Services.AddScoped<IUserDB, UserDB>();
builder.Services.AddScoped<IQueBl, QueBl>();
builder.Services.AddScoped<IQueDB, QueDB>();
builder.Services.AddScoped<ConflictAttribute>();
builder.Services.AddAutoMapper(typeof(MannegerMap));
builder.Services.Configure<AppSettings>(builder.Configuration);//����� ����� ���� ������ �� �� ����� �������
AppSettings appSetting = builder.Configuration.Get<AppSettings>();//���� ������ ��� ������ ������ ��� ���(����� �������� �
//����� ���� ���������� ���� :JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        //������ �� ����������
        options.TokenValidationParameters = new TokenValidationParameters
        {
            //����� �� �� ����� �� �����
            ValidateIssuer = true,
            ValidateAudience = true,
            //����� ���� ���� �� �����
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            //�� ���� �� �����-
            ValidIssuer = appSetting.Jwt.Issuer,
            ValidAudience = appSetting.Jwt.Audience,
            //���� ����� �� �-
            //jwt
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appSetting.Jwt.SecretKey)),
            //�� ����� �� ����� ����� ���� ���� �-5 ����
            //��� ����� ������
            ClockSkew = TimeSpan.Zero
        };
        //������ ��� ����� �-
        //jwt
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                //����� ��������� ����� �� ����� ���� ����- �
                //auteriton by header 
                //"���� ��� ����� �� �� �� �� �������� ���:"�����
                context.Token = context.Request.Cookies[CookiesKeys.AccessToken];
                return Task.CompletedTask;
            }
        };
    });

//(DB
builder.Services.AddDbContext<QueueManagmentContext>(o =>
{
    o.UseSqlServer(appSetting.ConnectionStrings.QueueManagment);
});
builder.Services.AddCors(option =>
{
    option.AddPolicy("CorsPolicy", builder =>
    builder.WithOrigins("http://locallhost:3000")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials()
    );
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
//����� �� �������� ���- ����� ��
app.UseAuthorization();
app.UseCors("CorsPolicy");
app.MapControllers();

app.Run();
