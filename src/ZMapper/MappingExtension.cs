using System.Linq.Expressions;

namespace ZMapper;

public static class MappingExtension
{
    public static MemberExpression GetMemberExpression(this Expression expression)
    {
        if (expression is MemberExpression)
        {
            return (MemberExpression)expression;
        }
        else if (expression is UnaryExpression)
        {
            return (MemberExpression)((UnaryExpression)expression).Operand;
        }
        else
        {
            throw new InvalidOperationException();
        }
    }
    public static Destination SafeGet<Source, Destination>(this Source obj, System.Func<Source, Destination> selector) 
    {
        try
        {
            return selector(obj) ?? default(Destination);
        }
        catch (System.NullReferenceException e)
        {
            return default(Destination);
        }
    }
}