using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IncreationsPMSDomain;
using System.Data;
using Dapper;


namespace IncreationsPMSDAL
{
    public class SubContractorRepository : BaseRepository
    {
        static string dataConnection = GetConnectionString("InCreationsConnection");
        public string ConnectionString()
        {
            return dataConnection;
        }

               public SubContractor Insert(SubContractor objSubContractor)
        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {
                var result = new SubContractor();

                IDbTransaction trn = connection.BeginTransaction();

                string sql = @"INSERT INTO SubContractor(SubRefNo,SubName,ContactPerson,Designation,OfficeNo,MobileNo,Email,Address1,Address2,Address3,Address4)
                VALUES(@SubRefNo,@SubName,@ContactPerson,@Designation,@OfficeNo,@MobileNo,@Email,@Address1,@Address2,@Address3,@Address4);
                SELECT CAST(SCOPE_IDENTITY() as int)";
                try
                {
                    int internalid = DatabaseCommonRepository.GetInternalIDFromDatabase(connection, trn, typeof(SubContractor).Name, "0", 1);
                    objSubContractor.SubRefNo = internalid.ToString();

                    int id = connection.Query<int>(sql, objSubContractor, trn).Single();
                    objSubContractor.SubContractorId = id;
                   //InsertLoginHistory(dataConnection, objSubContractor.CreatedBy, "Create", "Designation", id.ToString(), "0");
                    trn.Commit();
                }
                catch (Exception ex)
                {
                    trn.Rollback();
                    objSubContractor.SubContractorId = 0;
                    objSubContractor.SubRefNo = null;
                }
                return objSubContractor;
            }
        }
                        //public List<Customer> GetCustomer(string Customer = "")
        //{
        //    using (IDbConnection connection = OpenConnection(dataConnection))
        //    {
        //        string query = @"select C.CusId, C.CusName, C.CusShortName,
        //        R.RegionName, SM.SalesMgName, CC.CusCategory, S.StateName,
        //        CT.CountryName, C.ContactName, C.Designation, C.OfficeNo, C.MobileNo,
        //        C.EmailId
        //        from Customer C
        //        left join Region R on R.RegionId = C.RegionId
        //        left join SalesManager SM on SM.SalesMgId = C.SalesMgId
        //        left join CustomerCategory CC on CC.CusCatId = C.CusCatId
        //        left join [State] S on S.StateId = C.StateId
        //        left join Country CT on CT.CountryId = C.CountryId
        //        where C.CusName LIKE '%'+@Customer+'%'
        //        order by C.CusName";
        //        return connection.Query<Customer>(query, new { Customer = Customer }).ToList();
        //    }
        //}
        //public Customer GetCustomer(int Id)
        //{
        //    using (IDbConnection connection = OpenConnection(dataConnection))
        //    {
        //        string sql = @"select * from Customer where CusId=@Id";


        //        var objCustomer = connection.Query<Customer>(sql, new
        //        {
        //            Id = Id
        //        }).First<Customer>();

        //        return objCustomer;
        //    }


        //}
        //public Result Update(Customer model)
        //{
        //    Result res = new Result(false);
        //    try
        //    {
        //        using (IDbConnection connection = OpenConnection(dataConnection))
        //        {
        //            string sql = @" UPDATE Customer SET 
        //                           CusName=@CusName
        //                           ,CusPrefix=@CusPrefix
        //                           ,RegionId=@RegionId
        //                           ,SalesMgId=@SalesMgId
        //                           ,CusCatId=@CusCatId
        //                           ,PoBox=@PoBox
        //                           ,CrNo=@CrNo
        //                           ,ContactName=@ContactName
        //                           ,Designation=@Designation
        //                           ,OfficeNo=@OfficeNo
        //                           ,MobileNo=@MobileNo
        //                           ,EmailId=@EmailId
        //                           ,StateId=@StateId
        //                           ,CountryId=@CountryId
        //                           ,Address1=@Address1
        //                           ,Address2=@Address2
        //                           ,Address3=@Address3
        //                           ,CreditPeriod=@CreditPeriod
        //                           ,CreditPeriod2=@CreditPeriod2
        //                           ,CreditPeriod3=@CreditPeriod3
        //                           ,CreditPeriod4=@CreditPeriod4
        //                           ,CreditAmount=@CreditAmount WHERE CusId=@CusId";
        //            int id = connection.Execute(sql, model);
        //            if (id > 0)
        //            {
        //                return (new Result(true));
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return (new Result(false, ex.InnerException == null ? ex.Message : ex.InnerException.Message));
        //    }
        //    return res;
        //}
        //public Result Delete(Customer model)
        //{
        //    Result res = new Result(false);
        //    try
        //    {
        //        using (IDbConnection connection = OpenConnection(dataConnection))
        //        {
        //            string sql = @" Delete from Customer WHERE CusId=@CusId";
        //            int id = connection.Execute(sql, model);
        //            if (id > 0)
        //            {
        //                return (new Result(true));
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return (new Result(false, ex.InnerException == null ? ex.Message : ex.InnerException.Message));
        //    }
        //    return res;
        //}
        public string GetRefNo(SubContractor objSubContractor)
        {

            using (IDbConnection connection = OpenConnection(dataConnection))
            {
                string RefNo = "";
                var result = new SubContractor();

                IDbTransaction trn = connection.BeginTransaction();

                try
                {
                    int internalid = DatabaseCommonRepository.GetInternalIDFromDatabase(connection, trn, typeof(SubContractor).Name, "0", 0);
                    RefNo =  internalid.ToString();
                    trn.Commit();
                }
                catch (Exception ex)
                {
                    trn.Rollback();
                }
                return RefNo;
            }
        }


        public List<SubContractor> GetSubContractor()
        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {
                string query = @"select SubContractorId,SubRefNo,SubName,ContactPerson,Email,OfficeNo,MobileNo
                from SubContractor 
                order by SubName";
                return connection.Query<SubContractor>(query).ToList();
            }
        }

        public SubContractor GetSubContractorView(int SubContractorId)
        {

            using (IDbConnection connection = OpenConnection(dataConnection))
            {
                string query;
                IDbTransaction txn = connection.BeginTransaction();

                query = @"select * from SubContractor
                        where SubContractorId=@SubContractorId";

                var model = connection.Query<SubContractor>(query, new
                {
                    SubContractorId = SubContractorId
                }, txn).First<SubContractor>();

                //try
                //{
                //    query = @"DELETE FROM SubContractor WHERE SubContractorId = @SubContractorId";
                //    connection.Execute(query, new { SubContractorId = SubContractorId }, txn);
                //    txn.Rollback();

                //    model.isUsed = false;
                //    return model;
                //}
                //catch
                //{
                //    txn.Rollback();
                //    model.isUsed = true;
                //}
                return model;
            }
        }


        public string DeleteSubContractor(int Id)
        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {
                IDbTransaction txn = connection.BeginTransaction();
                try
                {
                    string query = @"DELETE FROM SubContractor OUTPUT deleted.SubRefNo WHERE SubContractorId = @Id;";
                    
                    string ref_no = connection.Query<string>(query, new { Id = Id }, txn).First();
                    //InsertLoginHistory(dataConnection, CreatedBy, "Delete", typeof(QuerySheet).Name, Id.ToString(), OrganizationId.ToString());
                    txn.Commit();
                    return ref_no;
                }
                catch (Exception ex)
                {
                    txn.Rollback();
                    throw ex;
                }
            }
        }

        public string UpdateSubContractor(SubContractor model)
        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {
                IDbTransaction txn = connection.BeginTransaction();
                try
                {
                    string query = @"Update SubContractor Set SubRefNo=@SubRefNo,SubName=@SubName,ContactPerson=@ContactPerson,
                                   Designation=@Designation,OfficeNo=@OfficeNo,MobileNo=@MobileNo,Email=@Email,Address1=@Address1,
                                   Address2=@Address2,Address3=@Address3,Address4=@Address4  
                                   OUTPUT INSERTED.SubContractorId WHERE SubContractorId=@SubContractorId";
                    
                    string ref_no = connection.Query<string>(query, model, txn).First();
                    //InsertLoginHistory(dataConnection, CreatedBy, "Delete", typeof(QuerySheet).Name, Id.ToString(), OrganizationId.ToString());
                    txn.Commit();
                    return ref_no;
                }
                catch (Exception ex)
                {
                    txn.Rollback();
                    throw ex;
                }
            }
        }






    }
}
