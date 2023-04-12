using LBT_Api.Entities;
using System.ComponentModel.DataAnnotations;
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
            if (obj1 == null)
                throw new ArgumentNullException(nameof(obj1));
            if (obj2 == null)
                throw new ArgumentNullException(nameof(obj2));

            foreach (PropertyInfo prop in obj1.GetType().GetProperties())
            {
                if (prop.GetValue(obj2, null) != null)
                    prop.SetValue(obj1, prop.GetValue(obj2, null));
            }

            return obj1;
        }

        public static bool AllStringPropsAreNotNull<T>(T obj)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));

            bool isValid = true;
            foreach (PropertyInfo prop in obj.GetType().GetProperties())
            {
                var type = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
                if (type == typeof(string) && prop.GetValue(obj, null) == null)
                {
                    isValid = false;
                    break;
                }
            }

            return isValid;
        }

        /// <summary>
        /// Validates given object
        /// </summary>
        /// <typeparam name="T">Object</typeparam>
        /// <param name="obj">Object to be validated</param>
        /// <returns>Validation result</returns>
        /// <exception cref="ArgumentNullException">When obj is null</exception>
        public static bool ModelIsValid<T>(T obj)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));

            var context = new ValidationContext(obj, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();

            return Validator.TryValidateObject(obj, context, results, true);
        }
    }
}
