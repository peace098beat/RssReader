using System;
using System.Net.Http;
using System.ServiceModel.Syndication;
using System.Windows.Forms;
using System.Xml;

namespace RssReader
{
    public partial class Form1 : Form
    {
        private HttpClient client;

        public Form1()
        {
            InitializeComponent();

            this.client = new HttpClient();

        }

        private async void button1_Click(object sender, EventArgs e)
        {
            var sw = System.Diagnostics.Stopwatch.StartNew();

            textBox1.Text += $"start" + Environment.NewLine;


            string url = @"https://www.google.co.jp/alerts/feeds/09047520966360389555/18173224082898862477";

            using (XmlReader rdr = XmlReader.Create(url))
            {
                SyndicationFeed feed = SyndicationFeed.Load(rdr);

                richTextBox1.Text = "";


                foreach (SyndicationItem item in feed.Items)
                {
                    TextSyndicationContent c = (TextSyndicationContent)item.Content;


                    richTextBox1.Text += $"{c.Text}" + Environment.NewLine;


                    //Console.WriteLine("item Title:" + item.Title.Text);
                    //Console.WriteLine("item Title:" + item.Content.Type);
                    //Console.WriteLine("item Title:" + c.Text);

                    //Console.WriteLine("link:" + (item.Links.Count > 0
                    //                ? item.Links[0].Uri.AbsolutePath : ""));
                }
            }

            textBox1.Text += $"{sw.Elapsed}" + Environment.NewLine;

        }

        private async void button2_Click(object sender, EventArgs e)
        {
            string url = @"https://www.google.co.jp/alerts/feeds/09047520966360389555/18173224082898862477";

            //string body = RssReader.RSSReaderRead(url);
            //richTextBox1.Text = body;

            var response = client.GetAsync(url);
            var content = await response.Result.Content.ReadAsStringAsync();

        }
    }
}

