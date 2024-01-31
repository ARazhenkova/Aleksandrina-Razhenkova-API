using EmployeesApi;
using EmployeesApi.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSystemServices();
builder.Services.AddCustomServices();
builder.Services.AddJwtServices(builder.Configuration);
builder.Services.AddOpenApiServices();

builder.Logging.AddNewLogger(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseMiddleware<HttpMessagesLoggingMiddleware>();

app.Run();
