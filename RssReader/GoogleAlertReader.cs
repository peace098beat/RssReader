using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml;

namespace RssReader
{
    public class Entry
    {
        public string id = "";
        public string title = "";
        public string link = "";
        public string published = "";
        public string updated = "";
        public string content = "";
    }

    public class GoogleAlertBody
    {
        public string title = "";
        public string link = "";
        public string updated = "";
        public List<Entry> Entries = new List<Entry>();
    }

    /// <summary>
    /// GoogleAlertを取得しパースするクラス
    /// </summary>
    public class GoogleAlertReader
    {
        private static HttpClient client = new HttpClient();
        private static XmlNamespaceManager nsmgr;

        public GoogleAlertReader()
        {
            // 名前空間を追加      
            string nameSpace = "http://www.w3.org/2005/Atom";
            nsmgr = new XmlNamespaceManager(new NameTable());
            nsmgr.AddNamespace("ns", nameSpace);
        }

        /// <summary>
        /// GETリクエストでXMLを取得
        /// URLを間違うと System.Net.Http.HttpRequestExceptionを発行する.
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<string> ReadOnesAsync(string url)
        {
            string xmlbody = await client.GetStringAsync(url);
            return xmlbody;
        }

        /// <summary>
        /// GETリクエストでXMLを取得して、パースまで
        /// </summary>
        public async Task<GoogleAlertBody> ReadAsyncAndParse(string url)
        {
            string xmlbody = await client.GetStringAsync(url);

            GoogleAlertBody body = Parse(xmlbody);

            return body;
        }

        // --- 以下, STATIC ---

        /// <summary>
        /// 全てをパース
        /// </summary>
        public GoogleAlertBody Parse(string XMLBody)
        {
            // 返却
            GoogleAlertBody ResponseBody = new GoogleAlertBody();

            // XMLをロード
            XmlDocument root = new XmlDocument();

            // XMLをロード
            try
            {
                root.LoadXml(XMLBody);

            }
            catch (Exception e)
            {
                // erro
                return ResponseBody;
            }

            // ======================================================================================

            // 1. タイトル要素を取得
            XmlNode Feed_TitleElem = root.SelectSingleNode("/ns:feed/ns:title", nsmgr);
            string Feed_Title = "";
            if (Feed_TitleElem != null)
                Feed_Title = Feed_TitleElem.InnerText;

            // 1. Link要素を取得
            XmlElement URL_elm = (XmlElement)root.SelectSingleNode("/ns:feed/ns:link", nsmgr);
            string Feed_URL = "";
            if (URL_elm != null)
            {
                Feed_URL = URL_elm.GetAttribute("href");
            }

            // 1. updated要素を取得
            XmlNode Feed_Updated_elem = root.SelectSingleNode("/ns:feed/ns:updated", nsmgr);
            string Feed_Updated = "";
            if (Feed_Updated_elem != null)
            {
                Feed_Updated = Feed_Updated_elem.InnerText;
            }


            // -- 返却処理
            ResponseBody.title = Feed_Title;
            ResponseBody.link = Feed_URL;
            ResponseBody.updated = Feed_Updated;

            // ======================================================================================

            // Entrys
            XmlNodeList XmlEntryNodeList = root.SelectNodes("/ns:feed/ns:entry", nsmgr);

            ResponseBody.Entries = ParseEntryes(XmlEntryNodeList);


            // -- 返却処理
            return ResponseBody;

        }

        /// <summary>
        /// entryeのリストをパース
        /// </summary>
        public static List<Entry> ParseEntryes(XmlNodeList EntriesNodeList)
        {
            List<Entry> EntryList = new List<Entry>();

            // null処理
            if (EntriesNodeList == null)
            {
                return EntryList;
            }

            foreach (XmlNode EntryNode in EntriesNodeList)
            {
                Entry e = ParseEntry(EntryNode);

                EntryList.Add(e);
            }

            return EntryList;
        }

        /// <summary>
        /// entryをパース
        /// </summary>
        public static Entry ParseEntry(XmlNode EntryNode)
        {
            // Entry
            Entry entry = new Entry();

            if (EntryNode == null)
            {
                return entry;
            }

            // =========================================
            // null判定をしておく
            // nullの時は""

            // Entry.title
            XmlNode title_node = EntryNode.SelectSingleNode("ns:title", nsmgr);
            string entry_title = (title_node != null) ? title_node.InnerText : "";


            // Entry.link
            XmlElement link_elem = (XmlElement)EntryNode.SelectSingleNode("ns:link", nsmgr);
            string entry_link = (link_elem != null) ? link_elem.GetAttribute("href") : "";

            // Entry.content
            XmlNode content_node = EntryNode.SelectSingleNode("ns:content", nsmgr);
            string entry_content = (content_node != null) ? content_node.InnerText : "";

            // Entry.content
            XmlNode published_node = EntryNode.SelectSingleNode("ns:published", nsmgr);
            string entry_published = (published_node != null) ? published_node.InnerText : "";

            // Entry.content
            XmlNode updated_node = EntryNode.SelectSingleNode("ns:updated", nsmgr);
            string entry_updated = (updated_node != null) ? updated_node.InnerText : "";

            // -----------------------------------------

            // 返却処理
            entry.title = entry_title;
            entry.link = entry_link;
            entry.content = entry_content;
            entry.published = entry_published;
            entry.updated = entry_updated;
            // =========================================

            return entry;
        }
    }
}
