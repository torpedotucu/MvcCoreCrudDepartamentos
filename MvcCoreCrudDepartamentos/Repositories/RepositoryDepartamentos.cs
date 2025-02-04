using Microsoft.Data.SqlClient;
using MvcCoreCrudDepartamentos.Models;
using System.Data;

namespace MvcCoreCrudDepartamentos.Repositories
{
    #region PROCEDIMIENTOS ALMACENADOS
    /*
      create procedure SP_INSERT_DEPARTAMENT
(@nombre nvarchar(50),@localidad nvarchar(50))
as
	declare @nextId int
	select @nextId=MAX(DEPT_NO)+1 from DEPT
	insert into DEPT values
		(@nextId,@nombre,@localidad)
go
      */
    #endregion
    public class RepositoryDepartamentos
    {
        SqlConnection cn;
        SqlCommand com;
        SqlDataReader reader;

        public RepositoryDepartamentos()
        {
            string connectionString = @"Data Source=LOCALHOST\SQLEXPRESS;Initial Catalog=HOSPITAL;Persist Security Info=True;User ID=SA;Encrypt=True;Trust Server Certificate=True";
            this.cn=new SqlConnection(connectionString);
            this.com=new SqlCommand();
            this.com.Connection=this.cn;
        }

        public async Task<List<Departamento>> GetDepartamentosAsync()
        {
            string sql = "select * from DEPT";
            this.com.CommandType=CommandType.Text;
            this.com.CommandText=sql;

            await this.cn.OpenAsync();
            this.reader=await this.com.ExecuteReaderAsync();
            List<Departamento> departamentos = new List<Departamento>();
            while(await this.reader.ReadAsync())
            {
                Departamento dept = new Departamento
                {
                    idDepartamento=int.Parse(this.reader["DEPT_NO"].ToString()),
                    Nombre=this.reader["DNOMBRE"].ToString(),
                    Localidad=this.reader["LOC"].ToString()
                };
                departamentos.Add(dept);
            }

            await this.reader.CloseAsync();
            await this.cn.CloseAsync();

            return departamentos;
        }

        public async Task InsertDepartament(string nombre, string localidad)
        {
            string sql = "SP_INSERT_DEPARTAMENT";
            this.com.Parameters.AddWithValue("@nombre", nombre);
            this.com.Parameters.AddWithValue("@localidad", localidad);
            this.com.CommandType=CommandType.StoredProcedure;
            this.com.CommandText=sql;

            await this.cn.OpenAsync();
            await this.com.ExecuteNonQueryAsync();
            await this.cn.CloseAsync();
            this.com.Parameters.Clear();
        }

        public async Task<Departamento> FindDepartamento(int idDept)
        {
            string sql = "select * from DEPT where DEPT_NO = @iddept";
            this.com.Parameters.AddWithValue("@iddept", idDept);
            this.com.CommandType=CommandType.Text;
            this.com.CommandText=sql;

            await this.cn.OpenAsync();
            this.reader=await this.com.ExecuteReaderAsync();
            Departamento dept = new Departamento();

            await this.reader.ReadAsync();

            dept.idDepartamento=int.Parse(this.reader["DEPT_NO"].ToString());
            dept.Nombre=this.reader["DNOMBRE"].ToString();
            dept.Localidad=this.reader["LOC"].ToString();

            await this.cn.CloseAsync();
            await this.reader.CloseAsync();
            this.com.Parameters.Clear();

            return dept;
        }

        public async Task UpdateDepartamentosAsync(int idDept, string nombre, string localidad)
        {
            string sql = "UPDATE DEPT SET DNOMBRE=@nombre, LOC=@localidad WHERE DEPT_NO=@iddept";
            this.com.Parameters.AddWithValue("@nombre", nombre);
            this.com.Parameters.AddWithValue("@localidad", localidad);
            this.com.Parameters.AddWithValue("@iddept", idDept);

            this.com.CommandType=CommandType.Text;
            this.com.CommandText=sql;

            await this.cn.OpenAsync();
            await this.com.ExecuteNonQueryAsync();
            await this.cn.CloseAsync();
            this.com.Parameters.Clear();

        }
    }
}
