using AutoMapper.Internal;
using LBT_Api.Entities;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace LBT_Api.Other
{
    public class Tools
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T">object</typeparam>
        /// <typeparam name="Z">object</typeparam>
        /// <param name="original">original object</param>
        /// <param name="mod">object with modifications</param>
        /// <returns>original with replaced fields</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static T UpdateObjectProperties<T, Z>(T original, Z mod)
        {
            if (original == null)
                throw new ArgumentNullException(nameof(original));
            if (mod == null)
                throw new ArgumentNullException(nameof(mod));

            var typesMod = mod.GetType().GetProperties();
            var typesOri = original.GetType().GetProperties();

            foreach (PropertyInfo propOri in typesOri)
            {
                foreach (var propMod in typesMod)
                {
                    if (propOri.Name == propMod.Name && propMod.GetValue(mod) != null)
                    {
                        propOri.SetMemberValue(original, propMod.GetValue(mod)!);
                    }
                }
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
