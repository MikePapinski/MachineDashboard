using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using MachineDashboard.Models;
using MachineDashboard.ViewModels;
using MachineDashboard.Views;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using MachineDashboard.Services;
using MachineDashboard.Repositories;
using System.Text.Json;
using System.IO;
using Newtonsoft.Json;

namespace MachineDashboard
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {

                var assembly = Assembly.GetExecutingAssembly();

                var settings = new MongoDbSettings();
                using (Stream stream = assembly.GetManifestResourceStream("MachineDashboard.appsettings.json"))
                using (StreamReader reader = new StreamReader(stream))
                {
                    string jsonFile = reader.ReadToEnd();
                    settings = JsonConvert.DeserializeObject<MongoDbSettings>(jsonFile);
                }

                var softwareVersion = new SoftwareVersion();
                using (Stream stream = assembly.GetManifestResourceStream("MachineDashboard.version.json"))
                using (StreamReader reader = new StreamReader(stream))
                {
                    string jsonFile2 = reader.ReadToEnd();
                    softwareVersion = JsonConvert.DeserializeObject<SoftwareVersion>(jsonFile2);
                }

                var machineRepository = new MongoRepository<Machine>(settings);
                var machineService = new MachineService(machineRepository);

                desktop.MainWindow = new MainWindow
                {
                    DataContext = new MainWindowViewModel(machineService, softwareVersion),
                };
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}
