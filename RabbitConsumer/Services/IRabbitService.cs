using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitConsumer.Services
{
    public interface IRabbitService
    {
        void ConsumeMessage();
    }
}
