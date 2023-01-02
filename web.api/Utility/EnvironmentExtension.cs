namespace web.api.Utility
{
    public static class EnvironmentExtension
    {
        public static bool IsTest(this IHostEnvironment hostEnvironment)
        {
            if (hostEnvironment == null)
            {
                throw new ArgumentNullException(nameof(hostEnvironment));
            }

            return hostEnvironment.IsEnvironment(MyEnvironments.Test);
        }
      
    }

    public static class MyEnvironments
    {
        public static readonly string Test = "Test";   
    }
}
