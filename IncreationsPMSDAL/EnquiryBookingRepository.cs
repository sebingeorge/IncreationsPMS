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
    public class EnquiryBookingRepository : BaseRepository
    {
    static string dataConnection = GetConnectionString("InCreationsConnection");
        public string ConnectionString()
        {
            return dataConnection;
        }
        public string GetRefNo(EnquiryBooking objEnquiryBooking)
        {

            using (IDbConnection connection = OpenConnection(dataConnection))
            {
                string RefNo = "";
                var result = new EnquiryBooking();

                IDbTransaction trn = connection.BeginTransaction();

                try
                {
                    int internalid = DatabaseCommonRepository.GetInternalIDFromDatabase(connection, trn, typeof(EnquiryBooking).Name, "0", 0);
                    RefNo = internalid.ToString();
                    trn.Commit();
                }
                catch (Exception ex)
                {
                    trn.Rollback();
                }
                return RefNo;
            }
        }

        public EnquiryBooking Insert(EnquiryBooking objEnquiryBooking)
        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {
                var result = new EnquiryBooking();

                IDbTransaction trn = connection.BeginTransaction();

                string sql = @"INSERT INTO EnquiryBooking(EnquiryRef,EnquiryDate,EnquiryClient,ClientTypeId,ModeofContactId,ProjectTypeId, 
                EnquiryReference,EnquiryContactNo,EnquiryEmail,EnquiryLocation,EnquiryDetails,EnquiryCancel,CreatedBy,CreatedDate,OrganizationId)
                VALUES(@EnquiryRef,@EnquiryDate,@EnquiryClient,@ClientTypeId,@ModeofContactId,@ProjectTypeId,@EnquiryReference,
                @EnquiryContactNo,@EnquiryEmail,@EnquiryLocation,@EnquiryDetails,0,@CreatedBy,@CreatedDate,@OrganizationId);
                SELECT CAST(SCOPE_IDENTITY() as int)";
                try
                {
                    int internalid = DatabaseCommonRepository.GetInternalIDFromDatabase(connection, trn, typeof(EnquiryBooking).Name, "0", 1);
                    objEnquiryBooking.EnquiryRef = internalid.ToString();

                    int id = connection.Query<int>(sql, objEnquiryBooking, trn).Single();
                    objEnquiryBooking.EnquiryId = id;
                    InsertLoginHistory(dataConnection, objEnquiryBooking.CreatedBy, "Create", "EnquiryBooking", id.ToString(), "0");
                    trn.Commit();
                }
                catch (Exception ex)
                {
                    trn.Rollback(); 
                    objEnquiryBooking.EnquiryId = 0;
                    objEnquiryBooking.EnquiryRef = null;
                }
                return objEnquiryBooking;
            }
        }
        public IEnumerable<PendingEnquiryStatus> GetPendingEnquiryStatus(DateTime? FromDate, DateTime? ToDate, string EnquiryClient = "")
        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {

                string query = @"SELECT
                                  EnquiryId,EnquiryRef,convert (varchar(50),EnquiryDate,103)EnquiryDate,EnquiryClient,ModeofContactName,
                                  ProjectTypeName from EnquiryBooking HD
                                  inner join ClientType CT on CT.ClientTypeId =HD.ClientTypeId
                                  inner join ModeOfContact MOC on MOC.ModeofContactId =HD.ModeofContactId
                                  inner join ProjectType PT on PT.ProjectTypeId =HD.ProjectTypeId
                                  where EnquiryCancel=0
                                  and  cast(convert(varchar(20),EnquiryDate,106) as datetime) between @FromDate and @ToDate
                                  AND EnquiryClient like '%'+@EnquiryClient+'%'
                                  order by EnquiryDate";
	                                                                    
                return connection.Query<PendingEnquiryStatus>(query, new { FromDate = FromDate, ToDate = ToDate, EnquiryClient = EnquiryClient }).ToList();
           
            }
        }

        public EnquiryStatus GetEnquiryStatusDetails(int EnquiryId)
        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {
                string qry = "select EnquiryId, EnquiryRef, convert (varchar(50), EnquiryDate, 103)EnquiryDate,EnquiryClient,";
                qry += " ModeofContactName,ClientTypeName,ProjectTypeName,EnquiryReference,EnquiryContactNo,";
                qry += " EnquiryEmail,EnquiryLocation,EnquiryDetails";
                qry += " from EnquiryBooking HD";
                qry += " inner join ClientType CT on CT.ClientTypeId = HD.ClientTypeId";
                qry += " inner join ModeOfContact MOC on MOC.ModeofContactId = HD.ModeofContactId";
                qry += " inner join ProjectType PT on PT.ProjectTypeId = HD.ProjectTypeId";
                qry += " where HD.EnquiryId = " + EnquiryId.ToString();

               EnquiryStatus EnquiryStatus = connection.Query<EnquiryStatus>(qry).FirstOrDefault();
                return EnquiryStatus;
            }
        }

                           
      public EnquiryStatus UpdateEnquiryStatus(EnquiryStatus model)
        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {
                string sql = @"UPDATE EnquiryBooking SET EnquiryProfileSending = @EnquiryProfileSending ,EnquiryOfferSending = @EnquiryOfferSending, 
                                 EnquiryLayoutReceiving = @EnquiryLayoutReceiving  
                                 OUTPUT INSERTED.EnquiryId WHERE EnquiryId = EnquiryId";

                try
                {
                    var id = connection.Execute(sql, model);
                    model.EnquiryId = id;
            }
                catch (Exception ex)
                {

                    model.EnquiryId = 0;

                }
                return model;
            }
        }

        public int UpdateEnquiryCancel(int Id)
        {
            int result = 0;
            using (IDbConnection connection = OpenConnection(dataConnection))
            {
                
                string sql = @"UPDATE EnquiryBooking SET EnquiryCancel = 1   WHERE EnquiryId=@Id";

                {

                    var id = connection.Execute(sql, new { Id = Id });
                  return id;

                }

            }
        }


    }
}
