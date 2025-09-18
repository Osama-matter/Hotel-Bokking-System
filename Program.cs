using Hotel_Bokking_System.Data;
using Hotel_Bokking_System.Interface;
using Hotel_Bokking_System.Repositry;
using Hotel_Bokking_System.UserApplection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();


// Add  Conection 
builder.Services.AddDbContext<Hotel_dbcontext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CS")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<Hotel_dbcontext>()
    .AddDefaultTokenProviders();


// add  Interface  service  

// Register Repositories in DI
builder.Services.AddScoped<IRoom, RoomsRepository>();
builder.Services.AddScoped<IBooking, BookingRepository>();
builder.Services.AddScoped<iCustomr , CustomarRepository>();
builder.Services.AddScoped<iPayment, PaymentRepository>();
builder.Services.AddScoped<IReview , ReviewsRepository>();

// Cors
// إضافة CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});


// Covert  Enum Choise  to string  not  int  
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // 👇 ده اللي بيخلي الـ Enum يتسلسل كـ String مش رقم
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

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

app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
