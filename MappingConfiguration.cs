using System.Linq.Expressions;

namespace DotMapper;

public class MappingConfiguration
{
    public string SourceName { get; set; }
    public string DestinationName { get; set; }
    public List<MappingDictionary> MappingDictionary { get; set; } = new();
}

public class MappingDictionary
{
    public Expression SourceExpression { get; set; }
    public Expression DestinationExpression { get; set; }
    public object SourceFunc { get; set; }
    public object DestinationFunc { get; set; }
}