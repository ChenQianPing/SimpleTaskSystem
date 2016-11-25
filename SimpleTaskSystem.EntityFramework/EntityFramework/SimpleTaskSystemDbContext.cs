using Abp.EntityFramework;
using SimpleTaskSystem.People;
using SimpleTaskSystem.Tasks;
using System.Data.Entity;

namespace SimpleTaskSystem.EntityFramework
{
    public class SimpleTaskSystemDbContext : AbpDbContext
    {
        //TODO: Define an IDbSet for each Entity...

        //Example:
        //public virtual IDbSet<User> Users { get; set; }

        /* NOTE: 
         *   使用EntityFramework需要先定义DbContext类，ABP的模板已经创建了DbContext文件，我们只需要把Task和Person类添加到IDbSet
         *   1.提到DbContext,对于经常使用DbFirst模式的开发者来说已经再熟悉不过了，EntityFramework全靠这员大将。
         *     它的作用是代表与数据库连接的会话，提供了查询、状态跟踪、保存等功能。
         *   2.还有一个重要的对象是DbSet，对实体类型提供了集合操作，比如Add、Attach、Remove。继承了DbQuery，所以可以提供查询功能。
         */
        public virtual IDbSet<Task> Tasks { get; set; }
        public virtual IDbSet<Person> People { get; set; }


        /* NOTE: 
         *   Setting "Default" to base class helps us when working migration commands on Package Manager Console.
         *   But it may cause problems when working Migrate.exe of EF. If you will apply migrations on command line, do not
         *   pass connection string name to base classes. ABP works either way.

         *   Web.config -> connectionStrings -> Default
         */
        public SimpleTaskSystemDbContext()
            : base("Default")
        {

        }

        /* NOTE:
         *   This constructor is used by ABP to pass connection string defined in SimpleTaskSystemDataModule.PreInitialize.
         *   Notice that, actually you will not directly create an instance of SimpleTaskSystemDbContext since ABP automatically handles it.
         */
        public SimpleTaskSystemDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {

        }
    }
}
