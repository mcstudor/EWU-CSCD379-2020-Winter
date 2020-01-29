using AutoMapper;
using SecretSanta.Data;

namespace SecretSanta.Business
{
    public interface IGiftService : IEntityService<Gift>
    { }

    public class GiftService : EntityService<Gift>, IGiftService
    {
        public GiftService(ApplicationDbContext dbContext, IMapper mapper)
            : base(dbContext, mapper)
        { }
    }
}
