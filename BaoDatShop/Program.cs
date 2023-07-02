using BaoDatShop.DTO;
using BaoDatShop.DTO.AccountRequest;
using BaoDatShop.Model.Context;
using BaoDatShop.Responsitories;
using BaoDatShop.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using System.Net.NetworkInformation;
using System.Text;


var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;
// Add services to the container.

builder.Services.AddControllers();
//

//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("AllowOrigin", options => options.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
//});
//builder.Services.AddControllersWithViews().AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore)
//    .AddNewtonsoftJson(options=>options.SerializerSettings.ContractResolver=new DefaultContractResolver());

builder.Services.AddCors();
//Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//
builder.Services.AddTransient<IAccountResponsitories, AccountResponsitories>();
builder.Services.AddTransient<IAccountService, AccountService>();

builder.Services.AddTransient<ICartResponsitories, CartResponsitories>();
builder.Services.AddTransient<ICartService, CartService>();

builder.Services.AddTransient<IInvoiceDetailResponsitories, InvoiceDetailResponsitories>();
builder.Services.AddTransient<IInvoiceDetailService, InvoiceDetailService>();

builder.Services.AddTransient<IInvoiceService, InvoiceService>();
builder.Services.AddTransient<IInvoiceResponsitories, InvoiceResponsitories>();

builder.Services.AddTransient<INewDetailResponsitories, NewDetailResponsitories>();
builder.Services.AddTransient<INewDetailService, NewDetailService>();

builder.Services.AddTransient<INewsResponsitories, NewsResponsitories>();
builder.Services.AddTransient<INewsService, NewsService>();

builder.Services.AddTransient<IProductResponsitories, ProductResponsitories>();
builder.Services.AddTransient<IProductService, ProductService>();

builder.Services.AddTransient<IProductTypeResponsitories, ProductTypeResponsitories>();
builder.Services.AddTransient<IProductTypeService, ProductTypeService>();

builder.Services.AddTransient<IReviewResponsitories, ReviewResponsitories>();
builder.Services.AddTransient<IReviewService, ReviewService>();

builder.Services.AddTransient<IAdvertisingPanelResponsitories, AdvertisingPanelResponsitories>();
builder.Services.AddTransient<IAdvertisingPanelService, AdvertisingPanelService>();

builder.Services.AddTransient<IDisscountRespositories, DisscountRespositories>();
builder.Services.AddTransient<IDisscountService, DisscountService>();

builder.Services.AddTransient<IProductSizeResponsitories, ProductSizeResponsitories>();
builder.Services.AddTransient<IProductSizeService, ProductSizeService>();

builder.Services.AddTransient<IImageProductResponsitories, ImageProductResponsitories>();
builder.Services.AddTransient<IImageProductService, ImageProductService>();

builder.Services.AddTransient<IVoucherRespositories, VoucherRespositories>();
builder.Services.AddTransient<IVoucherService, VoucherService>();

builder.Services.AddTransient<IHistoryAccountResponsitories, HistoryAccountResponsitories>();

builder.Services.AddTransient<IFavoriteProductRespositories, FavoriteProductResponsitories>();
builder.Services.AddTransient<IFavoriteProductService, FavoriteProductService>();


builder.Services.AddTransient<IEmailSender, EmailSender>();

// For Enity
builder.Services.AddDbContext<AppDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("LMS")));
// For Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();
// Adding Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
// Adding Jwt Bearer
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = configuration["JWT:ValidAudience"],
        ValidIssuer = configuration["JWT:ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]))
    };
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
//Image
app.UseStaticFiles();
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")),
    RequestPath = "/wwwroot",
});
app.UseHttpsRedirection();

app.UseAuthentication();

app.UseCors(options=>options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.UseAuthorization();

app.MapControllers();

app.Run();
