using Cadorinator.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cadorinator.Service.Model
{
    public class Schedule
    {
        private KeyValuePair<long, Operation> InternalSchedule;
        public int Tries
        {
            get => InternalSchedule.Value.Tries;
            set => InternalSchedule.Value.Tries = value;
        }
        public string Identifier
        {
            get => InternalSchedule.Value.Identifier;
            set => InternalSchedule.Value.Identifier = value;
        }
        public long Key => InternalSchedule.Key;
        public Operation Operation => InternalSchedule.Value;
        public DateTime ProspectedTime
        {
            get => InternalSchedule.Value.ProspectedTime;
            set => InternalSchedule.Value.ProspectedTime = value;
        }

        public Schedule(KeyValuePair<long, Operation> x)
        {
            this.InternalSchedule = x;
        }

        public async Task<bool> Execute(OperationSchedule operations) => await this.InternalSchedule.Value.Function(operations);
        public bool Delay(OperationSchedule operations)
        {
            this.ProspectedTime = DateTime.UtcNow.AddSeconds(4);
            this.Tries += 1;
            return operations.AddOperation(this);
        }
    }
}
