using MyProject2.Models;

namespace MyProject2.Repository.Interface
{
    public interface IDepartmentRepository
    {
        IEnumerable<Department> Get();
        Department Get(int ID);
        int Insert(Department department);
        int Update(Department department);
        int Delete(int ID);
    }
}
