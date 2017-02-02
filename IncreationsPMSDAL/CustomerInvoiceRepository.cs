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
  public   class CustomerInvoiceRepository : BaseRepository
    {
        static string dataConnection = GetConnectionString("InCreationsConnection");
        public string ConnectionString()
        {
            return dataConnection;
        }
        public IEnumerable<PendingCustomerInvoice> PendingCustomerInvoice(string ProjectEnquiry = "", string ClientName = "")
        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {
                string query = @"select Project.ProjectId,ProjectRefNo,ProjectDate,ProjectEnquiry,
                                 Description,Amount,ClientName,Paymentid 
                                 from Project
                                 inner join Client on Client.ClientId=Project.ClientId 
                                 inner join ProjectPaymentSchedule on ProjectPaymentSchedule.ProjectId=Project.ProjectId 
                                 where  ClientName LIKE '%'+@ClientName+'%'
                                 and Project.ProjectEnquiry like '%'+@ProjectEnquiry+'%'
                                 order by ProjectRefNo ";
                return connection.Query<PendingCustomerInvoice>(query, new { ProjectEnquiry = ProjectEnquiry, ClientName = ClientName}).ToList();
            }
        }
        public static string GetNextRefNo()
        {

            using (IDbConnection connection = BaseRepository.OpenConnection(dataConnection))
            {
                string query = @"select ISNULL(max(isnull(CustInvoiceRefNo,0)+1),1) from CustomerInvoice";
                return connection.Query<string>(query).Single();
            }
        }
        public Invoice GetInvoice(int Paymentid)
        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {
                string sql = @"SELECT Project.ClientId,ClientName,Address1,Address2,Address3,ProjectOrderRefNo
                               from Project
                              inner join ProjectPaymentSchedule on Project.ProjectId=ProjectPaymentSchedule.ProjectId
                              inner join Client on Client.ClientId =Project.ClientId
                               where Paymentid=@Paymentid";
                var objInvoice = connection.Query<Invoice>(sql, new
                {
                    Paymentid = Paymentid
                }).First<Invoice>();

                return objInvoice;
            }


        }
       
       public List<CustomerInvoiceItem> GetInvoiceItem(int Paymentid)
        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {
                string qry = "SELECT Project.ProjectId,ProjectRefNo,ProjectDate,ProjectEnquiry,Paymentid,Description,Amount";
                       qry += " FROM Project ";
                       qry += " inner join ProjectPaymentSchedule on Project.ProjectId=ProjectPaymentSchedule.ProjectId";
                       qry += " where Paymentid = " + Paymentid.ToString();
                return connection.Query<CustomerInvoiceItem>(qry).ToList();
            }
        }
        public Result Insert(Invoice model)
        {
            Result res = new Result(false);
            int id = 0;
            try
            {
                using (IDbConnection connection = OpenConnection(dataConnection))
                {

                    model.CustInvoiceRefNo = CustomerInvoiceRepository.GetNextRefNo();


                    string sql = @"INSERT INTO CustomerInvoice
                                              (CustInvoiceRefNo,CustInvoiceDate,ClientId,SpecialRemarks,PaymentTerms, 
                                              AdditionalRemarks,AddAmount,DeductionRemarks,DedAmount,BillDueDate)
                                        VALUES
                                         (@CustInvoiceRefNo,@CustInvoiceDate,@ClientId,@SpecialRemarks,@PaymentTerms,
                                         @AdditionalRemarks,@AddAmount,@DeductionRemarks,@DedAmount,@BillDueDate);
                                         SELECT CAST(SCOPE_IDENTITY() as int);";




                    model.CustInvoiceId = connection.Query<int>(sql, model).Single();

                    foreach (CustomerInvoiceItem items in model.CustomerInvoiceItem)
                    {
                        items.CustInvoiceId = model.CustInvoiceId;
                        sql = @"INSERT INTO CustomerInvoiceItem
                                   (CustInvoiceId,ProjectId,PaymentScheduleid,ScheduledAmount,ReceivedAmount)
                                   VALUES
                                  (@CustInvoiceId,@ProjectId,@Paymentid,@Amount,@ReceivedAmount);
                                  SELECT CAST(SCOPE_IDENTITY() as int);";

                        id = connection.Query<int>(sql, items).Single();
                        
                    }


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
        public List<Invoice> InvoiceList(DateTime? FromDate, DateTime? ToDate,string ClientName = "")
        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {
                string query = @"select hd.CustInvoiceId,CustInvoiceRefNo,CustInvoiceDate,ClientName,ProjectEnquiry,ReceivedAmount 
                from CustomerInvoice hd
                inner join CustomerInvoiceItem dt on dt.CustInvoiceId=hd.CustInvoiceId
                inner join Client on Client.ClientId =hd.ClientId
                inner join Project on Project.ProjectId =dt.ProjectId
                where cast(convert(varchar(20),CustInvoiceDate,106) as datetime) between @FromDate and @ToDate
                AND Client.ClientName like '%'+@ClientName+'%'
                order by CustInvoiceRefNo";
                return connection.Query<Invoice>(query, new { FromDate = FromDate,ToDate = ToDate,ClientName = ClientName }).ToList();
            }
        }
    

        public Invoice GetInvoiceView(int id)
        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {
                IDbTransaction txn = connection.BeginTransaction();

                string query = @"SELECT CustomerInvoice.CustInvoiceId,CustInvoiceRefNo,CustInvoiceDate,CustomerInvoice.ClientId,SpecialRemarks,PaymentTerms, 
                                     ClientName,Address1,Address2,Address3,ProjectOrderRefNo,
                                     AdditionalRemarks,AddAmount,DeductionRemarks,DedAmount,BillDueDate 
                                     FROM CustomerInvoice
                                     inner join CustomerInvoiceItem on CustomerInvoice.CustInvoiceId=CustomerInvoiceItem.CustInvoiceId
                                     inner join Client on Client.ClientId=CustomerInvoice.ClientId
                                     inner join project on Client.ClientId=project.ClientId
                                     and project.ProjectId =CustomerInvoiceItem.ProjectId
                                     WHERE CustomerInvoice.CustInvoiceId = @id";
                Invoice model = connection.Query<Invoice>(query, new { @id = id }, txn).FirstOrDefault();
                string sql = @"SELECT CustInvoiceItemId,CustInvoiceId,CustomerInvoiceItem.ProjectId,ProjectEnquiry,Description,
                               ProjectRefNo,ProjectDate,PaymentScheduleid Paymentid,Amount,ReceivedAmount
                               FROM CustomerInvoiceItem
                               inner join Project on CustomerInvoiceItem.ProjectId=Project.ProjectId
                               inner join ProjectPaymentSchedule on Paymentid=PaymentScheduleid
                               WHERE CustInvoiceId = @id;";
                model.CustomerInvoiceItem = connection.Query<CustomerInvoiceItem>(sql, new { id = id }, txn).ToList();

      

                return model;
            }
        }
        public int UpdateInvoice(Invoice model)
        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {
                IDbTransaction txn = connection.BeginTransaction();
                              
                try
                {
                    string query = @"UPDATE CustomerInvoice SET
                         CustInvoiceDate =@CustInvoiceDate,
                         SpecialRemarks=@SpecialRemarks,
                         PaymentTerms=@PaymentTerms,
                         AdditionalRemarks=@AdditionalRemarks,
                         AddAmount=@AddAmount,
                         DeductionRemarks=@DeductionRemarks,
                         DedAmount=@DedAmount,
                         BillDueDate=@BillDueDate
                         WHERE CustInvoiceId = @CustInvoiceId";
                    int id = connection.Execute(query, model, txn);
                    if (id <= 0) throw new Exception();
                    id = new CustomerInvoiceItemRepository().UpdateCustomerInvoiceItem(model, connection, txn);
                   if (id <= 0) throw new Exception();
                    txn.Commit();
                    return id;
                 }
                catch (Exception)
                {
                    txn.Rollback();
                    throw;
                }
            }
        }
        public string DeleteInvoice(int Id)
        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {
                IDbTransaction txn = connection.BeginTransaction();
                try
                {
                    string sql = @" DELETE FROM CustomerInvoiceItem WHERE CustInvoiceId=@Id;
                                    DELETE FROM CustomerInvoice OUTPUT deleted.CustInvoiceRefNo WHERE CustInvoiceId = @Id;";

                    string output = connection.Query<string>(sql, new { Id = Id }, txn).First();
                    txn.Commit();
                    return output;
                }
                catch (Exception ex)
                {
                    txn.Rollback();
                    throw ex;
                }
            }
        }
        public Invoice CustomerInvoiceHdforPrint(int CustInvoiceId)
        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {

                string sql = @" select CustInvoiceRefNo,CustInvoiceDate,ClientName,ProjectOrderRefNo,Address1,Address2,Address3,
                                SpecialRemarks,PaymentTerms,AdditionalRemarks,AddAmount,DeductionRemarks,DedAmount,BillDueDate 
                                FROM CustomerInvoice
                                inner join CustomerInvoiceItem on CustomerInvoice.CustInvoiceId=CustomerInvoiceItem.CustInvoiceId
                                inner join Client on Client.ClientId=CustomerInvoice.ClientId
                                inner join project on Client.ClientId=project.ClientId
                                and project.ProjectId =CustomerInvoiceItem.ProjectId
							    where CustomerInvoice.CustInvoiceId=@CustInvoiceId";

                var objInvoice = connection.Query<Invoice>(sql, new
                {
                    CustInvoiceId = CustInvoiceId,
                    //OrganizationId = OrganizationId
                }).First<Invoice>();

                return objInvoice;
            }
        }

        public List<CustomerInvoiceItem> customerInvoiceItemforPrint(int CustInvoiceId)
        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {
                string sql = @"SELECT ProjectRefNo,ProjectDate,ProjectEnquiry,Description,Amount,ReceivedAmount
                               FROM CustomerInvoiceItem
                               inner join Project on CustomerInvoiceItem.ProjectId=Project.ProjectId
                               inner join ProjectPaymentSchedule on Paymentid=PaymentScheduleid
                                WHERE CustInvoiceId = @CustInvoiceId";

                return connection.Query<CustomerInvoiceItem>(sql, new { CustInvoiceId = CustInvoiceId }).ToList();
            }
        }

    }
}
