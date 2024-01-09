
namespace Service.Implementation
{
    public interface IRatesUpdateScheduleService
    {
        void Dispose();
        Task StartAsync(CancellationToken cancellationToken);
        Task StopAsync(CancellationToken cancellationToken);
    }
}