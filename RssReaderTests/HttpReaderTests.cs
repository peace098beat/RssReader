using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Xml;


namespace RssReader.Tests
{
    [TestClass()]
    public class HttpReaderTests
    {
        [TestMethod()]
        public void HttpReaderTest_RSS()
        {
            string url_gg = @"https://www.google.co.jp/alerts/feeds/06743062077164786107/5387003833411359502";

            var reader = new HttpReader();
            Task<string> task = reader.ReadOnesAsync(url_gg);

            task.Wait();

            Debug.WriteLine(task.Result);

            string body = task.Result;
            Assert.AreNotEqual<string>(body, "");
        }

        [TestMethod()]
        public void HttpReaderTest_Google()
        {
            string url_gg = @"https://www.google.co.jp";

            var reader = new HttpReader();
            Task<string> task = reader.ReadOnesAsync(url_gg);

            task.Wait();

            Debug.WriteLine(task.Result);

            string body = task.Result;
            Assert.AreNotEqual<string>(body, "");
        }

        [TestMethod()]
        public void ParseTest_SampleXML()
        {
            XmlDocument xmlDocument = new XmlDocument();

            //xmlDocument.Load("RSS_Google.xml");
            //xmlDocument.Load("RSS_OpenCAE.xml");
            xmlDocument.Load("sample.xml");

            XmlNode feed = xmlDocument.DocumentElement;

            Assert.AreEqual(feed.Name, "doc");

            XmlNode node = feed.SelectSingleNode("head/title");
            Assert.IsTrue(node != null);


            Assert.AreEqual(node.InnerText, "TitleBody");

        }

        [TestMethod()]
        public void ParseTest_OpenCAE()
        {
            XmlDocument root = new XmlDocument();
            root.Load("RSS_OpenCAE.xml");

            // 名前空間を追加      
            string nameSpace = "http://www.w3.org/2005/Atom";
            var nsmgr = new XmlNamespaceManager(new NameTable());
            nsmgr.AddNamespace("ns", nameSpace);

            // タイトル要素を取得
            var Title = root.SelectNodes("/ns:feed/ns:title", nsmgr).Item(0).InnerText;
            Debug.WriteLine(Title);
            Assert.AreNotEqual(Title, "");

            // Link要素を取得
            XmlElement URL_elm = (XmlElement)root.SelectSingleNode("/ns:feed/ns:link", nsmgr);
            string URL = URL_elm.GetAttribute("href");
            Debug.WriteLine(URL);
            Assert.AreNotEqual(URL, "");
        }

        [TestMethod()]
        public void ParseTest_Google()
        {
            XmlDocument root = new XmlDocument();
            root.Load("RSS_Google.xml");

            // 名前空間を追加      
            string nameSpace = "http://www.w3.org/2005/Atom";
            var nsmgr = new XmlNamespaceManager(new NameTable());
            nsmgr.AddNamespace("ns", nameSpace);

            // タイトル要素を取得
            var Title = root.SelectNodes("/ns:feed/ns:title", nsmgr).Item(0).InnerText;
            Debug.WriteLine(Title);
            Assert.AreNotEqual(Title, "");

            // Link要素を取得
            XmlElement URL_elm = (XmlElement)root.SelectSingleNode("/ns:feed/ns:link", nsmgr);
            string URL = URL_elm.GetAttribute("href");
            Debug.WriteLine(URL);
            Assert.AreNotEqual(URL, "");

            // Entrys
            XmlNodeList entryes = root.SelectNodes("/ns:feed/ns:entry", nsmgr);
            Assert.AreNotEqual(entryes.Count, 0, "Entry Count == 0");

            // Entry test
            foreach (XmlNode entry in entryes)
            {
                Assert.AreEqual(entry.Name, "entry");

                // Entry.title
                string entry_title = entry.SelectSingleNode("ns:title", nsmgr).InnerText;
                Assert.AreNotEqual(entry_title, "", entry_title);
                Debug.WriteLine(entry_title);

                // Entry.content
                string entry_content = entry.SelectSingleNode("ns:content", nsmgr).InnerText;
                Assert.AreNotEqual(entry_content, "", entry_content);
                Debug.WriteLine(entry_content);
            }


        }
    }
}