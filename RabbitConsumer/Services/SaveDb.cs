using AutoMapper;
using RabbitConsumer.Domain;
using RabbitConsumer.Models;
using RabbitConsumer.Persistance;
using System.Threading;

namespace RabbitConsumer.Services
{
    public class SaveDb : ISaveDb
    {
        private readonly AppDbContext _appDbContext;
        private readonly IMapper _mapper;
        private static readonly AwaitLocker _locker = new AwaitLocker();
        public SaveDb(AppDbContext appDbContext, IMapper mapper) 
        {
            _appDbContext = appDbContext;
            _mapper = mapper;
        }

        public async Task SaveMessage<T>(T message)
        {
            await _locker.LockAsync(async () =>
            {
                switch (message)
                {
                    case LogMessageDTO lm:
                        var logMessage = _mapper.Map<LogMessage>(message);
                        _appDbContext.LogMessage.Add(logMessage);
                        break;
                }

                await _appDbContext.SaveChangesAsync();
            });
        }
    }
}
