using AutoMapper;
using DXGame.Messages.Events.Player;
using DXGame.Messages.Events.Playroom;
using DXGame.ReadModel.Models;

namespace DXGame.ReadModel.Infrastructure
{
    public class Mapper : Abstract.IMapper
    {
        public T Map<T>(object from)
            => AutoMapper.Mapper.Map<T>(from);
    }

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<PlayroomCreated, PlayroomProjection>();
            CreateMap<PlayerCreated, PlayerProjection>();
        }
    }
}