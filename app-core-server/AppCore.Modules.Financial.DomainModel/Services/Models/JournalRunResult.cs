using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCore.Modules.Financial.DomainModel.Services.Models
{
    public class JournalRunResult
    {
        public List<BaseJournal> Journals { get; set; } = new List<BaseJournal>();
        public List<string> Errors { get; set; } = new List<string>();
        public List<string> Holders { get; set; } = new List<string>();
        public List<string> Deferrals { get; set; } = new List<string>();

    }
}
