using Abp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleTaskSystem.Tasks
{
    /* NOTE: 
    *   定义仓储接口
    *   通过仓储模式，可以更好把业务代码与数据库操作代码更好的分离，可以针对不同的数据库有不同的实现类，而业务代码不需要修改。
    *   定义仓储接口的代码写到Core项目中，因为仓储接口是领域层的一部分。
    *   它继承自ABP框架中的IRepository泛型接口。
    *   所以ITaskRepository默认就有了上面那些方法。可以再加上它独有的方法GetAllWithPeople(...)。
    *   不需要为Person类创建一个仓储类，因为默认的方法已经够用了。
    *   ABP提供了一种注入通用仓储的方式，将在后面“创建应用服务”一节的TaskAppService类中看到。

    */
    public interface ITaskRepository : IRepository<Task, long>
    {
        List<Task> GetAllWithPeople(int? assignedPersonId, TaskState? state);
    }
}
