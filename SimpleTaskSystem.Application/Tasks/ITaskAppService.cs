using Abp.Application.Services;
using SimpleTaskSystem.Tasks.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleTaskSystem.Tasks
{
    // ITaskAppService继承自IApplicationService，ABP自动为这个类提供一些功能特性（比如依赖注入和参数有效性验证）。
    public interface ITaskAppService : IApplicationService
    {
        GetTasksOutput GetTasks(GetTasksInput input);

        void UpdateTask(UpdateTaskInput input);

        void CreateTask(CreateTaskInput input);
    }
}
