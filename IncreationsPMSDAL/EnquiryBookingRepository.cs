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
                EnquiryReference,EnquiryContactNo,EnquiryEmail,EnquiryLocation,EnquiryDetails)
                VALUES(@EnquiryRef,@EnquiryDate,@EnquiryClient,@ClientTypeId,@ModeofContactId,@ProjectTypeId,@EnquiryReference,
                @EnquiryContactNo,@EnquiryEmail,@EnquiryLocation,@EnquiryDetails);
                SELECT CAST(SCOPE_IDENTITY() as int)";
                try
                {
                    int internalid = DatabaseCommonRepository.GetInternalIDFromDatabase(connection, trn, typeof(EnquiryBooking).Name, "0", 1);
                    objEnquiryBooking.EnquiryRef = internalid.ToString();

                    int id = connection.Query<int>(sql, objEnquiryBooking, trn).Single();
                    objEnquiryBooking.EnquiryId = id;
                    //InsertLoginHistory(dataConnection, objEnquiryBooking.CreatedBy, "Create", "EnquiryBooking", id.ToString(), "0");
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
    }
}
