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
   public class ProjectsRepository : BaseRepository
    {
        static string dataConnection = GetConnectionString("InCreationsConnection");
        public string ConnectionString()
        {
            return dataConnection;
        }
        public string GetRefNo(Projects objProjects)
        {

            using (IDbConnection connection = OpenConnection(dataConnection))
            {
                string RefNo = "";
                var result = new Projects();

                IDbTransaction trn = connection.BeginTransaction();

                try
                {
                    int internalid = DatabaseCommonRepository.GetInternalIDFromDatabase(connection, trn, typeof(Projects).Name, "0", 0);
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


        public Client GetClientContactDetails(int Id)
        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {

                string query = "select MobileNo,Email,Address1,Address2,Address3 from Client where ClientId= @Id";
                return connection.Query<Client>(query, new { Id = Id }).First<Client>();
            }
        }
        public Projects Insert(Projects objProjects)
        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {
                var result = new Projects();

                IDbTransaction trn = connection.BeginTransaction();

                string sql = @"INSERT INTO Project(ProjectRefNo,ProjectDate,ClientId,ProjectEnquiry)
                             VALUES(@ProjectRefNo,@ProjectDate,@ClientId,@ProjectEnquiry);
                             SELECT CAST(SCOPE_IDENTITY() as int)";
                try
                {
                    int internalid = DatabaseCommonRepository.GetInternalIDFromDatabase(connection, trn, typeof(Projects).Name, "0", 1);
                    objProjects.ProjectRefNo = internalid.ToString();

                    int id = connection.Query<int>(sql, objProjects, trn).Single();
                    objProjects.ProjectId = id;

                    foreach (var ProjectTask in objProjects.ProjectTask)
                    {
                        if ((ProjectTask.MileStoneName == null)) continue;
                        new ProjectItemRepository().InsertProjectTask(new ProjectTask
                        {
                            ProjectId = id,
                            MileStoneName = ProjectTask.MileStoneName,
                            TaskName = ProjectTask.TaskName,
                            StartDate = ProjectTask.StartDate,
                            EndDate = ProjectTask.EndDate
                        }, connection, trn);

                    }

                    foreach (var ProjectPaymentSchedule in objProjects.ProjectPaymentSchedule)
                    {
                        if ((ProjectPaymentSchedule.Description == null)) continue;
                        new PaymentScheduleItemRepository().InsertProjectPaymentSchedule(new ProjectPaymentSchedule
                        {
                            ProjectId = id,
                            Paymentid = ProjectPaymentSchedule.Paymentid,
                            Date = ProjectPaymentSchedule.Date,
                            Description=ProjectPaymentSchedule.Description,
                            Percentage=ProjectPaymentSchedule.Percentage,
                            Amount=ProjectPaymentSchedule.Amount
                           }, connection, trn);

                    }
                    //InsertLoginHistory(dataConnection, objEnquiryBooking.CreatedBy, "Create", "EnquiryBooking", id.ToString(), "0");
                    trn.Commit();
                }
                catch (Exception ex)
                {
                    trn.Rollback();
                    objProjects.ProjectId = 0;
                    objProjects.ProjectRefNo = null;
                }
                return objProjects;
            }
        }
    }
}
