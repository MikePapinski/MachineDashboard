using MachineDashboard.Interfaces;
using MachineDashboard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MachineDashboard.Services
{
    public class MachineService
    {
        private readonly IMongoRepository<Machine> _machineRepository;

        public MachineService(IMongoRepository<Machine> machineRepository)
        {
            _machineRepository = machineRepository;
        }

        public async Task<List<Machine>> GetMachines()
        {

            var machines = _machineRepository.FilterBy(
                filter => filter.Ip != ""
            );
            return machines.ToList();
        }

        public async Task AddMachine(Machine machine)
        {
            await _machineRepository.InsertOneAsync(machine);
        }

        public async Task RemoveMachine(Machine machine)
        {

            await _machineRepository.DeleteByIdAsync(machine.Id.ToString());
        }


        public async Task SaveMachine(Machine machine)
        {
            await _machineRepository.ReplaceOneAsync(machine);
        }


    }
}
