using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battler
{
    public class BoutResult
    {
        public string Category { get; set; }
        public List<BoutMiniResult> Results { get; set; }

        public BoutResult()
        {
            Results = new List<BoutMiniResult>();
        }

        public class BoutMiniResult
        {
            public string Message { get; set; }
            public int Fighter1Hits { get; set; }
            public int Fighter2Hits { get; set; }
        }
    }
}