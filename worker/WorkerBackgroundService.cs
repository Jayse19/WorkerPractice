using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using shared;

namespace worker
{
       public class WorkerBackgroundService: BackgroundService
    {
        private readonly IServer server;
        private readonly IConfiguration config;
        private readonly WorkerState workerState;
        private readonly ILogger logger;
        private readonly IHostApplicationLifetime lifetime;

        public WorkerBackgroundService(IServer server, IConfiguration config, WorkerState workerState, ILogger<WorkerBackgroundService> logger, IHostApplicationLifetime lifetime)
        {
            this.server = server;
            this.config = config;
            this.workerState = workerState;
            this.logger = logger;
            this.lifetime = lifetime;

            logger.LogInformation("In WorkerBackgroundService Constructor");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            logger.LogInformation("In WorkerBackgroundService.ExecuteAsync()");
            
            var isStarted = await WaitForAppStartup(lifetime, stoppingToken);
            if(!isStarted)
            {
                return;
            }
            await enlistWithBoss();
        }
         static async Task<bool> WaitForAppStartup(IHostApplicationLifetime lifetime, CancellationToken stoppingToken)
        {
            var startedSource = new TaskCompletionSource();
            lifetime.ApplicationStarted.Register(() => startedSource.SetResult());
            var cancelledSource = new TaskCompletionSource();
            stoppingToken.Register(() => cancelledSource.SetResult());

            Task completedTask = await Task.WhenAny(startedSource.Task, cancelledSource.Task);

            // If the completed tasks was the "app started" task, return true, otherwise false
            return completedTask == startedSource.Task;
        }

        private async Task enlistWithBoss()
        {
            logger.LogInformation("Enlist with boss...");
            var addressFeature = server.Features.Get<IServerAddressesFeature>();
            var httpAddress = addressFeature.Addresses.First(a => a.Contains("http://"));
            var uri = new Uri(httpAddress);
            var host = uri.Host;
            var port = uri.Port;

            var enlistRequest = new EnlistRequest(host, port);
            var httpClient = new HttpClient();
            var token = await httpClient.PostAsJsonAsync($"{config["BOSS"]}/enlist", enlistRequest);
            workerState.Token = await token.Content.ReadAsStringAsync();
            logger.LogInformation("Your Token is: {token}", workerState.Token);
        }
    }
}