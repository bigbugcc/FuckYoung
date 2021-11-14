using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuckYoung.Entity
{
    public class ValuesItem
    {

        public string definition { get; set; }

        public string type { get; set; }

        public string value { get; set; }

        public bool? isNumber { get; set; }

        public string selectedId { get; set; }

        public string selectedItem { get; set; }

        public bool? isOther { get; set; }
    }

    public class ReqRoot
    {
        /// <summary>
        /// 
        /// </summary>
        public List<ValuesItem> values { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string formId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string wechat { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int duration { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string requestId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int formV { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string salt { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool? isEncrypt { get; set; }
    }

}
