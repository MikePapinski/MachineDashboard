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
        public MainWindowViewModel(MachineService machineService, SoftwareVersion softwareVersion)
        {
            _softwareVersion = softwareVersion;
            Machines = new ObservableCollection<Machine>();
            this.machineService = machineService;
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
          //  if (IsBusy)
            //    return;

            try
            {
              //  IsBusy = true;
                var machines = await machineService.GetMachines();

                if (Machines.Count != 0)
                    Machines.Clear();

                foreach (var machine in machines)
                    Machines.Add(machine);

            }
            catch (Exception ex)
            {
               // Debug.WriteLine($"Unable to get machines: {ex.Message}");
               // await Shell.Current.DisplayAlert("Error!", ex.Message, "OK");
            }
            finally
            {
              //  IsBusy = false;
            }
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
            if (Machines.Where(x => x.Id == machine.Id).FirstOrDefault() != default)
            {
                machineService.SaveMachine(machine);
                return;
            }
            machineService.AddMachine(machine);
        }

        public void RunDeleteMachine(Machine machine)
        {
            machineService.RemoveMachine(machine);
        }


        private async void MyFunction()
        {
            // Loop in here
            while (true)
            {
                var machines = await machineService.GetMachines();

                // Check for delete
                foreach (var machineObservable in Machines.ToList())
                {
                    var deleteMachine = machines.FirstOrDefault(x => x.Id == machineObservable.Id);
                    if (deleteMachine == null)
                    {
                        Machines.Remove(machineObservable);
                    }
                }

                // Check for add or update
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
                Thread.Sleep(1000);
            }
        }

      
    }
}
