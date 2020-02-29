using BeatSaberBackUpTool.Interfaces;
using BeatSaberBackUpTool.Services;
using BeatSaberBackUpTool.Views;
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
            return Container.Resolve<MainWindowView>();
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            var region = this.Container.Resolve<IRegionManager>();
            region.RegisterViewWithRegion("MainRegion", typeof(MainView));
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<ILoadingService, LoadingService>();
            containerRegistry.RegisterDialog<DialogView>();
        }
    }
}
