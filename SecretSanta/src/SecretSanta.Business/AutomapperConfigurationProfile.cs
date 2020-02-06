using AutoMapper;
using System.Reflection;
using SecretSanta.Business.Dto;
using Gift = SecretSanta.Data.Gift;
using User = SecretSanta.Data.User;
using Group = SecretSanta.Data.Group;

namespace SecretSanta.Business
{
    public class AutomapperConfigurationProfile : Profile
    {
        public AutomapperConfigurationProfile()
        {
            CreateMap<Gift, Dto.Gift>();
            CreateMap<GiftInput, Gift>();

            CreateMap<User, Dto.User>();
            CreateMap<UserInput, User>();

            CreateMap<Group, Dto.Group>();
            CreateMap<GroupInput, Group>();
        }

        public static IMapper CreateMapper()
        {
            var mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.AddMaps(Assembly.GetExecutingAssembly());
            });

            return mapperConfiguration.CreateMapper();
        }
    }
}
