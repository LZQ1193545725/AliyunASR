using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 阿里云ASR录音文件识别
{
    public class RequestBody
    {
        public string app_key = ""; //appkey 应用的key
        public string oss_link = ""; //语音文件存储地址
        private List<validTime> valid_times = null; //有效时间段ValidTime描述,可选字段
        private string vocabulary_id = null;


        public class validTime
        {

            private int begin_time;
            private int end_time;
            private int channel_id;

            public int getBegin_time()
            {
                return begin_time;
            }

            public void setBegin_time(int begin_time)
            {
                this.begin_time = begin_time;
            }

            public int getEnd_time()
            {
                return end_time;
            }

            public void setEnd_time(int end_time)
            {
                this.end_time = end_time;
            }

            public int getChannel_id()
            {
                return channel_id;
            }

            public void setChannel_id(int channel_id)
            {
                this.channel_id = channel_id;
            }

        }

        public void setApp_key(string appKey)
        {
            app_key = appKey;
        }
        public void setFile_link(string fileLine)
        {
            oss_link = fileLine;
        }
        public void setOss_link(string fileLine)
        {
            oss_link = fileLine;
        }
        public void addValid_time(int beginTime, int endTime, int channelId)
        {

            if (valid_times == null)
            {
                valid_times = new List<validTime>();
            }

            validTime valid_time = new validTime();

            valid_time.setBegin_time(beginTime);
            valid_time.setEnd_time(endTime);
            valid_time.setChannel_id(channelId);

            valid_times.Add(valid_time);
        }

        public List<validTime> getValid_times()
        {
            return valid_times;
        }

        public string getOss_link()
        {
            return oss_link;
        }

        public string getApp_key()
        {
            return app_key;
        }

        public string getVocabulary_id()
        {
            return vocabulary_id;
        }

        public void setVocabulary_id(string vocabulary_id)
        {
            this.vocabulary_id = vocabulary_id;
        }



    }
}
