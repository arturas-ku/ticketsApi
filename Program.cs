using Microsoft.EntityFrameworkCore;
using SupportAPI.Data;
using SupportAPI.Data.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<SupportDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SupportDBDefault")));

builder.Services.AddTransient<IProjectsRepository, ProjectsRepository>();
builder.Services.AddTransient<ITicketsRepository, TicketsRepository>();
builder.Services.AddTransient<ITicketTypesRepository, TicketTypesRepository>();
builder.Services.AddTransient<ITicketCommentsRepository, TicketCommentsRepository>();

builder.Services.AddAutoMapper(typeof(Program).Assembly);

var app = builder.Build();

app.UseRouting();
app.MapControllers();

app.Run();
