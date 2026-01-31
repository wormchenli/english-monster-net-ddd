using FileService.Infrastructure;
using FileService.Infrastructure.MinioService;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Middlewares.UnitOfWork;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// temporary: register here
builder.Services.Configure<SmbStorageOptions>(builder.Configuration.GetSection("SmbStorage"));
builder.Services.Configure<CloudStorageOptions>(builder.Configuration.GetSection("CloudStorage"));

builder.Services.Configure<MvcOptions>(o =>
{
    o.Filters.Add<UnitOfWorkFilter>();
});

builder.Services.AddDbContext<FileServiceDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("FileServiceDb");
    options.UseSqlServer(connectionString!);
});
new ModuleInitialize().Register(builder.Services);

//

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
