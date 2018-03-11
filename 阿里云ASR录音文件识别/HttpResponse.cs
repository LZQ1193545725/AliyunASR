using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 阿里云ASR录音文件识别
{
    public class HttpResponse
    {
        private int status;
        private string result;
        private string message;


        public int getStatus()
        {
            return status;
        }

        public void setStatus(int status)
        {
            this.status = status;
        }

        public string getResult()
        {
            return result;
        }

        public void setResult(string result)
        {
            this.result = result;
        }

        public string getMessage()
        {
            return message;
        }

        public void setMessage(string message)
        {
            this.message = message;
        }
    }
}
