using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Utitlity.Nuget.Packages.RabbitMq
{
    /* Nuget Package Dependency: RabbitMQ.Client */

    /// <summary>
    /// Publish & Consume RabbitMq messages. Setup RabbitMq message broker & create Exchange & Queues are prerequisit.
    /// </summary>
    public class RabbitMqTests
    {
        [Fact]
        public void ShouldPublishMessage()
        {
            var factory = new ConnectionFactory
            {
                UserName = "guest",
                Password = "guest",
                HostName = "localhost",
                VirtualHost = "/"
            };

            const string queue = "test_queie";
            const string exchange = "test_exchange";

            using IConnection con = factory.CreateConnection();
            using IModel channel = con.CreateModel();

            const string message = "Hello Rabbit MQ!!!!!!!!!!";
            byte[] messageBytes = System.Text.Encoding.UTF8.GetBytes(message);
            IBasicProperties props = channel.CreateBasicProperties();
            props.ContentType = "text/plain";
            props.DeliveryMode = 2;

            channel.BasicPublish(exchange, "", false, props, messageBytes);
        }

        [Fact]
        public void ShouldConsumeMessage()
        {
            const string queue = "test_queue";
            var factory = new ConnectionFactory
            {
                Uri = new Uri("amqp://guest:guest@localhost:5672/")
            };

            IConnection con = factory.CreateConnection();
            IModel channel = con.CreateModel();

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += (object sender, BasicDeliverEventArgs e) =>
            {
                byte[] dataByte = e.Body;
                string data = System.Text.Encoding.UTF8.GetString(dataByte);

                channel.BasicAck(e.DeliveryTag, true);
                //channel.BasicReject(90, false);
                //channel.BasicNack(90, false, false);
            };

            channel.BasicConsume(queue, false, "90", false, false, null, consumer);

            System.Threading.Thread.Sleep(5000);
        }
    }
}
