using System.Collections.Generic;
using System.Threading.Tasks;
using SimpleTaskSystem.People.Dtos;
using Abp.Domain.Repositories;
using Abp.AutoMapper;

namespace SimpleTaskSystem.People
{
    public class PersonAppService : IPersonAppService
    {
        private readonly IRepository<Person> _personRepository;

        /// <summary>
        /// ABP provides that we can directly inject IRepository<Person> (without creating any repository class)
        /// </summary>
        /// <param name="personRepository"></param>
        public PersonAppService(IRepository<Person> personRepository)
        {
            _personRepository = personRepository;
        }

        /// <summary>
        /// This method uses async pattern that is supported by ASP.NET Boilerplate
        /// </summary>
        /// <returns></returns>
        public async Task<GetAllPeopleOutput> GetAllPeople()
        {
            var people = await _personRepository.GetAllListAsync();
            return new GetAllPeopleOutput
            {
                People = people.MapTo<List<PersonDto>>()
            };
        }
    }
}
