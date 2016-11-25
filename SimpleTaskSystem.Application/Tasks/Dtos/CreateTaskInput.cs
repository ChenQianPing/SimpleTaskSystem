using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SimpleTaskSystem.Tasks.Dtos
{
    /// <summary>
    /// 数据验证
    /// 如果应用服务(Application Service)方法的参数对象实现了IInputDto或IValidate接口，ABP会自动进行参数有效性验证。
    /// </summary>
    public class CreateTaskInput : IInputDto
    {
        public int? AssignedPersonId { get; set; }

        /// <summary>
        /// Description属性通过注解指定它是必填项。也可以使用其他 Data Annotation 特性。
        /// </summary>
        [Required]
        public string Description { get; set; }

        public override string ToString()
        {
            return string.Format("[CreateTaskInput > AssignedPersonId = {0}, Description = {1}]", AssignedPersonId, Description);
        }
    }
}
