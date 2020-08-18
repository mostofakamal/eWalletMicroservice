using MassTransit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Lib.IntegrationEvents
{


    public interface IIntegrationEventConsumer<T> : IConsumer<T> where  T : class
    {
    }
}
