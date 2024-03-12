using HW_FP_122484.Controllers;
using HW_FP_122484.Models;
using HW_FP_122484.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace HW_FP_122484.Helpers
{
    public class AbsentHelper
    {
        public readonly ClaimAccessor _claimAccessor;

        public AbsentHelper()
        {
        }

        public AbsentHelper(ClaimAccessor claimAccessor)
        {
            _claimAccessor = claimAccessor;
        }


        public void AbsentYesNoBefore(string EmpNo, string AbsType, int Absent02, int Absent05, out bool canLeave)
        {
            canLeave = true;
            DataTable dt = new DataTable();

            //SQL Server連線
            string connectionString = "Persist Security Info=False;User ID=122484;Password=122484;Initial Catalog=TrainDB122484;Server=10.11.37.148;";

            SqlConnection conn = new SqlConnection(connectionString);

            //開啟連線
            conn.Open();

            //查詢字串
            string selectString = "select D.EmpNo, E.EmpName, E.Sex, COUNT(CASE WHEN H.AbsName = '事假' THEN 1 ELSE NULL END) AS 事假筆數, COUNT(CASE WHEN H.AbsName = '病假' THEN 1 ELSE NULL END) AS 病假筆數, COUNT(CASE WHEN H.AbsName = '公假' THEN 1 ELSE NULL END) AS 公假筆數 " +
                "from AbsDetail D " +
                "left join Absent H on D.AbsType = H.AbsType " +
                "left join Employee E on D.EmpNo = E.EmpNo " +
                "where D.EmpNo = '" + EmpNo + "' GROUP BY D.EmpNo, E.EmpName, E.Sex";

            SqlCommand command = new SqlCommand(selectString, conn);
            SqlDataReader dr = command.ExecuteReader();

            dt.Load(dr);

            // 需要的變數內容
            var empno = dt.Rows[0]["EmpNo"].ToString();       //員工編號
            //var empname = dt.Rows[0]["EmpName"].ToString();   //員工姓名
            var sex = dt.Rows[0]["Sex"].ToString();           //員工性別
            //var absent01 = dt.Rows[0]["事假筆數"].ToString();
            var absent02 = dt.Rows[0]["病假筆數"].ToString();
            var absent05 = dt.Rows[0]["公假筆數"].ToString();

            //病假的天數最大值(員工編號奇數5天 偶數3天)
            int maxAbsent02 = (Convert.ToInt32(empno.Substring(3)) % 2 != 0) ? Absent02 + 2 : 3;
            //int maxAbsent02 = (Convert.ToInt32(empno.Substring(3)) % 2 != 0) ? 5 : 3;

            //公假的天數最大值(員工編號奇數5天 偶數3天)
            int maxAbsent05 = (Convert.ToInt32(empno.Substring(3)) % 2 != 0) ? Absent05 + 2 : 3;
            //int maxAbsent05 = (Convert.ToInt32(empno.Substring(3)) % 2 != 0) ? 5 : 3;

            // 員工性別是女性
            if (sex == "F")
            {
                maxAbsent02++;  //病假天數+1
            }

            // 病假(02)超過最大值
            if (AbsType == "02" && Convert.ToInt32(absent02) >= maxAbsent02)
            {
                canLeave = false;
            }

            // 公假(05)超過最大值
            if (AbsType == "05" && Convert.ToInt32(absent05) >= maxAbsent05)
            {
                canLeave = false;
            }
        }


        public void AbsentYesNoAfter(List<string> listData, out bool canLeave)
        {
            string EmpNo = listData[0];
            string AbsType = listData[1];
            int Absent02 = Convert.ToInt32(listData[2]);
            int Absent05 = Convert.ToInt32(listData[3]);
            string sex = listData[4];
            int absent02_log = Convert.ToInt32(listData[5]);
            int absent05_log = Convert.ToInt32(listData[6]);

            canLeave = true;

            //病假的天數最大值(員工編號奇數5天 偶數3天)
            int maxAbsent02 = (Convert.ToInt32(EmpNo.Substring(3)) % 2 != 0) ? Absent02 + 2 : 3;
            //int maxAbsent02 = (Convert.ToInt32(empno.Substring(3)) % 2 != 0) ? 5 : 3;

            //公假的天數最大值(員工編號奇數5天 偶數3天)
            int maxAbsent05 = (Convert.ToInt32(EmpNo.Substring(3)) % 2 != 0) ? Absent05 + 2 : 3;
            //int maxAbsent05 = (Convert.ToInt32(empno.Substring(3)) % 2 != 0) ? 5 : 3;

            // 員工性別是女性
            if (sex == "F")
            {
                maxAbsent02++;  //病假天數+1
            }

            // 病假(02)超過最大值
            if (AbsType == "02" && Convert.ToInt32(absent02_log) >= maxAbsent02)
            {
                canLeave = false;
            }

            // 公假(05)超過最大值
            if (AbsType == "05" && Convert.ToInt32(absent05_log) >= maxAbsent05)
            {
                canLeave = false;
            }
        }

    }
}