using System.Runtime.CompilerServices;

namespace web.api.Utility.HostService
{
    public class HostService1 : BackgroundService
    {
        //https://www.bilibili.com/video/BV1pK41137He?p=152&vd_source=4b77e51739cdf477312534403b78daa9
        //private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IServiceScope _serviceScope;

        public HostService1(IServiceScopeFactory _serviceScopeFactory)
        {
            _serviceScope = _serviceScopeFactory.CreateScope();
        }

        public override void Dispose()
        {
            _serviceScope.Dispose();
            base.Dispose();
        }
        //public HostService1(IServiceScopeFactory serviceScopeFactory)
        //{
        //    _serviceScopeFactory = serviceScopeFactory;
        //}

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
  
            try
            {
                Console.WriteLine("Host Service 1 started");
                await Task.Delay(5000);
                string fileContent = await File.ReadAllTextAsync("d:/LibAntiPrtSc_INFORMATION.log");
                Console.WriteLine("File Read finish");
                await Task.Delay(5000);
                Console.WriteLine(fileContent);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
       
        }
    }
}
