using System.Linq.Expressions;
using System.Reflection;

namespace DotMapper;

public static class Mapping
{
    public static IQueryable<TDto> ToDto<TEntity, TDto>(this IQueryable<TEntity> queryable)
    {
        List<MemberAssignment> bindings = new List<MemberAssignment>();
        var mappingDictionary = MappingConfiguration.FirstOrDefault(c => c.SourceName == typeof(TEntity).Name && c.DestinationName == typeof(TDto).Name)?.MappingDictionary;

        var re = mappingDictionary.FirstOrDefault().DestinationExpression.GetMemberExpression().Expression.ToString();
        var parameter = Expression.Parameter(typeof(TEntity), re);
        foreach (var map in mappingDictionary)
        {
            var srcParameterExp = map.SourceExpression.GetMemberExpression();
            var property = srcParameterExp.ToString().Split(".").Where(c => c != re);
            if (property.Count() > 1)
            {
                var propertyExpression = Expression.Property(parameter, property.FirstOrDefault());

                property = property.Skip(1);

                foreach (var prop in property)
                {
                    propertyExpression = Expression.Property(propertyExpression, prop);
                }
                var memberInfo = typeof(TDto).GetProperty(map.DestinationExpression.GetMemberExpression().Member.Name);

                var bindingsExp = Expression.Bind(memberInfo, propertyExpression);

                bindings.Add(bindingsExp);
            }
            else
            {
                foreach (var prop in property)
                {
                    var propertyExpression = Expression.Property(parameter, prop);

                    var memberInfo = typeof(TDto).GetProperty(map.DestinationExpression.GetMemberExpression().Member.Name);

                    var bindingsExp = Expression.Bind(memberInfo, propertyExpression);
                    bindings.Add(bindingsExp);
                }
            }

        }

        var newT = Expression.MemberInit(Expression.New(typeof(TDto)), bindings);

        var lambda = Expression.Lambda<Func<TEntity, TDto>>(newT, parameter);
        var result = queryable.Select(lambda);

        return result;
    }


    public static TDestination Map<TSource, TDestination>(TSource entity)
    {
        var mappingDictionary = MappingConfiguration.FirstOrDefault(c => c.SourceName == typeof(TSource).Name
                                                                         && c.DestinationName == typeof(TDestination).Name)?.MappingDictionary;
        var destination = Activator.CreateInstance<TDestination>();
        var parameterExpression = mappingDictionary.FirstOrDefault().SourceExpression.GetMemberExpression().Expression.ToString();
        foreach (var map in mappingDictionary)
        {
            //map.DestinationExpression.GetMemberExpression().Member.SetValue(entity, map.SourceExpression.GetMemberExpression().Member.GetValue(entity));
            var sourcePropertyInfo = map.SourceExpression.GetMemberExpression().Member as PropertyInfo;

            var sourcePropertyInfos = map.SourceExpression.GetMemberExpression().ToString().Split(".")
                .Where(c => c != parameterExpression);
            if (sourcePropertyInfos.Count() > 1)
            {
                var result = GetValueFromNavigation<TSource, TDestination>(entity, sourcePropertyInfos);
                if (!result)
                {
                    continue;
                }

                var sourceProperty = Convert.ChangeType(entity.GetType().GetProperty(sourcePropertyInfo.Name).GetValue(entity), sourcePropertyInfo.PropertyType);

                var destinationProperty = destination.GetType().GetProperty(map.DestinationExpression.GetMemberExpression().Member.Name);

                destinationProperty.SetValue(destination, sourceProperty);


            }
            else
            {
                var sourceProperty = Convert.ChangeType(entity.GetType().GetProperty(sourcePropertyInfo.Name).GetValue(entity), sourcePropertyInfo.PropertyType);
                var destinationProperty = destination.GetType().GetProperty(map.DestinationExpression.GetMemberExpression().Member.Name);
                destinationProperty.SetValue(destination, sourceProperty);
            }


        }
        return destination;
    }

    private static bool GetValueFromNavigation<TSource, TDestination>(TSource entity, IEnumerable<string> sourcePropertyInfo)
    {
        var sb = new List<string>();

        foreach (var prop in sourcePropertyInfo)
        {
            sb.Add(prop);



            var ssprop = string.Join(".", sb);

            var property = entity.GetType().GetProperty(ssprop);

            var value = property.GetValue(entity);

            if (value is null)
            {
                return false;
            }


        }

        return true;
    }

    static List<MappingConfiguration> MappingConfiguration = new();

    private static Configuration<object, object> configuration;

    /*public static void CreateMap<Src, Dest>(Action<Configuration<Src, Dest>> action)
    {
        var configuration = new Configuration<Src, Dest>();
        action.Invoke(configuration);
        MappingConfiguration.Add(new()
        {
            SourceName = typeof(Src).Name,
            DestinationName = typeof(Dest).Name,
            MappingDictionary = configuration.MappingDictionary
        });
    }*/
    public static void CreateMap<Src, Dest>(Action<Configuration<Src, Dest>> action,Src user)
    {
        var _configuration = new Configuration<Src, Dest>();
        action.Invoke(_configuration);



    }
    /* public static void CreateMap<Src, Dest>(Func<Configuration<Src, Dest>> action)
     {

     }*/
    private static MemberExpression GetMemberExpression(this Expression expression)
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

    public static void MapTo<TSrc, TDest>(TSrc t)
    {
    }
}

public interface IMapper
{

}