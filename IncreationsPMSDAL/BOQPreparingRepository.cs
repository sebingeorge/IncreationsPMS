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
    public class BOQPreparingRepository : BaseRepository
    {
       
            static string dataConnection = GetConnectionString("InCreationsConnection");
            public string ConnectionString()
            {
                return dataConnection;
            }
            public IEnumerable<PendingBOQPreparing> GetNewBOQPreparing(string ClientName = "")
            {
                using (IDbConnection connection = OpenConnection(dataConnection))
                {
                    string query = @"SELECT ProjectId,ProjectRefNo,ProjectDate,HD.ClientId,ClientName,ProjectEnquiry
                                   FROM Project HD
                                   INNER JOIN Client C ON C.ClientId=HD.ClientId
                                   where C.ClientName LIKE '%'+@ClientName+'%'
                                   order by HD.ProjectDate ";
                    return connection.Query<PendingBOQPreparing>(query, new { ClientName = ClientName }).ToList();
                }
            }
        public BOQPreparing GetNewBOQPreparing(int Id)
        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {
                string sql = @"SELECT HD.ClientId,ClientName,ProjectEnquiry,ProjectId
                                    FROM Project HD
                                    INNER JOIN Client C ON C.ClientId=HD.ClientId
                                    where HD.ProjectId=@Id";


                var objBOQPreparing = connection.Query<BOQPreparing>(sql, new
                {
                    Id = Id
                }).First<BOQPreparing>();

                return objBOQPreparing;
            }


        }

        public string GetRefNo(BOQPreparing objBOQPreparing)
        {

            using (IDbConnection connection = OpenConnection(dataConnection))
            {
                string RefNo = "";
                var result = new BOQPreparing();

                IDbTransaction trn = connection.BeginTransaction();

                try
                {
                    int internalid = DatabaseCommonRepository.GetInternalIDFromDatabase(connection, trn, typeof(BOQPreparing).Name, "0", 0);
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

        public List<BOQPreparingItem> GetTaskDetails(int Id)
        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {
                string qry = "select hd.ProjectRefNo,hd.ProjectId,ProjectDate,ProjectTaskId,MileStoneName,TaskName";
                qry += " from Project hd  ";
                qry += " inner join ProjectTask task on task.ProjectId=hd.ProjectId";
                qry += " where task.ProjectId = " + Id.ToString();
                return connection.Query<BOQPreparingItem>(qry).ToList();
            }
        }
        public Result Insert(BOQPreparing  model)
        {
            Result res = new Result(false);
            int id = 0;
            try
            {
                using (IDbConnection connection = OpenConnection(dataConnection))
                {
                    
                 model.projectWorkRefNo = BOQPreparingRepository.GetNextRefNo();
                                     

                    string sql = @"INSERT INTO ProjectWorkBOQ
                                   (projectWorkRefNo,projectWorkDate,PreparedBy,ClientId)
                                    VALUES
                                    (@projectWorkRefNo,@projectWorkDate,@PreparedBy,@ClientId);
                                    SELECT CAST(SCOPE_IDENTITY() as int);";
           



                    model.ProjectWorkId = connection.Query<int>(sql, model).Single();

                    foreach (BOQPreparingItem items in model.BOQPreparingItem)
                    {
                        items.ProjectWorkId = model.ProjectWorkId;
                        sql = @"INSERT INTO ProjectWorkBOQItem
                                   (ProjectWorkId,ProjectTaskId,TotalAmount,PercentageComplete,IsCompleted)
                                   VALUES
                                  (@ProjectWorkId,@ProjectTaskId,@TotalAmount,0,0);
                                  SELECT CAST(SCOPE_IDENTITY() as int);";

                        items.ProjectWorkItemId = connection.Query<int>(sql, items).Single();

                        foreach (BOQPreparingItemWork item in items.BOQPreparingItemWork)
                        {
                            item.ProjectWorkItemId = items.ProjectWorkItemId;
                            item.ProjectWorkId = items.ProjectWorkId;
                            sql = @"insert  into ProjectWorkBOQItemWork(ProjectWorkId,ProjectWorkItemId,ProjectWorkDescription,
                                                SubContractorId,PlanedStartDate,PlanedEndDate,WorkAmount) 
                                                Values 
                                               (@ProjectWorkId,@ProjectWorkItemId,@ProjectWorkDescription,@SubContractorId,
                                                @PlanedStartDate,@PlanedEndDate,@WorkAmount)";
                                                
                               id = connection.Execute(sql, item);

                        }

                    }

                   
                    if (id > 0)
                    {
                        return (new Result(true));
                    }
                  
                }
            }
            catch (Exception ex)
            {
                return (new Result(false, ex.InnerException == null ? ex.Message : ex.InnerException.Message));
            }
            return res;
        }
        public static string GetNextRefNo()
        {

            using (IDbConnection connection = BaseRepository.OpenConnection(dataConnection))
            {
                string query = @"select ISNULL(max(isnull(projectWorkRefNo,0)+1),1) from ProjectWorkBOQ";
                return connection.Query<string>(query).Single();
            }
        }

        public List<BOQPreparing> GetBOQPreparingList()
        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {
                string query = @"select ProjectWorkId,projectWorkRefNo,projectWorkDate,ClientName,PreparedBy
                from ProjectWorkBOQ hd
                inner join Client c on c.ClientId=hd.ClientId
                order by projectWorkRefNo";
                return connection.Query<BOQPreparing>(query).ToList();
            }
        }
                     
       
        public BOQPreparing GetBOQPreparingView(int ProjectWorkId)
        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {
                BOQPreparing model = new BOQPreparing();
                //head//dt//work
                string query = @"select hd.ProjectWorkId,projectWorkRefNo, convert (varchar(50), projectWorkDate, 103)projectWorkDate,
                                 hd.ClientId ClientId,clientName,ProjectEnquiry,PreparedBy
                                 from ProjectWorkBOQ hd
                                 inner join ProjectWorkBOQItem dt on hd.ProjectWorkId=dt.ProjectWorkId
                                 inner join ProjectTask task on task.ProjectTaskId=dt.ProjectTaskId
                                 inner join Project on Project.ProjectId=task.ProjectId
                                 inner join Client c on c.ClientId =hd.ClientId
                                 where hd.ProjectWorkId=@id


                     SELECT ProjectWorkId, ProjectWorkItemId, ProjectRefNo, ProjectDate, MileStoneName, TaskName, TotalAmount
                     FROM ProjectWorkBOQItem   Item
                     inner join ProjectTask task on task.ProjectTaskId=Item.ProjectTaskId
                     inner join Project on Project.ProjectId=task.ProjectId
                     WHERE Item.ProjectWorkId = @id
                              
                                SELECT ProjectWorkItemId INTO #Item FROM ProjectWorkBOQItem WHERE ProjectWorkId = @id

                                SELECT
	                                *
                                FROM ProjectWorkBOQItemWork
                                WHERE ProjectWorkItemId IN (SELECT ProjectWorkItemId FROM #Item)

                              

                                DROP TABLE #Item;";

                using (var dataset = connection.QueryMultiple(query, new { id = ProjectWorkId }))
                {
                    model = dataset.Read<BOQPreparing>().First();
                    model.BOQPreparingItem = dataset.Read<BOQPreparingItem>().AsList();
                    List<BOQPreparingItemWork> Work = dataset.Read<BOQPreparingItemWork>().AsList();
                    
                    foreach (var item in model.BOQPreparingItem)
                    {
                        item.BOQPreparingItemWork = Work.Where(x => x.ProjectWorkItemId == item.ProjectWorkItemId).Select(x => x).ToList();
                    }
                }
                return model;
            }
        }
        public int UpdateBOQPreparing(BOQPreparing model)
        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {
                IDbTransaction txn = connection.BeginTransaction();
                try
                {
                    string sql = "";
                              
                    var row = 0;

                  
                   
                    foreach (BOQPreparingItem items in model.BOQPreparingItem)

                    {
                        foreach (BOQPreparingItemWork item in items.BOQPreparingItemWork)
                        {
                            item.ProjectWorkItemId = items.ProjectWorkItemId;
                            item.ProjectWorkId = items.ProjectWorkId;
                            sql = @"UPDATE   ProjectWorkBOQItemWork SET ProjectWorkId=@ProjectWorkId,ProjectWorkItemId=@ProjectWorkItemId,
                                                         ProjectWorkDescription=@ProjectWorkDescription,SubContractorId=@SubContractorId,
                                                      PlanedStartDate=@PlanedStartDate,PlanedEndDate=@PlanedEndDate,WorkAmount=@WorkAmount
                                                      WHERE ProjectWorkId = @ProjectWorkId";

                            row = connection.Execute(sql, item, txn);

                        }
                        
                    }
                                                        
                    //InsertLoginHistory(dataConnection, model.CreatedBy, "Update", typeof(QuerySheet).Name, model.QuerySheetId.ToString(), model.OrganizationId.ToString());
                    txn.Commit();
                    return row;


                }
                catch (Exception ex)
                {
                    txn.Rollback();
                    throw ex;
                }
            }
        }
        public string DeleteBOQPreparing(int Id)
        {
            using (IDbConnection connection = OpenConnection(dataConnection))
            {
                IDbTransaction txn = connection.BeginTransaction();
                try
                {
                               string query = @"DELETE FROM ProjectWorkBOQItemWork WHERE ProjectWorkId = @ProjectWorkId;
                                DELETE FROM ProjectWorkBOQItem WHERE ProjectWorkId = @ProjectWorkId;
                                DELETE FROM ProjectWorkBOQ OUTPUT deleted.projectWorkRefNo WHERE ProjectWorkId = @ProjectWorkId;";

                    string ref_no = connection.Query<string>(query, new { ProjectWorkId = Id }, txn).First();

                   
                    //InsertLoginHistory(dataConnection, CreatedBy, "Delete", typeof(QuerySheet).Name, Id.ToString(), OrganizationId.ToString());
                    txn.Commit();
                    return ref_no;
                }
                catch (Exception ex)
                {
                    txn.Rollback();
                    throw ex;
                }
            }
        }
    }
    }
