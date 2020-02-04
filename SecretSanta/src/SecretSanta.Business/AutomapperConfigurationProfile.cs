using AutoMapper;
using SecretSanta.Data;
using System.Reflection;

namespace SecretSanta.Business
{
    public class AutomapperConfigurationProfile : Profile
    {
        public AutomapperConfigurationProfile()
        {
            CreateMap<EntityBase, EntityBase>()
                .ForMember(property => property.Id, option => option.Ignore());
            CreateMap<FingerPrintEntityBase, FingerPrintEntityBase>()
                .ForMember(property => property.CreatedBy, option => option.Ignore())
                .IncludeBase<EntityBase, EntityBase>();
            CreateMap<Gift, Gift>().IncludeBase<FingerPrintEntityBase, FingerPrintEntityBase>();
            CreateMap<User, User>().IncludeBase<FingerPrintEntityBase, FingerPrintEntityBase>();
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
