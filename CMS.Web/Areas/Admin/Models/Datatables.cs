using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.Web.Areas.Admin.Models
{
    public class Datatables
    {

        public int draw { get; set; }
    }

    public class DataResult<T> where T:class
    {
        public int draw { get; set; }
        public int recordsFiltered { get; set; }
        public int recordsTotal { get; set; }
        public IQueryable<T> data { get; set; }
    }
    public class Sorting
    {
        public string ColumnName { get; set; }
        public string Direction { get; set; }
    }
}
