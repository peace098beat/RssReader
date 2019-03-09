using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Threading.Tasks;

namespace RssReader.Tests
{
    [TestClass()]
    public class GoogleAlertReaderTests
    {
        string url_googlealert = @"https://www.google.co.jp/alerts/feeds/06743062077164786107/5387003833411359502";
        string url_ignore = @"https://www.google.co.jp/alerts/feeds/06743062077164786107/ignore";

        GoogleAlertReader reader = new GoogleAlertReader();


        [TestMethod()]
        public void GoogleAlertReaderTest()
        {
        }

        /// <summary>
        /// WEB接続テスト
        /// </summary>
        [TestMethod()]
        public void ReadOnesAsyncTest()
        {

            Task<string> task = reader.ReadOnesAsync(url_googlealert);

            task.Wait(10000);

            string xmlbody = task.Result;

            Assert.AreNotEqual(xmlbody, "", "XMLbodyが空ですよ");
        }

        /// <summary>
        /// WEB接続テスト
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(System.Net.Http.HttpRequestException))]
        public void 失敗_ReadOnesAsyncTest()
        {

            try
            {
                Task<string> task = reader.ReadOnesAsync(url_ignore);

                task.Wait();
            }
            catch (AggregateException exc)
            {
                foreach (var innnerExc in exc.InnerExceptions)
                {
                    throw innnerExc;
                }
            }

        }


        /// <summary>
        /// 統合テスト
        /// </summary>
        [TestMethod()]
        public void ReadAsyncAndParseTest()
        {

            Task<GoogleAlertBody> task = reader.ReadAsyncAndParse(url_googlealert);

            task.Wait();

            GoogleAlertBody gabody = task.Result;

            // GoogleAlertBodyのエラーチェック
            Assert.AreNotEqual(gabody.title, "");
            Assert.AreNotEqual(gabody.link, "");

            Assert.IsTrue(gabody.Entries.Count > 0);


        }


        [TestMethod()]
        public void ParseTest()
        {

            string XMLBody = "";
            using (StreamReader sreader = new StreamReader("RSS_Google.xml"))
            {
                XMLBody = sreader.ReadToEnd();
            }


            GoogleAlertBody gabody = reader.Parse(XMLBody);

            // GoogleAlertBodyのエラーチェック
            Assert.AreNotEqual(gabody.title, "");
            Assert.AreNotEqual(gabody.link, "");

            Assert.IsTrue(gabody.Entries.Count > 0);

        }



        [TestMethod()]
        public void 失敗_ParseTest()
        {

            string XMLBody = "hogehoge";
          
            GoogleAlertBody gabody = reader.Parse(XMLBody);

            // GoogleAlertBodyのエラーチェック
            Assert.AreEqual(gabody.title, "");
            Assert.AreEqual(gabody.link, "");

            Assert.IsTrue(gabody.Entries.Count ==  0);

        }


    }
}