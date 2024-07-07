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
//HttpContext- הטמעת אפשרות להשתמש בכל הפרויקט 
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IUserBl, UserBl>();
builder.Services.AddScoped<IUserDB, UserDB>();
builder.Services.AddScoped<IQueBl, QueBl>();
builder.Services.AddScoped<IQueDB, QueDB>();
builder.Services.AddScoped<ConflictAttribute>();
builder.Services.AddAutoMapper(typeof(MannegerMap));
builder.Services.Configure<AppSettings>(builder.Configuration);//בשביל שיהיה אפשר להשתמש אם זה במהלך הפרויקט
AppSettings appSetting = builder.Configuration.Get<AppSettings>();//צורה יחודית כדי שיוכלו להשתמש בדף הזה(בשביל ההתחברות ל
//הוספת שיטת אוטניקצטיה מסוג :JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        //הגדרות על אוטניקצטיה
        options.TokenValidationParameters = new TokenValidationParameters
        {
            //הגדרה על מי שייצר את הטוקן
            ValidateIssuer = true,
            ValidateAudience = true,
            //לבדוק אורך חיים של הטוקן
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            //מי שיצר את הטוקן-
            ValidIssuer = appSetting.Jwt.Issuer,
            ValidAudience = appSetting.Jwt.Audience,
            //מפתח הסודי של ה-
            //jwt
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appSetting.Jwt.SecretKey)),
            //אל תתערב לי שתוקף הטוקן יהיה פחות מ-5 דקות
            //נטו בשביל הסטטים
            ClockSkew = TimeSpan.Zero
        };
        //ארועים בעת יצירת ה-
        //jwt
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                //באופן דיפולטיבי התוכן של הטוקן יהיה בתוך- ה
                //auteriton by header 
                //"ןכאן אני אומרת לו כך את זה מהעוגיות בשם:"כלשהו
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
//תפעיל את המידלוור הזה- תשתמש בו
app.UseAuthorization();
app.UseCors("CorsPolicy");
app.MapControllers();

app.Run();
