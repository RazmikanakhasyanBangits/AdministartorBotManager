using Grpc.Client.Abstraction;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Service.Implementation
{
    public sealed class RatesUpdateScheduleService : IHostedService, IDisposable
    {
        private static Timer _timer;
        private readonly IServiceScopeFactory _scopeFactory;

        public RatesUpdateScheduleService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory=scopeFactory;
            _timer = new Timer(UpdateRateByScheduleAsync, null, TimeSpan.Zero, TimeSpan.FromHours(1));

        }

        private async void UpdateRateByScheduleAsync(object state)
        {
            using (IServiceScope scope = _scopeFactory.CreateScope())
            {
                IServiceProvider scopedServices = scope.ServiceProvider;
                IRateActionClient rateActionClient = scopedServices.GetRequiredService<IRateActionClient>();
                await rateActionClient.UpdateRatesAsync();
            }

            Console.WriteLine("Scheduled task executed at: " + DateTime.Now);
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
