using DataAccess.Repositories.Implementation;
using Repository.Abstraction;
using Repository.Entity;
namespace Repository.Implementation;

public class UserRepository: GenericRepository<UsersActivityHistory>, IUserRepository
{
    public UserRepository(Tech42_TelegramBotDBContext context):base(context){ }
}
