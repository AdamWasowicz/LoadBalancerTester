using LBT_Api.Entities;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace LBT_Api.Other
{
    public class Tools
    {
        /// <summary>
        /// Replace original properties with non-null properties from mod
        /// </summary>
        /// <typeparam name="T">object</typeparam>
        /// <param name="original">Object to be updated</param>
        /// <param name="mod">Object with new values</param>
        /// <returns>Updated original</returns>
        public static T UpdateObjectProperties<T>(T original, T mod)
        {
            if (original == null)
                throw new ArgumentNullException(nameof(original));
            if (mod == null)
                throw new ArgumentNullException(nameof(mod));

            foreach (PropertyInfo prop in original.GetType().GetProperties())
            {
                if (prop.GetValue(mod, null) != null)
                    prop.SetValue(original, prop.GetValue(mod, null));
            }

            return original;
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
