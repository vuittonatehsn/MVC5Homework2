using PagedList.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MVC5Homework2.Models
{
    public static class testReflection
    {

        //public static RouteValueDictionary GetInfo<T, P>(this HtmlHelper html, Expression<Func<T, P>> action) where T : class
        //{
        //    var expression = (MemberExpression)action.Body;
        //    string name = expression.Member.Name;

        //    return GetInfo(html, name);
        //}

        public static string GetName(Expression<Func<object>> exp)
        {
            MemberExpression body = exp.Body as MemberExpression;

            if (body == null)
            {
                UnaryExpression ubody = (UnaryExpression)exp.Body;
                body = ubody.Operand as MemberExpression;
            }
            return body.Member.Name;
        }


        public static void  Action1()
        {
            MethodInfo method = typeof(客戶資料).GetProperty("Password").GetSetMethod();
            Action<客戶資料, string> setter1 = (Action<客戶資料, string>)
                Delegate.CreateDelegate(typeof(Action<客戶資料, string>), method);

            客戶資料 foo = new 客戶資料();
            setter1(foo, "abc");
            Console.WriteLine(foo.Password);
            
        }


        //Source: http://stackoverflow.com/questions/4726047/dynamically-sorting-with-linq/4726308#4726308
        // We want the overload which doesn't take an EqualityComparer.
        private static MethodInfo OrderByMethod = typeof(Enumerable)
            .GetMethods(BindingFlags.Public | BindingFlags.Static)
            .Where(method => method.Name == "OrderBy"
                   && method.GetParameters().Length == 2)
            .Single();

        public static IOrderedEnumerable<TSource> OrderByProperty<TSource>(
            this IEnumerable<TSource> source,
            string propertyName)
        {
            // TODO: Lots of validation :)
            PropertyInfo property = typeof(TSource).GetProperty(propertyName);
            MethodInfo getter = property.GetGetMethod();
            Type propType = property.PropertyType;
            Type funcType = typeof(Func<,>).MakeGenericType(typeof(TSource), propType);
            Delegate func = Delegate.CreateDelegate(funcType, getter);
            MethodInfo constructedMethod = OrderByMethod.MakeGenericMethod(
                typeof(TSource), propType);
            return (IOrderedEnumerable<TSource>)constructedMethod.Invoke(null,
                new object[] { source, func });
        }


    }
}