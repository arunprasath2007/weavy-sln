using System.Configuration;

namespace Doplace.Constants
{
    public static class ConfigurationConstants
    {
        public static string LUCENE_INDEX_FOLDER { get { return "lucene_index"; } }

        public static string CONNECTION_STRING { get { return ConfigurationManager.ConnectionStrings["defaultConnection"].ConnectionString; } }
    }
}
