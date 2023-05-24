using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyProject2.Models
{
    [Table("Department")]
    public class Department
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
    }
}
