using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace MyParser
{
    public partial class Form1 : Form
    {
        int countClicks = 0;
        int countNumbers = 0;
        private void GetPages(string url)
        {
            string writePath = @"C:\urls.txt";
            int i = 0;
            webBrowser1.Navigate(url);
            while (webBrowser1.ReadyState != WebBrowserReadyState.Complete)
                Application.DoEvents();
            string[] pages = new string[500];
            HtmlElementCollection theElementCollection = default(HtmlElementCollection);
            theElementCollection = webBrowser1.Document.GetElementsByTagName("a");
            foreach (HtmlElement curElement in theElementCollection)
            {
                if (curElement.GetAttribute("className").ToString().Contains("item-link item-link-visited-highlight"))
                {
                    pages[i] = curElement.GetAttribute("href");
                    i++;
                    using (StreamWriter sw = new StreamWriter(writePath, true, System.Text.Encoding.Default))
                    {
                        sw.WriteLine(curElement.GetAttribute("href"));
                    }
                }
            }
            Console.WriteLine("pages - " + i);
            GetPage(pages);
        }
        private void GetPage(string[] urls)
        {
            for (int i = 0; i < urls.Length; i++)
            {
                webBrowser1.Navigate(urls[i]);
                while (webBrowser1.ReadyState != WebBrowserReadyState.Complete)
                    Application.DoEvents();
            }
        }

        private void GetNumber(string url)
        {
            string writePath = @"C:\numbers.txt";
            HtmlElementCollection theElementCollection = default(HtmlElementCollection);
            theElementCollection = webBrowser1.Document.GetElementsByTagName("a");
            foreach (HtmlElement curElement in theElementCollection)
            {
                if (curElement.GetAttribute("className").ToString().Contains("action-show-number"))
                {
                    if (!curElement.GetAttribute("href").ToString().Contains("tel:"))
                    {
                        curElement.InvokeMember("click");
                        while (webBrowser1.ReadyState != WebBrowserReadyState.Complete)
                            Application.DoEvents();
                        countClicks++;
                    }
                    else
                    {
                        countNumbers++;
                        using (StreamWriter sw = new StreamWriter(writePath, true, System.Text.Encoding.Default))
                        {
                            sw.WriteLine(curElement.GetAttribute("href").Substring(4));
                        }
                    }
                    break;
                }
            }
        }

        private void PrintDocument(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            GetNumber(e.Url.ToString());
        }

        public Form1()
        {
            InitializeComponent();
            webBrowser1.Url = new Uri("http://google.com");
            webBrowser1.ScriptErrorsSuppressed = true;
            webBrowser1.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(PrintDocument);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            GetPages(textBox1.Text);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            webBrowser1.Navigate("https://m.avito.ru/rossiya");
            while (webBrowser1.ReadyState != WebBrowserReadyState.Complete)
                Application.DoEvents();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string str = string.Format("clicks - {0}, numbers - {1}", countClicks, countNumbers);
            Console.WriteLine(str);
        }
    }
}
