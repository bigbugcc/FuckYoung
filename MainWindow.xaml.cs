using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using FuckYoung.Entity;
using Newtonsoft.Json;
using RestSharp;

namespace FuckYoung
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private RestClient client;
        private string iurl = "https://gs0ywi.fanqier.cn/f/rbkxkxzs";
        private string url = "";
        private string curl = "";

        public MainWindow()
        {
            InitializeComponent();
            UrlResolve();
        }

        public void UrlResolve() {
            try {
                var va = iurl.Split('/');
                curl = "/"+va[va.Length-2]+"/"+va[va.Length-1]+"";
                url = "https://" + va[va.Length - 3];
                client = new RestClient(url);
                client.UserAgent = "MicroMessenger";//模拟微信参数
            } catch (Exception e) {
                Console.WriteLine(e.Message);
            }
        }
        public IRestResponse GetCookie() {
            var req =new RestRequest(curl,Method.GET);
            req.AddHeader("accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
            req.AddHeader("user-agent", "Mozilla/5.0 (Linux; Android 10; MI 8 UD Build/QKQ1.190828.002; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/89.0.4389.72 MQQBrowser/6.2 TBS/045811 Mobile Safari/537.36 MMWEBID/7601 MicroMessenger/8.0.16.2040(0x2800103F) Process/tools WeChat/arm64 Weixin NetType/WIFI Language/zh_CN ABI/arm64");
            var rep = client.Execute(req);
            return rep; 
        }



        public Root GetForm() {
            string requestId = Guid.NewGuid().ToString();
            var cook = GetCookie().Headers.ToList().Where(s => s.Name == "Set-Cookie").FirstOrDefault().Value.ToString().Split(';');
            var req = new RestRequest("/japi"+curl, Method.GET);

            req.AddHeader("Cookie", cook[0]);
            req.AddParameter("randomItem","1");
            req.AddParameter("requestId", requestId);
            req.AddHeader("Cookie", cook[0]); 
            req.AddHeader("content-type", "application/json;charset=UTF-8");
            req.AddHeader("accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");

            var resp = client.Execute(req);

            var data = JsonConvert.DeserializeObject<Root>(resp.Content.ToString());

            data.requestId = requestId;

            return data;
        }

        public static bool IsNumberic(string str)
        {
            double vsNum;
            bool isNum;
            isNum = double.TryParse(str, System.Globalization.NumberStyles.Float,
                System.Globalization.NumberFormatInfo.InvariantInfo, out vsNum);
            return isNum;
        }

        private void btn_start_Click(object sender, RoutedEventArgs e)
        {
            //test();
            var resp = GetForm();

            var sp = new ReqRoot();
            sp.formId = resp.data._id;
            sp.requestId = resp.requestId;
            sp.salt = "";
            sp.formV = 0;
            sp.duration = 52488;
            sp.isEncrypt = false;

            var arr = resp.data.items;

            var values = new List<ValuesItem>();

            var user = new Users() { 
                Name = "吴彦龙",
                StuId = "2023205000072",
                Grade = 2020,
                DepTitle = "2020级计算机科学与技术专升本团支部"
            };

            var jstr = JsonConvert.SerializeObject(user).Replace("\"", "").Split(',');

            for (var i=1;i<arr.Count;i++) {
                var info = i == (arr.Count - 1) ? new string[3] : jstr[i - 1].Split(':');
                var d = arr[i];
                values.Add(new ValuesItem {
                    
                    definition = d._id,
                    type = d.type,
                    value = i ==( arr.Count - 1) ? "":info[1],
                    isNumber = IsNumberic(i == (arr.Count - 1) ? "N":info[1]),
                    
                });
            }

            values.LastOrDefault().selectedId = "5f0bb5cf36febe14c800003a";
            values.LastOrDefault().selectedItem = "信息学院团委";

            sp.values = values;

            var alldata = JsonConvert.SerializeObject(sp);

            var purl = "/api/f/" + sp.formId + "?requestId=" + sp.requestId;

            var req = new RestRequest(purl, Method.POST);
            //req.AddParameter("requestId",sp.requestId);

            req.AddParameter("application/json;charset=UTF-8", alldata, ParameterType.RequestBody);

            var rep = client.Execute(req);

        }
    }
}
