using Microsoft.AspNetCore.Builder;
using Ambev.DeveloperEvaluation.Messaging.Configuration;

namespace Ambev.DeveloperEvaluation.IoC.ModuleInitializers;

public class MessagingModuleInitializer : IModuleInitializer
{
    public void Initialize(WebApplicationBuilder builder)
    {
        builder.Services.AddRabbitMQ(builder.Configuration);
        builder.Services.AddMessageProducer();
    }
}