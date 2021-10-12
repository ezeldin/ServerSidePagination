using System.IO;
using System.Reflection;
using System.Text;

namespace Helpers
{
    public static class StringBuilderExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="stringBuilder"></param>
        /// <param name="sqlScriptFileName">Sql ScriptFile Name</param>
        public static void AppendQuery(this StringBuilder stringBuilder, string assemblyName, string nameSpace, string sqlScriptFileName)
        {
            var resourcePath = $"{nameSpace}.{sqlScriptFileName}";

            string queryText;
            var assembly = Assembly.Load(assemblyName);
            using (var stream = assembly.GetManifestResourceStream(resourcePath))
            {
                using (var sr = new StreamReader(stream))
                {
                    queryText = sr.ReadToEnd();
                }
            }
            queryText = queryText.
                Replace("\r", " ").
                Replace("\n", " ");

            stringBuilder.Append(queryText);
        }
        public static void AppendQuery(this StringBuilder stringBuilder, string sqlScriptFileName)
        {

            string assemblyName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;//typeToGetAssemplyandNamespaceInfo.Assembly.FullName;
            string nameSpace = assemblyName.Split(',')[0] + ".Queries";
            AppendQuery(stringBuilder, assemblyName, nameSpace, sqlScriptFileName);
        }
    }
}