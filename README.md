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
Person实体更简单，只定义了一个Name属性：
```
using Abp.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimpleTaskSystem.People
{
    [Table("StsPeople")]
    public class Person : Entity
    {
        public virtual string Name { get; set; }
    }
}
```
在ABP框架中，有一个Entity基类，它有一个Id属性。因为Task类继承自Entity<long>，所以它有一个long类型的Id。Person类有一个int类型的Id，因为int类型是Entity基类Id的默认类型，没有特别指定类型时，实体的Id就是int类型。

# 创建DbContext
使用EntityFramework需要先定义DbContext类，ABP的模板已经创建了DbContext文件，我们只需要把Task和Person类添加到IDbSet，请看代码：
```
public class SimpleTaskSystemDbContext : AbpDbContext
{
	public virtual IDbSet<Task> Tasks { get; set; }
	public virtual IDbSet<Person> People { get; set; }

	public SimpleTaskSystemDbContext()
		: base("Default")
	{

	}

	public SimpleTaskSystemDbContext(string nameOrConnectionString)
		: base(nameOrConnectionString)
	{

	}
}
```

# 通过Database Migrations创建数据库表
我们使用EntityFramework的Code First模式创建数据库架构。ABP模板生成的项目已经默认开启了数据迁移功能
- 我们修改SimpleTaskSystem.EntityFramework项目下Migrations文件夹下的Configuration.cs文件
- 在VS2015底部的“程序包管理器控制台”窗口中，选择默认项目并执行命令“Add-Migration InitialCreate”,程序包管理器控制台：程序包源（K）：nuget.org，默认项目（J）：SimpleTaskSystem.EntityFramework
-  然后继续在“程序包管理器控制台”执行“Update-Database”，会自动在数据库创建相应的数据表.
- 以后修改了实体，可以再次执行Add-Migration InitialCreate和Update-Database，就能很轻松的让数据库结构与实体类的同步）

# 定义仓储接口
通过仓储模式，**可以更好把业务代码与数据库操作代码更好的分离，可以针对不同的数据库有不同的实现类**，而业务代码不需要修改。
定义仓储接口的代码写到Core项目中，因为仓储接口是领域层的一部分。
我们先定义Task的仓储接口：
```
public interface ITaskRepository : IRepository<Task, long>
{
	List<Task> GetAllWithPeople(int? assignedPersonId, TaskState? state);
}
```
它继承自ABP框架中的IRepository泛型接口。
在IRepository中已经定义了常用的增删改查方法：
所以ITaskRepository默认就有了上面那些方法。可以再加上它独有的方法GetAllWithPeople(...)。
不需要为Person类创建一个仓储类，因为默认的方法已经够用了。ABP提供了一种注入通用仓储的方式，将在后面“创建应用服务”一节的TaskAppService类中看到。
