using System;
using System.Configuration;
using Weavy.Core.Repos;

namespace Doplace.Constants
{
    public static class ConfigurationConstants
    {
        public static string LUCENE_INDEX_FOLDER { get { return "lucene_index"; } }

        public static string CONNECTION_STRING { get { return SqlHelper.ConnectionString; } }

        public static int DOP_GLOBAL_SPACE_ID
        {
            get { return Convert.ToInt32(ConfigurationManager.AppSettings["dop.global.space-id"]); }
        }

        public static int DOP_GLOBAL_FILES_APP_ID
        {
            get { return Convert.ToInt32(ConfigurationManager.AppSettings["dop.global.files-app-id"]); }
        }
    }
}
