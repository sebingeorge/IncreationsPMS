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
        public List<Size> GetSizes()

        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {
                string sql = @" SELECT SizeCode,SizeUserCode,SizeName,SizeRemarks FROM Size S";
                                
                                var objSize = connection.Query<Size>(sql).ToList<Size>();

                return objSize;
            }
        }
        public Size GetSize(int SizeCode)
        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {
                string sql = @"select * from Size
                        where SizeCode=@SizeCode";

                var objDesignation = connection.Query<Size>(sql, new
                {
                    DesignationId = SizeCode
                }).First<Size>();

                return objDesignation;
            }
        }
        //public int DeleteSize(Size objSize)
        //{
        //    int result = 0;
        //    using (IDbConnection connection = OpenConnection(dataConnection))
        //    {
        //        //string sql = @" Update Designation Set isActive=0 WHERE SizeCode=@SizeCode";
        //        try
        //        {

        //            var id = connection.Execute(sql, objSize);
        //            objSize.SizeCode = id;
        //           result = 0;

        //        }
        //        catch (SqlException ex)
        //        {
        //            int err = ex.Errors.Count;
        //            if (ex.Errors.Count > 0) // Assume the interesting stuff is in the first error
        //            {
        //                switch (ex.Errors[0].Number)
        //                {
        //                    case 547: // Foreign Key violation
        //                        result = 1;
        //                        break;

        //                    default:
        //                        result = 2;
        //                        break;
        //                }
        //            }

        //        }

        //        return result;
        //    }
        //}
    }
}
