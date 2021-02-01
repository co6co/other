using System;
using System.Collections.Generic;
using System.Text;

namespace H31SQLLibrary
{
    /// <summary>
    /// ���ݿ��������(ֻ������ҳ����,���������ݿ��������̳�)
    /// </summary>   
    internal class H31DBPage
    {
        /// <summary>
        /// ��ȡ��ҳSQL
        /// </summary>
        /// <param name="strCondition">����</param>
        /// <param name="pageSize">ÿҳ��ʾ����</param>
        /// <param name="pageIndex">�ڼ�ҳ</param>
        /// <param name="fldSort">�����ֶΣ����һ������Ҫ��д�����ǵ������磺id asc, name��</param>
        /// <param name="tblName">����</param>
        /// <param name="sort">���һ�������ֶε��������trueΪ����falseΪ����</param>
        /// <returns>�������ڷ�ҳ��SQL���</returns>
        protected string GetPagerSQL(string condition, int pageSize, int pageIndex, string fldSort,string tblName, bool sort)
        {
            string strSort = sort ? " DESC" : " ASC";

            //if (pageIndex == 1)
            //{
            //    return "select top " + pageSize.ToString() + " * from " + tblName.ToString()
            //        + ((string.IsNullOrEmpty(condition)) ? string.Empty : (" where " + condition))
            //        + " order by " + fldSort.ToString() + strSort;
            //}
            //else
            {
                StringBuilder strSql = new StringBuilder();
                strSql.AppendFormat("select top {0} * from", pageSize);
                strSql.AppendFormat(" (select top {0} {1},* from {2} ", pageSize * (pageIndex),
                    (fldSort.Substring(fldSort.LastIndexOf(',') + 1, fldSort.Length - fldSort.LastIndexOf(',') - 1)), tblName);
                if (!string.IsNullOrEmpty(condition))
                {
                    strSql.AppendFormat(" where {0} order by {1}{2})", condition, fldSort, strSort);
                }
                else
                {
                    strSql.AppendFormat(" order by {0}{1}) ", fldSort, strSort);
                }
                strSql.AppendFormat(" order by {0} ASC", fldSort);
                return strSql.ToString();
            }
        }
    }
}
