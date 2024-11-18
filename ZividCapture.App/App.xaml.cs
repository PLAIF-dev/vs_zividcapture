using System.Configuration;
using System.Data;
using System.Windows;
using ZividCapture.App.Dialogs.ViewModels;
using ZividCapture.App.Dialogs.Views;
using ZividCapture.App.Views;

namespace ZividCapture.App
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {

        protected override Window CreateShell()
        {
            return Container.Resolve<Views.MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<AppSettingPanel>();
            containerRegistry.RegisterForNavigation<FrameViewer>();
            containerRegistry.RegisterDialog<SelectCamera, SelectCameraViewModel>();
        }

        protected override void OnInitialized()
        {

            // IRegionManager를 통해 View를 ContentRegion에 Navigation
            var regionManager = Container.Resolve<IRegionManager>();
            regionManager.RequestNavigate("AppSettingRegion", nameof(AppSettingPanel));
            regionManager.RequestNavigate("FrameRegsion", nameof(FrameViewer));
            base.OnInitialized();

        }
    }
}
