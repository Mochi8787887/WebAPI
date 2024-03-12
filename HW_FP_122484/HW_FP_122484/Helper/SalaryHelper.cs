using HW_FP.Data.TrainDB122484.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace HW_FP_122484.Helpers
{
    public class SalaryHelper
    {
        //從 EmployeeController 傳入員工編號的變數 代入下方SQL查詢
        public void CalculateSalaryBefore(string EmpNo, out int seniority, out int result)
        {
            DataTable dt = new DataTable();

            //SQL Server連線
            string connectionString = "Persist Security Info=False;User ID=122484;Password=122484;Initial Catalog=TrainDB122484;Server=10.11.37.148;";

            SqlConnection conn = new SqlConnection(connectionString);

            //開啟連線
            conn.Open();

            //查詢字串
            string selectString = "select * from Employee where EmpNo = '" + EmpNo + "'";

            SqlCommand command = new SqlCommand(selectString, conn);
            SqlDataReader dr = command.ExecuteReader();

            dt.Load(dr);

            // 需要的變數內容
            var empno = dt.Rows[0]["EmpNo"].ToString();                       //資料庫中的員工編號
            var sex = dt.Rows[0]["Sex"].ToString();                           //資料庫中的性別
            var birth = dt.Rows[0]["Birth"].ToString().Substring(0, 4);       //資料庫中的出生年分
            var nowSalary = Convert.ToInt32(dt.Rows[0]["Salary"].ToString()); //目前的薪資

            // 年資(計算方式: 用今年的年份 - 出生年分 => 作業的內容)
            seniority = Convert.ToInt32(DateTime.Now.ToString().Substring(0, 4)) - Convert.ToInt32(birth);

            // 最終計算的薪資結果
            int countSalary;

            // 員工性別是女性
            if (sex == "F")
            {
                // 員工編號為奇數
                if (Convert.ToInt32(empno.Substring(3)) % 2 != 0)
                {
                    // 是女性 且 員工編號為奇數 (1000 + 500) * 1.2
                    countSalary = seniority * 1800;
                }
                else
                {
                    // 是女性 但 員工編號為偶數 (1000 + 500)
                    countSalary = seniority * 1500;
                }
            }
            else
            {
                // 員工編號為奇數
                if (Convert.ToInt32(empno.Substring(3)) % 2 != 0)
                {
                    // 是男性 且 員工編號為奇數 1000 * 1.2
                    countSalary = seniority * 1200;
                }
                else
                {
                    // 是男性 但 員工編號為偶數 1000
                    countSalary = seniority * 1000;
                }
            }

            // 薪資低於25000
            if ((nowSalary + countSalary) < 25000)
            {
                result = 25000;
            }
            else
            {
                result = nowSalary + countSalary;
            }
        }


        public void CalculateSalaryAfter(List<string> listData, out int seniority, out int result)
        {
            // 需要的變數內容
            var empno = listData[0];                            //資料庫中的員工編號
            var sex = listData[1];                              //資料庫中的性別
            var birth = listData[2];                            //資料庫中的出生年分
            int nowSalary = Convert.ToInt32(listData[3]);       //目前的薪資

            // 年資(計算方式: 用今年的年份 - 出生年分 => 作業的內容)
            seniority = Convert.ToInt32(DateTime.Now.ToString().Substring(0, 4)) - Convert.ToInt32(birth);

            // 最終計算的薪資結果
            int countSalary;

            // 員工性別是女性
            if (sex == "F")
            {
                // 員工編號為奇數
                if (Convert.ToInt32(empno.Substring(3)) % 2 != 0)
                {
                    // 是女性 且 員工編號為奇數 (1000 + 500) * 1.2
                    countSalary = seniority * 1800;
                }
                else
                {
                    // 是女性 但 員工編號為偶數 (1000 + 500)
                    countSalary = seniority * 1500;
                }
            }
            else
            {
                // 員工編號為奇數
                if (Convert.ToInt32(empno.Substring(3)) % 2 != 0)
                {
                    // 是男性 且 員工編號為奇數 1000 * 1.2
                    countSalary = seniority * 1200;
                }
                else
                {
                    // 是男性 但 員工編號為偶數 1000
                    countSalary = seniority * 1000;
                }
            }

            // 薪資低於25000
            if ((nowSalary + countSalary) < 25000)
            {
                result = 25000;
            }
            else
            {
                result = nowSalary + countSalary;
            }
        }


    }
}