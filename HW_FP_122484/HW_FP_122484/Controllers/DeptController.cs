using HW_FP.Data.TrainDB122484.Data;
using HW_FP_122484.Models;
using HW_FP_122484.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace HW_FP_122484.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeptController : APIControllerBase
    {
        public DeptController(ClaimAccessor claimAccessor, TrainDB122484Context trainDB122484Context) : base(claimAccessor, trainDB122484Context)
        {
        }


        //================================【列出所有部門】================================
        // 列出所有部門(連線字串寫法)   http://localhost:5000/api/Dept/SelectAll
        [HttpGet("SelectAll")]
        public ActionResult SelectAllDept()
        {
            try
            {
                DataTable dt = new DataTable();

                //SQL Server連線
                string connectionString = "Persist Security Info=False;User ID=122484;Password=122484;Initial Catalog=TrainDB122484;Server=10.11.37.148;";

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    //開啟連線
                    conn.Open();

                    //查詢字串
                    string selectString = "select * from Dept order by DeptNo";

                    SqlCommand command = new SqlCommand(selectString, conn);
                    SqlDataReader dr = command.ExecuteReader();

                    dt.Load(dr);

                    //關閉 & 釋放資源
                    conn.Close();
                    conn.Dispose();

                    return Ok(new Response(dt));
                }
            }
            catch (Exception GG)
            {
                return Ok(new Response(GG));
            }
        }


        // 列出所有部門(DBcontext寫法)  http://localhost:5000/api/Dept/SelectAll_dbContext
        [HttpGet("SelectAll_dbContext")]
        public ActionResult SelectAllDept_dbContext()
        {
            try
            {
                var data = _TrainDB122484Context.Dept.ToList();
                return Ok(new Response(data));
            }
            catch (Exception GG)
            {
                return Ok(new Response(GG));
            }
        }


        //================================【新增部門】================================
        // 新增部門     http://localhost:5000/api/Dept/Insert
        [HttpPost("Insert")]
        public ActionResult Insert([FromBody] Models.Dept dept)
        {
            try
            {
                DataTable dt = new DataTable();

                // 新增資料的變數
                string deptNo = dept.DeptNo;
                string deptName = dept.DeptName;

                //SQL Server連線
                string connectionString = "Persist Security Info=False;User ID=122484;Password=122484;Initial Catalog=TrainDB122484;Server=10.11.37.148;";

                SqlConnection conn = new SqlConnection(connectionString);

                //開啟連線
                conn.Open();

                //新增字串
                string insertString = "insert into Dept (DeptNo, DeptName) values('" + deptNo + "', '" + deptName + "')";
                SqlCommand insertCommand = new SqlCommand(insertString, conn);
                insertCommand.ExecuteNonQuery();

                //查詢字串(回傳dt用)
                string selectString = "select * from Dept where DeptNo = '" + deptNo + "'";
                SqlCommand selectCommand = new SqlCommand(selectString, conn);
                SqlDataReader drSelect = selectCommand.ExecuteReader();
                dt.Load(drSelect);

                //關閉連線
                conn.Close();

                return Ok(new Response(dt));
            }
            catch (Exception GG)
            {
                return Ok(new Response(GG));
            }
        }


        // 新增部門(DBcontext寫法)     http://localhost:5000/api/Dept/Insert_dbContext
        [HttpPost("Insert_dbContext")]
        public ActionResult Insert_dbContext([FromBody] HW_FP.Data.TrainDB122484.Data.Dept dept)
        {
            try
            {
                // 新增部門
                _TrainDB122484Context.Dept.Add(dept);
                _TrainDB122484Context.SaveChanges();

                return Ok(new Response(dept));
            }
            catch (Exception GG)
            {
                return Ok(new Response(GG));
            }
        }


        //================================【刪除部門】================================
        // 刪除部門     http://localhost:5000/api/Dept/Delete
        [HttpPost("Delete")]
        public ActionResult Delete([FromBody] Models.Dept dept)
        {
            try
            {
                DataTable dt = new DataTable();

                // 刪除資料的變數
                string deptNo = dept.DeptNo;

                //SQL Server連線
                string connectionString = "Persist Security Info=False;User ID=122484;Password=122484;Initial Catalog=TrainDB122484;Server=10.11.37.148;";

                SqlConnection conn = new SqlConnection(connectionString);

                //開啟連線
                conn.Open();

                //刪除字串
                string deleteString = "delete from Dept where DeptNo = '" + deptNo + "'";
                SqlCommand deleteCommand = new SqlCommand(deleteString, conn);
                deleteCommand.ExecuteNonQuery();

                //查詢字串(回傳dt用)
                string selectString = "select * from Dept";
                SqlCommand selectCommand = new SqlCommand(selectString, conn);
                SqlDataReader drSelect = selectCommand.ExecuteReader();
                dt.Load(drSelect);

                //關閉連線
                conn.Close();

                return Ok(new Response(dt));
            }
            catch (Exception GG)
            {
                return Ok(new Response(GG));
            }
        }


        // 刪除部門(DBcontext寫法)    http://localhost:5000/api/Dept/Delete_dbContext
        [HttpPost("Delete_dbContext")]
        public ActionResult Delete_dbContext([FromBody] HW_FP.Data.TrainDB122484.Data.Dept dept)
        {
            try
            {
                // 找到要刪除的部門
                var deptValue = _TrainDB122484Context.Dept.FirstOrDefault(x => x.DeptNo == dept.DeptNo);

                if (deptValue != null)
                {
                    // 刪除部門
                    _TrainDB122484Context.Dept.Remove(deptValue);
                    _TrainDB122484Context.SaveChanges();
                    return Ok(new Response(dept));
                }
                else
                {
                    return Ok(new Response(dept, "找不到需要修改的部門"));
                    //return Ok(new Response("找不到需要修改的部門"));
                }
            }
            catch (Exception GG)
            {
                return Ok(new Response(GG));
            }
        }


        //================================【修改部門】================================
        // 修改部門     http://localhost:5000/api/Dept/Update
        [HttpPost("Update")]
        public ActionResult Update([FromBody] Models.Dept dept)
        {
            try
            {
                DataTable dt = new DataTable();

                // 修改資料的變數
                string deptNo = dept.DeptNo;
                string deptName = dept.DeptName;

                //SQL Server連線
                string connectionString = "Persist Security Info=False;User ID=122484;Password=122484;Initial Catalog=TrainDB122484;Server=10.11.37.148;";

                SqlConnection conn = new SqlConnection(connectionString);

                //開啟連線
                conn.Open();

                //修改字串
                string updateString = "update Dept set DeptName = '" + deptName + "' where DeptNo = '" + deptNo + "'";
                SqlCommand updateCommand = new SqlCommand(updateString, conn);
                updateCommand.ExecuteNonQuery();

                //查詢字串
                string selectString = "select * from Dept where DeptNo = '" + deptNo + "'";
                SqlCommand selectCommand = new SqlCommand(selectString, conn);
                SqlDataReader dr = selectCommand.ExecuteReader();
                dt.Load(dr);

                //關閉連線
                conn.Close();

                return Ok(new Response(dt));
            }
            catch (Exception GG)
            {
                return Ok(new Response(GG));
            }
        }


        // 修改部門(DBcontext寫法)    http://localhost:5000/api/Dept/Update_dbContext
        [HttpPost("Update_dbContext")]
        public ActionResult Update_dbContext([FromBody] HW_FP.Data.TrainDB122484.Data.Dept dept)
        {
            try
            {
                // 找到要修改的部門
                var deptValue = _TrainDB122484Context.Dept.FirstOrDefault(x => x.DeptNo == dept.DeptNo);

                if (deptValue != null)
                {
                    // 修改部門
                    //deptValue.DeptName = dept.DeptName;

                    _TrainDB122484Context.Update(dept);
                    _TrainDB122484Context.SaveChanges();

                    return Ok(new Response(dept));
                }
                else
                {
                    return Ok(new Response(dept, "找不到需要修改的部門"));
                    //return NotFound("找不到需要修改的部門");
                }
            }
            catch (Exception GG)
            {
                return Ok(new Response(GG));
            }

        }


        //======================【全部部門底下全部員工】======================
        // 全部部門底下全部員工   http://localhost:5000/api/Dept/SelectAllEmployee
        [HttpGet("SelectAllEmployee")]
        public ActionResult SelectAllEmployee()
        {
            try
            {
                DataTable dt = new DataTable();

                //SQL Server連線
                string connectionString = "Persist Security Info=False;User ID=122484;Password=122484;Initial Catalog=TrainDB122484;Server=10.11.37.148;";
                SqlConnection conn = new SqlConnection(connectionString);

                //開啟連線
                conn.Open();

                //查詢字串
                string selectString = "select * from Dept D left join Employee E on D.DeptNo = E.DeptNo";

                SqlCommand command = new SqlCommand(selectString, conn);
                SqlDataReader dr = command.ExecuteReader();
                dt.Load(dr);

                //關閉 & 釋放資源
                conn.Close();
                conn.Dispose();

                return Ok(new Response(dt));
            }
            catch (Exception GG)
            {
                return Ok(new Response(GG));
            }
        }


        //======================【某個部門底下全部員工】======================
        // 某個部門底下全部員工   http://localhost:5000/api/Dept/SelectOfEmployee
        [HttpPost("SelectOfEmployee")]
        public ActionResult SelectOfEmployee([FromBody] Models.Dept dept)
        {
            try
            {
                DataTable dt = new DataTable();

                //string deptValue = gg.Dept;
                string deptValue = dept.DeptNo;

                //SQL Server連線
                string connectionString = "Persist Security Info=False;User ID=122484;Password=122484;Initial Catalog=TrainDB122484;Server=10.11.37.148;";
                SqlConnection conn = new SqlConnection(connectionString);

                //開啟連線
                conn.Open();

                //查詢字串
                string selectString = "select * from Dept D left join Employee E on D.DeptNo = E.DeptNo where D.DeptNo = '" + deptValue + "'";

                SqlCommand command = new SqlCommand(selectString, conn);
                SqlDataReader dr = command.ExecuteReader();
                dt.Load(dr);

                //關閉 & 釋放資源
                conn.Close();
                conn.Dispose();

                return Ok(dt);
            }
            catch (Exception GG)
            {
                return Ok(new Response(GG));
            }
        }

    }
}
