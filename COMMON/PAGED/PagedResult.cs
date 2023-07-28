using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMMON.PAGED
{
    public class PagedResult<T>
    {
        public int TotalRecords { get; set; }
        public int TotalPages { get; set; }
        public IEnumerable<T> Collection { get; set; }

        public PagedResult(IEnumerable<T> collection , int totalRecords , int pageSize)
        {
            this.Collection = collection;
            this.TotalRecords = totalRecords;
            this.TotalPages = (int)Math.Ceiling((double)totalRecords / pageSize); ;
        }
    }
}
