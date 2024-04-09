using System.Linq.Expressions;

namespace ZMapper;

public class MappingDictionary
{
    public Expression SourceExpression { get; set; }
    public Expression DestinationExpression { get; set; }
    public string SourceName { get; set; }
    public string DestinationName { get; set; }
}