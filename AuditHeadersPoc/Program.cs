using AuditHeadersPoc.OpenApi;
using AuditHeadersPoc.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => {
    options.DocumentFilter<AddAuditHeadersDocumetFilter>();
});

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<IAuditHeaderService, AuditHeaderService>();
builder.Services.AddScoped<IAuditHeaderLoggerService, AuditHeaderLoggerService>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();
