using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using System.Text.Json.Serialization;

namespace MyProject2.Models
{
    [Table("Employee")]
    public class Employee
    {
        [Key]
        public string NIK { get; set; }
        public string FirsName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public DateTime BirthDate { get; set; }
        //format yyyy-mm-dd
        public int Salary { get; set; }
        public string Email { get; set; }
        public Gender Gender { get; set; }
        public virtual Department? Department { get; set; }

        /*[ForeignKey("Department")]
        public int DepartmentID { get; set; }*/
        [JsonIgnore]
        public virtual Account Account { get; set; }
    }
    public enum Gender
    {
        Male,
        Female
    }
}
