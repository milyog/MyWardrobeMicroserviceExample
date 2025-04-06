using RabbitMQ.Client;
using Newtonsoft.Json;
using System.Text;

namespace MyWardrobe
{
    public class Send
    {
        private const string QueueName = "wardrobeItem_used_events";

        public async void PublishWardrobeItemUsedEvent(Guid id, string category, string subCategory, int wardrobeItemUsage, DateTime lastTimeUsed)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using var connection = await factory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(
                queue: QueueName, 
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null
                );

            var wardrobeItemUsedEvent = new
            {
                Id = id,
                Category = category,
                SubCategory = subCategory,
                LastTimeUsed = lastTimeUsed
            };

            var message = JsonConvert.SerializeObject(wardrobeItemUsedEvent);
            var body = Encoding.UTF8.GetBytes(message);

            await channel.BasicPublishAsync(exchange: string.Empty, routingKey: QueueName, body: body);
        }
    }
}
