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
using System.Windows.Data;

namespace FuckYoungs
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private RestClient client;
        private string url = "http://home.yngqt.org.cn";
        private string loginUrl = "/qndxx/login.ashx";
        private string cookie = "";
        private string studyId = "1";
        private bool IsRefresh = false;
        private bool IsStart = false;
        private Thread thread;
        private SqlSugarProvider _context = null;
        private ObservableCollection<User> users = new ObservableCollection<User>();
        private ObservableCollection<User> failed = new ObservableCollection<User>();
        private ObservableCollection<string> Log = new ObservableCollection<string>();

        public MainWindow()
        {
            InitializeComponent();
            _context = new DbContext.DbContext().Client.Context;
            Init();
            LB_LogView.ItemsSource = Log;
            Log.CollectionChanged += Log_CollectionChanged;
            users.CollectionChanged += Users_CollectionChanged;
            failed.CollectionChanged += Failed_CollectionChanged;
            GetNew();
        }

        private void Failed_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            //TB_FailedNum.Text = failed.Count.ToString();
        }

        //初始化方法
        private void Init()
        {
            client = new RestClient(url);
            _context.Queryable<User>().ToListAsync().Result.ForEach(f => users.Add(f));
            LV_UserList.ItemsSource = users;
        }

        //日志监听方法
        private void Log_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            LB_LogView.ScrollIntoView(LB_LogView.SelectedItem);
        }

        //用户组
        private void Users_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {

            if (e.Action == NotifyCollectionChangedAction.Add && !IsRefresh) {
                var user = e.NewItems[0] as User;
                var ss = _context.Insertable<User>(user).ExecuteCommand();
                if (ss == 1)
                {
                    TB_username.Text = "";
                    TB_password.Text = "";
                    Log.Add(user.txtusername + " 用户添加成功！");
                }
                else
                {
                    users.Remove(user);
                    Log.Add(user.txtusername + " 用户添加失败！");
                }
            } else if (e.Action == NotifyCollectionChangedAction.Remove && !IsRefresh) {
                var user = e.OldItems[0] as User;
                _context.Deleteable<User>().Where(u => u.Id == user.Id).ExecuteCommand();
            }
        }

        //登录
        private IRestResponse Login(User user) {

            client.UserAgent = HttpHelp.UA_PC;
            var req = new RestRequest(loginUrl, Method.POST);
            req.AddHeader("Accept", HttpHelp.Accept);
            req.AddHeader("Content-Type", HttpHelp.ContentType);
            req.AddParameter(HttpHelp.ContentType, user, ParameterType.RequestBody);

            var rep = client.ExecuteAsync(req).Result;

            if (rep.StatusCode == HttpStatusCode.OK)
            {
                var co = rep.Cookies.LastOrDefault();

                cookie = co.Name + "=" + co.Value;
            }
            else {
                Log.Add(rep.Content);
            }
            return rep;
        }

        //签到
        private HttpStatusCode CheckIn() {
            var req = new RestRequest("/qndxx/user/qiandao.ashx", Method.POST);

            req.AddHeader("Cookie", cookie);

            var rep = client.Execute(req);

            Log.Add(rep.Content);

            return rep.StatusCode;
        }

        //学习
        private HttpStatusCode Study(string cookies) {

            var req = new RestRequest("/qndxx/xuexi.ashx", Method.POST);

            req.AddHeader("Cookie", cookies);

            req.AddParameter(HttpHelp.ContentType, "{txtid:" + studyId + "}", ParameterType.RequestBody);

            var rep = client.Execute(req);

            return rep.StatusCode;
        }

        //个人学习记录
        public void StudyLog() {
            var req = new RestRequest("/qndxx/user/pc/study.aspx", Method.GET);

            req.AddHeader("Cookie", cookie);

            var rep = client.Execute(req);
        }

        //抓取最新一期学习Id
        public void GetNew() {
            Log.Add("正在抓取最新的序号.......");
            new Thread(() => {
                this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() => {
                    var req = new RestRequest("/qndxx/default.aspx", Method.GET);
                    client.UserAgent = "MicroMessenger";
                    var rep = client.ExecuteAsync(req).Result;
                    if (rep.StatusCode == HttpStatusCode.OK)
                    {
                        try
                        {
                            string pattern = @"[s][t][u][d][y][(].*?[)]+";
                            string str = rep.Content.ToString();
                            var va = Regex.Matches(str, pattern).FirstOrDefault().Value;
                            studyId = Regex.Replace(va, @"[^0-9]+", "");
                            Log.Add("成功！当前最新序号: " + studyId);
                            TB_Num.Text = studyId;
                        }
                        catch (Exception e)
                        {
                            Log.Add(e.Message);
                        }
                    }
                    else
                    {
                        Log.Add("抓取失败，请重试！");
                    }
                }));
            }).Start();
        }

        private void btn_start_Click(object sender, RoutedEventArgs e)
        {
            if (users.Count <= 0) {
                Log.Add("请先添加用户信息后再试！");
                return;
            } else if (studyId.Equals("1")) {
                Log.Add("前先抓取学习期数后再试！");
                return;
            }

            StartStatus();
            if (IsStart)
            {
                AutoResetEvent waiter = new AutoResetEvent(false);
                thread = new Thread(() =>
                {
                    this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
                    {
                        
                        foreach (var item in users)
                        {
                            waiter.WaitOne(1000);
                            var login = Login(item);
                            if (login.StatusCode == HttpStatusCode.OK)
                            {
                                waiter.WaitOne(1000);
                                //学习
                                if (Study(cookie) != HttpStatusCode.OK)
                                {
                                    Log.Add(item.txtusername + " 学习失败！");
                                }
                                else
                                {
                                    //失败
                                    failed.Add(item);
                                }
                                waiter.WaitOne(1000);
                                //签到
                                if (CB_CheckIn.IsChecked.Value && CheckIn() == HttpStatusCode.OK)
                                {
                                    Log.Add(item.txtusername + "签到成功！");
                                }

                            }
                            else
                            {
                                Log.Add(item.txtusername + " 登录失败！");
                                failed.Add(item);
                                continue;
                            }
                        }
                        waiter.Close();
                        StartStatus();
                    }));
                });
                thread.Start();
            }
        }

        private void StartStatus() {
            if (!IsStart)
            {
                btn_start.Content = "停止";
                IsStart = !IsStart;
            }
            else
            {
                btn_start.Content = "开始";
                IsStart = !IsStart;
            }

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

        private void Btn_Del_Click(object sender, RoutedEventArgs e)
        {
            var user =  LV_UserList.SelectedItem as User;
            if (user==null) {
                MessageBox.Show("请在列表中选择要删除的用户！"); 
                return;
            }
            if (_context.Queryable<User>().Any(u => u.Id == user.Id))
            {
                users.Remove(users.Where(u=>u.Id == user.Id).FirstOrDefault());
                Log.Add(user.txtusername+" 用户已删除！");
                return;
            }
            else {
                MessageBox.Show("该用户不存在！");
            }
        }

        private void btn_ClearLog_Click(object sender, RoutedEventArgs e)
        {
            Log.Clear();
            Log.Add("日志清除成功！");
        }

        private void Btn_Refresh_Click(object sender, RoutedEventArgs e)
        {
            IsRefresh = true;
            users.Clear();
            _context.Queryable<User>().ToListAsync().Result.ForEach(f => users.Add(f));
            MessageBox.Show("刷新成功！");
            IsRefresh = false;

        }

        private void btn_GetStudyId_Click(object sender, RoutedEventArgs e)
        {
            GetNew();
        }
    }
    
}
