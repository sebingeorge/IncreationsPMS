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
  public   class PaymentScheduleItemRepository :BaseRepository
    {
        static string dataConnection = GetConnectionString("InCreationsConnection");
        public string ConnectionString()
        {
            return dataConnection;
        }
        public int InsertProjectPaymentSchedule(ProjectPaymentSchedule model, IDbConnection connection, IDbTransaction trn)
        {
            try
            {
                string sql = @"INSERT INTO ProjectPaymentSchedule(ProjectId,Date,Description,Percentage,Amount) 
                            VALUES (@ProjectId,@Date,@Description,@Percentage,@Amount);
                            SELECT CAST(SCOPE_IDENTITY() AS INT)";

                var id = connection.Query<int>(sql, model, trn).Single();
                return id;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
