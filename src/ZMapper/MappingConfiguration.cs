using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography;

namespace ZMapper;

public class MappingConfiguration
{
    private List<MappingDictionary> _mappingDictionary;

    public void AddConfiguration<Source, Destination>(Action<MappingProfile<Source, Destination>> action)
    {
        var mappingDictionary = new MappingProfile<Source, Destination>();

        action.Invoke(mappingDictionary);
        _mappingDictionary = mappingDictionary.MappingDictionary;
    }

    public Destination MapTo<Source, Destination>(Source source)
    {
        var sourceName = typeof(Source).Name;
        var destinationName = typeof(Destination).Name;

        var mappingDics = _mappingDictionary.Where(x => x.SourceName == sourceName && x.DestinationName == destinationName);

        if (mappingDics is null)
            throw new InvalidOperationException("Mapping not found");

        var destination = Activator.CreateInstance<Destination>();

        foreach (var mappingDic in mappingDics)
        {
            var sourceExpression = mappingDic.SourceExpression as Expression<Func<Source, object>>;
            var destinationExpression = mappingDic.DestinationExpression as Expression<Func<Destination, object>>;

            var value = source.SafeGet(sourceExpression.Compile());
            var property = destinationExpression.Body.GetMemberExpression().Member as PropertyInfo;

            property.SetValue(destination, value);
        }

        return destination;
    }
    public async Task<Destination> MapToAsync<Source, Destination>(Source source)
    {
        var sourceName = typeof(Source).Name;
        var destinationName = typeof(Destination).Name;

        var mappingDics = _mappingDictionary.Where(x => x.SourceName == sourceName && x.DestinationName == destinationName);

        if (mappingDics is null)
            throw new InvalidOperationException("Mapping not found");

        var destination = Activator.CreateInstance<Destination>();

        await Parallel.ForEachAsync(mappingDics, (mappingDic, ct) =>
        {

            var sourceExpression = mappingDic.SourceExpression as Expression<Func<Source, object>>;
            var destinationExpression = mappingDic.DestinationExpression as Expression<Func<Destination, object>>;

            var value = source.SafeGet(sourceExpression.Compile());
            var property = destinationExpression.Body.GetMemberExpression().Member as PropertyInfo;

            property.SetValue(destination, value);

            return ValueTask.CompletedTask;
        });

        return destination;
    }
}

public class MappingProfile<Source, Destination>
{
    public List<MappingDictionary> MappingDictionary { get; set; } = new();
    public void CreateMap(Expression<Func<Source, object>> source, Expression<Func<Destination, object>> destination)
    {
        MappingDictionary.Add(new MappingDictionary()
        {
            DestinationExpression = destination,
            SourceExpression = source,
            SourceName = typeof(Source).Name,
            DestinationName = typeof(Destination).Name,
        });
    }
}