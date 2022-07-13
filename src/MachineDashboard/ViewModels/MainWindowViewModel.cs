using MachineDashboard.Models;
using MachineDashboard.Services;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MachineDashboard.Interfaces;
using System.Text.Json;
using MessageBox.Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Models;
using MessageBoxAvaloniaEnums = MessageBox.Avalonia.Enums;

namespace MachineDashboard.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private ISoftwareVersion _softwareVersion { get; }
        public string WindowHeader => "Machine Dashboard v" + _softwareVersion.Major + "." + _softwareVersion.Minor + "." + _softwareVersion.Patch;
        public ReactiveCommand<Unit, Unit> NewMachine { get; }
        public ReactiveCommand<Machine, Unit> ShowMachine { get; }
        public ReactiveCommand<Machine, Unit> SaveMachine { get; }
        public ReactiveCommand<Machine, Unit> DeleteMachine { get; }

        public ObservableCollection<Machine> Machines { get; }
        public ObservableCollection<HistoryEvent> History { get; }
        public Machine _selectedMachine;
        private string _machineHeader;

        public Machine SelectedMachine
        {
            get => _selectedMachine;
            set => this.RaiseAndSetIfChanged(ref _selectedMachine, value);
        }
        public string MachineHeader
        {
            get => _machineHeader;
            set => this.RaiseAndSetIfChanged(ref _machineHeader, value);
        }
        MachineService machineService;
        HistoryService historyService;
        public MainWindowViewModel(MachineService machineService, HistoryService historyService, SoftwareVersion softwareVersion)
        {
            _softwareVersion = softwareVersion;
            Machines = new ObservableCollection<Machine>();
            History = new ObservableCollection<HistoryEvent>();
            this.machineService = machineService;
            this.historyService = historyService;
            GetMachines();
            Task task = Task.Run((Action)MyFunction);
            SelectedMachine = new Machine();
            MachineHeader = "New Machine";
            NewMachine = ReactiveCommand.Create(RunNewMachine);
            ShowMachine = ReactiveCommand.Create<Machine>(RunShowMachine);
            SaveMachine = ReactiveCommand.Create<Machine>(RunSaveMachine);
            DeleteMachine = ReactiveCommand.Create<Machine>(RunDeleteMachine);
        }

        async Task GetMachines()
        {
                var machines = await machineService.GetMachines();

                if (Machines.Count != 0)
                    Machines.Clear();

                foreach (var machine in machines)
                    Machines.Add(machine);
        }

        public void RunShowMachine(Machine machine)
        {
            if (machine == null)
                return;
            SelectedMachine = new Machine()
            {
                Id = machine.Id,
                Ip = machine.Ip,
                Name = machine.Name,
                ReservationStatus = machine.ReservationStatus,
                Group = machine.Group,
                JenkinsBringOnline = machine.JenkinsBringOnline,
                JenkinsLabels = machine.JenkinsLabels,
                JenkinsDescription = machine.JenkinsDescription
            };
            MachineHeader = SelectedMachine.Name;
        }

        public void RunNewMachine()
        {
            SelectedMachine = new Machine();
            MachineHeader = "New Machine";
        }

        public void RunSaveMachine(Machine machine)
        {
             var messageBoxStandardWindow = MessageBox.Avalonia.MessageBoxManager.GetMessageBoxStandardWindow(
                new MessageBoxStandardParams
                {
                    ButtonDefinitions = MessageBoxAvaloniaEnums.ButtonEnum.OkCancel,
                    ContentTitle = "title",
                    ContentHeader = "header",
                    ContentMessage = "Message",
                    WindowIcon = new WindowIcon("icon-rider.png")
                });
            var rslt = messageBoxStandardWindow.Show();
            //await messageBoxMarkdownWindow.ShowDialog(this);

            var findMachine = Machines.Where(x => x.Id == machine.Id).FirstOrDefault();
            if (findMachine != default)
            {
                machineService.SaveMachine(machine);
                historyService.AddHistory( new HistoryEvent()
                {
                    Action = "Update",
                    Before = JsonSerializer.Serialize<Machine>(findMachine),
                    After = JsonSerializer.Serialize<Machine>(machine),
                });
                return;
            }
            
            machineService.AddMachine(machine);
            historyService.AddHistory( new HistoryEvent()
            {
                Action = "Add",
                Before = "",
                After = JsonSerializer.Serialize<Machine>(machine),
            });
        }

        public void RunDeleteMachine(Machine machine)
        {
            machineService.RemoveMachine(machine);
            historyService.AddHistory( new HistoryEvent()
            {
                Action = "Delete",
                Before =  JsonSerializer.Serialize<Machine>(machine),
                After = "",
            });
        }


        private async void MyFunction()
        {
            // Loop in here
            while (true)
            {
                var machines = await machineService.GetMachines();
                var history = await historyService.GetHistory();

                // Check for delete
                foreach (var machineObservable in Machines.ToList())
                {
                    var deleteMachine = machines.FirstOrDefault(x => x.Id == machineObservable.Id);
                    if (deleteMachine == null)
                    {
                        Machines.Remove(machineObservable);
                    }
                }

                // Check for add or update for machine
                foreach (var machine in machines)
                {
                    var newMachine = Machines.FirstOrDefault(x => x.Id == machine.Id);
                    if (newMachine == null)
                    {
                        Machines.Add(machine);
                    }
                    else
                    {
                        if (newMachine.Ip != machine.Ip ||
                           newMachine.Id != machine.Id ||
                           newMachine.Name != machine.Name ||
                           newMachine.ReservationStatus != machine.ReservationStatus ||
                           newMachine.JenkinsBringOnline != machine.JenkinsBringOnline ||
                           newMachine.JenkinsLabels != machine.JenkinsLabels ||
                           newMachine.JenkinsDescription != machine.JenkinsDescription ||
                           newMachine.Group != machine.Group)
                        {
                            Machines.Remove(newMachine);
                            Machines.Add(machine);
                        }
                    }
                }

                // Check for add for history
                foreach (var historyEvent in history)
                {
                    var newHistory = History.FirstOrDefault(x => x.Id == historyEvent.Id);
                    if (newHistory == null)
                    {
                        History.Add(historyEvent);
                    }
                }

                Thread.Sleep(1000);
            }
        }

      
    }
}
