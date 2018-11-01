using Doplace.Dto;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Weavy.Core.Models;

namespace Wvy.Models
{
    [Serializable]
    [Guid("df8beb98-7dd5-4c40-93bc-3da22d03d616")]
    [App(Icon = "application", Name = "Advanced Search", Description = "Advanced search for space.", AllowMultiple = false)]
    public class AdvancedSearchApp : App
    {
        //public AdvancedSearch Search { get; set; }
    }

    public class AdvancedSearch
    {
        public string Query { get; set; }

        public string Referrer { get; set; }

        public int AppId { get; set; }

        public List<IndexDataDto> SearchResults { get; set; }
    }
}