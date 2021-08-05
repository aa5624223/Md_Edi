using Castle.ActiveRecord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Md_Edi.Models
{
    /// <summary>
    /// 现有库存修改记录表
    /// </summary>
    [ActiveRecord("HC_EDI_WLDZ", DynamicInsert = true, DynamicUpdate = true)]
    public class HC_EDI_WLDZ_Log : ActiveRecordBase
    {
        #region 构造方法

        public HC_EDI_WLDZ_Log()
        {

        }

        public HC_EDI_WLDZ_Log(long Id,string supplyItemCode,string supplyItemCode_a,string itemDescription,string itemDescription_a,string Project,string Project_a,string ProjectName,string ProjectName_a,string demandItemCode,string demandItemCode_a,string demandItemName,string demandItemName_a,string organizationCode,string organizationCode_a,string organizationName,string organizationName_a,string orgName,string orgName_a,DateTime StarDATE,DateTime ButeTime,DateTime EndDATE,long flag,long flag_a,string reference,string reference_a,string Remark,string Type,decimal QTY,decimal QTY_a) {
            this.Id = Id;
            this.supplyItemCode = supplyItemCode;
            this.supplyItemCode_a = supplyItemCode_a;
            this.itemDescription = itemDescription;
            this.itemDescription_a = itemDescription_a;
            this.Project = Project;
            this.Project_a = Project_a;
            this.ProjectName = ProjectName;
            this.ProjectName_a = ProjectName_a;
            this.demandItemCode = demandItemCode;
            this.demandItemCode_a = demandItemCode_a;
            this.demandItemName = demandItemName;
            this.demandItemName_a = demandItemName_a;
            this.organizationCode = organizationCode;
            this.organizationCode_a = organizationCode_a;
            this.organizationName = organizationName;
            this.organizationName_a = organizationName_a;
            this.orgName = orgName;
            this.orgName_a = orgName_a;
            this.StarDATE = StarDATE;
            this.ButeTime = ButeTime;
            this.EndDATE = EndDATE;
            this.flag = flag;
            this.flag_a = flag_a;
            this.reference = reference;
            this.Remark = Remark;
            this.Type = Type;
            this.QTY = QTY;
            this.QTY_a = QTY_a;
        }

        #endregion

        #region 属性

        #region 属性

        /// <summary>
        /// 主键
        /// </summary>
        [PrimaryKey(PrimaryKeyType.Native)]
        public long Id { get; set; }

        /// <summary>
        /// 供方物料编码 唯一
        /// </summary>
        [Property()]
        public string supplyItemCode { get; set; }

        [Property()]
        public string supplyItemCode_a { get; set; }

        /// <summary>
        /// 供方物料描述
        /// </summary>
        [Property()]
        public string itemDescription { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Property()]
        public string itemDescription_a { get; set; }

        /// <summary>
        /// 项目编号 唯一
        /// </summary>
        [Property()]
        public string Project { get; set; }

        /// <summary>
        /// 项目编号 唯一
        /// </summary>
        [Property()]
        public string Project_a { get; set; }

        /// <summary>
        /// 项目名
        /// </summary>
        [Property()]
        public string ProjectName { get; set; }

        /// <summary>
        /// 项目名
        /// </summary>
        [Property()]
        public string ProjectName_a { get; set; }

        /// <summary>
        /// 需求方物料编码 唯一
        /// </summary>
        [Property()]
        public string demandItemCode { get; set; }

        /// <summary>
        /// 需求方物料编码 唯一
        /// </summary>
        [Property()]
        public string demandItemCode_a { get; set; }

        /// <summary>
        /// 需求方物料描述
        /// </summary>
        [Property()]
        public string demandItemName { get; set; }

        /// <summary>
        /// 需求方物料描述
        /// </summary>
        [Property()]
        public string demandItemName_a { get; set; }

        /// <summary>
        /// 库存组织编码 唯一
        /// </summary>
        [Property()]
        public string organizationCode { get; set; }

        /// <summary>
        /// 库存组织编码 唯一
        /// </summary>
        [Property()]
        public string organizationCode_a { get; set; }

        /// <summary>
        /// 生产地
        /// </summary>
        [Property()]
        public string organizationName { get; set; }

        /// <summary>
        /// 生产地
        /// </summary>
        [Property()]
        public string organizationName_a { get; set; }

        /// <summary>
        /// 不用
        /// </summary>
        [Property()]
        public string orgName { get; set; }

        /// <summary>
        /// 不用
        /// </summary>
        [Property()]
        public string orgName_a { get; set; }

        /// <summary>
        /// 每次更新时 更新
        /// </summary>
        [Property()]
        public DateTime StarDATE { get; set; }

        /// <summary>
        /// 每次更新时 更新
        /// </summary>
        [Property()]
        public DateTime ButeTime { get; set; }

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
        /// 是否启用 启用1 不启用0
        /// 供货状态 修改后改为2
        /// </summary>
        [Property()]
        public long flag_a { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Property()]
        public string reference { get; set; }

        /// <summary>
        /// 修改备注
        /// </summary>
        [Property()]
        public string Remark { get; set; }

        /// <summary>
        /// 修改类型
        /// </summary>
        [Property()]
        public string Type { get; set; }

        /// <summary>
        /// 确认数量
        /// </summary>
        [Property()]
        public decimal QTY { get; set; }

        /// <summary>
        /// 确认数量
        /// </summary>
        [Property()]
        public decimal QTY_a { get; set; }

        /// <summary>
        /// 操作人
        /// </summary>
        [Property()]
        public string UserCode { get; set; }

        #endregion

        #endregion

        #region 构造方法

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