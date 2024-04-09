using System.Linq.Expressions;

namespace DotMapper;

public class Configuration<T, T1>
{
    internal List<MappingDictionary>  MappingDictionary = new();
    /*public void CreateMap(Expression<Func<T, object>> src, Expression<Func<T1, object>> dest)
    {
        MappingDictionary.Add(new()
        {
            SourceExpression = src.Body, 
            DestinationExpression = dest.Body
        });
    }*/
    public void CreateMap(Func<T, object> src, Func<T1, object> dest)
    {
        MappingDictionary.Add(new()
        {
            SourceFunc = src,
            DestinationFunc= dest
        });
    }
}