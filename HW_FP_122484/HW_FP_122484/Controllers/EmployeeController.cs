using HW_FP.Data.TrainDB122484.Data;
using HW_FP_122484.Helpers;
using HW_FP_122484.Models;
using HW_FP_122484.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace HW_FP_122484.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : APIControllerBase
    {
        public EmployeeController(ClaimAccessor claimAccessor, TrainDB122484Context trainDB122484Context) : base(claimAccessor, trainDB122484Context)
        {
        }


        //================================【新增員工】================================
        // 新增員工     http://localhost:5000/api/Employee/Insert
        [HttpPost("Insert")]
        public ActionResult Insert([FromBody] Models.Employee employee)
        {
            try
            {
                DataTable dt = new DataTable();

                // 新增資料的變數
                var EmpNo = employee.EmpNo;
                var EmpName = employee.EmpName;
                var Sex = employee.Sex;
                var DeptNo = employee.DeptNo;
                var Birth = employee.Birth;
                var Salary = employee.Salary;

                //SQL Server連線
                string connectionString = "Persist Security Info=False;User ID=122484;Password=122484;Initial Catalog=TrainDB122484;Server=10.11.37.148;";

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    //開啟連線
                    conn.Open();

                    //新增字串
                    string insertString = "insert into Employee(EmpNo, EmpName, Sex, DeptNo, Birth, Salary) values('" + EmpNo + "', '" + EmpName + "', '" + Sex + "', '" + DeptNo + "', '" + Birth + "', '" + Salary + "')";
                    SqlCommand insertCommand = new SqlCommand(insertString, conn);
                    insertCommand.ExecuteNonQuery();

                    //查詢字串(回傳dt用)
                    string selectString = "select * from Employee where EmpNo = '" + EmpNo + "'";
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


        // 新增員工(DBcontext寫法)    http://localhost:5000/api/Employee/Insert_dbContext
        [HttpPost("Insert_dbContext")]
        public ActionResult Insert_dbContext([FromBody] HW_FP.Data.TrainDB122484.Data.Employee employee)
        {
            try
            {
                _TrainDB122484Context.Employee.Add(employee);
                _TrainDB122484Context.SaveChanges();
                return Ok(new Response(employee));
            }
            catch (Exception GG)
            {
                return Ok(new Response(GG));
            }
        }


        //================================【刪除員工】================================
        // 刪除員工的API     http://localhost:5000/api/Employee/Delete
        [HttpPost("Delete")]
        public ActionResult Delete([FromBody] Models.Employee employee)
        {
            try
            {
                DataTable dt = new DataTable();

                // 刪除資料的變數
                var EmpNo = employee.EmpNo;

                //SQL Server連線
                string connectionString = "Persist Security Info=False;User ID=122484;Password=122484;Initial Catalog=TrainDB122484;Server=10.11.37.148;";

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    //開啟連線
                    conn.Open();

                    //刪除字串
                    string insertString = "delete from Employee where EmpNo = '" + EmpNo + "'";
                    SqlCommand insertCommand = new SqlCommand(insertString, conn);
                    insertCommand.ExecuteNonQuery();

                    //查詢字串(回傳dt用)
                    string selectString = "select * from Employee";
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


        // 刪除員工(DBcontext寫法)    http://localhost:5000/api/Employee/Delete_dbContext
        [HttpPost("Delete_dbContext")]
        public ActionResult Delete_dbContext([FromBody] HW_FP.Data.TrainDB122484.Data.Employee employee)
        {
            try
            {
                var deleteEmployee = _TrainDB122484Context.Employee.FirstOrDefault(x => x.EmpNo == employee.EmpNo);

                if (deleteEmployee != null)
                {
                    _TrainDB122484Context.Employee.Remove(deleteEmployee);
                    _TrainDB122484Context.SaveChanges();
                    return Ok(new Response(employee));
                }
                else
                {
                    return Ok(new Response(employee, "找不到可以刪除的員工資料"));
                    //return NotFound("找不到可以刪除的員工資料");
                }
            }
            catch (Exception GG)
            {
                return Ok(new Response(GG));
            }
        }


        //================================【修改員工】================================
        // 修改員工     http://localhost:5000/api/Employee/Update
        [HttpPost("Update")]
        public ActionResult Update([FromBody] Models.Employee employee)
        {
            try
            {
                DataTable dt = new DataTable();

                // 新增資料的變數
                var EmpNo = employee.EmpNo;
                var DeptNo = employee.DeptNo;

                //SQL Server連線
                string connectionString = "Persist Security Info=False;User ID=122484;Password=122484;Initial Catalog=TrainDB122484;Server=10.11.37.148;";

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    //開啟連線
                    conn.Open();

                    //修改字串
                    string insertString = "update Employee set DeptNo = '" + DeptNo + "' where EmpNo = '" + EmpNo + "'";
                    SqlCommand insertCommand = new SqlCommand(insertString, conn);
                    insertCommand.ExecuteNonQuery();

                    //查詢字串(回傳dt用)
                    string selectString = "select * from Employee where EmpNo = '" + EmpNo + "'";
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


        // 修改員工(DBcontext寫法)    http://localhost:5000/api/Employee/Update_dbContext
        [HttpPost("Update_dbContext")]
        public ActionResult Update_dbContext([FromBody] HW_FP.Data.TrainDB122484.Data.Employee employee)
        {
            try
            {
                var updateEmployee = _TrainDB122484Context.Employee.FirstOrDefault(x => x.EmpNo == employee.EmpNo);

                if (updateEmployee != null)
                {
                    updateEmployee.Salary = employee.Salary;

                    _TrainDB122484Context.SaveChanges();
                    return Ok(new Response(employee));
                }
                else
                {
                    return Ok(new Response(employee, "找不到需要修改的員工資料"));
                    //return NotFound("找不到需要修改的員工資料");
                }
            }
            catch (Exception GG)
            {
                return Ok(new Response(GG));
            }
        }


        //========================【試算員工幾年後的薪資】========================
        //  http://localhost:5000/api/Employee/SalaryBefore
        [HttpPost("SalaryBefore")]
        public ActionResult SalaryBefore([FromBody] Models.Employee employee)
        {
            // 新增資料的變數
            var EmpNo = employee.EmpNo;

            // 呼叫 Helper 試算員工薪資，最後將 年資 & 薪資計算結果回傳
            SalaryHelper salaryHelper = new SalaryHelper();
            salaryHelper.CalculateSalaryBefore(EmpNo, out int seniority, out int result);

            return Ok(new Response($"員工編號：{EmpNo}  " + $"年資：{seniority}年  " + $"試算薪資：${result}"));
        }


        //  http://localhost:5000/api/Employee/SalaryAfter
        [HttpPost("SalaryAfter")]
        public ActionResult SalaryAfter([FromBody] Models.Employee employee)
        {
            // 新增資料的變數
            var EmpNo = employee.EmpNo;

            DataTable dt = new DataTable();

            //SQL Server連線
            string connectionString = "Persist Security Info=False;User ID=122484;Password=122484;Initial Catalog=TrainDB122484;Server=10.11.37.148;";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
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

                List<string> listData = new List<string>
                {
                    empno,
                    sex,
                    birth,
                    nowSalary.ToString()
                };

                // 呼叫 Helper 試算員工薪資，最後將 年資 & 薪資計算結果回傳
                SalaryHelper salaryHelper = new SalaryHelper();
                salaryHelper.CalculateSalaryAfter(listData, out int seniority, out int result);

                return Ok(new Response($"員工編號：{EmpNo}  " + $"年資：{seniority}年  " + $"試算薪資：${result}"));
            }
        }

    }
}