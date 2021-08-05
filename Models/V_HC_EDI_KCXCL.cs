using Castle.ActiveRecord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Md_Edi.Models
{
    /// <summary>
    /// 视图 只和 HC_EDI_WLDZ 进行联合查询
    /// </summary>
    [ActiveRecord("V_HC_EDI_KCXCL", DynamicInsert = true, DynamicUpdate = true)]
    public class V_HC_EDI_KCXCL : ActiveRecordBase
    {

        #region 构造方法

        public V_HC_EDI_KCXCL()
        {

        }

        public V_HC_EDI_KCXCL(string 料品编码,decimal 结存数量,string 项目编码,string 项目名称) {
            this.料品编码 = 料品编码;
            this.结存数量 = 结存数量;
            this.项目编码 = 项目编码;
            this.项目名称 = 项目名称;
        }

        #endregion

        #region 属性

        [PrimaryKey(PrimaryKeyType.Native)]
        public string 料品编码 { get; set; }

        [Property()]
        public decimal 结存数量 { get; set; }

        [Property()]
        public string 项目编码 { get; set; }

        [Property()]
        public string 项目名称 { get; set; }

        #endregion

        #region 实现方法

        public static void DeleteAll()
        {
            ActiveRecordBase.DeleteAll(typeof(V_HC_EDI_KCXCL));
        }

        public static V_HC_EDI_KCXCL[] FindAll()
        {
            return ((V_HC_EDI_KCXCL[])(ActiveRecordBase.FindAll(typeof(V_HC_EDI_KCXCL))));
        }

        #endregion
    }
}