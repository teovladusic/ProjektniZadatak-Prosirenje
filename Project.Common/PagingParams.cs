using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class PagingParams
    {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }

        public PagingParams(int currentPage, int pageSize)
        {
            CurrentPage = currentPage;
            PageSize = pageSize;
        }
    }
}
