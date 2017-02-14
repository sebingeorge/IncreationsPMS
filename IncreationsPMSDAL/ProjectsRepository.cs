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

                string sql = @"INSERT INTO Project(ProjectRefNo,ProjectDate,ClientId,ProjectEnquiry,ProjectOrderRefNo,CreatedBy,CreatedDate,EnquiryId)
                             VALUES(@ProjectRefNo,@ProjectDate,@ClientId,@ProjectEnquiry,@ProjectOrderRefNo,@CreatedBy,@CreatedDate,@EnquiryId);
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
                    InsertLoginHistory(dataConnection, objProjects.CreatedBy, "Create", "Project", id.ToString(), "0");
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

        public IEnumerable<PendingProjects> GetPendingProjects(DateTime? FromDate, DateTime? ToDate, string ClientName = "")
        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {

                string query = @"SELECT
                                 hd.ProjectId,ProjectRefNo,ProjectDate,ClientName,
                                  TaskName,StartDate,EndDate
                                  from Project hd
                                  inner join ProjectTask task on task.ProjectId =hd.ProjectId
                                  inner join Client c on c.ClientId =hd.ClientId
                                  where cast(convert(varchar(20),ProjectDate,106) as datetime) between @FromDate and @ToDate
                                  AND ClientName like '%'+@ClientName+'%'
                                  order by ProjectDate";

                return connection.Query<PendingProjects>(query, new { FromDate = FromDate, ToDate = ToDate, ClientName = ClientName }).ToList();
            }
        }

        public Projects GetProjectsDetails(int ProjectId)
        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {
                string qry = "select ProjectId,EnquiryId,ProjectRefNo, convert (varchar(50), ProjectDate, 103)ProjectDate,";
                       qry += " hd.ClientId ClientId,MobileNo,Email,Address1,Address2,Address3,ProjectOrderRefNo,ProjectEnquiry";
                       qry += " from Project hd ";
                       qry += " inner join Client c on c.ClientId =hd.ClientId";
                       qry += " where ProjectId = " + ProjectId.ToString();

                Projects Projects = connection.Query<Projects>(qry).FirstOrDefault();
                return Projects;
            }
        }

       public List<ProjectTask> GetTaskDetails(int ProjectId)
        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {
                string qry = "select hd.ProjectId,MileStoneName,TaskName,StartDate,EndDate";
                       qry += " from Project hd  ";
                       qry += " inner join ProjectTask task on task.ProjectId=hd.ProjectId";
                       qry += " where task.ProjectId = " + ProjectId.ToString();
                       return connection.Query<ProjectTask>(qry).ToList();
            }
        }
        public List<ProjectPaymentSchedule> GetPaymentDetails(int ProjectId)
        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {
                string qry = "select hd.ProjectId,Date,Description,Percentage,Amount";
                qry += " from Project hd  ";
                qry += " inner join  ProjectPaymentSchedule pay on pay.ProjectId=hd.ProjectId";
                qry += " where pay.ProjectId = " + ProjectId.ToString();
                return connection.Query<ProjectPaymentSchedule>(qry).ToList();
            }
        }

        public Projects UpdateProjects(Projects model)
        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {
                IDbTransaction txn = connection.BeginTransaction();
                try
                {
               
                    //model.ProjectTask = model.ProjectTask
                    //    .Where(x => (x.AdditionId != null || x.AdditionId != 0) && x.Addition > 0)
                    //    .Sum(x => x.Addition);
                    //model.ProjectPaymentSchedule = model.ProjectPaymentSchedule
                    //    .Where(x => (x.DeductionId != null || x.DeductionId != 0) && x.Deduction > 0)
                    //    .Sum(x => x.Deduction);

                    //string sql = @"UPDATE Projects SET 
                    //          ProjectEnquiry = @ProjectEnquiry WHERE ProjectId = @ProjectId";
                    //var id = connection.Execute(sql, model, txn);

                    int output = new ProjectItemRepository().DeleteTask(model.ProjectId, connection, txn);

                    output = new ProjectItemRepository().DeletePayment(model.ProjectId, connection, txn);

                    output = InsertProjectDT(model, connection, txn);

                    InsertLoginHistory(dataConnection, model.CreatedBy, "Update", "Projects", model.ProjectId.ToString(), "0");

                    txn.Commit();
                    return model;
                }
                catch (Exception)
                {
                    txn.Rollback();
                    throw;
                }
            }
        }

        public int InsertProjectDT(Projects model, IDbConnection connection, IDbTransaction txn)
        {
                                            
            foreach (var ProjectTask in model.ProjectTask)
            {
                if ((ProjectTask.MileStoneName == null)) continue;
                new ProjectItemRepository().InsertProjectTask(new ProjectTask
                {
                    ProjectId = model.ProjectId,
                    MileStoneName = ProjectTask.MileStoneName,
                    TaskName = ProjectTask.TaskName,
                    StartDate = ProjectTask.StartDate,
                    EndDate = ProjectTask.EndDate
                }, connection, txn);

            }

                                  
         foreach (var ProjectPaymentSchedule in model.ProjectPaymentSchedule)
            {
                if ((ProjectPaymentSchedule.Description == null)) continue;
                new PaymentScheduleItemRepository().InsertProjectPaymentSchedule(new ProjectPaymentSchedule
                {
                    ProjectId = model.ProjectId,
                    Paymentid = ProjectPaymentSchedule.Paymentid,
                    Date = ProjectPaymentSchedule.Date,
                    Description = ProjectPaymentSchedule.Description,
                    Percentage = ProjectPaymentSchedule.Percentage,
                    Amount = ProjectPaymentSchedule.Amount
                }, connection, txn);

            }

            return 1;

        }



    }

}
