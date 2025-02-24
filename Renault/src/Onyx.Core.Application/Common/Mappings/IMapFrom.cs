using AutoMapper;

namespace Onyx.Application.Common.Mappings;

public interface IMapFrom<T>
{
    void Mapping(Profile profile)
    {
        try
        {
            profile.CreateMap(typeof(T), GetType());
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
    }
}
