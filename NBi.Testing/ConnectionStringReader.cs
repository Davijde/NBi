using System;
using System.IO;
using System.Xml;
using System.Linq;

namespace NBi.Testing
{
    class ConnectionStringReader
    {
        protected static string Get(string name)
        {
            var xmldoc = new XmlDocument();
            xmldoc.Load(GetFilename());
            XmlNodeList nodes = xmldoc.GetElementsByTagName("add");
            foreach (XmlNode node in nodes)
                if (node.Attributes["name"].Value == name)
                    return node.Attributes["connectionString"].Value;
            throw new Exception();
        }

        private static string GetFilename()
        {
            var basePath = Path.GetDirectoryName(typeof(ConnectionStringReader).Assembly.Location);
            var filenames = new[] { "ConnectionString.user.config", "ConnectionString.config" };
            return filenames.Select(x => $@"{basePath}\{x}").FirstOrDefault(x => File.Exists(x));
        }

        public static string GetOleDbCube() => Get("OleDbCube");
        public static string GetOleDbSql() => Get("OleDbSql");
        public static string GetOdbcSql()  => Get("OdbcSql");
        public static string GetLocalOleDbSql() => Get("LocalOleDbSql");
        public static string GetLocalOdbcSql()  => Get("LocalOdbcSql");
        public static string GetAdomd() => Get("Adomd");
        public static string GetSqlClient() => Get("SqlClient");
        public static string GetAdomdTabular() => Get("AdomdTabular");
        public static string GetLocalSqlClient() => Get("LocalSqlClient");
        public static string GetReportServerDatabase() => Get("ReportServerDatabase");
        public static string GetIntegrationServer() => Get("IntegrationServer");
        public static string GetIntegrationServerTargetDatabase() => Get("IntegrationServerTargetDatabase");
    }
}
