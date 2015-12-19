using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;

namespace TradeManager.Functions
{
    public class GetWeb
    {
        //public static void GetMyWeb(string KeyWord)
        //{
        //    try {
        //        WebClient MyWebClient = new WebClient();
        //        MyWebClient.Credentials = CredentialCache.DefaultCredentials;//获取或设置用于对向Internet资源的请求进行身份验证的网络凭据。
        //        Byte[] pageData = MyWebClient.DownloadData(http://www.163.com); //从指定网站下载数据
        //        string pageHtml = Encoding.Default.GetString(pageData);  //如果获取网站页面采用的是GB2312，则使用这句              
        //        //string pageHtml = Encoding.UTF8.GetString(pageData); //如果获取网站页面采用的是UTF-8，则使用这句
        //        Console.WriteLine(pageHtml);//在控制台输入获取的内容
        //        using (StreamWriter sw = new StreamWriter("c://test//ouput.html"))//将获取的内容写入文本
        //        {
        //            sw.Write(pageHtml);
        //        }
        //        Console.ReadLine(); //让控制台暂停,否则一闪而过了               
        //        }
        //    catch(WebException webEx) {
        //        Console.WriteLine(webEx.Message.ToString());
        //    }
        //}

public static string GetPageHtml(string strLink)
        {
            // prepare the web page we will be asking for
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(strLink);
            
            Stream resStream = null;

            // used to build entire input
            StringBuilder sb = new StringBuilder();

            // used on each read operation
            byte[] buf = new byte[8192];
            string tempString = null;
            int count = 0;

            // execute the request
            try
            {
                while (true)
                {
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                    //if web server doesn't response, keep waiting for response
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        continue;
                    }

                    // read data via the response stream
                    resStream = response.GetResponseStream();

                    do
                    {
                        // fill the buffer with data
                        count = resStream.Read(buf, 0, buf.Length);

                        // make sure we read some data
                        if (count != 0)
                        {
                            // translate from bytes to UTF8 text
                            tempString = Encoding.UTF8.GetString(buf, 0, count);

                            // continue building the string
                            sb.Append(tempString);
                        }
                    }
                    while (count > 0); // any more data to read?

                    //close the current stream and release the connection
                    resStream.Close();
                    break;
                }
                string strHtml = sb.ToString();

                return strHtml;
            }
            catch
            {
                return "Well, cannot resolve the web.";
            }
            finally
            {

            }

        }




    }
}