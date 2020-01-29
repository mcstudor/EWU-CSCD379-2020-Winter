using AutoMapper;
using SecretSanta.Data;

namespace SecretSanta.Business
{
    public interface IUserService : IEntityService<User>
    {
    }

    public class UserService : EntityService<User>, IUserService
    {
        public UserService(ApplicationDbContext dbContext, IMapper mapper) 
        : base(dbContext, mapper) { }
    }
}
