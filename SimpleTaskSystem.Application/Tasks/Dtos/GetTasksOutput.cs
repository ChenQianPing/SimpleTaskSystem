using Abp.Application.Services.Dto;
using System.Collections.Generic;

namespace SimpleTaskSystem.Tasks.Dtos
{
    public class GetTasksOutput : IOutputDto
    {
        public List<TaskDto> Tasks { get; set; }
    }
}
