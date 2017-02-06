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
   public class CustomerInvoiceItemRepository : BaseRepository
    {
        static string dataConnection = GetConnectionString("InCreationsConnection");
        public string ConnectionString()
        {
            return dataConnection;
        }
        internal int UpdateCustomerInvoiceItem(Invoice model, IDbConnection connection, IDbTransaction txn)
        {
            try
            {
                string sql = @"DELETE FROM CustomerInvoiceItem WHERE CustInvoiceId = @id";
                var id = connection.Execute(sql, new { id = model.CustInvoiceId }, txn);
                if (id <= 0) throw new Exception();
                foreach (var item in model.CustomerInvoiceItem)
                {
                    item.CustInvoiceId = model.CustInvoiceId;
                    InsertCustomerInvoiceItem(item, connection, txn);
                }
                return id;
            }
            catch (Exception)
            {
                return 0;
            }
        }
        internal int InsertCustomerInvoiceItem(CustomerInvoiceItem item, IDbConnection connection, IDbTransaction txn)
        {
            try
            {
                string sql = @"insert  into CustomerInvoiceItem(CustInvoiceId,ProjectId,PaymentScheduleid,ScheduledAmount,InvoiceAmount)
                                           Values (@CustInvoiceId,@ProjectId,@Paymentid,@Amount,@InvoiceAmount);
                SELECT CAST(SCOPE_IDENTITY() as int)";
                var id = connection.Query<int>(sql, item, txn).FirstOrDefault();
                return id;
            }
            catch (Exception)
            {
                throw;
            }
        }

      
    }
}
