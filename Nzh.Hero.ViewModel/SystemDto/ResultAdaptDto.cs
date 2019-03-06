using System;
using System.Collections.Generic;
using System.Text;

namespace Nzh.Hero.ViewModel.SystemDto
{
    public class ResultAdaptDto
    {
        public ResultAdaptDto()
        {
            statusCode = 200;
            this.data = new Dictionary<string, object>();
        }
        public int statusCode { get; set; }
        public string msg { get; set; }
        public Dictionary<string, object> data { get; set; }
    }
}
