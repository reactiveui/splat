namespace Splat.NLog
{
    public static class Helpers
    {
        public static void UseNLog()
        {
            var funcLogManager = new FuncLogManager(type =>
            {
                var actualLogger = global::NLog.LogManager.GetLogger(type.ToString());
                return new NLogSplatLogger(actualLogger);
            });

            Locator.CurrentMutable.RegisterConstant(funcLogManager, typeof(ILogManager));
        }
    }
}
