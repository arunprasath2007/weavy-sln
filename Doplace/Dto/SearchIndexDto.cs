using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doplace.Dto
{
    public class SearchIndexDto
    {
        public string SearchQuery { get; set; }

        public string SearchField { get; set; }

        public string FilterField { get; set; }

        public string FilterValue { get; set; }
    }
}
