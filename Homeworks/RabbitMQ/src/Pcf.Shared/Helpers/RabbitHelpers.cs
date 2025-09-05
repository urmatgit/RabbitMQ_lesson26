using MassTransit;
using MassTransit.RabbitMqTransport;
using Microsoft.Extensions.Configuration;
using Pcf.Shared.Services;
using Pcf.Shared.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace Pcf.Shared.Helpers
{
    public class RabbitHelpers
    {
        /// <summary>
        /// Конфигурирование RMQ.
        /// </summary>
        /// <param name="configurator"> Конфигуратор RMQ. </param>
        /// <param name="configuration"> Конфигурация приложения. </param>
        public static void ConfigureRmq(IRabbitMqBusFactoryConfigurator configurator, IConfiguration configuration)
        {
            var rmqSettings = configuration.Get<ApplicationSettings>().RmqSettings;
            configurator.Host(rmqSettings.Host,
                rmqSettings.VHost,
                h =>
                {
                    h.Username(rmqSettings.Login);
                    h.Password(rmqSettings.Password);
                });
        }

        /// <summary>
        /// регистрация эндпоинтов
        /// </summary>
        /// <param name="configurator"></param>
        public static void RegisterEndPoints<T>(IRabbitMqBusFactoryConfigurator configurator) 
        {
            configurator.ReceiveEndpoint($"masstransit_event_queue_1", e =>
            {
                e.Consumer<T>();
                e.UseMessageRetry(r =>
                {
                    r.Incremental(3, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1));
                });
            });

        }
    }
}
