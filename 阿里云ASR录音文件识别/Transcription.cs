using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading;

namespace 阿里云ASR录音文件识别
{
    public class Transcription
    {
        private static string url = "https://nlsapi.aliyun.com/transcriptions";
        private static RequestBody body = new RequestBody();
        private static HttpUtil request = new HttpUtil();



        public static string GetData(string oss_link)
        {
            string ak_id = ""; //数加管控台获得的accessId
            string ak_secret = ""; // 数加管控台获得的accessSecret


            body.app_key = "nls-service-telephone8khz"; //简介页面给出的Appkey
            //body.oss_link = "http://qq1193545725.oss-cn-shenzhen.aliyuncs.com/932a631c-2112-11e8-8a5b-511ac20b5043.mp3";//离线文件识别的文件url,推荐使用oss存储文件。链接大小限制为128MB
            body.oss_link = oss_link;
            //热词接口
            //使用热词需要指定Vocabulary_id字段，如何设置热词参考文档：[热词设置](~~49179~~)
            //body.setVocabulary_id("vocab_id");

            /* 获取完整识别结果，无需设置本参数！*/
            //body.addValid_time(100,2000,0);       //validtime  可选字段  设置的是语音文件中希望识别的内容,begintime,endtime以及channel
            //body.addValid_time(2000,10000,1);   //validtime  默认不设置。可选字段  设置的是语音文件中希望识别的内容,begintime,endtime以及channel
            /* 获取完整识别结果，无需设置本参数！*/

            MessageBox.Show("Recognize begin!");

            /*
            * 发送录音转写请求
            * **/
            
            string bodystring;
            bodystring = JsonConvert.SerializeObject(body);
            #region  关键json格式处理
            string[] result = bodystring.Substring(1, bodystring.Length - 2).Split(',');
            StringBuilder sb = new StringBuilder();
            foreach (var item in result)
            {
                sb.Append("\n\t" + item+",");
            }
            sb.Remove(sb.Length - 1, 1);
            sb.Append("\n}");
            string str = "{" + sb.ToString();
            #endregion
            MessageBox.Show("bodystring is:" + bodystring);
            HttpWebResponse httpResponse = HttpUtil.sendPost(url, str, ak_id, ak_secret);
            string message = "";
            if (httpResponse.StatusCode == HttpStatusCode.OK)   //200
            {
                using (StreamReader reader = new StreamReader(httpResponse.GetResponseStream(), Encoding.UTF8))
                {
                    message = reader.ReadToEnd();
                }
                MessageBox.Show("post response is:" + message);
            }
            else
            {
                MessageBox.Show("error msg: " + httpResponse.StatusCode.ToString());
            }



            /*
            * 通过TaskId获取识别结果
            * **/
            string dataresult = "";
            if (httpResponse.StatusCode == HttpStatusCode.OK)   //200
            {
                string TaskId = ((JObject)JsonConvert.DeserializeObject(message))["id"].ToString();
                string status = "RUNNING";
                
                while (status.Equals("RUNNING"))
                {
                    Thread.Sleep(10000);
                    dataresult = HttpUtil.sendGet(url, TaskId, ak_id, ak_secret);
                    status = ((JObject)JsonConvert.DeserializeObject(dataresult))["status"].ToString();
                    if (status == "SUCCEED")
                    {
                        
                        var jsondata = ((JObject)JsonConvert.DeserializeObject(dataresult))["result"];
                        dataresult = "";
                        for (int i = 0; i < jsondata.Count(); i++)
                        {
                            dataresult += jsondata[i]["text"].ToString()+"。";
                        }
                    }
                    
                }
                
                MessageBox.Show("Recognize over!");
            }
            return dataresult;
        }


    }
}
