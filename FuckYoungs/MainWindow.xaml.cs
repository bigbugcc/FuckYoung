using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
using RestSharp;

namespace FuckYoungs
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private RestClient client;
        private string url = "http://home.yngqt.org.cn";
        public MainWindow()
        {
            InitializeComponent();

        }

        private void Init() {
            client = new RestClient(url);
            client.UserAgent = "MicroMessenger";
        }
        public static string Md5(string txt)
        {
            byte[] sor = Encoding.UTF8.GetBytes(txt);
            MD5 md5 = MD5.Create();
            byte[] result = md5.ComputeHash(sor);
            StringBuilder strbul = new StringBuilder(40);
            for (int i = 0; i < result.Length; i++)
            {
                strbul.Append(result[i].ToString("x2"));
            }
            return strbul.ToString();
        }


        private IRestResponse Login() {
            string name = "wuyanlong";
            string passowrd = "qwer1234.";

            string spa = Md5(passowrd);
            return null;
        }

        private void btn_login_Click(object sender, RoutedEventArgs e)
        {
            Login();
        }
    }
}
