using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ToolModel
{
    public class WeatherJson
    {
        public string time { set; get; }
        public CityInfo cityInfo { set; get; }

        public string date { set; get; }

        public string message { set; get; }

        public int status { set; get; }

        public DataInfo data { set; get; }

    }

    public class CityInfo
    {
        public string City { get; set; } // 请求城市  
        public string CityId { get; set; } // 请求ID  
        public string Parent { get; set; } // 上级，一般是省份  
        public string UpdateTime { get; set; } // 天气更新时间  
    }

    public class WeatherData
    {
        public string date { get; set; }
        public string ymd { get; set; }
        public string week { get; set; }
        public string sunrise { get; set; }
        public string high { get; set; }
        public string low { get; set; }
        public string sunset { get; set; }
        public double aqi { get; set; }
        public string fx { get; set; }
        public string fl { get; set; }
        public string type { get; set; }
        public string notice { get; set; }
    }


    public class DataInfo
    {
        public string shidu { set; get; }

        public double pm25 { set; get; }

        public double pm10 { set; get; }
        public string quality { set; get; }

        public string wendu { set; get; }

        public string ganmao { set; get; }

        public WeatherData yesterday { set; get; }

        public List<WeatherData> forecast { set; get; }
    }
}
