using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IncreationsPMSDomain;
using System.Data;
using System.Data.SqlClient;
using Dapper;

namespace IncreationsPMSDAL
{
    public class SizeRepository : BaseRepository
    {
        static string dataConnection = GetConnectionString("InCreationsConnection");
        public string ConnectionString()
        {
            return dataConnection;
        }
        public Size InsertSize(Size objSize)
        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {
                var result = new Size();

                IDbTransaction trn = connection.BeginTransaction();

                string sql = @"INSERT INTO Size(SizeUserCode,SizeName,SizeRemarks) 
                               VALUES(@SizeUserCode,@SizeName,@SizeRemarks);
                               SELECT CAST(SCOPE_IDENTITY() as int)";
                try
                {
                    //int internalid = DatabaseCommonRepository.GetInternalIDFromDatabase(connection, trn, typeof(Size).Name, "0", 1);
                    //objSize.SizeUserCode = "C/" + internalid;

                    int id = connection.Query<int>(sql, objSize, trn).Single();
                    objSize.SizeCode = id;

                    trn.Commit();
                }
                catch (Exception ex)
                {
                    trn.Rollback();
                    objSize.SizeCode = 0;
                    objSize.SizeUserCode = null;

                }
                return objSize;
            }
        }
    }
}
