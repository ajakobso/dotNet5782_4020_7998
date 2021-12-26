using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PL.PO
{
    /// <summary>
    /// PLמחלקה להעתקה עמוקה בשכבת ה
    /// </summary>
    public static class Cloning
    {
        public static void Clone<T, S>(this S from, T to)
        {
            foreach (PropertyInfo propTo in to.GetType().GetProperties())
            {
                PropertyInfo propFrom = from.GetType().GetProperty(propTo.Name);
                if (propFrom == null)
                    continue;
                var value = propFrom.GetValue(from, null);
                if (value is ValueType || value is string)
                    propTo.SetValue(to, value);
            }
        }
        public static object CloneNew<S>(this S from, Type type)
        {
            object to = Activator.CreateInstance(type);
            from.Clone(to);
            return to;
        }
        internal static T Clone<T>(this T original)
        {
            T copyToObject = (T)Activator.CreateInstance(original.GetType());
            //...

            foreach (PropertyInfo sourcePropertyInfo in original.GetType().GetProperties())
            {
                PropertyInfo destPropertyInfo = original.GetType().GetProperty(sourcePropertyInfo.Name);

                destPropertyInfo.SetValue(copyToObject, sourcePropertyInfo.GetValue(original, null), null);
            }

            return copyToObject;
        }
    }
}