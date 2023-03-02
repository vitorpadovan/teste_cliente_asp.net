using AutoMapper;
using CadastroClienteBff.Config;
using CadastroClienteBff.Config.Exceptions;
using CadastroClienteBff.Controllers.Contracts.Request;
using CadastroClienteBff.Controllers.Contracts.Response;
using CadastroClienteBff.Database;
using CadastroClienteBff.Model;

var builder = WebApplication.CreateBuilder(args);

var config = new AutoMapper.MapperConfiguration(cfg =>
{
    cfg.CreateMap<Cliente, SalvarClienteRequest>();
    cfg.CreateMap<SalvarClienteRequest, Cliente>();
    cfg.CreateMap<SalvarClienteResponse, Cliente>();
    cfg.CreateMap<Cliente, SalvarClienteResponse>();
});
var mapper = config.CreateMapper();
builder.Services.AddSingleton(mapper);

builder.Services.AddControllers(options =>{
        options.Filters.Add<HttpResponseExceptionFilter>();
        //options.Filters.Add<ValidationFilter>();
    }
).AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter()));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ContextoBanco>();
builder.Services.AddCors();

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

app.UseAuthorization();

app.MapControllers();

app.UseCors(x =>x.AllowAnyMethod()
.AllowAnyHeader()
.SetIsOriginAllowed(origin => true)
.AllowCredentials());

app.Run();
