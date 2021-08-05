using Castle.ActiveRecord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Md_Edi.Models
{
    /// <summary>
    /// 现有库存表
    /// </summary>
    [ActiveRecord("HC_EDI_WLDZ", DynamicInsert = true, DynamicUpdate = true)]
    public class HC_EDI_WLDZ : ActiveRecordBase
    {
        #region 构造方法

        public HC_EDI_WLDZ() {

        }

        public HC_EDI_WLDZ(string supplyItemCode){
            this.supplyItemCode = supplyItemCode;
        }

        public HC_EDI_WLDZ(string supplyItemCode,string itemDescription,string Project,string ProjectName,string demandItemCode,string demandItemName,string organizationCode,string organizationName,string orgName,DateTime StarDATE,DateTime EndDATE,long flag,string reference,decimal QTY,string CmpId,string UserId, string 料品编码,decimal 结存数量,string 项目编码,string 项目名称) {
            this.supplyItemCode = supplyItemCode;
            this.itemDescription = itemDescription;
            this.Project = Project;
            this.ProjectName = ProjectName;
            this.demandItemCode = demandItemCode;
            this.demandItemName = demandItemName;
            this.organizationCode = organizationCode;
            this.organizationName = organizationName;
            this.orgName = orgName;
            this.StarDATE = StarDATE;
            this.EndDATE = EndDATE;
            this.flag = flag;
            this.reference = reference;
            this.QTY = QTY;
            this.CmpId = CmpId;
            this.UserId = UserId;
            this.View = new V_HC_EDI_KCXCL()
            {
                料品编码 = 料品编码,
                结存数量 = 结存数量,
                项目编码 = 项目编码,
                项目名称 = 项目名称
            };
        }

        #endregion

        #region 属性

        /// <summary>
        /// 供方物料编码 唯一
        /// </summary>
        [PrimaryKey(PrimaryKeyType.Native)]
        public string supplyItemCode { get; set; }

        /// <summary>
        /// 供方物料描述
        /// </summary>
        [Property()]
        public string itemDescription { get; set; }

        /// <summary>
        /// 项目编号 唯一
        /// </summary>
        [Property()]
        public string Project { get; set; }

        //工厂号

        /// <summary>
        /// 项目名
        /// </summary>
        [Property()]
        public string ProjectName { get; set; }

        /// <summary>
        /// 需求方物料编码 唯一
        /// </summary>
        [Property()]
        public string demandItemCode { get; set; }

        /// <summary>
        /// 需求方物料描述
        /// </summary>
        [Property()]
        public string demandItemName { get; set; }

        /// <summary>
        /// 库存组织编码 唯一
        /// </summary>
        [Property()]
        public string organizationCode { get; set; }

        /// <summary>
        /// 生产地
        /// </summary>
        [Property()]
        public string organizationName { get; set; }

        /// <summary>
        /// 不用
        /// </summary>
        [Property()]
        public string orgName { get; set; }

        /// <summary>
        /// 每次更新时 更新
        /// </summary>
        [Property()]
        public DateTime StarDATE { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Property()]
        public DateTime EndDATE { get; set; }

        /// <summary>
        /// 是否启用 启用1 不启用0
        /// 供货状态 修改后改为2
        /// </summary>
        [Property()]
        public long flag { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Property()]
        public string reference { get; set; }

        /// <summary>
        /// 确认数量
        /// </summary>
        [Property()]
        public decimal QTY { get; set; }

        /// <summary>
        /// 公司名称
        /// 2020-12-30(后添加)
        /// </summary>
        [Property()]
        public string CmpId { get; set; }

        /// <summary>
        /// 操作员 
        /// 2020-12-30(后添加)
        /// </summary>
        [Property()]
        public string UserId { get; set; }

        #endregion

        #region 额外属性

        public V_HC_EDI_KCXCL View { get; set; }

        #endregion

        #region 实现方法

        public static void DeleteAll()
        {
            ActiveRecordBase.DeleteAll(typeof(HC_EDI_WLDZ));
        }

        public static HC_EDI_WLDZ[] FindAll()
        {
            return ((HC_EDI_WLDZ[])(ActiveRecordBase.FindAll(typeof(HC_EDI_WLDZ))));
        }

        public static HC_EDI_WLDZ Find(string supplyItemCode)
        {
            return ((HC_EDI_WLDZ)(ActiveRecordBase.FindByPrimaryKey(typeof(HC_EDI_WLDZ), supplyItemCode)));
        }

        #endregion

    }
}