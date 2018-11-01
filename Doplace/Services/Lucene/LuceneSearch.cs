using Doplace.Constants;
using Lucene.Net.Index;
using Lucene.Net.Store;
using System.IO;

namespace Doplace.Services.Lucene
{
    public abstract class LuceneSearch
    {
        protected readonly string _rootPath;
        protected string _luceneFolderName { get { return ConfigurationConstants.LUCENE_INDEX_FOLDER; } }
        protected string _luceneFolderPhysicalPath { get { return _rootPath + _luceneFolderName; } }

        protected LuceneSearch(string rootPath)
        {
            _rootPath = rootPath;
        }

        private FSDirectory _directoryTemp;
        protected FSDirectory _directory
        {
            get
            {
                if (_directoryTemp == null) _directoryTemp = FSDirectory.Open(new DirectoryInfo(_luceneFolderPhysicalPath));
                if (IndexWriter.IsLocked(_directoryTemp)) IndexWriter.Unlock(_directoryTemp);
                var lockFilePath = Path.Combine(_luceneFolderPhysicalPath, "write.lock");
                if (File.Exists(lockFilePath)) File.Delete(lockFilePath);
                return _directoryTemp;
            }
        }
    }
}
