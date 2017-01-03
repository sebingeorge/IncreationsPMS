using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IncreationsPMSDomain
{
    public class Result
    {
        public Result(bool val)
        {
            Value = val;
        }
        public Result(bool val, string message)
        {
            Value = val;
            Message = message;
        }
        public ArrayList Errors { get; set; }
        public string Message { get; set; }
        public bool Value { get; set; }
    }
}
