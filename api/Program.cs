using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton(new KafkaProducer("kafka:29092", "topic"));

var app = builder.Build();

app.MapGet("/", async (KafkaProducer producer) =>
{
    string message = DateTime.Now.ToString();
    await producer.SendMessageAsync(message);
    return Results.Ok($"Sent message: {message}");
});

app.Run();