using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Net;
using FuckYoungs.Entity;
using FuckYoungs.Util;
using RestSharp;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System.Threading;
using SqlSugar;
using FuckYoungs.DbContext;
using System.Windows.Threading;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace FuckYoungs
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private RestClient client;
        private string url = "http://home.yngqt.org.cn";
        private string cookie = "";
        private string studyId = "88";
        private SqlSugarProvider _context = null;
        private ObservableCollection<User> users = new ObservableCollection<User>();
        private ObservableCollection<string> Log = new ObservableCollection<string>();

        public MainWindow()
        {
            InitializeComponent();
            _context = new DbContext.DbContext().Client.Context;
            Init();
            LB_LogView.ItemsSource = Log;
            Log.CollectionChanged += Log_CollectionChanged;
        }

        private void Log_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            TB_allPeple.Text = users.Count.ToString();
            //日志滚动
            LB_LogView.SelectedIndex = LB_LogView.Items.Count - 1;
            LB_LogView.ScrollIntoView(LB_LogView.SelectedItem);
        }

        private void Init() 
        {
            client = new RestClient(url);
            client.UserAgent = HttpHelp.UA_PC;
            _context.Queryable<User>().ToListAsync().Result.ForEach(f=>users.Add(f));
            TB_Num.DataContext = users.Count;
            users.CollectionChanged += Users_CollectionChanged;
            TB_allPeple.Text = users.Count.ToString();
        }

        private void Users_CollectionChanged(object sender,NotifyCollectionChangedEventArgs e)
        {
            
            if (e.Action == NotifyCollectionChangedAction.Add) {
                var user = e.NewItems[0] as User;
                var ss = _context.Insertable<User>(user).ExecuteCommand();
                if (ss == 1)
                {
                    Log.Add(user.txtusername + " 用户添加成功！");
                }
                else
                {
                    users.Remove(user);
                    Log.Add(user.txtusername + " 用户添加失败！");
                }
            } else if (e.Action == NotifyCollectionChangedAction.Remove) {
                var user = e.OldItems[0] as User;
                _context.Deleteable<User>().Where(u=>u.Id == user.Id).ExecuteCommand();
            }
        }

        //登录
        private IRestResponse Login() {
            string loginUrl = "/qndxx/login.ashx";

            var user = new User{
                txtusername = "wuyanlong",
                txtpassword = Utils.Md5("qwer1234.")
            };
            var data = JsonConvert.SerializeObject(user);


            var req = new RestRequest(loginUrl,Method.POST);
            req.AddHeader("Accept", HttpHelp.Accept);
            req.AddHeader("Content-Type", HttpHelp.ContentType);
            req.AddParameter(HttpHelp.ContentType, data, ParameterType.RequestBody);

            var rep = client.ExecuteAsync(req).Result;

            if (rep.StatusCode == HttpStatusCode.OK)
            {
                Log.Add(rep.Content);

                var co = rep.Cookies.LastOrDefault();

                cookie = co.Name + "=" + co.Value;

                GetNew();
            }
            else {
                Log.Add(rep.Content);
            }
            return rep;
        }

        //签到
        private HttpStatusCode CheckIn() {
            var req = new RestRequest("/qndxx/user/qiandao.ashx",Method.POST);

            req.AddHeader("Cookie", cookie);

            var rep = client.Execute(req);

            Log.Add(rep.Content);

            return rep.StatusCode;
        }

        //学习
        private HttpStatusCode Study() {

            var req = new RestRequest("/qndxx/xuexi.ashx", Method.POST);

            req.AddHeader("Cookie",cookie);

            req.AddParameter(HttpHelp.ContentType, "{txtid:"+studyId+"}", ParameterType.RequestBody);

            var rep = client.Execute(req);

            Log.Add(rep.Content);

            return rep.StatusCode;
        }

        //个人学习记录
        public void StudyLog() {
            var req = new RestRequest("/qndxx/user/pc/study.aspx",Method.GET);

            req.AddHeader("Cookie",cookie);

            var rep = client.Execute(req);
        }

        //抓取最新一期学习Id
        public void GetNew() {
            Thread.Sleep(1500);
            var req = new RestRequest("/qndxx/default.aspx", Method.GET);

            req.AddHeader("Cookie", cookie);
            client.UserAgent = HttpHelp.UA_Phone;
            var rep = client.ExecuteAsync(req).Result;
            if (rep.StatusCode == HttpStatusCode.OK)
            {
                try
                {
                    string pattern = @"[s][t][u][d][y][(].*?[)]+";
                    string str = rep.Content.ToString();
                    var va = Regex.Matches(str, pattern).FirstOrDefault().Value;

                    studyId = Regex.Replace(va, @"[^0-9]+", "");

                    Log.Add("最新一期序号为:" + studyId);
                    TB_Num.Text = studyId;
                }
                catch (Exception e)
                {
                    Log.Add(e.Message);
                }
            }
        }

        private void btn_start_Click(object sender, RoutedEventArgs e)
        {
            new Thread(() => { this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() => {
                var login = Login();
                if (login.StatusCode == HttpStatusCode.OK)
                {

                }
                else {
                    return;
                }

            })); 
            }).Start();
            
        }

        private void btn_dbInsert_Click(object sender, RoutedEventArgs e)
        {
            string username = TB_username.Text;
            string password = TB_password.Text;
            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
            {
                if (_context.Queryable<User>().Any(u=>u.txtusername == username)) {
                    MessageBox.Show("该用户已存在！");
                    return;
                }

                var u = new User
                {
                    Id = Guid.NewGuid().ToString(),
                    txtusername = username,
                    txtpassword = Utils.Md5(password)
                };

                users.Add(u);
            }
            else {
                MessageBox.Show("请输入用户名或密码！");
            }
        }

        private void Btn_LookAll_Click(object sender, RoutedEventArgs e)
        {
            Log.Clear();
            _context.Queryable<User>().ToListAsync().Result.ForEach(f=>Log.Add(f.txtusername));
        }

        private void Btn_Del_Click(object sender, RoutedEventArgs e)
        {
            var username =  LB_LogView.SelectedItem.ToString();
            if (string.IsNullOrEmpty(username))
                MessageBox.Show("请在下方的列表中选择要删除的用户！"); return;

            if (_context.Queryable<User>().Any(u => u.txtusername == username))
            {
                users.Remove(users.Where(u=>u.txtusername == username).FirstOrDefault());
                return;
            }
            else {
                MessageBox.Show("该用户不存在！");
            }
        }

        private void btn_ClearLog_Click(object sender, RoutedEventArgs e)
        {
            Log.Clear();
        }
    }
    
}
