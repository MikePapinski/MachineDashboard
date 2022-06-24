using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using MachineDashboard.Interfaces;
using ReactiveUI;

namespace MachineDashboard.Models
{
    [BsonCollection("machine")]
    public class Machine : Document
    {
        private string _ip;
        public string Ip
        {
            get => _ip;
            set => this.RaiseAndSetIfChanged(ref _ip, value);
        }

        private string _name;
        public string Name
        {
            get => _name;
            set => this.RaiseAndSetIfChanged(ref _name, value);
        }

        private string _reservationStatus;
        public string ReservationStatus
        {
            get => _reservationStatus;
            set => this.RaiseAndSetIfChanged(ref _reservationStatus, value);
        }

        private string _group;
        public string Group
        {
            get => _group;
            set => this.RaiseAndSetIfChanged(ref _group, value);
        }

        private bool _jenkinsBringOnline;
        public bool JenkinsBringOnline
        {
            get => _jenkinsBringOnline;
            set => this.RaiseAndSetIfChanged(ref _jenkinsBringOnline, value);
        }

        private string _jenkinsLabels;
        public string JenkinsLabels
        {
            get => _jenkinsLabels;
            set => this.RaiseAndSetIfChanged(ref _jenkinsLabels, value);
        }

        private string _jenkinsDescription;
        public string JenkinsDescription
        {
            get => _jenkinsDescription;
            set => this.RaiseAndSetIfChanged(ref _jenkinsDescription, value);
        }
    }
}
