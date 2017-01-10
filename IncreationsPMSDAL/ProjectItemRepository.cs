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
   public  class ProjectItemRepository :BaseRepository
    {
        static string dataConnection = GetConnectionString("InCreationsConnection");
        public string ConnectionString()
        {
            return dataConnection;
        }
        public int InsertProjectTask(ProjectTask model, IDbConnection connection, IDbTransaction trn)
        {
            try
            {
                string sql = @"INSERT INTO ProjectTask(ProjectId,MileStoneName,TaskName,StartDate,EndDate) 
                            VALUES (@ProjectId,@MileStoneName,@TaskName,@StartDate,@EndDate);
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
