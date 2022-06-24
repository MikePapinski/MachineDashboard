using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MachineDashboard.Interfaces;
using MongoDB.Bson;
using ReactiveUI;

namespace MachineDashboard.Models
{
    public abstract class Document : ReactiveObject, IDocument
    {
        public ObjectId Id { get; set; }

        public DateTime CreatedAt => Id.CreationTime;
    }
}
