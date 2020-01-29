using AutoMapper;
using SecretSanta.Data;

namespace SecretSanta.Business
{
    public interface IGroupService : IEntityService<Group>
    { }

    public class GroupService : EntityService<Group>, IGroupService
    {
        public GroupService(ApplicationDbContext dbContext, IMapper mapper)
            : base(dbContext, mapper)
        { }
    }
}
