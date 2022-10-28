using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using OrderService.Composers;
using OrderService.Composers.Interfaces;
using OrderService.Repositories;
using OrderService.Repositories.Interfaces;
using OrderService.Validators;
using System.Security.Authentication;
using System.Xml.Xsl;

var builder = WebApplication.CreateBuilder(args);


string connectionString = builder.Configuration.GetConnectionString("DestinationDB");
MongoClientSettings settings = MongoClientSettings.FromUrl(new MongoUrl(connectionString));
settings.SslSettings = new SslSettings() { EnabledSslProtocols = SslProtocols.Tls12 };
builder.Services.AddSingleton<IMongoClient>(c => new MongoClient(settings));
builder.Services.AddScoped(c => c.GetRequiredService<IMongoClient>().StartSession());
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<IInsertOrderComposer, InsertOrderComposer>();
builder.Services.AddTransient<ISearchOrderComposer, SearchOrderComposer>();
builder.Services.AddTransient<ISearchOrderRepository, SearchOrderRepository>();
builder.Services.AddTransient<IInsertOrderRepository, InsertOrderRepository>();
builder.Services.Add(ServiceDescriptor.Transient(typeof(InsertOrdersValidator), typeof(InsertOrdersValidator)));
builder.Services.Add(ServiceDescriptor.Transient(typeof(SearchOrderValidator), typeof(SearchOrderValidator)));
builder.Services.Add(ServiceDescriptor.Transient(typeof(OrderCollectionValidator), typeof(OrderCollectionValidator)));
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
