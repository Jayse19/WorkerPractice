using shared;

namespace boss
{
    public class BossLogic
    {
        private readonly ILogger<BossLogic> logger;
        private readonly IConfiguration config;
        private readonly HttpClient httpClient;
        private List<Cell> board;

       public BossLogic(ILogger<BossLogic> logger, IConfiguration config)
       {
        this.logger = logger;
        this.httpClient = new HttpClient();
        this.config = config;
       }
        internal async Task StartRunning(string password)
        {
            logger.LogInformation("BossLogic got call to start running");

            if (password != config["PASSWORD"])
            {
                logger.LogWarning("Invalid Password");
                return;
            }
            var server = config["SERVER"] ?? "https://hungrygame.azurewebsites.net";
            board = await httpClient.GetFromJsonAsync<List<Cell>>($"{server}/board");
        }

    }


}