using Doplace.Services.Lucene;
using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Web.Hosting;
using Weavy.Core.Models;

namespace Wvy.Models
{
    [Serializable]
    [Guid("3f1182d6-dfea-46ad-b8d6-b7390b337fa4")]
    [Daemon(Icon = "find-replace", Name = "Lucene Index Builder", Description = "Builds the index for Lucene search engine", Schedule = "*/15 * * * *")]

    public class IndexBuilderDaemon : Daemon
    {
        public override bool Run(CancellationToken token, params string[] args)
        {
            token.ThrowIfCancellationRequested();
            try
            {
                var setupService = new LuceneSearchSetupService(HostingEnvironment.MapPath("/"));
                setupService.CreateIndex(DateTime.UtcNow.AddMinutes(-15), DateTime.UtcNow);

                return true;
            }
            catch (Exception ex)
            {
                Output.WriteLine(ex.Message);
            }
            return false;
        }

    }
}