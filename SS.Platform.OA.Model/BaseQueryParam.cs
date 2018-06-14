using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SS.Platform.OA.Model
{
    public class BaseQueryParam
    {
        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public int Total { get; set; }

        public string SCode { get; set; }

        public string SName { get; set; }
    }
}
