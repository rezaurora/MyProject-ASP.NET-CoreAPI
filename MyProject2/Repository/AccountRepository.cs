using Microsoft.EntityFrameworkCore;
using MyProject2.Context;
using MyProject2.Models;
using MyProject2.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace MyProject2.Repository
{
    public class AccountRepository
    {
        private readonly MyContext myContext;

        public AccountRepository(MyContext myContext)
        {
            this.myContext = myContext;
        }

        
        public IEnumerable<Account> Get()
        {
            return myContext.Accounts.ToList();
        }

        public int Insert(Account account)
        {
            myContext.Entry(account).State = EntityState.Added;
            var save = myContext.SaveChanges();
            return save;
        }



        public int Register(RegisterVM registerVM)
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

            
            Account account = new Account
            {
                NIK = employee.NIK,
                Password = BCrypt.Net.BCrypt.HashPassword(registerVM.Password),
            };

            myContext.Accounts.Add(account);
            return myContext.SaveChanges();
        }

        public bool isExist(string phone, string email)
        {
            return myContext.Employees.Any(e => e.Phone == phone || e.Email == email);
        }

        public Object Login(LoginVM loginVM)
        {
            Account cekaccount = myContext.Accounts.SingleOrDefault(a => a.Employee.Email == loginVM.Email);
            var pass = BCrypt.Net.BCrypt.Verify(loginVM.Password, cekaccount.Password);
            var result = (from a in myContext.Accounts
                          join e in myContext.Employees
                          on a.NIK equals e.NIK
                          where (loginVM.Email == e.Email && pass == true)
                          select new
                          {
                              NIK = a.NIK,
                              Password = a.Password,
                              Employee = a.Employee
                          });

            if (result.Count() != 0)
            {
                return result.ToList();
            }
            return null;
        }
        /*public IEnumerable<object> Login(string Email, string Password)
        {
            *//*var result = myContext.Accounts.Include(e => e.Employee)
                .Select(e => new
                {
                    NIK = e.Employee.NIK,
                    Email = e.Employee.Email,
                    Password = e.Password
                }).ToList();
            return result;*//*
            var account = myContext.Accounts.FirstOrDefault(a => a.Employee.Email == Email);
            var pass = BCrypt.Net.BCrypt.Verify(Password, account.Password);
            var result = (from a in myContext.Accounts
                          join e in myContext.Employees
                          on a.Employee.NIK equals e.NIK
                          where Email == e.Email && pass == true
                          select new
                          {
                              Email = e.Email,
                              Password = a.Password,
                              Employee = a.Employee
                          });
            
            return result.ToList();
        }*/
    }
}
