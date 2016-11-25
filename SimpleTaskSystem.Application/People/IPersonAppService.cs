using Abp.Application.Services;
using SimpleTaskSystem.People.Dtos;
using System.Threading.Tasks;

namespace SimpleTaskSystem.People
{
    public interface IPersonAppService : IApplicationService
    {
        Task<GetAllPeopleOutput> GetAllPeople();
    }
}
