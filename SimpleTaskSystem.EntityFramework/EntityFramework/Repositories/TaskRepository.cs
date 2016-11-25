using Abp.EntityFramework;
using SimpleTaskSystem.Tasks;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace SimpleTaskSystem.EntityFramework.Repositories
{
    public class TaskRepository : SimpleTaskSystemRepositoryBase<Tasks.Task, long>, ITaskRepository
    {
        public TaskRepository(IDbContextProvider<SimpleTaskSystemDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public List<Tasks.Task> GetAllWithPeople(int? assignedPersonId, TaskState? state)
        {
            // 在仓储方法中，不用处理数据库连接、DbContext和数据事务，ABP框架会自动处理。
            var query = GetAll(); //GetAll() 返回一个 IQueryable<T>接口类型
            // 添加一些Where条件

            // AssignedPersonId
            if (assignedPersonId.HasValue)
            {
                query = query.Where(task => task.AssignedPerson.Id == assignedPersonId.Value);
            }

            // State
            if (state.HasValue)
            {
                query = query.Where(task => task.State == state);
            }

            return query
                .OrderByDescending(task => task.CreationTime)
                .Include(task => task.AssignedPerson)
                .ToList();
        }
    }
}
