using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
namespace 阿里云ASR录音文件识别
{
    public class HttpUtil
    {
    /*
     * 计算MD5+BASE64
     */
    public static string MD5Base64(string s)
    {
           
        if (s == null)
            return null;
        string encodeStr = "";

        //string 编码必须为utf-8
        
        byte[] utfBytes = Encoding.UTF8.GetBytes(s);

        MD5 mdTemp;
        try {
                mdTemp =new MD5CryptoServiceProvider();
                byte[] md5Bytes = mdTemp.ComputeHash(utfBytes);
                encodeStr = Convert.ToBase64String(md5Bytes);
        } catch (Exception ex) {
            throw new Exception("Failed to generate MD5 : " + ex.Message);
            }
        return encodeStr;
    }

        /*
         * 计算 HMAC-SHA1
         */
        public static string HMACSha1(string data, string key)
        {
            string result;
            try
            {
                HMACSHA1 hmacsha1 = new HMACSHA1(Encoding.UTF8.GetBytes(key));
                byte[] rstRes = hmacsha1.ComputeHash(Encoding.UTF8.GetBytes(data));
                result = Convert.ToBase64String(rstRes);
                
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to generate HMAC : " + ex.Message);
            }
            return result;
        }



        /*
         * 发送POST请求
         */
        public static HttpWebResponse sendPost(string url, string body, string ak_id, string ak_secret)
        {
            
            string method = "POST";
            string accept = "application/json";
            string content_type = "application/json";
            DateTime date = DateTime.Now.ToUniversalTime();
            // 1.对body做MD5+BASE64加密
            string bodyMd5 = HttpUtil.MD5Base64(body);
            string stringToSign = method + "\n" + accept + "\n" + bodyMd5 + "\n" + content_type + "\n" + date.ToString("r");
            // 2.计算 HMAC-SHA1
            string signature = HttpUtil.HMACSha1(stringToSign, ak_secret);
            // 3.得到 authorization header
            //string authHeader = "Dataplus " + ak_id + ":nEvUsOWrjsnkwZapW+VSVxqpAMU=";
            string authHeader = "Dataplus " + ak_id + ":" + signature;


            HttpWebRequest httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(url);

            httpWebRequest.Method = method;
            httpWebRequest.Accept = accept;
            httpWebRequest.ContentType = content_type;
            httpWebRequest.Date = date;
            //httpWebRequest.Headers["Date"] = date.ToString("r");
            httpWebRequest.Headers["Authorization"] = authHeader;



            byte[] data = Encoding.UTF8.GetBytes(body);
            httpWebRequest.GetRequestStream().Write(data, 0, data.Length);


            HttpWebResponse httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

            return httpResponse;
        }


        ///*
        // * GET请求
        // */

        public static string sendGet(string url, string task_id, string ak_id, string ak_secret)
        {
            string result = "";
            //BufferedReader in = null;
            
            try
            {
                Uri realUrl = new Uri(url + "/" + task_id);
                /*
                 * http header 参数
                 */
                string method = "GET";
                string accept = "application/json";
                string content_type = "application/json";
                //string path = realUrl.getFile();
                DateTime date = DateTime.Now.ToUniversalTime() ;
                // 1.对body做MD5+BASE64加密
                //string bodyMd5 = MD5Base64("");
                string stringToSign = method + "\n" + accept + "\n" + "" + "\n" + content_type + "\n" + date.ToString("r");
                // 2.计算 HMAC-SHA1
                string signature = HMACSha1(stringToSign, ak_secret);
                // 3.得到 authorization header
                string authHeader = "Dataplus " + ak_id + ":" + signature;
                // 打开和URL之间的连接
                HttpWebRequest httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(realUrl);

                httpWebRequest.Method = method;
                httpWebRequest.Accept = accept;
                httpWebRequest.ContentType = content_type;
                httpWebRequest.Date = date;
                httpWebRequest.Headers["Authorization"] = authHeader;
                HttpWebResponse httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                if (httpResponse.StatusCode == HttpStatusCode.OK)
                {
                    using (StreamReader reader = new StreamReader(httpResponse.GetResponseStream(), Encoding.UTF8))
                    {
                        result = reader.ReadToEnd();
                    }
                }
                return result;
              
            }
            catch (Exception e)
            {
                throw e;
            }
            
        }
    }
}
