# SimpleTaskSystem
“简单任务系统”例子，演示如何运用ABP开发项目
ABP是土耳其的一位架构师hikalkan开发的，现在又加入一个ismcagdas开发者。

要让项目运行起来，还得创建一个数据库。这个模板假设你正在使用SQL2008或者更新的版本。当然也可以很方便地换成其他的关系型数据库。
打开Web.Config文件可以查看和配置链接字符串：
```
<!-- 配置链接字符串 -->
<connectionStrings>
  <add name="Default" connectionString="Server=localhost; Database=SimpleTaskSystemDb; Trusted_Connection=True;" providerName="System.Data.SqlClient" />
</connectionStrings>
```
（在后面用到EF的Code first数据迁移时，会自动在SQL Server数据库中创建一个名为SimpleTaskSystemDb的数据库。）

下面将逐步实现这个简单的任务系统程序
# 创建实体
把实体类写在Core项目中，因为实体是领域层的一部分。
一个简单的应用场景：创建一些任务(tasks)并分配给人。 我们需要Task和Person这两个实体。
Task实体有几个属性：描述（Description）、创建时间（CreationTime）、任务状态（State），还有可选的导航属性(AssignedPerson)来引用Person。
```
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using SimpleTaskSystem.People;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimpleTaskSystem.Tasks
{
    [Table("StsTasks")]
    public class Task : Entity<long>, IHasCreationTime
    {
        [ForeignKey("AssignedPersonId")]
        public virtual Person AssignedPerson { get; set; }
        public virtual int? AssignedPersonId { get; set; }
        public virtual string Description { get; set; }
        public virtual DateTime CreationTime { get; set; }
        public virtual TaskState State { get; set; }
        public Task()
        {
            CreationTime = DateTime.Now;
            State = TaskState.Active;
        }
    }
}
```

