using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.Pagination
{
    public class PaginationFilter
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public string SearchQuery { get; set; }
        public PaginationFilter()
        {
            this.PageNumber = 1;
           // this.PageSize = 10;
        }
        public PaginationFilter(int pageNumber, int pageSize)
        {
            this.PageNumber = pageNumber < 1 ? 1 : pageNumber;
            this.PageSize = pageSize;

            //this.PageSize = pageSize > 10 ? 10 : pageSize;
            //this.PageSize = (pageSize <= 10 && pageSize > 0) ? pageSize : 10;
        }
    }
}
