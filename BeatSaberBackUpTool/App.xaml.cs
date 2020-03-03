using BeatSaberBackUpTool.Interfaces;
using BeatSaberBackUpTool.Services;
using BeatSaberBackUpTool.Views;
using NLog;
using NLog.Config;
using NLog.Targets;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using Prism.Unity;
using System.Windows;

namespace BeatSaberBackUpTool
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        protected override Window CreateShell()
        {
            return this.Container.Resolve<MainWindowView>();
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            var region = this.Container.Resolve<IRegionManager>();
            region.RegisterViewWithRegion("MainRegion", typeof(MainView));

            var config = new LoggingConfiguration();
            var logfile = new FileTarget("logfile") { FileName = "log.txt", ConcurrentWrites = true};
            var logconsole = new ColoredConsoleTarget("logconsole");

            config.AddRule(LogLevel.Trace, LogLevel.Fatal, logconsole);
            config.AddRule(LogLevel.Trace, LogLevel.Fatal, logfile);

            LogManager.Configuration = config;
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<ILoadingService, LoadingService>();
            containerRegistry.RegisterDialog<DialogView>();
        }
    }
}
