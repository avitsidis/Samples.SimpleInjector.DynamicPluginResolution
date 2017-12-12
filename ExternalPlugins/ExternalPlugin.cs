using Common;

namespace ExternalPlugins
{
    public class ExternalPlugin : IPlugin
    {
        private readonly ILogger logger;

        public ExternalPlugin(ILogger logger)
        {
            this.logger = logger;
        }
        public void Execute()
        {
            logger.Info(GetType().FullName);
        }
    }
}
