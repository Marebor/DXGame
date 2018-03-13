using AutoMapper;

namespace DXGame.ReadModel.Infrastructure
{
    public class Mapper : Abstract.IMapper
    {
        IMapper _mapper;
        public T Map<T>(object from)
            => _mapper.Map<T>(from);
    }
}