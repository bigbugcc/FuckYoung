using System;
using System.Linq;
using System.Windows;
using System.Net;
using FuckYoungs.Entity;
using FuckYoungs.Util;
using RestSharp;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Threading;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using SqlSugar;
using System.Threading.Tasks;
using Color = System.Drawing.Color;
using System.Windows.Media;
using System.Windows.Controls;
using Newtonsoft.Json;

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
        private Task task = null;
        private CancellationTokenSource tokenSource;
        private SqlSugarProvider _context = null;
        private ObservableCollection<User> users = new ObservableCollection<User>();
        private ObservableCollection<User> failed = new ObservableCollection<User>();
        private Action<int> setLog = null;
        private delegate void LogAppendDelegate(Color color, string text);

        public MainWindow()
        {
            InitializeComponent();
            _context = new DbContext.DbContext().Client.Context;
            tokenSource = new CancellationTokenSource();
            Init();
            SetNotice();
            users.CollectionChanged += Users_CollectionChanged;
            failed.CollectionChanged += Failed_CollectionChanged;
            RT_Log.TextChanged += RT_Log_TextChanged;
            GetNew();


        }

        private void RT_Log_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            RT_Log.ScrollToEnd();
        }

        private void Failed_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            TB_FailedNum.Text = failed.Count.ToString();
        }

        //初始化方法
        private void Init()
        {
            client = new RestClient(url);
            _context.Queryable<User>().ToListAsync().Result.ForEach(f => users.Add(f));
            LV_UserList.ItemsSource = users;
        }

        //用户组
        private void Users_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {

            if (e.Action == NotifyCollectionChangedAction.Add && !IsRefresh)
            {
                var user = e.NewItems[0] as User;
                var ss = _context.Insertable<User>(user).ExecuteCommand();
                if (ss == 1)
                {
                    TB_username.Text = "";
                    TB_password.Text = "";
                    TB_Cookie.Text = "";
                    LogInfo(user.txtusername + " 用户添加成功！");
                }
                else
                {
                    users.Remove(user);
                    LogError(user.txtusername + " 用户添加失败！");
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove && !IsRefresh)
            {
                var user = e.OldItems[0] as User;
                _context.Deleteable<User>().Where(u => u.Id == user.Id).ExecuteCommand();
            }
        }

        //登录
        private IRestResponse Login(User user)
        {
            client.UserAgent = HttpHelp.UA_PC;
            var req = new RestRequest(loginUrl, Method.POST);

            //var data = new { txtusername:user.txtusername, txtpassword: user.txtpassword };
            req.AddHeader("Accept", HttpHelp.Accept);
            req.AddHeader("Content-Type", HttpHelp.ContentType);
            req.AddParameter(HttpHelp.ContentType,JsonConvert.SerializeObject(user) , ParameterType.RequestBody);

            var rep = client.ExecuteAsync(req).Result;

            if (rep.StatusCode == HttpStatusCode.OK)
            {
                var co = rep.Cookies.LastOrDefault();

                //网页登录接口关闭
                if (co.Name.Equals("DianCMSUser"))
                {
                    cookie = co.Value;
                }
                else {
                    cookie = null;
                }


            }
            else
            {
                LogError("请求异常！");
            }
            return rep;
        }

        //签到
        private HttpStatusCode CheckIn(string cookies)
        {
            var req = new RestRequest("/qndxx/user/qiandao.ashx", Method.POST);

            req.AddCookie("DianCMSUser", cookies);

            var rep = client.Execute(req);

            LogInfo(rep.Content);

            return rep.StatusCode;
        }

        //学习
        private HttpStatusCode Study(string cookies)
        {
            client.UserAgent = HttpHelp.UA_PC;

            var req = new RestRequest("/qndxx/xuexi.ashx", Method.POST);        

            req.AddHeader("Content-Type",HttpHelp.ContentType);

            req.AddHeader("Accept",HttpHelp.Accept);

            req.AddCookie("DianCMSUser", cookies);

            req.AddParameter(HttpHelp.ContentType, "{txtid:" + studyId + "}", ParameterType.RequestBody);

            var rep = client.Execute(req);

            return rep.StatusCode;
        }

        //个人学习记录
        public void StudyLog()
        {
            var req = new RestRequest("/qndxx/user/pc/study.aspx", Method.GET);

            req.AddHeader("Cookie", cookie);

            var rep = client.Execute(req);
        }

        //抓取最新一期学习Id
        public async Task GetNew()
        {
            LogInfo("正在抓取最新的序号.......");
            var req = new RestRequest("/qndxx/default.aspx", Method.GET);
            client.UserAgent = "MicroMessenger";
            var rep = client.ExecuteAsync(req).Result;
            if (rep.StatusCode == HttpStatusCode.OK)
            {
                try
                {
                    string pattern = @"[s][t][u][d][y][(].*?[)]+";
                    string str = rep.Content.ToString();
                    var va = Regex.Matches(str, pattern)[0].Value;
                    studyId = Regex.Replace(va, @"[^0-9]+", "");
                    LogInfo("成功！当前最新序号: " + studyId);
                    TB_Num.Text = studyId;
                }
                catch (Exception e)
                {
                    LogError(e.Message);
                }
            }
            else
            {
                LogError("抓取失败，请重试!");
            }
        }

        //日志委托附加属性
        private void LogAppend(Color color, string text)
        {
            RT_Log.Foreground = new SolidColorBrush(System.Windows.Media.Color.FromRgb(color.R, color.G, color.B));
            RT_Log.AppendText(DateTime.Now.ToString(" yyyy.MM.dd HH:mm:ss ") + text);
            RT_Log.AppendText(Environment.NewLine);

        }

        //Info
        public void LogInfo(string text)
        {
            LogAppendDelegate la = new LogAppendDelegate(LogAppend);
            RT_Log.Dispatcher.Invoke(la, Color.GreenYellow, text);
        }

        //Error
        public void LogError(string text)
        {
            LogAppendDelegate la = new LogAppendDelegate(LogAppend);
            RT_Log.Dispatcher.Invoke(la, Color.Red, text);
        }

        private void btn_start_Click(object sender, RoutedEventArgs e)
        {
            if (users.Count <= 0)
            {
                LogInfo("请先添加用户信息后再试！");
                return;
            }
            else if (studyId.Equals("1"))
            {
                LogInfo("前先抓取学习期数后再试！");
                return;
            }
            StartStatus();

            if (IsStart)
            {
                task = Start();
            }
        }

        //开始执行任务
        private async Task Start() {
            var subType = (CB_SubType.SelectedItem as ComboBoxItem).Tag;
            LogInfo("当前提交方式为："+subType);
            foreach (var item in users)
            {
                if (!IsStart) { return; }//手动中断线程
                if (Login(item).StatusCode == HttpStatusCode.OK && !string.IsNullOrEmpty(cookie)&& subType.Equals("Auto"))
                {
                    await Task.Delay(1500);
                    //学习
                    if (Study(cookie) == HttpStatusCode.OK)
                    {
                        LogInfo(item.txtusername + " 学习成功！");
                    }
                    else
                    {
                        LogInfo(item.txtusername + " 学习失败！");
                        failed.Add(item);
                    }
                    await Task.Delay(1500);
                    //签到
                    if (CB_CheckIn.IsChecked.Value && CheckIn(cookie) == HttpStatusCode.OK)
                    {
                        LogInfo(item.txtusername + " 签到成功！");
                    }
                    await Task.Delay(1500);
                }
                //使用冷Cookie进行提交
                else if (subType.Equals("Cookie")) {
                    ColCokieSub(item);
                }
                else
                {
                    LogError(item.txtusername + " 登录失败！正在尝试使用冷Cooki进行提交！");
                    ColCokieSub(item);
                    failed.Add(item);
                    continue;
                }
            }
            StartStatus();
            LogInfo(" 运行结束！");
        }

        private async Task ColCokieSub(User user) {
            string colkie = user.cookie;
            client.UserAgent = HttpHelp.UA_Phone;
            //检查Cookie是否为空
            if (!string.IsNullOrEmpty(colkie))
            {
                //学习
                await Task.Delay(1500);
                if (Study(colkie) == HttpStatusCode.OK)
                {
                    LogInfo(user.txtusername + " 学习成功！");
                }
                else
                {
                    LogInfo(user.txtusername + " 学习失败！");
                    failed.Add(user);
                }
                await Task.Delay(1500);
                //签到
                if (CB_CheckIn.IsChecked.Value && CheckIn(colkie) == HttpStatusCode.OK)
                {
                    LogInfo(user.txtusername + " 签到成功！");
                }
            }
            else
            {
                LogInfo(user.txtusername + " 暂无冷Cookie,即将跳过！");
            }
        }

        //切换按钮状态
        private async Task StartStatus()
        {
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
            string username = TB_username.Text.Trim();
            string password = TB_password.Text.Trim();
            string cookie = TB_Cookie.Text.Trim();
            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
            {
                if (_context.Queryable<User>().Any(s=>s.txtusername == username))
                {
                    MessageBox.Show("该用户已存在！");
                    return;
                }
                var u = new User
                {
                    Id = Guid.NewGuid().ToString(),
                    txtusername = username,
                    txtpassword = Utils.Md5(password),
                    cookie = cookie
                };

                users.Add(u);
            }
            else
            {
                MessageBox.Show("请输入用户名或密码！");
            }
        }

        private void Btn_Del_Click(object sender, RoutedEventArgs e)
        {
            var user = LV_UserList.SelectedItem as User;
            if (user == null)
            {
                MessageBox.Show("请在列表中选择要删除的用户！");
                return;
            }
            if (_context.Queryable<User>().Any(u => u.Id == user.Id))
            {
                users.Remove(users.Where(u => u.Id == user.Id).FirstOrDefault());
                LogInfo(user.txtusername + " 用户已删除！");
                return;
            }
            else
            {
                MessageBox.Show("该用户不存在！");
            }
        }

        private void btn_ClearLog_Click(object sender, RoutedEventArgs e)
        {
            RT_Log.Document.Blocks.Clear();
            LogInfo("日志清除成功！");
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

        //协议告知
        private void SetNotice() {
            TB_Notic.Text = "" +
                "·  以任何方式查看此项目的人或直接或间接使用该项目都应仔细阅读此声明。作者保留随时更改或补充此免责声明的权利。一旦使用并复制了此项目，则视为您已接受此免责声明。\n" +
                "·  您必须在下载后的24小时内从计算机或手机中完全删除该软件。\n" +
                "·  本软件中涉及的任何功能，仅用于测试和学习研究，禁止用于商业用途，不能保证其合法性，准确性，完整性和有效性，请根据情况自行判断。\n" +
                "·  请勿将该项目的任何内容用于商业或非法目的，否则后果自负。\n" +
                "·  任何单位或个人因下载使用而产生的任何意外、疏忽、合约毁坏、诽谤、版权或知识产权侵犯及其造成的损失 (包括但不限于直接、间接、附带或衍生的损失等)，本人不承担任何法律责任。\n" +
                "·  因项目所产生的任何法律问题(包括宪法，加法、减法、乘法、除法、剑法、拳法、脚法、指法、民法，刑法，书法，公检法，基本法，劳动法，婚姻法，输入法，没办法，国际法与文中涉及或可能涉及以及未涉及之法，各地治安管理条例)纠纷或责任本人概不负责。\n" +
                "·  若使用此程序过程中出现电脑中毒，请立即关闭该程序，并用20%高锰酸钾+75%乙醇对键盘、硬盘、电压插座、显示器、鼠标、cpu进行灌溉消毒。";

        }

        private void BlogHl_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/bigbugcc/FuckYoung/");
        }

    }

}
