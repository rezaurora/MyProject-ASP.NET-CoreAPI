using Microsoft.EntityFrameworkCore;
using MyProject2.Context;
using MyProject2.Models;
using MyProject2.ViewModels;

namespace MyProject2.Repository
{
    public class EmployeeRepository
    {
        private readonly MyContext myContext;

        public class CheckValidation
        {
            public const int Successful = 1;
            public const int NullPointer = 2;

            public const Employee[] NullPointerEmpList = null;
            public const Employee NullPointerEmp = null;
        }


        public EmployeeRepository(MyContext myContext)
        {
            this.myContext = myContext;
        }

        public IEnumerable<Employee> Get()
        {
            return myContext.Employees.ToList();

        }

        public Employee Get(string NIK)
        {
            return myContext.Employees.Find(NIK); //ini khusus cuma bisa nyari lewat Primary Key aja
                                                  //return myContext.Employees.FirstOrDefault(e => e.NIK == NIK); ini beda tempatnya aja
                                                  //return myContext.Employees.Where(e=>e.NIK==NIK).FirstOrDefault(); data yg sama dikeluarin semua
                                                  //return myContext.Employees.Where(e=>e.NIK==NIK).SingleOrDefault(); mastiin datanya gak duplicate
        }

        public IEnumerable<Object> GetRows()
        {
            var myObject = (from e in myContext.Employees
                            join d in myContext.Departments
                            on e.Department.ID equals d.ID
                            select new
                            {
                                NIK = e.NIK,
                                FullName = e.FirsName + " " + e.LastName,
                                DepartmentName = d.Name
                            });
            if (myObject.Count() != 0)
            {
                return myObject.ToList();
            }
            return CheckValidation.NullPointerEmpList;
        }

        public int Insert(Employee employee)
        {
            bool duplicate = false;
            var checkDuplicate = myContext.Employees.Where(e => e.Email == employee.Email || e.Phone == employee.Phone).FirstOrDefault();
            if (checkDuplicate != null)
            {
                duplicate = true;
            }
            else
            {
                var lastEmployee = myContext.Employees.OrderByDescending(e => e.NIK).FirstOrDefault();
                var currentDate = DateTime.Now.ToString("ddMMyyyy");
                int sequenceNumber = 1;
                if (lastEmployee != null && lastEmployee.NIK.StartsWith(currentDate))
                {
                    sequenceNumber = int.Parse(lastEmployee.NIK.Substring(currentDate.Length)) + 1;
                }
                string newNIK = $"{currentDate}{sequenceNumber.ToString("D3")}";
                employee.NIK = newNIK;
                //myContext.Employees.Add(employee);
                myContext.Entry(employee).State = EntityState.Added;
                var save = myContext.SaveChanges();
                return save;
            }
            if (duplicate)
            {
                return 0;
            }
            else
            {
                return 1;
            }

        }

        public int Update(Employee employee)
        {
            myContext.Employees.Entry(employee).State = EntityState.Modified;
            //myContext.Employees.Update(employee);
            var savee = myContext.SaveChanges();
            return savee;

        }
        public int Delete(string NIK)
        {
            var employeeData = myContext.Employees.Find(NIK);
            if (employeeData != null)
            {
                myContext.Employees.Remove(employeeData);
                var savee = myContext.SaveChanges();
                return savee;
            }
            return 0;
        }

        /*public string GetNIK()
        {
            var lastEmployee = myContext.Employees.OrderByDescending(e => e.NIK).FirstOrDefault();
            var currentDate = DateTime.Now.ToString("ddMMyyyy");
            int sequenceNumber = 1;
            if (lastEmployee != null && lastEmployee.NIK.StartsWith(currentDate))
            {
                sequenceNumber = int.Parse(lastEmployee.NIK.Substring(currentDate.Length)) + 1;
            }
            string newNIK = $"{currentDate}{sequenceNumber.ToString("D3")}";
            return newNIK;
        }*/

        /*public int Register(RegisterVM registerVM)
        {
            var newNIK = DateTime.Today.ToString("ddMMyyyy") + (myContext.Employees.Count() + 1).ToString("D3");
            Employee employee = new Employee
            {
                NIK = newNIK,
                FirsName = registerVM.FirsName,
                LastName = registerVM.LastName,
                Phone = registerVM.Phone,
                BirthDate = registerVM.BirthDate,
                Salary = registerVM.Salary,
                Email = registerVM.Email,
                Gender = (Gender)registerVM.Gender,
                Department = new Department { ID = registerVM.DepartmentID }
            };
            myContext.Entry(employee).State = EntityState.Added;

            var encrypt = BCrypt.Net.BCrypt.HashPassword(registerVM.Password);

            Account account = new Account
            {
                NIK = employee.NIK,
                Password = registerVM.Password,
            };           

            myContext.Accounts.Add(account);
            return myContext.SaveChanges();
        }*/
    }
}
