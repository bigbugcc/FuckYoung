using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuckYoung.Entity
{
    public class FormTemplate
    {
        /// <summary>
        /// 
        /// </summary>
        public int sceneId { get; set; }
    }

    public class GroupInfo
    {
        /// <summary>
        /// 
        /// </summary>
        public string groupBrandStyle { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string isImageRadioVertical { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string isShowAds { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string isShowBrand { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string isShowOwnerLogo { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string isShowReport { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<string> morePermissions { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int version { get; set; }
    }

    public class ItemsItem
    {
        /// <summary>
        /// 
        /// </summary>
        public string _id { get; set; }
        /// <summary>
        /// <p>江山就是人民，人民就是江山</p>
        /// </summary>
        public string description { get; set; }
        /// <summary>
        /// 云南师范大学“青年大学习”第十二季第六期
        /// </summary>
        public string text { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string titleAlign { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string type { get; set; }
    }

    public class Plugins
    {
    }

    public class Replies
    {
        /// <summary>
        /// 
        /// </summary>
        public int all { get; set; }
    }

    public class CheckReplies
    {
        /// <summary>
        /// 
        /// </summary>
        public string data { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string stat { get; set; }
    }

    public class ReturnCommentsItem
    {
        /// <summary>
        /// 
        /// </summary>
        public string _id { get; set; }
        /// <summary>
        /// 真有意思，快叫小伙伴们也来测下！
        /// </summary>
        public string comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<string> range { get; set; }
    }

    public class SkipUrlCondition
    {
        /// <summary>
        /// 
        /// </summary>
        public List<string> options { get; set; }
    }

    public class SmsTrigger
    {
        /// <summary>
        /// 
        /// </summary>
        public string autoSent { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string disableSmsCodeType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int smsCodeType { get; set; }
    }

    public class VerificationCode
    {
        /// <summary>
        /// 数据提交成功，请妥善保管您的核销码
        /// </summary>
        public string description { get; set; }
        /// <summary>
        /// 核销码
        /// </summary>
        public string title { get; set; }
    }

    public class Settings
    {
        /// <summary>
        /// 
        /// </summary>
        public string acceptReply { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string amountUnit { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string anonymousCollection { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string beginTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public CheckReplies checkReplies { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string collectContacts { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string collectDingtalkUserInfo { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string collectWechatUserInfo { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string conditionMsg { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<string> dailyOpenningTimes { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string dingRemind { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string dingRemindSuperAdmin { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string enableDataPush { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string endTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string evaRandom { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<string> evaRandomRules { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string limitKlmyAPP { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string limitKlmyAPPTips { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string limitOnceOnTheSameDay { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string limitOnceTimeType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string limitPerDevice { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string limitPerDingUser { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string limitPerFamaily { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string limitPerIp { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string limitPerWechatUser { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string limitSubmitByDingtalk { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string limitSubmitByWechat { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int limitTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string maxSubmitLimitText { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string needChaptcha { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string password { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string payOnline { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string paymentRemind { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string paymentRemindSuperAdmin { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<string> periodDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string periodStopDateTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<string> periodTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string periodType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<ReturnCommentsItem> returnComments { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string returnMessage { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string returnScore { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string saveLastSubmitData { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string serialNumberShow { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string showEvaluationAns { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string showResult { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string showShareAfterSubmit { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string showTest { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string skipUrl { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public SkipUrlCondition skipUrlCondition { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string smsRemindMyself { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string smsRemindOthers { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public SmsTrigger smsTrigger { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string submitAgain { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string submitShowType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string subscribeWechat { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string testItemRandom { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string useWhiteList { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public VerificationCode verificationCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string wechatRemind { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string whiteListType { get; set; }
    }

    public class Share
    {
        /// <summary>
        /// 江山就是人民，人民就是江山
        /// </summary>
        public string description { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string imgUrl { get; set; }
        /// <summary>
        /// 云南师范大学“青年大学习”第十二季第六期
        /// </summary>
        public string title { get; set; }
    }

    public class Description
    {
        /// <summary>
        /// 
        /// </summary>
        public string color { get; set; }
    }

    public class Options
    {
        /// <summary>
        /// 
        /// </summary>
        public string color { get; set; }
    }

    public class Title
    {
        /// <summary>
        /// 
        /// </summary>
        public string color { get; set; }
    }

    public class Items
    {
        /// <summary>
        /// 
        /// </summary>
        public Description description { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Options options { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Title title { get; set; }
    }

    public class Text
    {
        /// <summary>
        /// 
        /// </summary>
        public string color { get; set; }
    }

    public class Font
    {
        /// <summary>
        /// 
        /// </summary>
        public Items items { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Title title { get; set; }
    }

    public class Background
    {
        /// <summary>
        /// 
        /// </summary>
        public string color { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public float transparent { get; set; }
    }

    public class Form
    {
        /// <summary>
        /// 
        /// </summary>
        public Background background { get; set; }
    }

    public class HeadPicture
    {
        /// <summary>
        /// 
        /// </summary>
        public string url { get; set; }
    }


    public class Page
    {
        /// <summary>
        /// 
        /// </summary>
        public Background background { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int backgroundType { get; set; }
    }

    public class SubmitButton
    {
        /// <summary>
        /// 
        /// </summary>
        public Background background { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Font font { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string hidden { get; set; }
        /// <summary>
        /// 提交
        /// </summary>
        public string text { get; set; }
    }

    public class Style
    {
        /// <summary>
        /// 
        /// </summary>
        public Font font { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Form form { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public HeadPicture headPicture { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Page page { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public SubmitButton submitButton { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string theme { get; set; }
    }

    public class Data
    {
        /// <summary>
        /// 
        /// </summary>
        public string _id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int adminCheck { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int attchmentSize { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int dataIndex { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<string> dbPages { get; set; }
        /// <summary>
        /// 江山就是人民，人民就是江山
        /// </summary>
        public string description { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string domain { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<string> extendProperties { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public long firstPublishTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string folder { get; set; }
        /// <summary>
        /// [副本][副本][副本][副本]云南师范大学“青年大学习”第十二季第六期
        /// </summary>
        public string formName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public FormTemplate formTemplate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int formV { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string @group { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public GroupInfo groupInfo { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string hasMobile { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string hideEmptyItems { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string isEncrypt { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string isShowCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string isShowPay { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<ItemsItem> items { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<string> logicRules { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string maximumData { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string maximumFile { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string owner { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int pVersion { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Plugins plugins { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<string> readonlyIdList { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int remindLeftTimes { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Replies replies { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int sceneId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Settings settings { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Share share { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string shortId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string smsCodeIndex { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int status { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Style style { get; set; }
        /// <summary>
        /// 云南师范大学“青年大学习”第十二季第六期
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string verification { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int version { get; set; }
    }

    public class Status
    {
        /// <summary>
        /// 
        /// </summary>
        public int code { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string message { get; set; }
    }

    public class Root
    {
        /// <summary>
        /// 
        /// </summary>
        public Data data { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string qId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int qTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Status status { get; set; }

        public string requestId { get; set; }
    }

}
