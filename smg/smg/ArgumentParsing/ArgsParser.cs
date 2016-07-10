using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using smg.ArgumentParsing.Attributes;
using smg.ArgumentParsing.Exceptions;

namespace smg.ArgumentParsing
{
    public class ArgsParser
    {
        /// <summary>
        /// Initializes a type which describes the arguments using a collection of arguments.
        /// </summary>
        /// <typeparam name="T">The type which describes the arguments.</typeparam>
        /// <param name="args">The arguments to initialize the initialized type with.</param>
        /// <returns>An instance of type <typeparamref name="T"/> initialized with the given <paramref name="args"/>.</returns>
        public static T Parse<T>(IEnumerable<string> args) where T : class, new()
        {
            if (args == null || !args.Any())
            {
                return null;
            }
            T arguments = new T();
            foreach (PropertyInfo propertyInfo in typeof(T).GetProperties())
            {
                BooleanArgAttribute booleanArgAttribute = propertyInfo.GetCustomAttributes<BooleanArgAttribute>().FirstOrDefault();
                if (booleanArgAttribute != null)
                {
                    if (propertyInfo.PropertyType != typeof (bool))
                    {
                        throw new BooleanArgumentPropertyIsNotBoolException($"The property {propertyInfo.Name} of the type {typeof(T).FullName} which represents a boolean argument is not of type bool.");
                    }
                    propertyInfo.SetValue(arguments, BooleanArgumentExists(args, booleanArgAttribute));
                    continue;
                }
            }
            return arguments;
        }

        /// <summary>
        /// Returns a value indicating whether a boolean argument exists within a collection of arguments in one of its name's variation.
        /// </summary>
        /// <param name="args">The collection of arguments to be checked.</param>
        /// <param name="booleanArgAttribute">The boolean argument to be checked.</param>
        /// <returns>A value indicating whether a boolean argument exists within a collection of arguments in one of its name's variation.</returns>
        private static bool BooleanArgumentExists(IEnumerable<string> args, BooleanArgAttribute booleanArgAttribute)
        {
            return args.Any(x => booleanArgAttribute.PossibleArgNames
                .Any(y => x.Equals(y, StringComparison.InvariantCultureIgnoreCase)));
        }
    }
}