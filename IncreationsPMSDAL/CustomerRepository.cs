using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Data;
using IncreationsPMSDomain;


namespace IncreationsPMSDAL
{
    public class CustomerRepository : BaseRepository
    {
        static string dataConnection = GetConnectionString("InCreationsConnection");
        public Result Insert(Client model)
        {
            Result res = new Result(false);
            try
            {
                using (IDbConnection connection = OpenConnection(dataConnection))
                {
                    string sql = @"INSERT INTO Client
                                   (ClientName
                                   ,ClientShortName
                                   ,ContactPerson
                                   ,Designation
                                   ,OfficeNo
                                   ,MobileNo
                                   ,Email
                                   ,Pin
                                   ,District
                                   ,Address1
                                   ,Address2
                                   ,Address3
                                   ,CreditPeriod
                                   ,CreditLimit
                                   ,CreatedBy
                                   ,CreatedDate
                                   ,OrganizationId
                                   ,isActive
                               )
                             VALUES
                                   (@ClientName
                                   ,@ClientShortName
                                   ,@ContactPerson
                                   ,@Designation
                                   ,@OfficeNo
                                   ,@MobileNo
                                   ,@Email
                                   ,@Pin
                                   ,@District
                                   ,@Address1
                                   ,@Address2
                                   ,@Address3
                                   ,@CreditPeriod
                                   ,@CreditLimit
                                   ,@CreatedBy
                                   ,getdate()
                                   ,@OrganizationId
                                   ,1);SELECT CAST(SCOPE_IDENTITY() as int);";
                    int id = connection.Query<int>(sql, model).Single();
                    if (id > 0)
                    {
                        return (new Result(true));
                    }
                }
            }
            catch (Exception ex)
            {
                return (new Result(false, ex.InnerException == null ? ex.Message : ex.InnerException.Message));
            }
            return res;
        }


        public List<Client> GetCustomer(string Customer = "")
        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {
                string query = @"select ClientId,ClientName,ContactPerson +' || '+ Designation as ContactPerson  from Client  where ClientName LIKE '%'+@Customer+'%' order by ClientName";

                   return connection.Query<Client>(query, new { Customer = Customer }).ToList();
            }
        }
        public Client GetCustomer(int Id)
        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {
                string sql = @"select * from Client where ClientId=@Id";


                var objCustomer = connection.Query<Client>(sql, new
                {
                    Id = Id
                }).First<Client>();

                return objCustomer;
            }


        }
        public Result Update(Client model)
        {
            Result res = new Result(false);
            try
            {
                using (IDbConnection connection = OpenConnection(dataConnection))
                {
                    string sql = @" update Client set
                                   ClientName=@ClientName
                                   ,ClientShortName=@ClientShortName
                                   ,ContactPerson=@ContactPerson
                                   ,Designation=@Designation
                                   ,OfficeNo=@OfficeNo
                                   ,MobileNo=@MobileNo
                                   ,Email=@Email
                                   ,pin=@pin
                                   ,District=@District
                                   ,Address1=@Address1
                                   ,Address2=@Address2
                                   ,Address3=@Address3
                                   ,CreditPeriod=@CreditPeriod
                                   ,CreditLimit=@CreditLimit WHERE ClientId=@ClientId";
                    int id = connection.Execute(sql, model);
                    if (id > 0)
                    {
                        return (new Result(true));
                    }
                }
            }
            catch (Exception ex)
            {
                return (new Result(false, ex.InnerException == null ? ex.Message : ex.InnerException.Message));
            }
            return res;
        }
        public Result Delete(Client model)
        {
            Result res = new Result(false);
            try
            {
                using (IDbConnection connection = OpenConnection(dataConnection))
                {
                    string sql = @" Delete from Client WHERE ClientId=@ClientId";
                    int id = connection.Execute(sql, model);
                    if (id > 0)
                    {
                        return (new Result(true));
                    }
                }
            }
            catch (Exception ex)
            {
                return (new Result(false, ex.InnerException == null ? ex.Message : ex.InnerException.Message));
            }
            return res;
        }
    }
}
