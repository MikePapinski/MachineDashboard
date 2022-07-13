using MachineDashboard.Interfaces;
using MachineDashboard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MachineDashboard.Services
{
    public class HistoryService
    {
        private readonly IMongoRepository<HistoryEvent> _historyRepository;

        public HistoryService(IMongoRepository<HistoryEvent> historyRepository)
        {
            _historyRepository = historyRepository;
        }

        public async Task<List<HistoryEvent>> GetHistory()
        {

            var history = _historyRepository.FilterBy(
                filter => filter.Ip != ""
            );
            return history.ToList();
        }

        public async Task AddHistory(HistoryEvent historyEvent)
        {
            await _historyRepository.InsertOneAsync(historyEvent);
        }

        public async Task RemoveHistory(HistoryEvent historyEvent)
        {

            await _historyRepository.DeleteByIdAsync(historyEvent.Id.ToString());
        }


        public async Task SaveHistory(HistoryEvent historyEvent)
        {
            await _historyRepository.ReplaceOneAsync(historyEvent);
        }


    }
}
