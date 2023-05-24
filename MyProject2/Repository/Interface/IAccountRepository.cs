using MyProject2.Models;

namespace MyProject2.Repository.Interface
{
    public interface IAccountRepository
    {
        IEnumerable<Account> Get();
        int Insert(Account account);
    }
}
