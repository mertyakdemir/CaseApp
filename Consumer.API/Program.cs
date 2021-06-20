using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Consumer.API
{
    public class Program
    {
        public static bool EmailSend(string email)
        {
            MailMessage mailMessage = new MailMessage();
            SmtpClient smtpClient = new SmtpClient();

            mailMessage.From = new MailAddress("caserefapp@gmail.com");
            mailMessage.To.Add(email);
            mailMessage.Subject = "Test Email";
            mailMessage.Body = "This is an example email content";
            mailMessage.IsBodyHtml = true;

            smtpClient.Host = "smtp.gmail.com";
            smtpClient.Port = 587;
            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new System.Net.NetworkCredential("caserefapp@gmail.com", "Test.1234");
            smtpClient.Send(mailMessage);

            return true;

        }
        public static void Main(string[] args)
        {
            bool result = false;
            var host = CreateHostBuilder(args).Build();

            var factory = new ConnectionFactory
            {
                Uri = new Uri("amqps://rmvawcvo:xBrOAZlvrZX_MAvUYvMyKEqIz6pWVOPp@snake.rmq2.cloudamqp.com/rmvawcvo")
            };

            using var connection = factory.CreateConnection();

            using var channel = connection.CreateModel();

            channel.ExchangeDeclare("user-exchange", ExchangeType.Direct, true, false, null);

            channel.QueueBind(queue: "Email", exchange: "user-exchange", "SendEmail");

            channel.BasicQos(0, 1, false);

            var consumer = new EventingBasicConsumer(channel);

            channel.BasicConsume("Email", false, consumer);

            consumer.Received += (model, ea) =>
            {
                var messageBody = Encoding.UTF8.GetString(ea.Body.ToArray());

                ConsumerModel createUserCommand = JsonConvert.DeserializeObject<ConsumerModel>(messageBody);

                result = EmailSend(createUserCommand.Email);

                channel.BasicAck(ea.DeliveryTag, false);
            };

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
