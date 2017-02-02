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
    public class FinancialYearRepository : BaseRepository
    {
        static string dataConnection = GetConnectionString("InCreationsConnection");
        public DateTime GetFinStartDate()
        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {

                string qry = @"select FyStartDate from [dbo].[FinancialYear] where year(FyStartDate)=year(GETDATE())";


                return connection.Query<DateTime>(qry).First();
            }
        }
    }
}
