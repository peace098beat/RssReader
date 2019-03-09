using System;
using System.ServiceModel.Syndication;
using System.Threading.Tasks;
using System.Xml;

namespace RssReader
{
    static public class RssReader
    {
        /// <summary>
        ///  RSSから文章をロードする
        /// </summary>
        public static string RSSReaderRead(string url)
        {
            //string url = @"https://www.google.co.jp/alerts/feeds/09047520966360389555/18173224082898862477";
            string body = "call";
            using (XmlReader rdr = XmlReader.Create(url))
            {
                SyndicationFeed feed = SyndicationFeed.Load(rdr);

                foreach (SyndicationItem item in feed.Items)
                {
                    TextSyndicationContent c = (TextSyndicationContent)item.Content;
                    body += $"{c.Text}" + Environment.NewLine;
                }
            }
            return body;
        }

        /// <summary>
        ///  RSSから文章をロードする (非同期で)
        /// </summary>
        public static Task<string> RSSReaderReadAsync(string url)
        {
            // 著目著目
            Task<string> body = Task.Run(delegate ()
            {
                return RSSReaderRead(url);
            });
            return body;
        }
    }


}
