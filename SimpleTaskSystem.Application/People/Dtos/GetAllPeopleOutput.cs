using Abp.Application.Services.Dto;
using System.Collections.Generic;

namespace SimpleTaskSystem.People.Dtos
{
    public class GetAllPeopleOutput : IOutputDto
    {
        public List<PersonDto> People { get; set; }
    }
}
