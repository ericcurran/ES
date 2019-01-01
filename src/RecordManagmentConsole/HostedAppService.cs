using System.Threading;
using System.Threading.Tasks;
using BusinessLogic;
using Microsoft.Extensions.Hosting;

namespace RecordManagmentConsole
{
    public class HostedAppService : IHostedService
    {
        private readonly AppLogic _app;

        public HostedAppService(AppLogic app)
        {
            _app = app;
        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _app.Run();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}