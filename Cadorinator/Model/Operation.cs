using Cadorinator.Service.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cadorinator.Model
{
    public class Operation
    {
        public Func<OperationSchedule, Task<bool>> Function { get; set; }
        public DateTime ProspectedTime { get; set; }
        public ActionType ActionType { get; set; }
        public string Identifier { get; set; }
    }
}
