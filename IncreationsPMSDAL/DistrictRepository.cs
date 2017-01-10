using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IncreationsPMSDomain;
using Dapper;
using System.Data;




namespace IncreationsPMSDAL
{
    public class DistrictRepository:BaseRepository
    {
        static string dataConnection = GetConnectionString("InCreationsConnection");
        public List<District> GetDistrict()
        {
             using (IDbConnection connection = OpenConnection(dataConnection))
             {
                 return connection.Query<District>("select DistrictId, DistrictName from District order by DistrictName").ToList();
             }
        }
    }
}


