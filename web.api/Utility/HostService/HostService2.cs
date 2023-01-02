namespace web.api.Utility.HostService
{
    public class HostService2 : BackgroundService
    {
        private int count = 0;
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                Console.WriteLine("Background servcie started ------------>>");
                while (!stoppingToken.IsCancellationRequested)
                {
                    Console.WriteLine($"background service is running {count}");
                    count++;
                    await Task.Delay(1000);
                }

                Console.WriteLine("Background servcie stopped <<------------");
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }
        }
    }
}
