using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class SortParams
    {
        public string OrderBy { get; set; }

        public SortParams(string orderBy)
        {
            OrderBy = orderBy;
        }
    }
}
