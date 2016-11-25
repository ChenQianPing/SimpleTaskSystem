using Abp.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimpleTaskSystem.People
{
    /// <summary>
    /// Person类有一个int类型的Id，因为int类型是Entity基类Id的默认类型，没有特别指定类型时，实体的Id就是int类型。
    /// </summary>
    [Table("StsPeople")]
    public class Person : Entity
    {
        public virtual string Name { get; set; }
    }
}
