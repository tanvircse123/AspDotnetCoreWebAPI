using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API.Model
{
    public class PaginationMetadata
    {
        public PaginationMetadata(int totalitemcount,int pagesize,int currentpage)
        {   
            this.TotalItemCount = totalitemcount;
            this.PageSize = pagesize;
            this.CurrentPage = currentpage;
            this.TotalPageCount = (int)Math.Ceiling(totalitemcount/(double)pagesize);
        }
        public int TotalItemCount {get;set;}
        public int TotalPageCount {get;set;}
        public int PageSize      {get;set;}
        public int CurrentPage   {get;set;} 
    }
}