using HW_FP.Data.TrainDB122484.Data;
using HW_FP_122484.Helpers;
using HW_FP_122484.Models;
using HW_FP_122484.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace HW_FP_122484.Controllers
{
    public class AbsentController : APIControllerBase
    {
        public AbsentController(ClaimAccessor claimAccessor, TrainDB122484Context trainDB122484Context) : base(claimAccessor, trainDB122484Context)
        {
        }

        //========================【新增事假】========================
        //  http://localhost:5000/api/Absent/Insert01
        [HttpPost("Insert01")]
        public ActionResult Insert01([FromBody] Models.AbsDetail absdetail)
        {
            try
            {
                DataTable dt = new DataTable();

                // 新增資料的變數
                var EmpNo = absdetail.EmpNo;
                var AbsType = absdetail.AbsType;
                var AbsDate = absdetail.AbsDate.ToString();
                DateTime dateTimeValue = DateTime.ParseExact(AbsDate, "yyyy/M/d tt h:mm:ss", null);
                var AbsHour = absdetail.AbsHour;

                //SQL Server連線
                string connectionString = "Persist Security Info=False;User ID=122484;Password=122484;Initial Catalog=TrainDB122484;Server=10.11.37.148;";

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    //開啟連線
                    conn.Open();

                    //新增字串
                    string insertString = "insert into AbsDetail(EmpNo, AbsType, AbsDate, AbsHour) values('" + EmpNo + "', '" + AbsType + "', '" + dateTimeValue.ToString("yyyy-MM-dd HH:mm:ss") + "', '" + AbsHour + "')";
                    SqlCommand insertCommand = new SqlCommand(insertString, conn);
                    insertCommand.ExecuteNonQuery();

                    //查詢字串(回傳dt用)
                    string selectString = "select * from AbsDetail where EmpNo = '" + EmpNo + "' and AbsType = '" + AbsType + "'";
                    SqlCommand selectCommand = new SqlCommand(selectString, conn);
                    SqlDataReader drSelect = selectCommand.ExecuteReader();
                    dt.Load(drSelect);

                    return Ok(new Response(dt));
                }
            }
            catch (Exception GG)
            {
                return Ok(new Response(GG));
            }
        }


        //========================【新增病假】========================
        //  http://localhost:5000/api/Absent/Insert02Before
        [HttpPost("Insert02Before")]
        public ActionResult Insert02Before([FromBody] Models.AbsDetail absdetail)
        {
            try
            {
                DataTable dt = new DataTable();

                // 新增資料的變數
                var EmpNo = absdetail.EmpNo;
                var AbsType = absdetail.AbsType;
                var AbsDate = absdetail.AbsDate.ToString();
                DateTime dateTimeValue = DateTime.ParseExact(AbsDate, "yyyy/M/d tt h:mm:ss", null);
                var AbsHour = absdetail.AbsHour;

                //SQL Server連線
                string connectionString = "Persist Security Info=False;User ID=122484;Password=122484;Initial Catalog=TrainDB122484;Server=10.11.37.148;";

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    //開啟連線
                    conn.Open();

                    int Absent02 = _claimAccessor._settings.Absent02;
                    int Absent05 = _claimAccessor._settings.Absent05;


                    // 呼叫 Helper檢查請假規則
                    AbsentHelper absentHelper = new AbsentHelper();
                    absentHelper.AbsentYesNoBefore(EmpNo, AbsType, Absent02, Absent05, out bool canLeave);
                    //absentHelper.AbsentYesNo(listData, out bool canLeave);

                    // 請假結果可以請
                    if (canLeave == true)
                    {
                        //新增字串
                        string insertString = "insert into AbsDetail(EmpNo, AbsType, AbsDate, AbsHour) values('" + EmpNo + "', '" + AbsType + "', '" + dateTimeValue.ToString("yyyy-MM-dd HH:mm:ss") + "', '" + AbsHour + "')";
                        SqlCommand insertCommand = new SqlCommand(insertString, conn);
                        insertCommand.ExecuteNonQuery();

                        //查詢字串(回傳dt用)
                        string selectString = "select * from AbsDetail where EmpNo = '" + EmpNo + "' and AbsType = '" + AbsType + "'";
                        SqlCommand selectCommand = new SqlCommand(selectString, conn);
                        SqlDataReader drSelect = selectCommand.ExecuteReader();
                        dt.Load(drSelect);

                        return Ok(new Response(dt));
                    }
                    else
                    {
                        return Ok($"員工編號：{EmpNo}  " + "病假已請超過上限");
                    }
                }
            }
            catch (Exception GG)
            {
                return Ok(new Response(GG));
            }
        }


        //  http://localhost:5000/api/Absent/Insert02After
        [HttpPost("Insert02After")]
        public ActionResult Insert02After([FromBody] Models.AbsDetail absdetail)
        {
            try
            {
                DataTable dt = new DataTable();

                // 新增資料的變數
                var EmpNo = absdetail.EmpNo;
                var AbsType = absdetail.AbsType;

                // 查詢員工的請假狀況詳細資訊
                SelectDetail(EmpNo, out List<string> absentDetail);
                string sex = absentDetail[0];           //員工性別
                string absent02_log = absentDetail[1];  //目前病假天數
                string absent05_log = absentDetail[2];  //目前公假天數

                // 員工資訊有異常(欄位資訊有任何空值)
                if (sex == "" || absent02_log == "" || absent05_log == "")
                {
                    return Ok(new Response(dt, "員工資訊異常，檢查輸入的資訊是否錯誤"));
                }
                else
                {
                    //SQL Server連線
                    string connectionString = "Persist Security Info=False;User ID=122484;Password=122484;Initial Catalog=TrainDB122484;Server=10.11.37.148;";

                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        //開啟連線
                        conn.Open();

                        // 將員工資訊帶入 Helper 進行資料處理
                        List<string> listData = new List<string>
                        {
                            EmpNo,          //員工編號
                            AbsType,        //請假類型
                            _claimAccessor._settings.Absent02.ToString(),       //環境變數的病假天數
                            _claimAccessor._settings.Absent05.ToString(),       //環境變數的公假天數
                            sex,            //員工性別
                            absent02_log,   //目前病假天數
                            absent05_log    //目前公假天數
                        };

                        // 呼叫 Helper檢查請假規則
                        AbsentHelper absentHelper = new AbsentHelper();
                        absentHelper.AbsentYesNoAfter(listData, out bool canLeave);

                        // 請假的資訊
                        var AbsDate = absdetail.AbsDate.ToString();
                        DateTime dateTimeValue = DateTime.ParseExact(AbsDate, "yyyy/M/d tt h:mm:ss", null);
                        var AbsHour = absdetail.AbsHour;


                        // 如果可以請假
                        if (canLeave == true)
                        {
                            //新增字串
                            string insertString = "insert into AbsDetail(EmpNo, AbsType, AbsDate, AbsHour) values('" + EmpNo + "', '" + AbsType + "', '" + dateTimeValue.ToString("yyyy-MM-dd HH:mm:ss") + "', '" + AbsHour + "')";
                            SqlCommand insertCommand = new SqlCommand(insertString, conn);
                            insertCommand.ExecuteNonQuery();

                            //查詢字串(回傳dt用)
                            string selectString = "select * from AbsDetail where EmpNo = '" + EmpNo + "' and AbsType = '" + AbsType + "'";
                            SqlCommand selectCommand = new SqlCommand(selectString, conn);
                            SqlDataReader drSelect = selectCommand.ExecuteReader();
                            dt.Load(drSelect);

                            return Ok(new Response(dt));
                        }
                        else
                        {
                            return Ok(new Response(dt, $"員工編號：{EmpNo}  " + "病假已請超過上限"));
                        }
                    }
                }
            }
            catch (Exception GG)
            {
                return Ok(new Response(GG));
            }
        }


        //========================【新增公假】========================
        //  http://localhost:5000/api/Absent/Insert05Before
        [HttpPost("Insert05Before")]
        public ActionResult Insert05Before([FromBody] Models.AbsDetail absdetail)
        {
            DataTable dt = new DataTable();

            // 新增資料的變數
            var EmpNo = absdetail.EmpNo;
            var AbsType = absdetail.AbsType;
            var AbsDate = absdetail.AbsDate.ToString();
            DateTime dateTimeValue = DateTime.ParseExact(AbsDate, "yyyy/M/d tt h:mm:ss", null);
            var AbsHour = absdetail.AbsHour;

            //SQL Server連線
            string connectionString = "Persist Security Info=False;User ID=122484;Password=122484;Initial Catalog=TrainDB122484;Server=10.11.37.148;";

            SqlConnection conn = new SqlConnection(connectionString);

            //開啟連線
            conn.Open();

            int Absent02 = _claimAccessor._settings.Absent02;
            int Absent05 = _claimAccessor._settings.Absent05;

            // 呼叫 Helper檢查請假規則
            AbsentHelper absentHelper = new AbsentHelper();
            absentHelper.AbsentYesNoBefore(EmpNo, AbsType, Absent02, Absent05, out bool canLeave);

            // 請假結果可以請
            if (canLeave == true)
            {
                //新增字串
                string insertString = "insert into AbsDetail(EmpNo, AbsType, AbsDate, AbsHour) values('" + EmpNo + "', '" + AbsType + "', '" + dateTimeValue.ToString("yyyy-MM-dd HH:mm:ss") + "', '" + AbsHour + "')";
                SqlCommand insertCommand = new SqlCommand(insertString, conn);
                insertCommand.ExecuteNonQuery();

                //查詢字串(回傳dt用)
                string selectString = "select * from AbsDetail where EmpNo = '" + EmpNo + "' and AbsType = '" + AbsType + "'";
                SqlCommand selectCommand = new SqlCommand(selectString, conn);
                SqlDataReader drSelect = selectCommand.ExecuteReader();
                dt.Load(drSelect);

                return Ok(new Response(dt));
            }
            else
            {
                return Ok($"員工編號：{EmpNo}  " + "公假已請超過上限");
            }
        }


        //  http://localhost:5000/api/Absent/Insert05After
        [HttpPost("Insert05After")]
        public ActionResult Insert05After([FromBody] Models.AbsDetail absdetail)
        {
            try
            {
                DataTable dt = new DataTable();

                // 新增資料的變數
                var EmpNo = absdetail.EmpNo;
                var AbsType = absdetail.AbsType;

                // 查詢員工的請假狀況詳細資訊
                SelectDetail(EmpNo, out List<string> absentDetail);
                string sex = absentDetail[0];           //員工性別
                string absent02_log = absentDetail[1];  //目前病假天數
                string absent05_log = absentDetail[2];  //目前公假天數

                // 員工資訊有異常(欄位資訊有任何空值)
                if (sex == "" || absent02_log == "" || absent05_log == "")
                {
                    return Ok(new Response(dt, "員工資訊異常，檢查輸入的資訊是否錯誤"));
                }
                else
                {
                    //SQL Server連線
                    string connectionString = "Persist Security Info=False;User ID=122484;Password=122484;Initial Catalog=TrainDB122484;Server=10.11.37.148;";

                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        //開啟連線
                        conn.Open();

                        // 將員工資訊帶入 Helper 進行資料處理
                        List<string> listData = new List<string>
                        {
                            EmpNo,          //員工編號
                            AbsType,        //請假類型
                            _claimAccessor._settings.Absent02.ToString(),       //環境變數的病假天數
                            _claimAccessor._settings.Absent05.ToString(),       //環境變數的公假天數
                            sex,            //員工性別
                            absent02_log,   //目前病假天數
                            absent05_log    //目前公假天數
                        };

                        // 呼叫 Helper檢查請假規則
                        AbsentHelper absentHelper = new AbsentHelper();
                        absentHelper.AbsentYesNoAfter(listData, out bool canLeave);

                        // 請假的資訊
                        var AbsDate = absdetail.AbsDate.ToString();
                        DateTime dateTimeValue = DateTime.ParseExact(AbsDate, "yyyy/M/d tt h:mm:ss", null);
                        var AbsHour = absdetail.AbsHour;


                        // 如果可以請假
                        if (canLeave == true)
                        {
                            //新增字串
                            string insertString = "insert into AbsDetail(EmpNo, AbsType, AbsDate, AbsHour) values('" + EmpNo + "', '" + AbsType + "', '" + dateTimeValue.ToString("yyyy-MM-dd HH:mm:ss") + "', '" + AbsHour + "')";
                            SqlCommand insertCommand = new SqlCommand(insertString, conn);
                            insertCommand.ExecuteNonQuery();

                            //查詢字串(回傳dt用)
                            string selectString = "select * from AbsDetail where EmpNo = '" + EmpNo + "' and AbsType = '" + AbsType + "'";
                            SqlCommand selectCommand = new SqlCommand(selectString, conn);
                            SqlDataReader drSelect = selectCommand.ExecuteReader();
                            dt.Load(drSelect);

                            return Ok(new Response(dt));
                        }
                        else
                        {
                            return Ok(new Response(dt, $"員工編號：{EmpNo}  " + "公假已請超過上限"));
                        }
                    }
                }
            }
            catch (Exception GG)
            {
                return Ok(new Response(GG));
            }
        }

        //========================【讓員工查詢請了多少假】========================
        [HttpPost("SelectAbsDetail")]
        public ActionResult SelectAbsDetail([FromBody] Models.AbsDetail absdetail)
        {
            DataTable dt = new DataTable();

            // 新增資料的變數
            var EmpNo = absdetail.EmpNo;

            //SQL Server連線
            string connectionString = "Persist Security Info=False;User ID=122484;Password=122484;Initial Catalog=TrainDB122484;Server=10.11.37.148;";

            SqlConnection conn = new SqlConnection(connectionString);

            //開啟連線
            conn.Open();

            //新增字串
            string selectString = "select D.EmpNo, E.EmpName, H.AbsName, D.AbsType, AbsHour from AbsDetail D left join Absent H on D.AbsType = H.AbsType left join Employee E on D.EmpNo = E.EmpNo where D.EmpNo = '" + EmpNo + "' order by AbsType";

            //查詢字串(回傳dt用)
            SqlCommand selectCommand = new SqlCommand(selectString, conn);
            SqlDataReader drSelect = selectCommand.ExecuteReader();
            dt.Load(drSelect);

            return Ok(new Response(dt));
        }


        //========================【查詢員工的請假狀況詳細資訊】========================
        public void SelectDetail(string EmpNo, out List<string> absentDetail)
        {

            DataTable dt = new DataTable();

            //SQL Server連線
            string connectionString = "Persist Security Info=False;User ID=122484;Password=122484;Initial Catalog=TrainDB122484;Server=10.11.37.148;";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
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
                string sex = (dt.Rows.Count > 0) ? dt.Rows[0]["Sex"].ToString() : "";
                string absent02_log = (dt.Rows.Count > 0) ? dt.Rows[0]["病假筆數"].ToString() : "";
                string absent05_log = (dt.Rows.Count > 0) ? dt.Rows[0]["公假筆數"].ToString() : "";

                // 傳出 性別 病假天數 公假天數的資訊
                absentDetail = new List<string>
                {
                    sex,
                    absent02_log,
                    absent05_log
                };
            }

        }
    }
}