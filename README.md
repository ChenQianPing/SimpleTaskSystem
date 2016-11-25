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

# 实现仓储类
我们将在EntityFramework项目中实现上面定义的ITaskRepository仓储接口。
通过模板建立的项目已经定义了一个**仓储基类：SimpleTaskSystemRepositoryBase（这是一种比较好的实践，因为以后可以在这个基类中添加通用的方法）**。
```
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
```
TaskRepository继承自SimpleTaskSystemRepositoryBase并且实现了上面定义的ITaskRepository接口。

# 创建应用服务（Application Services）
在Application项目中定义应用服务。首先定义Task的应用服务层的接口：
```
public interface ITaskAppService : IApplicationService
{
	GetTasksOutput GetTasks(GetTasksInput input);

	void UpdateTask(UpdateTaskInput input);

	void CreateTask(CreateTaskInput input);
}
```
ITaskAppService继承自IApplicationService，ABP自动为这个类提供一些功能特性（比如依赖注入和参数有效性验证）。
然后，我们写TaskAppService类来实现ITaskAppService接口：
```
public class TaskAppService : ApplicationService, ITaskAppService
{
	private readonly ITaskRepository _taskRepository;
	private readonly IRepository<Person> _personRepository;

	/// <summary>
	/// 构造函数自动注入我们所需要的类或接口
	/// </summary>
	/// <param name="taskRepository"></param>
	/// <param name="personRepository"></param>
	public TaskAppService(ITaskRepository taskRepository, IRepository<Person> personRepository)
	{
		_taskRepository = taskRepository;
		_personRepository = personRepository;
	}

	public void CreateTask(CreateTaskInput input)
	{
		Logger.Info("Creating a task for input: " + input);

		// 通过输入参数，创建一个新的Task实体
		var task = new Task { Description = input.Description };

		if (input.AssignedPersonId.HasValue)
		{
			task.AssignedPersonId = input.AssignedPersonId.Value;
		}

		// 调用仓储基类的Insert方法把实体保存到数据库中
		_taskRepository.Insert(task);
	}

	public GetTasksOutput GetTasks(GetTasksInput input)
	{
		// 调用Task仓储的特定方法GetAllWithPeople
		var tasks = _taskRepository.GetAllWithPeople(input.AssignedPersonId, input.State);

		// 用AutoMapper自动将List<Task>转换成List<TaskDto>
		return new GetTasksOutput
		{
			Tasks = Mapper.Map<List<TaskDto>>(tasks)
		};
	}

	public void UpdateTask(UpdateTaskInput input)
	{
		// 可以直接Logger,它在ApplicationService基类中定义的
		Logger.Info("Updating a task for input: " + input);

		// 通过仓储基类的通用方法Get，获取指定Id的Task实体对象
		var task = _taskRepository.Get(input.TaskId);

		// 修改task实体的属性值
		if (input.State.HasValue)
		{
			task.State = input.State.Value;
		}

		if (input.AssignedPersonId.HasValue)
		{
			task.AssignedPerson = _personRepository.Load(input.AssignedPersonId.Value);
		}

		// 我们都不需要调用Update方法
		// 因为应用服务层的方法默认开启了工作单元模式（Unit of Work）
		// ABP框架会工作单元完成时自动保存对实体的所有更改，除非有异常抛出。有异常时会自动回滚，因为工作单元默认开启数据库事务。
	}
}
```
TaskAppService使用仓储进行数据库操作，它通往构造函数注入仓储对象的引用。
