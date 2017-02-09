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
   public  class ReportRepository : BaseRepository 
    {
        static string dataConnection = GetConnectionString("InCreationsConnection");
        public string ConnectionString()
        {
            return dataConnection;
        }
        //   public List<AgeingSummary> GetAgeingSummaryBasedCommittedDate(string Client = "")
        //   {
        //       using (IDbConnection connection = OpenConnection(dataConnection))
        //       {
        //           string query = @"

        //               select Client.ClientId,ClientName,ProjectEnquiry,
        // isnull(CustomerInvoiceItem.InvoiceAmount,0)-sum(isnull(ReceivedAmount,0)) as TotalReceivable,
        //  0 Amount1,0 Amount2,0 Amount3,0 Amount4,0 Amount5 INTO #Result  
        //                from CustomerInvoice
        // inner join CustomerInvoiceItem on CustomerInvoiceItem.CustInvoiceId=CustomerInvoice.CustInvoiceId
        // inner join Client on Client.ClientId =CustomerInvoice.ClientId 
        // inner join ProjectPaymentSchedule on ProjectPaymentSchedule.Paymentid=CustomerInvoiceItem.PaymentScheduleid
        //                inner join Project on Project.ProjectId =ProjectPaymentSchedule.ProjectId
        // left join Receipt on CustomerInvoiceItem.CustInvoiceId=Receipt.CustInvoiceId
        // group by Client.ClientId,ClientName,ProjectEnquiry,CustomerInvoiceItem.InvoiceAmount
        //                having  isnull(CustomerInvoiceItem.InvoiceAmount,0)-sum(isnull(ReceivedAmount,0))>0;

        //                 with A as (
        //                select CustomerInvoice.ClientId, isnull(CustomerInvoiceItem.InvoiceAmount,0)-sum(isnull(ReceivedAmount,0)) as Amount
        // from CustomerInvoice
        // inner join CustomerInvoiceItem on CustomerInvoiceItem.CustInvoiceId=CustomerInvoice.CustInvoiceId
        // left join Receipt on CustomerInvoiceItem.CustInvoiceId=Receipt.CustInvoiceId
        //                WHERE  DATEDIFF(DAY, GETDATE(), CustInvoiceDate) <=15 
        // and  DATEDIFF(DAY, GETDATE(), CustInvoiceDate)>0  group by CustomerInvoice.ClientId,CustomerInvoiceItem.InvoiceAmount)
        //                update R set R.Amount1 = A.Amount from A 
        // inner join #Result R on R.ClientId = A.ClientId;


        //              with A as (
        //                select CustomerInvoice.ClientId,isnull(CustomerInvoiceItem.InvoiceAmount,0)-sum(isnull(ReceivedAmount,0)) as Amount
        // from CustomerInvoice
        // inner join CustomerInvoiceItem on CustomerInvoiceItem.CustInvoiceId=CustomerInvoice.CustInvoiceId
        // left join Receipt on CustomerInvoiceItem.CustInvoiceId=Receipt.CustInvoiceId
        //                WHERE  DATEDIFF(DAY, GETDATE(), CustInvoiceDate)
        // BETWEEN 15 AND 30  group by CustomerInvoice.ClientId,CustomerInvoiceItem.InvoiceAmount)
        //                update R set R.Amount2 = A.Amount from A 
        // inner join #Result R on R.ClientId = A.ClientId;

        //               with A as (
        //                select CustomerInvoice.ClientId, isnull(CustomerInvoiceItem.InvoiceAmount,0)-sum(isnull(ReceivedAmount,0)) as Amount 
        //from CustomerInvoice
        // inner join CustomerInvoiceItem on CustomerInvoiceItem.CustInvoiceId=CustomerInvoice.CustInvoiceId
        // left join Receipt on CustomerInvoiceItem.CustInvoiceId=Receipt.CustInvoiceId
        //                WHERE  DATEDIFF(DAY, GETDATE(), CustInvoiceDate)  
        // BETWEEN 30 AND 60   group by CustomerInvoice.ClientId,CustomerInvoiceItem.InvoiceAmount)
        //                update R set R.Amount3 = A.Amount from A 
        // inner join #Result R on R.ClientId = A.ClientId;

        //                with A as (
        //                select CustomerInvoice.ClientId,isnull(CustomerInvoiceItem.InvoiceAmount,0)-sum(isnull(ReceivedAmount,0)) as Amount 
        //from CustomerInvoice
        // inner join CustomerInvoiceItem on CustomerInvoiceItem.CustInvoiceId=CustomerInvoice.CustInvoiceId
        // left join Receipt on CustomerInvoiceItem.CustInvoiceId=Receipt.CustInvoiceId
        //                WHERE  DATEDIFF(DAY, GETDATE(), CustInvoiceDate)  
        // BETWEEN 60 AND 90  group by CustomerInvoice.ClientId,CustomerInvoiceItem.InvoiceAmount)
        //                update R set R.Amount4 = A.Amount from A 
        // inner join #Result R on R.ClientId = A.ClientId;

        //               with A as (
        //                select CustomerInvoice.ClientId, isnull(CustomerInvoiceItem.InvoiceAmount,0)-sum(isnull(ReceivedAmount,0)) as Amount
        // from CustomerInvoice
        // inner join CustomerInvoiceItem on CustomerInvoiceItem.CustInvoiceId=CustomerInvoice.CustInvoiceId
        // left join Receipt on CustomerInvoiceItem.CustInvoiceId=Receipt.CustInvoiceId
        //                WHERE  DATEDIFF(DAY, GETDATE(), CustInvoiceDate)  
        // >90   group by CustomerInvoice.ClientId,CustomerInvoiceItem.InvoiceAmount)
        //                update R set R.Amount5 = A.Amount from A 
        // inner join #Result R on R.ClientId = A.ClientId;

        //    select * from #Result where  ClientName LIKE '%'+@Client+'%' order by ClientName";




        //           return connection.Query<AgeingSummary>(query, new { Client = Client }).ToList();
        //       }
        //   }
        public List<AgeingSummary> GetAgeingSummaryBasedCommittedDate(string Client = "")
        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {
                string query = @"

                    
                  select Client.ClientId,ClientName,ProjectEnquiry,sum(CustomerInvoiceItem.InvoiceAmount) InvoiceAmount
                  ,sum(isnull(ReceivedAmount,0))ReceivedAmount,(sum(isnull(CustomerInvoiceItem.InvoiceAmount,0))-sum(isnull(ReceivedAmount,0)))TotalReceivable
                  ,0 Amount1,0 Amount2,0 Amount3,0 Amount4,0 Amount5 INTO #Result  
                  from CustomerInvoice
                  inner join CustomerInvoiceItem on CustomerInvoiceItem.CustInvoiceId=CustomerInvoice.CustInvoiceId
                  inner join Client on Client.ClientId =CustomerInvoice.ClientId 
                  inner join ProjectPaymentSchedule on ProjectPaymentSchedule.Paymentid=CustomerInvoiceItem.PaymentScheduleid
                  inner join Project on Project.ProjectId =ProjectPaymentSchedule.ProjectId
                  left join Receipt on CustomerInvoiceItem.CustInvoiceId=Receipt.CustInvoiceId
                  group by Client.ClientId,ClientName,ProjectEnquiry
                  having (sum(isnull(CustomerInvoiceItem.InvoiceAmount,0))-sum(isnull(ReceivedAmount,0))) >0;
                    
                      with A as (
                     select CustomerInvoice.ClientId, 
                     (sum(isnull(CustomerInvoiceItem.InvoiceAmount,0))-sum(isnull(ReceivedAmount,0)))  Amount
					 from CustomerInvoice
					 inner join CustomerInvoiceItem on CustomerInvoiceItem.CustInvoiceId=CustomerInvoice.CustInvoiceId
					 left join Receipt on CustomerInvoiceItem.CustInvoiceId=Receipt.CustInvoiceId
                     WHERE  DATEDIFF(DAY, GETDATE(), CustInvoiceDate) <=15 
					 and  DATEDIFF(DAY, GETDATE(), CustInvoiceDate)>=0 
                     group by CustomerInvoice.ClientId)
                     update R set R.Amount1 = A.Amount from A 
					 inner join #Result R on R.ClientId = A.ClientId;

                     
                   with A as (
                     select CustomerInvoice.ClientId,
                     (sum(isnull(CustomerInvoiceItem.InvoiceAmount,0))-sum(isnull(ReceivedAmount,0)))  Amount
					 from CustomerInvoice
					 inner join CustomerInvoiceItem on CustomerInvoiceItem.CustInvoiceId=CustomerInvoice.CustInvoiceId
					 left join Receipt on CustomerInvoiceItem.CustInvoiceId=Receipt.CustInvoiceId
                     WHERE  DATEDIFF(DAY, GETDATE(), CustInvoiceDate)
					 BETWEEN 15 AND 30  group by CustomerInvoice.ClientId)
                     update R set R.Amount2 = A.Amount from A 
					 inner join #Result R on R.ClientId = A.ClientId;
                     
                    with A as (
                     select CustomerInvoice.ClientId, 
                     (sum(isnull(CustomerInvoiceItem.InvoiceAmount,0))-sum(isnull(ReceivedAmount,0)))  Amount
					from CustomerInvoice
					 inner join CustomerInvoiceItem on CustomerInvoiceItem.CustInvoiceId=CustomerInvoice.CustInvoiceId
					 left join Receipt on CustomerInvoiceItem.CustInvoiceId=Receipt.CustInvoiceId
                     WHERE  DATEDIFF(DAY, GETDATE(), CustInvoiceDate)  
					 BETWEEN 30 AND 60   group by CustomerInvoice.ClientId)
                     update R set R.Amount3 = A.Amount from A 
					 inner join #Result R on R.ClientId = A.ClientId;
                     
                     with A as (
                     select CustomerInvoice.ClientId,
                     (sum(isnull(CustomerInvoiceItem.InvoiceAmount,0))-sum(isnull(ReceivedAmount,0)))  Amount
					from CustomerInvoice
					 inner join CustomerInvoiceItem on CustomerInvoiceItem.CustInvoiceId=CustomerInvoice.CustInvoiceId
					 left join Receipt on CustomerInvoiceItem.CustInvoiceId=Receipt.CustInvoiceId
                     WHERE  DATEDIFF(DAY, GETDATE(), CustInvoiceDate)  
					 BETWEEN 60 AND 90  group by CustomerInvoice.ClientId)
                     update R set R.Amount4 = A.Amount from A 
					 inner join #Result R on R.ClientId = A.ClientId;

                    with A as (
                     select CustomerInvoice.ClientId, 
                     (sum(isnull(CustomerInvoiceItem.InvoiceAmount,0))-sum(isnull(ReceivedAmount,0)))  Amount
					 from CustomerInvoice
					 inner join CustomerInvoiceItem on CustomerInvoiceItem.CustInvoiceId=CustomerInvoice.CustInvoiceId
					 left join Receipt on CustomerInvoiceItem.CustInvoiceId=Receipt.CustInvoiceId
                     WHERE  DATEDIFF(DAY, GETDATE(), CustInvoiceDate)  
					 >90   group by CustomerInvoice.ClientId)
                     update R set R.Amount5 = A.Amount from A 
					 inner join #Result R on R.ClientId = A.ClientId;

				     select * from #Result where  ClientName LIKE '%'+@Client+'%' order by ClientName";




                return connection.Query<AgeingSummary>(query, new { Client = Client }).ToList();
            }
        }

        public List<OverDueTaskReport> GetOverDueTaskReport(string Client = "")
        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {
                string query = @"

                    select ProjectRefNo,ProjectEnquiry,ProjectTask.ProjectTaskId,TaskName,PercentageComplete,
                    Client.ClientId,ClientName,ProjectWorkDescription,DATEDIFF(day,EndDate,GETDATE ()) AS DiffDate  
                    from Project
                    inner join ProjectTask on ProjectTask.ProjectId =Project.ProjectId
                    inner join ProjectWorkBOQItem on ProjectWorkBOQItem.ProjectTaskId =ProjectTask.ProjectTaskId
                    inner join ProjectWorkBOQItemWork on ProjectWorkBOQItemWork.ProjectWorkItemId=ProjectWorkBOQItem.ProjectWorkItemId
                    inner join  Client on Client.ClientId=Project.ClientId
                    where PercentageComplete<>100 and ClientName LIKE '%'+@Client+'%' order by ClientName";
                
                return connection.Query<OverDueTaskReport>(query, new { Client = Client }).ToList();
            }
        }
    }
}
