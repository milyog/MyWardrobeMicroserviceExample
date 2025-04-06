using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using Newtonsoft.Json;
using MyWardrobeStatistics.Persistence;
using MyWardrobeStatistics.Models;
using MyWardrobeStatistics.Controllers;
using Microsoft.EntityFrameworkCore;

namespace MyWardrobeStatistics
{
    public class Receive(IServiceProvider serviceProvider)
    {
        private const string QueueName = "wardrobeItem_used_events";
        private readonly IServiceProvider _serviceProvider = serviceProvider;

        public async Task ListenForWardrobeItemUsedEvent()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            var connection = await factory.CreateConnectionAsync();
            var channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(
                queue: QueueName,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null
                );

            var consumer = new AsyncEventingBasicConsumer(channel);

            consumer.ReceivedAsync += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var wardrobeItemUsedEvent = JsonConvert.DeserializeObject<WardrobeItemStatistics>(message);

                using (var scope = _serviceProvider.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<MyWardrobeStatisticsDbContext>();
                    var itemAlreadyExists = await context.WardrobeItemsStatistics.FirstOrDefaultAsync(x => x.Id == wardrobeItemUsedEvent.Id);

                    if (itemAlreadyExists is null)
                    {
                        var newWardrobeItemStatisticsEvent = new WardrobeItemStatistics
                        {
                            Id = wardrobeItemUsedEvent.Id,
                            Category = wardrobeItemUsedEvent.Category,
                            Subcategory = wardrobeItemUsedEvent.Subcategory,
                            WardrobeItemUsage = wardrobeItemUsedEvent.WardrobeItemUsage,
                            LastTimeUsed = wardrobeItemUsedEvent.LastTimeUsed
                        };

                        context.WardrobeItemsStatistics.Add(newWardrobeItemStatisticsEvent);
                    }
                    else
                    {
                        itemAlreadyExists.WardrobeItemUsage = wardrobeItemUsedEvent.WardrobeItemUsage;
                        itemAlreadyExists.LastTimeUsed = wardrobeItemUsedEvent.LastTimeUsed;

                        context.WardrobeItemsStatistics.Update(itemAlreadyExists);
                    }

                    await context.SaveChangesAsync();
                }

                await channel.BasicAckAsync(ea.DeliveryTag, multiple: false);
            };

            await channel.BasicConsumeAsync(QueueName, autoAck: false, consumer: consumer);

        }
    }   
}

