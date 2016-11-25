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
        *   ����ʹ��EntityFramework��Code Firstģʽ�������ݿ�ܹ���ABPģ�����ɵ���Ŀ�Ѿ�Ĭ�Ͽ���������Ǩ�ƹ���
        *   1.�����޸�SimpleTaskSystem.EntityFramework��Ŀ��Migrations�ļ����µ�Configuration.cs�ļ�
        *   2.��VS2015�ײ��ġ����������������̨�������У�ѡ��Ĭ����Ŀ��ִ�����Add-Migration InitialCreate��,
        *     ���������������̨�������Դ��K����nuget.org��Ĭ����Ŀ��J����SimpleTaskSystem.EntityFramework
        *   3.Ȼ������ڡ����������������̨��ִ�С�Update-Database�������Զ������ݿⴴ����Ӧ�����ݱ�.
        *   4.�Ժ��޸���ʵ�壬�����ٴ�ִ��Add-Migration InitialCreate��Update-Database�����ܺ����ɵ������ݿ�ṹ��ʵ�����ͬ����
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
