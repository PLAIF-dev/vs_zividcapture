using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ZividCapture.App.Dialogs.Views;
using ZividCapture.App.Events;
using ZividCapture.Cameras;

namespace ZividCapture.App.ViewModels
{
    public class AppSettingPanelViewModel : AppSettingViewModel
    {
        [Browsable(false)]
        public DelegateCommand SetCameraCommand { get; set; }
        [Browsable(false)]
        public DelegateCommand SetFileSettingCommand { get; set; }
        [Browsable(false)]
        public DelegateCommand SetSaveDirectoryCommand { get; set; }

        public AppSettingPanelViewModel(IDialogService dialogService, IEventAggregator eventAggregator)
        {
            SetCameraCommand = new(() =>
            {
                dialogService.ShowDialog(nameof(SelectCamera), (r) =>
                {
                    if (r.Result == ButtonResult.OK)
                    {
                        Task.Factory.StartNew(() =>
                        {
                            try
                            {
                                eventAggregator.GetEvent<WorkingRequestEvent>().Publish(new(true, "Connecting Camera ..."));

                                if (Camera != null && Camera.IsConnected)
                                    Camera.Disconnect();

                                Camera = r.Parameters.GetValue<ICamera>("camera");
                                Camera.Connect();
                            }
                            catch (Exception e)
                            {
                                MessageBox.Show(e.Message);
                                Camera = null;
                            }
                            finally
                            {
                                eventAggregator.GetEvent<WorkingRequestEvent>().Publish(new(false, "Connecting Camera ..."));
                            }
                        });
                    }
                });
            });

            SetFileSettingCommand = new(() =>
            {
                if (Camera == null)
                    return;

                string filter = "";
                string title = "";
                switch (Camera.Manufacturor)
                {
                    case Manufacturor.Zivid:
                        filter = "YAML files (*.yml)|*.yml";
                        title = "Select a YAML File";
                        break;
                }

                var openFileDialog = new OpenFileDialog
                {
                    Filter = filter,
                    Title = title,
                    Multiselect = false
                };

                bool? result = openFileDialog.ShowDialog();

                if (result == true)
                {
                    string filePath = openFileDialog.FileName;
                    CaptureSettingFileInfo = new(filePath);
                }
            });

            SetSaveDirectoryCommand = new(() =>
            {
                var dialog = new CommonOpenFileDialog
                {
                    IsFolderPicker = true,
                    Title = "Select a folder"
                };

                if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    string selectedPath = dialog.FileName;
                    SaveDirectory = new(selectedPath);
                }
            });

            PropertyChanged += (s, e) =>
            {
                eventAggregator.GetEvent<AppSettingChangedEvent>().Publish(this);
            };
        }
    }
}
