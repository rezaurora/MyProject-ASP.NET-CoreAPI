using Microsoft.EntityFrameworkCore;
using MyProject2.Context;
using MyProject2.Models;

namespace MyProject2.Repository
{
    public class DepartmentRepository
    {
        private readonly MyContext myContext;

        public DepartmentRepository(MyContext myContext)
        {
            this.myContext = myContext;
        }

        public async Task<IEnumerable<Department>> Get()
        {
            return await myContext.Departments.ToListAsync();
        }

        public Department Get(int ID)
        {
            return myContext.Departments.Find(ID);
        }

        public int Insert(Department department)
        {
            myContext.Departments.Add(department);
            var save = myContext.SaveChanges();
            return save;
        }

        public int Update(Department department)
        {
            //myContext.Departments.Update(department);
            myContext.Departments.Entry(department).State = EntityState.Modified;
            var save = myContext.SaveChanges();
            return save;
        }

        public int Delete(int ID)
        {
            var dataDepart = myContext.Departments.Find(ID);
            if (dataDepart != null)
            {
                myContext.Departments.Remove(dataDepart);
                var save = myContext.SaveChanges();
                return save;
            }
            return 0;
        }
    }
}
