using IncreationsPMSDAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Collections;
using IncreationsPMSDomain;
namespace IncreationsPMSDAL
{
    public class DropdownRepository : BaseRepository
    {
        static string dataConnection = GetConnectionString("InCreationsConnection");
        //public List<Dropdown> SizeDropdown()
        //{
        //    using (IDbConnection connection = OpenConnection(dataConnection))
        //    {
        //        return connection.Query<Dropdown>("select [SizeCode] Id,[SizeName] Name from [dbo].[Size]").ToList();
        //    }
        //}
        public List<Dropdown> GetClientType()
        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {
                return connection.Query<Dropdown>("SELECT ClientTypeId Id, ClientTypeName Name FROM ClientType").ToList();
            }
        }
        public List<Dropdown> GetProjectType()
        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {
                return connection.Query<Dropdown>("SELECT ProjectTypeId Id, ProjectTypeName Name FROM ProjectType").ToList();
            }
        }
        public List<Dropdown> GetModeOfContact()
        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {
                return connection.Query<Dropdown>("SELECT ModeofContactId Id, ModeofContactName Name FROM ModeOfContact").ToList();
            }
        }


        public List<Dropdown> GetClient()
        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {
                return connection.Query<Dropdown>("SELECT ClientId Id, ClientName Name FROM Client").ToList();
            }
        }
        public List<Dropdown> FillSubContractor()
        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {
                var param = new DynamicParameters();
                return connection.Query<Dropdown>("select SubContractorId Id,SubName Name from SubContractor").ToList();
            }
        }
        public List<Dropdown> FillPaymentMode()
        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {
                var param = new DynamicParameters();
                return connection.Query<Dropdown>("select PaymentModeId Id,PaymentModeName Name from PaymentMode").ToList();
            }
        }
    }
}
