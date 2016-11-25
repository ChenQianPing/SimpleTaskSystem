using System.Reflection;
using Abp.Application.Services;
using Abp.Modules;
using Abp.WebApi;
using Abp.WebApi.Controllers.Dynamic.Builders;

namespace SimpleTaskSystem
{
    [DependsOn(typeof(AbpWebApiModule), typeof(SimpleTaskSystemApplicationModule))]
    public class SimpleTaskSystemWebApiModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());

            /* NOTE: ChenQP
            *   ABP可以非常轻松地把Application Service的public方法发布成Web Api接口，可以供客户端通过ajax调用。
            *   SimpleTaskSystemApplicationModule这个程序集中所有继承了IApplicationService接口的类，
            *   都会自动创建相应的ApiController，其中的公开方法，就会转换成WebApi接口方法。

            *   可以通过http://xxx/api/services/tasksystem/Task/GetTasks这样的路由地址进行调用。
            */
            DynamicApiControllerBuilder
                .ForAll<IApplicationService>(typeof(SimpleTaskSystemApplicationModule).Assembly, "tasksystem")
                .Build();
        }
    }
}
