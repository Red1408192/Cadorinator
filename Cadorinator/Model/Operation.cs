using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cadorinator.Model
{
    public struct Operation
    {
        public Func<Task<IList<Operation>>> Function { get; set; }
        public DateTimeOffset ScheduledTime { get; set; }
        public ActionType ActionType { get; set; }
        public string Identifier { get; set; }
    }
}
