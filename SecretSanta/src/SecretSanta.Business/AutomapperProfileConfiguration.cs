using System.Reflection;
using AutoMapper;
using SecretSanta.Data;

namespace SecretSanta.Business
{
    public class AutomapperProfileConfiguration : Profile
    {
        public AutomapperProfileConfiguration()
        {
            CreateMap<EntityBase, EntityBase>().ForMember(property => property.Id,
                option => option.Ignore());

            CreateMap<User, User>().IncludeBase<EntityBase, EntityBase>();

            CreateMap<Gift, Gift>().IncludeBase<EntityBase, EntityBase>();

            CreateMap<Group, Group>().IncludeBase<EntityBase, EntityBase>();

        }

        static public IMapper CreateMapper()
        {
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddMaps(Assembly.GetExecutingAssembly());
            } );
            return mapperConfig.CreateMapper();
        }
    }
}
