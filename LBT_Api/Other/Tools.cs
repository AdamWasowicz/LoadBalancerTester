using LBT_Api.Entities;
using System.Reflection;

namespace LBT_Api.Other
{
    public class Tools
    {
        /// <summary>
        /// Replace obj1 properties with non-null properties from obj2
        /// </summary>
        /// <typeparam name="T">object</typeparam>
        /// <param name="obj1">Object to be updated</param>
        /// <param name="obj2">Object with new values</param>
        /// <returns>Updated obj1</returns>
        public static T UpdateObjectProperties<T>(T obj1, T obj2)
        {
            foreach (PropertyInfo prop in obj1.GetType().GetProperties())
            {
                if (prop.GetValue(obj2, null) != null)
                    prop.SetValue(obj1, prop.GetValue(obj2, null));
            }

            return obj1;
        }
    }
}
