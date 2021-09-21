using CMS.ApplicationCore.DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Infrastructure
{
    public enum OperatorType
    {
        Integer,
        String,
        Date,
        Boolean
    }
    public enum Operator
    {
        eq,
        neq,
        lt,
        gt,
        lteq,
        gteq,
        contains,
        startsWith,
        endsWith,
        isNull,
        isNotNull,
        freeTextSearch
    }

    public static class Common
    {
        public static string CapitalFirstLetter(string value)
        {
            return char.ToUpper(value[0]) + value.Substring(1);
        }
        public static Expression<Func<T, bool>> CreateFilterExpression<T>(List<FilterRequest> filters)
        {
            Expression combined = null;
            ParameterExpression paramExpression = Expression.Parameter(typeof(T), "e");

            for (int i = 0; i < filters.Count; i++)
            {
                FilterRequest filter = filters[i];
                string field = CapitalFirstLetter(filter.field);
                string type = filter.type;//.Split("_")[1];
                string operand = filter.operand;
                string value = (type == "boolean") ? (int.Parse(filter.value) == 0 ? "false" : "true") : filter.value;

                Expression expression = null;
                PropertyInfo propertyInfo = typeof(T).GetProperty(field);
                MemberExpression memberExpression = Expression.MakeMemberAccess(paramExpression, propertyInfo);

                string typeName = propertyInfo.PropertyType.FullName;
                Type dataType = GetDataType(typeName);
                object propertyValue = Convert.ChangeType(value, dataType);

                expression = CreateExpression(memberExpression, operand, propertyValue, Type.GetType(typeName));
                if (combined == null)
                    combined = expression;
                else
                {
                    if (filters[(i - 1)].logic.ToLower() == "and")
                        combined = Expression.And(combined, expression);
                    else
                        combined = Expression.Or(combined, expression);
                }
            }

            Expression<Func<T, bool>> lamdaExpression = Expression.Lambda<Func<T, bool>>(combined, paramExpression);
            return lamdaExpression;
        }



        private static Type GetDataType(string typeName)
        {
            Type dataType = Type.GetType(typeName);
            if (Nullable.GetUnderlyingType(dataType) != null)
                dataType = Nullable.GetUnderlyingType(dataType);

            return dataType;
        }

        private static Expression CreateExpression(MemberExpression memberExpression, string operand, object value, Type dataType)
        {
            Expression expression = null;
            if (dataType.Name.ToLower() == "string")
            {
                expression = Expression.Call(memberExpression, typeof(string).GetMethod("ToLower", Type.EmptyTypes));
            }
            else
                expression = memberExpression;

            switch((Operator)Enum.Parse(typeof(Operator), operand, true)){
                case Operator.eq:
                    expression = Expression.Equal(expression, Expression.Constant(value, dataType));
                    break;
                case Operator.neq:
                    expression = Expression.NotEqual(expression, Expression.Constant(value, dataType));
                    break;
                case Operator.lt:
                    expression = Expression.LessThan(expression, Expression.Constant(value, dataType));
                    break;
                case Operator.gt:
                    expression = Expression.GreaterThan(expression, Expression.Constant(value, dataType));
                    break;
                case Operator.lteq:
                    expression = Expression.LessThanOrEqual(expression, Expression.Constant(value, dataType));
                    break;
                case Operator.gteq:
                    expression = Expression.GreaterThanOrEqual(expression, Expression.Constant(value, dataType));
                    break;
                case Operator.isNull:
                    expression = Expression.Equal(expression, Expression.Constant(null, dataType));
                    break;
                case Operator.isNotNull:
                    expression = Expression.NotEqual(expression, Expression.Constant(null, dataType));
                    break;
                case Operator.contains:
                    expression = Expression.Constant(value);
                    break;
                case Operator.freeTextSearch:
                    var freeText = typeof(SqlServerDbFunctionsExtensions).GetMethod("FreeText", new[] { typeof(DbFunctions), typeof(string), typeof(string) });
                    expression = Expression.Call(null, freeText, Expression.Constant(EF.Functions), memberExpression, Expression.Constant(value));
                    break;
                case Operator.startsWith:
                    var startsWithMethid = typeof(string).GetMethod("StartsWith", new [] { typeof(string), typeof(string) });
                    expression = Expression.Call(null, startsWithMethid, Expression.Constant(EF.Functions), memberExpression, Expression.Constant(value));
                    break;
                default:
                    var endsWithMethod = typeof(string).GetMethod("EndsWith", new[] { typeof(string), typeof(string) });
                    expression = Expression.Call(null, endsWithMethod, Expression.Constant(EF.Functions), memberExpression, Expression.Constant(value));
                    break;
            }

            return expression;
        }

        public static List<FilterRequest> BuildValuesForFreeText<T>(string searchValue)
        {
            List<FilterRequest> filters = new List<FilterRequest>();
            if (!string.IsNullOrEmpty(searchValue))
            {
                var properties = typeof(T).GetProperties();
                foreach (var property in properties)
                {
                    filters.Add(new FilterRequest() {

                        field = property.Name,
                        type = property.PropertyType.Name,
                        operand = "freeTextSearch",
                        value = searchValue
                    });

                }
            }
            return filters;
            
        }
    }

    public static class LinqExtension
    {
        public static IQueryable<TEntity> OrderBy<TEntity>(this IQueryable<TEntity> source, List<SorterRequest> sorters) where TEntity : class
        {
            var queryExpr = source.Expression;
            var methodAsc = "OrderBy";
            var methodDesc = "OrderByDescending";

            //var orderByValues = orderByStrValues.Trim().Split(',').Select(x => x.Trim()).ToList();

            foreach (var sorter in sorters)
            {
                var command = sorter.sortOrder.ToUpper() == "DESC" ? methodDesc : methodAsc;

                //Get propertyname and remove optional ASC or DESC
                var propertyName = Common.CapitalFirstLetter(sorter.sortName);

                var type = typeof(TEntity);
                var parameter = Expression.Parameter(type, "p");

                PropertyInfo property;
                MemberExpression propertyAccess;

                if (propertyName.Contains('.'))
                {
                    // support to be sorted on child fields. 
                    var childProperties = propertyName.Split('.');

                    property = SearchProperty(typeof(TEntity), childProperties[0]);
                    if (property == null)
                        continue;

                    propertyAccess = Expression.MakeMemberAccess(parameter, property);

                    for (int i = 1; i < childProperties.Length; i++)
                    {
                        var t = property.PropertyType;
                        property = SearchProperty(t, childProperties[i]);

                        if (property == null)
                            continue;

                        propertyAccess = Expression.MakeMemberAccess(propertyAccess, property);
                    }
                }
                else
                {
                    property = null;
                    property = SearchProperty(type, propertyName);

                    if (property == null)
                        continue;

                    propertyAccess = Expression.MakeMemberAccess(parameter, property);
                }

                var orderByExpression = Expression.Lambda(propertyAccess, parameter);

                queryExpr = Expression.Call(typeof(Queryable), command, new Type[] { type, property.PropertyType }, queryExpr, Expression.Quote(orderByExpression));

                methodAsc = "ThenBy";
                methodDesc = "ThenByDescending";
            }

            return source.Provider.CreateQuery<TEntity>(queryExpr); ;
        }

        private static PropertyInfo SearchProperty(Type type, string propertyName)
        {
            foreach (var item in type.GetProperties())
                if (item.Name.ToLower() == propertyName.ToLower())
                    return item;
            return null;
        }
    }
}
