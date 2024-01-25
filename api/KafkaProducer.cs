using Confluent.Kafka;
using System;
using System.Threading.Tasks;

public class KafkaProducer
{
    private readonly string _bootstrapServers;
    private readonly string _topic;

    public KafkaProducer(string bootstrapServers, string topic)
    {
        _bootstrapServers = bootstrapServers;
        _topic = topic;
    }

    public async Task SendMessageAsync(string message)
    {
        var config = new ProducerConfig { BootstrapServers = _bootstrapServers };

        using var producer = new ProducerBuilder<Null, string>(config).Build();
        try
        {
            var result = await producer.ProduceAsync(_topic, new Message<Null, string> { Value = message, Timestamp = Timestamp.Default });
            Console.WriteLine($"Message '{message}' sent to {result.TopicPartitionOffset}");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error encountered: {e.Message}");
        }
    }
}