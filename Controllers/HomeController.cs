using Castle.ActiveRecord.Queries;
using Md_Edi.Models;
using Md_Edi.tool;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Md_Edi.Controllers
{
    public class HomeController : Controller
    {
        #region 安全验证
        /// <summary>
        /// 查询用户是否有权限
        /// 有true
        /// </summary>
        /// <returns></returns>
        public V_HC_EDI_USER authrize()
        {
            #region 用于测试可删除的

           //V_HC_EDI_USER user1 = V_HC_EDI_USER.Find(1);
            //时效
            //HttpContext.Session.Timeout = 60 * 24 * 10;
            //密码正确 设置session
            //HttpContext.Session["UserInfo"] = user1;

            #endregion
            if (HttpContext.Session["UserInfo"] != null)
            {
                V_HC_EDI_USER UserInfo = (V_HC_EDI_USER)HttpContext.Session["UserInfo"];
                if (UserInfo == null)
                {
                    return null;
                }
                return UserInfo;
            }
            else
            {
                return null;
            }
        }

        public V_HC_EDI_USER RefuseRedirect()
        {
            V_HC_EDI_USER user = authrize();
            if (user == null)
            {
                HttpContext.Response.Redirect("~/Home/Login");
                return null;
            }
            else
            {
                return user;
            }
        }

        #endregion

        #region 视图

        public ActionResult Login() {
            return View();
        }

        public ActionResult Index()
        {
            #region 验证
            RefuseRedirect();
            #endregion

            return View();
        }

        #endregion

        #region 添加

        [HttpPost]
        public string Add_HC_EDI_WLDZ(FormCollection fc)
        {
            JObject msg = new JObject();

            #region 验证
            V_HC_EDI_USER user = authrize();
            if (user == null)
            {
                msg.Add("msg", "refuse");
                return msg.ToString();
            }
            #endregion
            try
            {
                #region 获取数据
                string DataStr = fc["DataList"];

                #endregion
                #region 转化数据
                JArray DataList = (JArray)JsonConvert.DeserializeObject(DataStr);
                #endregion
                #region 处理请求
                string connStr = System.Configuration.ConfigurationManager.AppSettings["connStr"];
                SqlConnection conn = new SqlConnection(connStr);
                SqlTransaction tr = null;

                try
                {
                    conn.Open();
                    tr = conn.BeginTransaction();
                    SqlCommand scmd = new SqlCommand();
                    scmd.Connection = conn;
                    scmd.Transaction = tr;
                    foreach (JToken jt in DataList)
                    {
                        JObject jo = (JObject)jt;
                        HC_EDI_WLDZ bean = new HC_EDI_WLDZ()
                        {
                            supplyItemCode = jo["supplyItemCode"].ToString(),
                            itemDescription = jo["itemDescription"].ToString(),
                            Project = jo["Project"].ToString(),
                            ProjectName = jo["ProjectName"].ToString(),
                            demandItemCode = jo["demandItemCode"].ToString(),
                            demandItemName = jo["demandItemName"].ToString(),
                            organizationCode = jo["organizationCode"].ToString(),
                            organizationName = jo["organizationName"].ToString(),
                            reference = jo["reference"].ToString(),
                            StarDATE = DateTime.Now,
                            EndDATE = DateTime.Now,
                            flag = (long)jo["flag"],
                            QTY = (decimal)jo["QTY"]
                        };
                        string sql = "INSERT INTO HC_EDI_WLDZ(supplyItemCode, itemDescription, Project, ProjectName, demandItemCode, demandItemName, organizationCode, organizationName, StarDATE, flag, reference, QTY,CmpId)" +
                            "VALUES('" + bean.supplyItemCode + "','" + bean.itemDescription + "','" + bean.Project + "','" + bean.ProjectName + "','" + bean.demandItemCode + "','" + bean.demandItemName + "','" + bean.organizationCode + "','" + bean.organizationName + "','" + bean.StarDATE.ToString() + "'," + bean.flag + ",'" + bean.reference + "'," + bean.QTY + ", '"+user.组织编码+"')";
                        scmd.CommandText = sql;
                        int i = scmd.ExecuteNonQuery();
                        if (i != 1)
                        {
                            throw new Exception("没有保存一条数据");
                        }
                        sql = "INSERT INTO HC_EDI_WLDZ_Log(supplyItemCode, itemDescription, Project, ProjectName, demandItemCode, demandItemName, organizationCode, organizationName, StarDATE, flag, reference, QTY,ButeTime,Type,UserCode)" +
                            "VALUES('" + bean.supplyItemCode + "','" + bean.itemDescription + "','" + bean.Project + "','" + bean.ProjectName + "','" + bean.demandItemCode + "','" + bean.demandItemName + "','" + bean.organizationCode + "','" + bean.organizationName + "','" + bean.StarDATE.ToString() + "'," + bean.flag + ",'" + bean.reference + "'," + bean.QTY + ",'" + bean.StarDATE.ToString() + "','添加','"+user.用户编码+"')";
                        scmd.CommandText = sql;
                        i = scmd.ExecuteNonQuery();
                        if (i != 1)
                        {
                            throw new Exception("没有保存一条数据");
                        }
                    }
                    tr.Commit();
                }
                catch (Exception _e2)
                {
                    if (tr != null)
                    {
                        tr.Rollback();
                    }
                    msg.Add("msg", "error");
                    return msg.ToString();
                    throw;
                }
                finally
                {
                    if (conn != null && conn.State != System.Data.ConnectionState.Closed)
                    {
                        conn.Close();
                    }
                }

                #endregion
                #region 返回数据
                //JsonConvert.SerializeObject(bean).ToString().Replace("\r\n", "")
                msg.Add("msg", "OK");
                return msg.ToString();
                #endregion
            }
            catch (Exception _e)
            {
                msg.Add("msg", "error");
                throw;
            }
        }

        public string Add_Check_Excel_HC_EDI_WLDZ(FormCollection fc) {
            JObject msg = new JObject();
            #region 验证
            V_HC_EDI_USER user = authrize();
            if (user == null)
            {
                msg.Add("msg", "refuse");
                return msg.ToString();
            }
            #endregion
            try
            {
                #region 获取数据
                string DataStr = fc["DataList"];

                #endregion
                #region 转化数据
                JArray DataList = (JArray)JsonConvert.DeserializeObject(DataStr);
                #endregion
                #region 处理请求
                string connStr = System.Configuration.ConfigurationManager.AppSettings["connStr"];
                SqlConnection conn = new SqlConnection(connStr);
                SqlTransaction tr = null;
                List<HC_EDI_WLDZ> beans = new List<HC_EDI_WLDZ>();
                HC_EDI_WLDZ bean = null;
                try
                {
                    conn.Open();
                    tr = conn.BeginTransaction();
                    SqlCommand scmd = new SqlCommand();
                    scmd.Connection = conn;
                    scmd.Transaction = tr;
                    foreach (JToken jt in DataList)
                    {
                        JObject jo = (JObject)jt;
                        bean = new HC_EDI_WLDZ()
                        {
                            supplyItemCode = jo["supplyItemCode"].ToString(),
                            itemDescription = jo["itemDescription"].ToString(),
                            Project = jo["Project"].ToString(),
                            ProjectName = jo["ProjectName"].ToString(),
                            demandItemCode = jo["demandItemCode"].ToString(),
                            demandItemName = jo["demandItemName"].ToString(),
                            organizationCode = jo["organizationCode"].ToString(),
                            organizationName = jo["organizationName"].ToString(),
                            reference = jo["reference"].ToString(),
                            StarDATE = DateTime.Now,
                            EndDATE = DateTime.Now,
                            flag = (long)jo["flag"],
                            QTY = (decimal)jo["QTY"]
                        };
                        
                        string sql = "INSERT INTO HC_EDI_WLDZ(supplyItemCode, itemDescription, Project, ProjectName, demandItemCode, demandItemName, organizationCode, organizationName, StarDATE, flag, reference, QTY, CmpId)" +
                            "VALUES('" + bean.supplyItemCode + "','" + bean.itemDescription + "','" + bean.Project + "','" + bean.ProjectName + "','" + bean.demandItemCode + "','" + bean.demandItemName + "','" + bean.organizationCode + "','" + bean.organizationName + "','" + bean.StarDATE.ToString() + "'," + bean.flag + ",'" + bean.reference + "'," + bean.QTY + ",'"+user.组织编码+"')";
                        scmd.CommandText = sql;
                        int i = scmd.ExecuteNonQuery();
                        if (i != 1)
                        {
                            throw new Exception("没有保存一条数据");
                        }
                        sql = "INSERT INTO HC_EDI_WLDZ_Log(supplyItemCode, itemDescription, Project, ProjectName, demandItemCode, demandItemName, organizationCode, organizationName, StarDATE, flag, reference, QTY,ButeTime,Type,UserCode)" +
                            "VALUES('" + bean.supplyItemCode + "','" + bean.itemDescription + "','" + bean.Project + "','" + bean.ProjectName + "','" + bean.demandItemCode + "','" + bean.demandItemName + "','" + bean.organizationCode + "','" + bean.organizationName + "','" + bean.StarDATE.ToString() + "'," + bean.flag + ",'" + bean.reference + "'," + bean.QTY + ",'" + bean.StarDATE.ToString() + "','添加','"+user.用户编码+"')";
                        scmd.CommandText = sql;
                        i = scmd.ExecuteNonQuery();
                        if (i != 1)
                        {
                            throw new Exception("没有保存一条数据");
                        }
                    }
                    tr.Commit();
                }
                catch (Exception _e2)
                {
                    if (tr != null)
                    {
                        tr.Rollback();
                    }
                    msg.Add("msg", "SQLERROR");
                    msg.Add("data", JsonConvert.SerializeObject(bean).ToString().Replace("\r\n", ""));
                    return msg.ToString();
                    throw;
                }
                finally
                {
                    if (conn != null && conn.State != System.Data.ConnectionState.Closed)
                    {
                        conn.Close();
                    }
                }

                #endregion
                #region 返回数据
                //JsonConvert.SerializeObject(bean).ToString().Replace("\r\n", "")
                msg.Add("msg", "OK");
                return msg.ToString();
                #endregion
            }
            catch (Exception _e)
            {
                msg.Add("msg", "error");
                throw;
            }
        }

        #endregion

        #region 删除

        [HttpPost]
        public string Del_Check_HC_EDI_WLDZ(FormCollection fc) {
            JObject msg = new JObject();
            #region 验证
            V_HC_EDI_USER user = authrize();
            if (user == null)
            {
                msg.Add("msg", "refuse");
                return msg.ToString();
            }
            #endregion
            try
            {
                #region 获取数据
                string DataStr = fc["DataList"];

                #endregion
                #region 转化数据

                JArray DataList = (JArray)JsonConvert.DeserializeObject(DataStr);

                #endregion
                #region 处理请求

                string connStr = System.Configuration.ConfigurationManager.AppSettings["connStr"];
                SqlConnection conn = new SqlConnection(connStr);
                SqlTransaction tr = null;
                try
                {
                    conn.Open();
                    tr = conn.BeginTransaction();
                    SqlCommand scmd = new SqlCommand();
                    scmd.Connection = conn;
                    scmd.Transaction = tr;
                    foreach (JToken jt in DataList) {
                        JObject jo = (JObject)jt;
                        string supplyItemCode = jo["supplyItemCode"].ToString();
                        string Project = jo["Project"].ToString();
                        string demandItemCode = jo["demandItemCode"].ToString();
                        string organizationCode = jo["organizationCode"].ToString();
                        string sql = "UPDATE HC_EDI_WLDZ SET flag=-1 WHERE supplyItemCode = '" + supplyItemCode + "'AND Project='" + Project + "'AND demandItemCode='" + demandItemCode + "' AND organizationCode='" + organizationCode + "'";
                        scmd.CommandText = sql;
                        int i = scmd.ExecuteNonQuery();
                        if (i != 1)
                        {
                            throw new Exception("更新了" + i + "个数据");
                        }
                        sql = "INSERT INTO HC_EDI_WLDZ_Log(Type,supplyItemCode,Project,demandItemCode,organizationCode,UserCode)VALUES('删除','" + supplyItemCode + "','" + Project + "','" + demandItemCode + "','" + organizationCode + "','"+user.用户编码+"')";
                        scmd.CommandText = sql;
                        i = scmd.ExecuteNonQuery();
                        if (i != 1)
                        {
                            throw new Exception("更新了" + i + "个数据");
                        }
                    }
                    tr.Commit();
                    
                }
                catch (Exception _e2)
                {
                    if (tr != null)
                    {
                        tr.Rollback();
                    }
                    msg.Add("msg","error");
                    throw;
                }
                finally
                {
                    if (conn != null && conn.State != System.Data.ConnectionState.Closed)
                    {
                        conn.Close();
                    }
                }

                #endregion
                #region 返回数据
                //JsonConvert.SerializeObject(bean).ToString().Replace("\r\n", "")
                msg.Add("msg", "OK");
                return msg.ToString();
                #endregion
            }
            catch (Exception _e)
            {
                msg.Add("msg", "error");
                throw;
            }

        }

        #endregion

        #region 修改
        [HttpPost]
        public string Edit_HC_EDI_WLDZ(FormCollection fc)
        {
            JObject msg = new JObject();
            #region 验证
            V_HC_EDI_USER user = authrize();
            if (user == null)
            {
                msg.Add("msg", "refuse");
                return msg.ToString();
            }
            #endregion
            try
            {

                #region 获取数据

                string supplyItemCode = fc["supplyItemCode"];
                string itemDescription = fc["itemDescription"];
                string Project = fc["Project"];
                string ProjectName = fc["ProjectName"];
                string demandItemCode = fc["demandItemCode"];
                string demandItemName = fc["demandItemName"];
                string organizationCode = fc["organizationCode"];
                string organizationName = fc["organizationName"];
                string reference = fc["reference"];
                string QTY = fc["QTY"];
                string flag = fc["flag"];

                string supplyItemCode_id = fc["supplyItemCode_id"];
                string Project_id = fc["Project_id"];
                string demandItemCode_id = fc["demandItemCode_id"];
                string organizationCode_id = fc["organizationCode_id"];

                #endregion

                #region 转化数据

                decimal de_QTY = decimal.Parse(QTY);
                long de_flag = long.Parse(flag);

                #endregion

                #region 处理请求
                //save
                string connStr = System.Configuration.ConfigurationManager.AppSettings["connStr"];
                SqlConnection conn = new SqlConnection(connStr);
                SqlTransaction tr = null;
                try
                {
                    conn.Open();
                    tr = conn.BeginTransaction();
                    SqlCommand scmd = new SqlCommand();
                    scmd.Connection = conn;
                    scmd.Transaction = tr;
                    string sql = "update HC_EDI_WLDZ SET supplyItemCode='" + supplyItemCode + "',itemDescription='" + itemDescription + "',Project='" + Project + "',ProjectName='" + ProjectName + "',demandItemCode='" + demandItemCode + "',demandItemName='" + demandItemName + "',organizationCode='" + organizationCode + "',organizationName='" + organizationName + "',reference='" + reference + "',QTY=" + QTY + ",flag=" + flag + ",StarDATE='" + DateTime.Now.ToString() + "' WHERE supplyItemCode='" + supplyItemCode_id + "' AND Project='" + Project_id + "' AND demandItemCode='" + demandItemCode_id + "' AND organizationCode='" + organizationCode_id + "'";
                    scmd.CommandText = sql;
                    int i = scmd.ExecuteNonQuery();
                    if (i != 1)
                    {
                        throw new Exception("更新了" + i + "个数据");
                    }
                    tr.Commit();
                }
                catch (Exception _e2)
                {
                    if (tr != null)
                    {

                        tr.Rollback();
                    }
                    msg.Add("msg", "error");
                    //return msg.ToString();
                    throw;
                }
                finally
                {
                    if (conn != null && conn.State != System.Data.ConnectionState.Closed)
                    {
                        conn.Close();
                    }
                }
                #endregion

                #region 返回数据
                //JsonConvert.SerializeObject(bean).ToString().Replace("\r\n", "")
                msg.Add("msg", "OK");
                return msg.ToString();
                #endregion

            }
            catch (Exception _e)
            {
                msg.Add("msg", "error");
                throw;
            }
        }

        [HttpPost]
        public string Edit_Check_HC_EDI_WLDZ(FormCollection fc)
        {
            JObject msg = new JObject();
            #region 验证

            #endregion
            try
            {
                #region 获取数据
                string DataStr = fc["DataList"];
                #endregion
                #region 转化数据
                JArray DataList = (JArray)JsonConvert.DeserializeObject(DataStr);
                #endregion
                #region 处理请求

                string connStr = System.Configuration.ConfigurationManager.AppSettings["connStr"];
                SqlConnection conn = new SqlConnection(connStr);
                SqlTransaction tr = null;
                try
                {
                    conn.Open();
                    tr = conn.BeginTransaction();
                    SqlCommand scmd = new SqlCommand();
                    scmd.Connection = conn;
                    scmd.Transaction = tr;
                    foreach (JToken jt in DataList)
                    {
                        JObject jo = (JObject)jt;
                        string supplyItemCode = jo["supplyItemCode"].ToString();
                        string Project = jo["Project"].ToString();
                        string demandItemCode = jo["demandItemCode"].ToString();
                        string organizationCode = jo["organizationCode"].ToString();
                        decimal QTY = (decimal)jo["QTY"];
                        string sql = "update HC_EDI_WLDZ SET QTY="+ QTY + "WHERE supplyItemCode='"+ supplyItemCode + "' AND Project='"+ Project + "' AND demandItemCode = '"+ demandItemCode + "' AND organizationCode = '"+ organizationCode + "' ";
                        scmd.CommandText = sql;
                        int i = scmd.ExecuteNonQuery();
                        if (i != 1)
                        {
                            throw new Exception("更新了" + i + "个数据");
                        }
                    }
                    tr.Commit();
                }
                catch (Exception _e2)
                {
                    if (tr!=null) {
                        tr.Rollback();
                    }
                    msg.Add("msg", "error");
                    throw;
                }
                finally
                {
                    if (conn!=null && conn.State != System.Data.ConnectionState.Closed) {
                        conn.Close();
                    }
                }
                
                #endregion
                #region 返回数据
                //JsonConvert.SerializeObject(bean).ToString().Replace("\r\n", "")
                msg.Add("msg", "OK");
                return msg.ToString();
                #endregion
            }
            catch (Exception _e)
            {
                //Log.Error("Home/Edit_Check_HC_EDI_WLDZ", _e);
                msg.Add("msg", "error");
                throw;
            }
        }

        /// <summary>
        /// 通过Excel 修改
        /// </summary>
        /// <param name="fc"></param>
        /// <returns></returns>
        [HttpPost]
        public string Edit_Check_Excel_HC_EDI_WLDZ(FormCollection fc) {
            JObject msg = new JObject();
            #region 验证

            #endregion
            //记录多少数据没有被更新
            List<HC_EDI_WLDZ> beans = new List<HC_EDI_WLDZ>();
            try
            {
                #region 获取数据
                string DataStr = fc["DataList"];
                #endregion
                #region 转化数据
                JArray DataList = (JArray)JsonConvert.DeserializeObject(DataStr);
                #endregion
                #region 处理请求

                string connStr = System.Configuration.ConfigurationManager.AppSettings["connStr"];
                SqlConnection conn = new SqlConnection(connStr);
                SqlTransaction tr = null;
                try
                {
                    conn.Open();
                    tr = conn.BeginTransaction();
                    SqlCommand scmd = new SqlCommand();
                    scmd.Connection = conn;
                    scmd.Transaction = tr;
                    
                    foreach (JToken jt in DataList)
                    {
                        JObject jo = (JObject)jt;
                        string supplyItemCode = jo["supplyItemCode"].ToString();
                        string Project = jo["Project"].ToString();
                        string demandItemCode = jo["demandItemCode"].ToString();
                        string organizationCode = jo["organizationCode"].ToString();
                        decimal QTY = (decimal)jo["QTY"];
                        string sql = "update HC_EDI_WLDZ SET QTY=" + QTY + "WHERE supplyItemCode='" + supplyItemCode + "' AND Project='" + Project + "' AND demandItemCode = '" + demandItemCode + "' AND organizationCode = '" + organizationCode + "' ";
                        scmd.CommandText = sql;
                        int i = scmd.ExecuteNonQuery();
                        if (i > 1)
                        {
                            throw new Exception("更新了" + i + "个数据");
                        }
                        if (i==0) {
                            HC_EDI_WLDZ bean = new HC_EDI_WLDZ()
                            {
                                supplyItemCode = supplyItemCode,
                                Project = Project,
                                demandItemCode = demandItemCode,
                                organizationCode = organizationCode,
                                QTY = QTY
                            };
                            beans.Add(bean);
                        }
                    }
                    if (beans.Count>0) {
                        tr.Rollback();
                    }
                    else
                    {
                        tr.Commit();
                    }
                }
                catch (Exception _e2)
                {
                    if (tr != null)
                    {
                        tr.Rollback();
                    }
                    msg.Add("msg", "error");
                    throw;
                }
                finally
                {
                    if (conn != null && conn.State != System.Data.ConnectionState.Closed)
                    {
                        conn.Close();
                    }
                }

                #endregion
                #region 返回数据
                //
                msg.Add("msg", "OK");
                msg.Add("data", JsonConvert.SerializeObject(beans).ToString().Replace("\r\n", ""));
                return msg.ToString();
                #endregion
            }
            catch (Exception _e)
            {
                //Log.Error("Home/Edit_Check_HC_EDI_WLDZ", _e);
                msg.Add("msg", "error");
                throw;
            }
        }

        #endregion

        #region 查询
        [HttpPost]
        public string Search_HC_EDI_WLDZ(FormCollection fc)
        {
            JObject msg = new JObject();
            #region 验证
            V_HC_EDI_USER user = authrize();
            if (user == null)
            {
                msg.Add("msg", "refuse");
                return msg.ToString();
            }
            #endregion
            try
            {
                #region 获取数据
                #endregion
                #region 转化数据
                #endregion
                #region 处理请求
                string hql = "SELECT NEW HC_EDI_WLDZ(a.supplyItemCode,a.itemDescription,a.Project,a.ProjectName,a.demandItemCode,a.demandItemName,a.organizationCode,a.organizationName,a.orgName,a.StarDATE,a.EndDATE,a.flag,a.reference,a.QTY,a.CmpId,a.UserId,b.料品编码,b.结存数量,b.项目编码,b.项目名称)FROM HC_EDI_WLDZ a,V_HC_EDI_KCXCL b WHERE a.supplyItemCode = b.料品编码 and a.Project = b.项目编码 and CmpId = ?";
                SimpleQuery<HC_EDI_WLDZ> query = new SimpleQuery<HC_EDI_WLDZ>(hql,user.组织编码);
                HC_EDI_WLDZ[] beans = query.Execute();
                
                #endregion
                #region 返回数据

                msg.Add("msg", "OK");
                msg.Add("data", JsonConvert.SerializeObject(beans).ToString().Replace("\r\n", ""));

                return msg.ToString();
                #endregion
            }
            catch (Exception _e)
            {
                msg.Add("msg", "error");
                throw;
            }
        }


        #endregion

        #region 检验
        /// <summary>
        /// 验证数据的唯一性
        /// </summary>
        /// <param name="fc"></param>
        /// <returns></returns>
        [HttpPost]
        public string check_HC_EDI_WLDZ(FormCollection fc)
        {
            JObject msg = new JObject();
            #region 验证

            #endregion
            try
            {
                #region 获取数据

                string supplyItemCode = fc["supplyItemCode"];
                string Project = fc["Project"];
                string demandItemCode = fc["demandItemCode"];
                string organizationCode = fc["organizationCode"];

                #endregion
                #region 转化数据

                #endregion
                #region 处理请求

                string hql = "SELECT NEW HC_EDI_WLDZ(a.supplyItemCode)FROM HC_EDI_WLDZ a WHERE a.supplyItemCode=? AND a.Project=? AND a.demandItemCode=? AND a.organizationCode=?";
                SimpleQuery<HC_EDI_WLDZ> query = new SimpleQuery<HC_EDI_WLDZ>(hql, supplyItemCode, Project, demandItemCode, organizationCode);
                HC_EDI_WLDZ[] beans = query.Execute();

                #endregion

                #region 返回数据
                if (beans.Length > 0)
                {
                    msg.Add("msg", "NOT");
                }
                else
                {
                    msg.Add("msg", "OK");
                }
                return msg.ToString();

                #endregion
            }
            catch (Exception _e)
            {
                msg.Add("msg", "error");
                throw;
            }

        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="fc"></param>
        /// <returns></returns>
        [HttpPost]
        public string UserLogin(FormCollection fc)
        {
            JObject msg = new JObject();
            
            try
            {
                #region 获取数据

                string 用户编码 = fc["UserName"];
                string 用户密码 = fc["Password"];
                //测试
                //Crypt2 crypt = new Crypt2();
                //crypt.UnlockComponent("test");
                //crypt.HashAlgorithm = "md5";
                //crypt.EncodingMode = "base64";
                //crypt.Charset = "Unicode";
                //string str = crypt.HashStringENC(用户密码);
                string str = Md5Base64.encode(用户密码);
                //lw_utils.GetMD5("123456");

                #endregion
                #region 转化数据
                #endregion
                #region 处理请求

                string hql = "SELECT NEW  V_HC_EDI_USER(a.角色编码,a.角色名称,a.用户编码,a.用户名称,a.用户密码,a.组织编码,a.组织名称)FROM V_HC_EDI_USER a WHERE a.用户编码 = ? AND a.用户密码=? ";
                SimpleQuery<V_HC_EDI_USER> Query = new SimpleQuery<V_HC_EDI_USER>(hql,用户编码, str);
                V_HC_EDI_USER[] Beans = Query.Execute();
                if (Beans.Length>0) {
                    HttpContext.Session.Timeout = 60 * 24 * 10;
                    HttpContext.Session["UserInfo"] = Beans[0];
                    msg.Add("msg","OK");
                }
                else
                {
                    msg.Add("msg", "NOTFOUNT");
                }
                #endregion
                #region 返回数据
                //JsonConvert.SerializeObject(bean).ToString().Replace("\r\n", "")
                return msg.ToString();
                #endregion
            }
            catch (Exception _e)
            {
                msg.Add("msg", "error");
                throw;
            }
        }


        #endregion

    }
}