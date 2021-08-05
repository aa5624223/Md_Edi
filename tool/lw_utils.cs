using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Md_Edi.tool
{
    public static class lw_utils
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="encypStr"></param>
        /// <param name="charset"></param>
        /// <returns></returns>
        public static string GetMD5(string encypStr, string charset) {
            string retStr;
            MD5CryptoServiceProvider m5 = new MD5CryptoServiceProvider();
            
            //创建md5对象
            byte[] inputBye;
            byte[] outputBye;
            try
            {
                inputBye = Encoding.GetEncoding(charset).GetBytes(encypStr);
            }
            catch (Exception ex)
            {
                inputBye = Encoding.GetEncoding("GB2312").GetBytes(encypStr);
            }
            
            outputBye = m5.ComputeHash(inputBye);
            retStr = System.BitConverter.ToString(outputBye);
            retStr = retStr.Replace("-", "").ToUpper();
            return retStr;
        }
    }
}