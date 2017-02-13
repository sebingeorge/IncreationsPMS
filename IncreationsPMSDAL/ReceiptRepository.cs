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
   public  class ReceiptRepository : BaseRepository 
    {
        static string dataConnection = GetConnectionString("InCreationsConnection");
        public string ConnectionString()
        {
            return dataConnection;
        }
        public List<Receipt> PendingReceiptList(DateTime? FromDate, DateTime? ToDate, string ClientName = "")
        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {
                string query = @"select CustomerInvoice.CustInvoiceId,CustInvoiceRefNo,CustInvoiceDate,ClientName,
                isnull(CustomerInvoiceItem.InvoiceAmount,0)-sum(isnull(ReceivedAmount,0)) as InvoiceAmount
                from CustomerInvoice
                inner join CustomerInvoiceItem on CustomerInvoiceItem.CustInvoiceId=CustomerInvoice.CustInvoiceId
                inner join Client on Client.ClientId =CustomerInvoice.ClientId 
                left join Receipt on CustomerInvoiceItem.CustInvoiceId=Receipt.CustInvoiceId
                where cast(convert(varchar(20),CustInvoiceDate,106) as datetime) between @FromDate and @ToDate
                AND Client.ClientName like '%'+@ClientName+'%'
                group by CustomerInvoice.CustInvoiceId,CustInvoiceRefNo,CustInvoiceDate,ClientName,CustomerInvoiceItem.InvoiceAmount
				having  isnull(CustomerInvoiceItem.InvoiceAmount,0)-sum(isnull(ReceivedAmount,0)) > 0
                order by CustInvoiceRefNo";
                return connection.Query<Receipt>(query, new { FromDate = FromDate, ToDate = ToDate, ClientName = ClientName }).ToList();
            }
        }
        public static string GetNextRefNo()
        {

            using (IDbConnection connection = BaseRepository.OpenConnection(dataConnection))
            {
                string query = @"select ISNULL(max(isnull(ReceiptRefNo,0)+1),1) from Receipt";
                return connection.Query<string>(query).Single();
            }
        }

        public Receipt GetNewReceipt(int CustInvoiceId)
        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {
                string sql = @"select CustomerInvoice.CustInvoiceId,CustInvoiceRefNo,Client.ClientId,ClientName,
                              isnull(CustomerInvoiceItem.InvoiceAmount,0)-sum(isnull(ReceivedAmount,0)) as InvoiceAmount
                              from CustomerInvoice
                              inner join CustomerInvoiceItem on CustomerInvoiceItem.CustInvoiceId=CustomerInvoice.CustInvoiceId
                              inner join Client on Client.ClientId =CustomerInvoice.ClientId 
                              left join Receipt on CustomerInvoiceItem.CustInvoiceId=Receipt.CustInvoiceId
                              where CustomerInvoice.CustInvoiceId=@CustInvoiceId
                              group by CustomerInvoice.CustInvoiceId,CustInvoiceRefNo,CustInvoiceDate,ClientName,
                              Client.ClientId,CustomerInvoiceItem.InvoiceAmount ";
                var objReceipt = connection.Query<Receipt>(sql, new
                {
                    CustInvoiceId = CustInvoiceId
                }).First<Receipt>();

                return objReceipt;
            }


        }
        public Result Insert(Receipt model)
        {
            Result res = new Result(false);
            try
            {
                using (IDbConnection connection = OpenConnection(dataConnection))
                {

                    model.ReceiptRefNo = ReceiptRepository.GetNextRefNo();


                    string sql = @"INSERT INTO Receipt(ReceiptRefNo,ReceiptDate,ClientId,CustInvoiceId,InvoiceAmount,
                                  ReceivedAmount,ReceivableAmount,PaymentModeId,ChequeNo,VoucherNo,
                                  SpecialRemarks,CreatedBy,CreatedDate)
                                    VALUES
                                    (@ReceiptRefNo,@ReceiptDate,@ClientId,@CustInvoiceId,@InvoiceAmount,
                                    @ReceivedAmount,@ReceivableAmount,@PaymentModeId,@ChequeNo,@VoucherNo,@SpecialRemarks,
                                    @CreatedBy,@CreatedDate);
                                    SELECT CAST(SCOPE_IDENTITY() as int);";
                    model.ReceiptId = connection.Query<int>(sql, model).Single();
                    if (model.ReceiptId > 0)
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
        public List<Receipt> ReceiptList(string ClientName = "")
        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {
                string query = @"select ReceiptId,ReceiptRefNo,ReceiptDate,ClientName,ReceivedAmount,ReceivableAmount 
                from Receipt
               inner join Client on Client.ClientId=Receipt.ClientId
                where ClientName like '%'+@ClientName+'%'
                order by ReceiptRefNo";
                return connection.Query<Receipt>(query, new { ClientName = ClientName }).ToList();
            }
        }
        public Receipt ReceiptView(int ReceiptId)
        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {
                string sql = @"select ReceiptId, ReceiptRefNo,ReceiptDate,Receipt.ClientId,CustInvoiceId,ClientName,
                              InvoiceAmount,ReceivedAmount,ReceivableAmount,PaymentModeId,ChequeNo,VoucherNo,SpecialRemarks
                              from Receipt
                              inner join Client on Client.ClientId=Receipt.ClientId
                              where ReceiptId=@ReceiptId";
                 var objReceipt = connection.Query<Receipt>(sql, new
                {
                    ReceiptId = ReceiptId
                }).First<Receipt>();

                return objReceipt;
            }

        }
        public string UpdateReceipt(Receipt model)
        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {
                IDbTransaction txn = connection.BeginTransaction();
                try
                {
                    string query = @"Update Receipt Set ReceiptRefNo=@ReceiptRefNo,ReceiptDate=@ReceiptDate,ClientId=@ClientId,
                                   CustInvoiceId=@CustInvoiceId,InvoiceAmount=@InvoiceAmount,ReceivedAmount=@ReceivedAmount,
                                   ReceivableAmount=@ReceivableAmount,PaymentModeId=@PaymentModeId,ChequeNo=@ChequeNo,VoucherNo=@VoucherNo,
                                   SpecialRemarks=@SpecialRemarks,CreatedBy=@CreatedBy,CreatedDate=@CreatedDate
                                   OUTPUT INSERTED.ReceiptId WHERE ReceiptId=@ReceiptId";
                    string ref_no = connection.Query<string>(query, model, txn).First();
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
        public string DeleteReceipt(int Id)
        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {
                IDbTransaction txn = connection.BeginTransaction();
                try
                {
                    string query = @"DELETE FROM Receipt OUTPUT deleted.ReceiptRefNo WHERE ReceiptId = @ReceiptId;";

                    string ref_no = connection.Query<string>(query, new { ReceiptId = Id }, txn).First();
                    
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
        public IEnumerable<Receipt> GetReceiptReport( int ClientId)
        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {
                //              
                string qry = @"  SELECT CustInvoiceRefNo,CustInvoiceDate,ClientName,CustomerInvoice.ClientId,
                                isnull(CustomerInvoiceItem.InvoiceAmount,0)-sum(isnull(ReceivedAmount,0)) as ReceivableAmount,
                                 Address1   + ' / ' +Address2   + '/' +Address3 as Address
                                from CustomerInvoice
                                inner join CustomerInvoiceItem on CustomerInvoiceItem.CustInvoiceId=CustomerInvoice.CustInvoiceId
                                inner join Client on Client.ClientId =CustomerInvoice.ClientId 
                                left join Receipt on CustomerInvoiceItem.CustInvoiceId=Receipt.CustInvoiceId
                                WHERE CustomerInvoice.ClientId = ISNULL(NULLIF(@ClientId, 0), CustomerInvoice.ClientId) 
                                group by CustInvoiceRefNo,CustInvoiceDate,ClientName,
                                CustomerInvoice.ClientId,Address1,Address2,Address3,CustomerInvoiceItem.InvoiceAmount
                                having isnull(CustomerInvoiceItem.InvoiceAmount,0)-sum(isnull(ReceivedAmount,0)) >0
                                ORDER BY CustInvoiceDate";
                
                return connection.Query<Receipt>(qry, new { ClientId = ClientId }).ToList();
            }
        }
        public IEnumerable<Receipt> GetReceiptPrint( int ClientId, string ClientName)
        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {
                //              
                string qry = @" SELECT CustInvoiceRefNo,CustInvoiceDate,ClientName,CustomerInvoice.ClientId,
                               isnull(CustomerInvoiceItem.InvoiceAmount,0)-sum(isnull(ReceivedAmount,0)) as ReceivableAmount,
                                Address1   + ' / ' +Address2   + '/' +Address3 as Address
                                from CustomerInvoice
                                inner join CustomerInvoiceItem on CustomerInvoiceItem.CustInvoiceId=CustomerInvoice.CustInvoiceId
                                inner join Client on Client.ClientId =CustomerInvoice.ClientId 
                                left join Receipt on CustomerInvoiceItem.CustInvoiceId=Receipt.CustInvoiceId
                                WHERE  CustomerInvoice.ClientId = ISNULL(NULLIF(@ClientId, 0), CustomerInvoice.ClientId) 
                                group by CustInvoiceRefNo,CustInvoiceDate,ClientName,
                                CustomerInvoice.ClientId,Address1,Address2,Address3,CustomerInvoiceItem.InvoiceAmount
                                having isnull(CustomerInvoiceItem.InvoiceAmount,0)-sum(isnull(ReceivedAmount,0)) >0
                                ORDER BY CustInvoiceDate";
           return connection.Query<Receipt>(qry, new { ClientId = ClientId, ClientName = ClientName}).ToList();
            }
        }
    }
}
 