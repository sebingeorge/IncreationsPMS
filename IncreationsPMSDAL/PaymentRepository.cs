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
   public class PaymentRepository : BaseRepository
    {
        static string dataConnection = GetConnectionString("InCreationsConnection");
        public string ConnectionString()
        {
            return dataConnection;
        }
        public IEnumerable<PendingSubContractorPayment> PendingSubcontractorPayment(string ProjectEnquiry = "", string TaskName = "", string SubName = "")
        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {
                string query = @"SELECT hd.ProjectWorkId,projectWorkRefNo,projectWorkDate,MileStoneName,TaskName,WorkAmount,
                                 ProjectWorkDescription,SubName,ProjectWorkDetailsId,item.ProjectWorkItemId,ProjectEnquiry,PercentageComplete
                                 FROM ProjectWorkBOQ hd
                                 inner join ProjectWorkBOQItem item on item.ProjectWorkId = hd.ProjectWorkId
                                 inner join ProjectWorkBOQItemWork work on work.ProjectWorkItemId = item.ProjectWorkItemId
                                 inner join ProjectTask task on task.ProjectTaskId = item.ProjectTaskId
                                 inner join SubContractor s on s.SubContractorId = work.SubContractorId
                                 inner join Project on Project.ProjectId = task.ProjectId
                                 where work.PercentageComplete >0
                                 and task.TaskName LIKE '%'+@TaskName+'%'
                                 and Project.ProjectEnquiry like '%'+@ProjectEnquiry+'%'
                                 and s.SubName like '%'+@SubName+'%'
                                 order by HD.projectWorkDate ";
                return connection.Query<PendingSubContractorPayment>(query, new { ProjectEnquiry = ProjectEnquiry, TaskName = TaskName, SubName = SubName }).ToList();
            }
        }
        public Payment GetNewPayment(int ProjectWorkDetailsId)
        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {
                string sql = @"SELECT work.SubContractorId,SubName,ProjectWorkDetailsId,WorkAmount
                               from ProjectWorkBOQItemWork work 
                               inner join SubContractor s on s.SubContractorId = work.SubContractorId
                               where work.ProjectWorkDetailsId=@ProjectWorkDetailsId";
             var objPayment = connection.Query<Payment>(sql, new
                {
                    ProjectWorkDetailsId = ProjectWorkDetailsId
                }).First<Payment>();

                return objPayment;
            }


        }
        public static string GetNextRefNo()
        {

            using (IDbConnection connection = BaseRepository.OpenConnection(dataConnection))
            {
                string query = @"select ISNULL(max(isnull(PaymentRefNo,0)+1),1) from Payment";
                return connection.Query<string>(query).Single();
            }
        }
        public Result Insert(Payment model)
        {
            Result res = new Result(false);
            try
            {
                using (IDbConnection connection = OpenConnection(dataConnection))
                {

                    model.PaymentRefNo = PaymentRepository.GetNextRefNo();


                    string sql = @"INSERT INTO Payment(PaymentRefNo,PaymentDate,SubContractorId,WorkAmount,AcceptedAmount,
                                   PaymentModeId,ChequeNo,SpecialRemarks,ProjectWorkDetailsId,CreatedBy,CreatedDate)
                                    VALUES
                                    (@PaymentRefNo,@PaymentDate,@SubContractorId,@WorkAmount,@AcceptedAmount,
                                    @PaymentModeId,@ChequeNo,@SpecialRemarks,@ProjectWorkDetailsId,@CreatedBy,@CreatedDate);
                                    SELECT CAST(SCOPE_IDENTITY() as int);";
                   model.PaymentId = connection.Query<int>(sql, model).Single();
              if (model.PaymentId > 0)
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
        public List<Payment> PaymentList(string SubName = "")
        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {
                string query = @"select PaymentId,PaymentRefNo,PaymentDate,SubName,AcceptedAmount 
                from Payment
                inner join SubContractor on SubContractor.SubContractorId=Payment.SubContractorId
                where SubContractor.SubName like '%'+@SubName+'%'
                order by PaymentRefNo";
                return connection.Query<Payment>(query, new { SubName = SubName }).ToList();
            }
        }
        public Payment PaymentView(int PaymentId)
        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {
                string sql = @"select PaymentId,PaymentRefNo,PaymentDate,Payment.SubContractorId,SubName,
                               WorkAmount,AcceptedAmount,PaymentModeId,ChequeNo,SpecialRemarks,ProjectWorkDetailsId
                               from Payment 
                              inner join SubContractor on SubContractor.SubContractorId=Payment.SubContractorId
                               where PaymentId=@PaymentId";
                var objPayment = connection.Query<Payment>(sql, new
                {
                    PaymentId = PaymentId
                }).First<Payment>();

                return objPayment;
            }

     }
        public string UpdatePayment(Payment model)
        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {
                IDbTransaction txn = connection.BeginTransaction();
                try
                {
                    string query = @"Update Payment Set PaymentRefNo=@PaymentRefNo,PaymentDate=@PaymentDate,SubContractorId=@SubContractorId,
                                   WorkAmount=@WorkAmount,AcceptedAmount=@AcceptedAmount,PaymentModeId=@PaymentModeId,
                                   ChequeNo=@ChequeNo,SpecialRemarks=@SpecialRemarks,ProjectWorkDetailsId=@ProjectWorkDetailsId,
                                   CreatedBy=@CreatedBy,CreatedDate=@CreatedDate
                                   OUTPUT INSERTED.PaymentId WHERE PaymentId=@PaymentId";
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
        public string DeletePayment(int Id)
        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {
                IDbTransaction txn = connection.BeginTransaction();
                try
                {
                    string query = @"DELETE FROM Payment OUTPUT deleted.PaymentRefNo WHERE PaymentId = @PaymentId;";
                           
                    string ref_no = connection.Query<string>(query, new { PaymentId = Id }, txn).First();


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
        public Payment PaymentPrint(int PaymentId)
        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {

                string sql = @" select PaymentRefNo,PaymentDate,SubName,
                                WorkAmount,AcceptedAmount,PaymentModeName,ChequeNo,SpecialRemarks
                                from Payment 
                                inner join SubContractor on SubContractor.SubContractorId=Payment.SubContractorId
                                inner join PaymentMode on PaymentMode.PaymentModeId=Payment.PaymentModeId
							    where PaymentId=@PaymentId";

                var objPayment = connection.Query<Payment>(sql, new
                {
                    PaymentId = PaymentId,
                   
                }).First<Payment>();

                return objPayment;
            }
        }
        public IEnumerable<Payment> GetPaymentReport(int SubContractorId)
        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {
                //              
                string qry = @"  SELECT PaymentRefNo,PaymentDate,SubName, 
                               isnull(Payment.WorkAmount,0)-isnull (sum(AcceptedAmount),0)PayableAmount,
                               Address1   + ' / ' +Address2   + '/' +Address3 as Address 
                               FROM Payment
                               inner join SubContractor on SubContractor.SubContractorId=Payment.SubContractorId
                               inner join ProjectWorkBOQItemWork on  ProjectWorkBOQItemWork.ProjectWorkDetailsId=Payment.ProjectWorkDetailsId
                               WHERE PercentageComplete=100
                               AND SubContractor.SubContractorId = ISNULL(NULLIF(@SubContractorId, 0), SubContractor.SubContractorId)  
                               GROUP BY PaymentRefNo,PaymentDate,SubName,Address1,Address2,Address3,Payment.WorkAmount
                               Having  isnull(Payment.WorkAmount,0)-isnull (sum(AcceptedAmount),0) >0
                               ORDER BY PaymentDate";

                return connection.Query<Payment>(qry, new { SubContractorId = SubContractorId }).ToList();
            }
        }
        public IEnumerable<Payment> GetPaymentPrint(int SubContractorId, string SubName)
        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {
                //              
                string qry = @" SELECT PaymentRefNo,PaymentDate,SubName, 
                                isnull(Payment.WorkAmount,0)-isnull (sum(AcceptedAmount),0)PayableAmount,
                                Address1   + ' / ' +Address2   + '/' +Address3 as Address 
                                FROM Payment
                               inner join SubContractor on SubContractor.SubContractorId=Payment.SubContractorId
                               inner join ProjectWorkBOQItemWork on  ProjectWorkBOQItemWork.ProjectWorkDetailsId=Payment.ProjectWorkDetailsId
                               WHERE PercentageComplete=100
                               AND SubContractor.SubContractorId = ISNULL(NULLIF(@SubContractorId, 0), SubContractor.SubContractorId)  
                               GROUP BY PaymentRefNo,PaymentDate,SubName,Address1,Address2,Address3,Payment.WorkAmount
                               Having  isnull(Payment.WorkAmount,0)-isnull (sum(AcceptedAmount),0) >0
                               ORDER BY PaymentDate";
                return connection.Query<Payment>(qry, new { SubContractorId = SubContractorId, SubName = SubName }).ToList();
            }
        }

    }
}
