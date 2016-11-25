using SimpleTaskSystem.People;

namespace SimpleTaskSystem.Migrations
{
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<SimpleTaskSystem.EntityFramework.SimpleTaskSystemDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "SimpleTaskSystem";
        }

        /* NOTE: 
        *   我们使用EntityFramework的Code First模式创建数据库架构。ABP模板生成的项目已经默认开启了数据迁移功能
        *   1.我们修改SimpleTaskSystem.EntityFramework项目下Migrations文件夹下的Configuration.cs文件
        *   2.在VS2015底部的“程序包管理器控制台”窗口中，选择默认项目并执行命令“Add-Migration InitialCreate”,
        *     程序包管理器控制台：程序包源（K）：nuget.org，默认项目（J）：SimpleTaskSystem.EntityFramework
        *   3.然后继续在“程序包管理器控制台”执行“Update-Database”，会自动在数据库创建相应的数据表.
        *   4.以后修改了实体，可以再次执行Add-Migration InitialCreate和Update-Database，就能很轻松的让数据库结构与实体类的同步）
        */
        protected override void Seed(SimpleTaskSystem.EntityFramework.SimpleTaskSystemDbContext context)
        {
            // This method will be called every time after migrating to the latest version.
            // You can add any seed data here...
            context.People.AddOrUpdate(
            p => p.Name,
            new Person { Name = "Isaac Asimov" },
            new Person { Name = "Thomas More" },
            new Person { Name = "George Orwell" },
            new Person { Name = "Douglas Adams" }
            );
        }
    }
}
