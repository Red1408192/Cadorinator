using Cadorinator.Model;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cadorinator.Service.Model
{
    public class OperationSchedule
    {
        private SortedList<long, Operation> Schedule = new SortedList<long, Operation>();
        private int _maxOffset;
        private ILogger _logger;

        public OperationSchedule(int maxOffset, ILogger logger)
        {
            _maxOffset = maxOffset;
            _logger = logger;
        }

        public bool AddOperation(Operation operation, int delaySeconds = 0, int granularity = 4, bool required = false)
        {
            try
            {
                if (delaySeconds < _maxOffset && delaySeconds > -_maxOffset || required)
                {
                    var targetTime = operation.ProspectedTime.AddSeconds(delaySeconds).ToBinary();
                    if (this.Schedule.ContainsKey(targetTime))
                    {
                        var newDelay = delaySeconds >= 0 ? (delaySeconds * -1) - granularity : delaySeconds * -1;
                        return this.AddOperation(operation, newDelay);
                    }
                    else
                    {
                        return Schedule.TryAdd(targetTime, operation);
                    }
                }
                else if (granularity > 1)
                {
                    return AddOperation(operation, 0, granularity / 2);
                }
                else
                {
                    return false;
                }
            }
            catch(Exception ex)
            {
                _logger.Error(ex, $"Exception during add operation");
                return false;
            }

        }
        public bool AddOperations(IList<Operation> operations)
        {
            foreach(var op in operations)
            {
                AddOperation(op);
            }
            return true;            
        }

        public IEnumerable<KeyValuePair<long,Operation>> GetOperations(DateTime until)
        {
            var value = until.ToBinary();
            return Schedule.TakeWhile(x => x.Key < value).ToArray();
        }

        public bool Pop(long key) => Schedule.Remove(key);

        public bool Any(ActionType actionType)
        {
            return Schedule.Any(x => x.Value.ActionType == actionType);
        }

        public bool Any(string id)
        {
            return Schedule.Any(x => x.Value.Identifier == id);
        }

        public DateTime[] GetSchedule() => Schedule.Keys.Select(x => DateTime.FromBinary(x)).ToArray();
    }
}
