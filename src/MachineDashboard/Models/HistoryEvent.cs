using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using MachineDashboard.Interfaces;
using ReactiveUI;

namespace MachineDashboard.Models
{
    [BsonCollection("machine")]
    public class HistoryEvent : Document
    {
        private string _ip;
        public string Ip
        {
            get => _ip;
            set => this.RaiseAndSetIfChanged(ref _ip, value);
        }

        private string _action;
        public string Action
        {
            get => _action;
            set => this.RaiseAndSetIfChanged(ref _action, value);
        }

        private string _before;
        public string Before
        {
            get => _before;
            set => this.RaiseAndSetIfChanged(ref _before, value);
        }

        private string _after;
        public string After
        {
            get => _after;
            set => this.RaiseAndSetIfChanged(ref _after, value);
        }
        
    }
}