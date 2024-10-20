var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Middleware to handle exceptions and development-specific settings
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();  // Enables detailed error pages in development
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Optional: Comment out if HTTPS isn't configured locally
// app.UseHttpsRedirection();

app.UseAuthorization();

// Map controller routes directly
app.MapControllers();

app.Run();
