using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using IncreationsPMSDomain;
using System.Data;
using Dapper;

namespace IncreationsPMSDAL
{
    public class UserRepository: BaseRepository
    {
        static string dataConnection = GetConnectionString("InCreationsConnection");
        public int InsertUser(User user)
        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {
                try
                {
                    string sql = "INSERT INTO [dbo].[User](UserName, UserEmail, UserPassword, UserSalt)";
                    sql += " VALUES(@UserName, @UserEmail, @UserPassword, @UserSalt);";
                    sql += " SELECT CAST(SCOPE_IDENTITY() as int);";

                    var id = connection.Query<int>(sql, user).Single();
                    
                    InsertLoginHistory(dataConnection, user.CreatedBy, "Create", "Unit", id.ToString(), "0");
                    return id;
                }
                catch (Exception ex)
                {
                    return 0;
                }
            }
        }
        public int UpdateUser(User user)
        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {
                try
                {
                    string sql = "Update [User] set UserEmail = @UserEmail";
                    if (user.UserPassword != null && user.UserPassword != "")
                    {
                        sql += ", UserPassword = @UserPassword, @UserSalt = UserSalt";
                    }
                    sql += " where UserId = @UserId;";
                    connection.Query(sql, user);                    
                }
                catch
                {
                    return 0;
                }
            }
            InsertLoginHistory(dataConnection, user.CreatedBy, "Update", "Unit", user.UserId.ToString(), "0");
            return user.UserId ?? 0;
        }
        public User GetUserByUserNameAndPassword(string username, string password)
        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {
                string sql = "select * from [User] where UserName='" + username + "' and UserPassword='" + password + "'";
                var user = new User();
                try
                {
                    user = connection.Query<User>(sql).Single();
                }
                catch
                {

                }
                return user;
            }
        }
        public bool IsValidUser(int UserId, string Username, string ControllName, string ActionName, int OrganizationId, string SessionID)
        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {
                if (ActionName != "LoadQuickView" && ControllName != "Home")
                {
                    string sql = @"insert into TransactionHistory(UserId, TransTime, Mode, Form, OrganizationId, SessionID)
                               values('" + UserId.ToString() + "', GetDate(), '" + ActionName + "', '" + ControllName + "', '" + OrganizationId.ToString() + "','" + SessionID + "')";
                    connection.Query(sql);
                }
            }
            return true;
        }
        public void InsertLoginHistory(User user, string sessionId, string ipAddress, string OrganizationId)
        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {
                string sql = "Insert into LoginHistory(UserId, LogIn, SessionID, IpAddress, OrganizationId)";
                sql += " values('" + user.UserId.ToString() + "', GetDate(), '" + sessionId + "','" + ipAddress + "','" + OrganizationId + "')";
                connection.Query(sql);
            }
        }
        public void UpdateLoginHistory(string sessionId)
        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {
                string sql = "update LoginHistory set LogoutTime = Getdate() where SessionID = " + sessionId;
            }
        }
    }
}
