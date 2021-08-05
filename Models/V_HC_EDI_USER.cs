using Castle.ActiveRecord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Md_Edi.Models
{
    /// <summary>
    /// 账号表
    /// </summary>
    [ActiveRecord("V_HC_EDI_USER", DynamicInsert = true, DynamicUpdate = true)]
    public class V_HC_EDI_USER : ActiveRecordBase
    {
        #region 构造方法

        public V_HC_EDI_USER() { 
            
        }

        public V_HC_EDI_USER(string 角色编码,string 角色名称,string 用户编码,string 用户名称,string 用户密码)
        {
            this.角色编码 = 角色编码;
            this.角色名称 = 角色名称;
            this.用户编码 = 用户编码;
            this.用户名称 = 用户名称;
            this.用户密码 = 用户密码;
        }

        public V_HC_EDI_USER(string 角色编码, string 角色名称, string 用户编码, string 用户名称, string 用户密码,string 组织编码,string 组织名称)
        {
            this.角色编码 = 角色编码;
            this.角色名称 = 角色名称;
            this.用户编码 = 用户编码;
            this.用户名称 = 用户名称;
            this.用户密码 = 用户密码;
            this.组织编码 = 组织编码;
            this.组织名称 = 组织名称;
        }

        #endregion

        #region 属性

        /// <summary>
        /// 
        /// </summary>
        [PrimaryKey(PrimaryKeyType.Native)]
        public string 用户编码 { get; set; }

        /// <summary>
        /// 角色编码
        /// </summary>
        [Property()]
        public string 角色编码 { get; set; }

        /// <summary>
        /// 角色编码
        /// </summary>
        [Property()]
        public string 角色名称 { get; set; }

        /// <summary>
        /// 角色编码
        /// </summary>
        [Property()]
        public string 用户名称 { get; set; }

        /// <summary>
        /// 角色编码
        /// </summary>
        [Property()]
        public string 用户密码 { get; set; }

        /// <summary>
        /// 组织编码
        /// </summary>
        [Property()]
        public string 组织编码 { get; set; }

        /// <summary>
        /// 组织名称
        /// </summary>
        [Property()]
        public string 组织名称 { get; set; }

        #endregion

        #region 实现方法

        public static void DeleteAll()
        {
            ActiveRecordBase.DeleteAll(typeof(V_HC_EDI_USER));
        }

        public static V_HC_EDI_USER[] FindAll()
        {
            return ((V_HC_EDI_USER[])(ActiveRecordBase.FindAll(typeof(V_HC_EDI_USER))));
        }

        public static V_HC_EDI_USER Find(string 用户编码)
        {
            return ((V_HC_EDI_USER)(ActiveRecordBase.FindByPrimaryKey(typeof(V_HC_EDI_USER), 用户编码)));
        }

        #endregion
    }
}