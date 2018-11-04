using Doplace.Services.Lucene;
using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Web.Hosting;
using Weavy.Core.Models;

namespace Wvy.Models
{
    [Serializable]
    [Guid("ea89d467-1aec-4285-8d76-de5b27dffa57")]
    [Plugin(Name = "Rebuild Lucene Index", Description = "Delete and rebuild lucene search index.")]
    public class IndexBuilderTool : Function, ICommand, ITool
    {
        /// <summary>
        /// Writes a message to the log file.
        /// </summary>
        /// <returns></returns>
        public override bool Run(CancellationToken token, params string[] args)
        {
            try
            {
                var setupService = new LuceneSearchSetupService(HostingEnvironment.MapPath("/"));
                setupService.CreateIndex(DateTime.UtcNow.AddYears(-100), DateTime.UtcNow);

                return true;
            }
            catch (Exception ex)
            {
                Output.WriteLine(ex.Message);
                return false;
            }
        }
    }
    
}