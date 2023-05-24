using MyProject2.Models;

namespace MyProject2.ViewModels
{
    public class RegisterVM
    {
        public string FirsName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public DateTime BirthDate { get; set; }
        //format yyyy-mm-dd
        public int Salary { get; set; }
        public string Email { get; set; }
        public int Gender { get; set; }
        public int DepartmentID { get; set; }
        public string Password { get; set; }
    }
}
