using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace ExpressionTrees.Task2.ExpressionMapping
{
    public class MappingGenerator
    {
        public Mapper<TSource, TDestination> Generate<TSource, TDestination>() where TDestination: new()
        {
            //TODO: 1. => get prop of source
            //TODO: 2. => get props of destination
            //TODO: 3. => set values and types from source to destination

            var parameter = Expression.Parameter(typeof(TSource), "source");
            var inInstance = Expression.Variable(typeof(TSource), "input");
            var outInstance = Expression.Variable(typeof(TDestination), "result");

            Type inType = typeof(TSource);
            PropertyInfo[] inProperties = inType.GetProperties();

            Type outType = typeof(TDestination);
            var outProperties = outType.GetProperties().ToDictionary(x => x.Name);

            var expressions = new List<Expression>();

            expressions.Add(Expression.Assign(inInstance, parameter));
            expressions.Add(Expression.Assign(outInstance,Expression.New(typeof(TDestination))));


            for (int i = 0; i < inProperties.Length; i++)
            {
                if (!outProperties.TryGetValue(inProperties[i].Name, out var outProperty)) continue;

                var sourceValue = Expression.Property(inInstance, inProperties[i].Name);
                var outValue = Expression.Property(outInstance, outProperty);

                expressions.Add(Expression.Assign(outValue, sourceValue));
            }

            expressions.Add(outInstance);

            var body = Expression.Block(new[] { inInstance, outInstance }, expressions);

            return new Mapper<TSource, TDestination>(Expression.Lambda<Func<TSource, TDestination>>(body, parameter).Compile());
        }
    }
}
