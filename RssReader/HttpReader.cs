using System.Net.Http;
using System.Threading.Tasks;
using System.Xml;

namespace RssReader
{
    public class HttpReader
    {
        private static HttpClient client = new HttpClient();

        public HttpReader()
        {

        }


        public async Task<string> ReadOnesAsync(string url)
        {
            string response = await client.GetStringAsync(url);
            return response;
        }

        public static void Parse(string XMLBody)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(XMLBody);

            XmlNodeList nodeList = xmlDocument.SelectNodes("/feed/title");

            for (int i = 0; i < nodeList.Count; i++)
            {
                XmlNode node = nodeList[i];
            }

        }
    }
}
