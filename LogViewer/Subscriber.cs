public class Subscriber
{
    channel.ExchangeDeclare(_exchangeName, ExchangeType.fanout);

    var queueName = tempQueue.QueueName;

    channel.QueueBind(queue: queueName,
                      exchange: _exchangeName,
                      routingKey: "");
}