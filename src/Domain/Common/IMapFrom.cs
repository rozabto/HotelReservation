using AutoMapper;

namespace Domain.Common
{
    public interface IMapFrom<T>
    {
        void Mapping(Profile profile);
    }
}