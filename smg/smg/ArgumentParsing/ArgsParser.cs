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
        /// Creates and initializes a type which describes the arguments using a collection of arguments.
        /// </summary>
        /// <typeparam name="T">The type which describes the arguments.</typeparam>
        /// <param name="args">The arguments to initialize the initialized type with.</param>
        /// <param name="argumentContainer">The object to be filled with parsed arguments.</param>
        /// <returns>A value indicating whether the argument loading has succeeded.</returns>
        public static bool TryParse<T>(IEnumerable<string> args, out T argumentContainer) where T : class, new()
        {
            try
            {
                argumentContainer = Parse<T>(args);
                return argumentContainer != null;
            }
            catch (Exception)
            {
                argumentContainer = null;
                return false;
            }
        }

        /// <summary>
        /// Creates and initializes a type which describes the arguments using a collection of arguments.
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
            IEnumerable<PropertyInfo> properties = typeof(T).GetProperties();
            ValidateProperties(properties);
            foreach (PropertyInfo propertyInfo in properties)
            {
                if (HandleBooleanArgument(args, propertyInfo, arguments))
                {
                    continue;
                }
                HandleIndexedArgument(args, propertyInfo, arguments);
            }
            return arguments;
        }

        /// <summary>
        /// Validates that a collection of given properties describe a valid argument set.
        /// </summary>
        /// <param name="properties">The collection of properties to be examined.</param>
        private static void ValidateProperties(IEnumerable<PropertyInfo> properties)
        {
            if (properties == null || !properties.Any())
            {
                return;
            }
            List<int> indices = new List<int>();
            foreach (PropertyInfo propertyInfo in properties)
            {
                IndexedArgAttribute indexedArgAttribute = propertyInfo.GetCustomAttribute<IndexedArgAttribute>();
                if (indexedArgAttribute != null)
                {
                    indices.Add(indexedArgAttribute.Index);
                }
            }
            if (indices.GroupBy(x => x).Any(x => x.Count() > 1))
            {
                throw new ArgumentParsingException($"More than one property of type {properties.First().ReflectedType} refer to the argument the same index.");
            }
            IEnumerable<int> orderedIndices = indices.OrderBy(x => x);
            //Indices : 3, 5, 2, 6, 1, 0
            //Ordered : 0, 1, 2, 3, 5, 6
            //First   : 0, 1, 2, 3, 5 (Didn't take last)
            //Second  : 1, 2, 3, 5, 6 (Skipped first)
            //Result  : 1, 1, 1, 2, 1 (2 is invalid)
            if (orderedIndices
                   .Take(indices.Count - 1)
                   .Zip(orderedIndices.Skip(1), (x, y) => y - x)
                   .Any(x => x > 1))
            {
                throw new ArgumentParsingException($"The indexed arguments described by the properties of type {properties.First().ReflectedType} are not consecutive.");
            }
        }

        /// <summary>
        /// Parses a boolean argument into a given property.
        /// </summary>
        /// <typeparam name="T">The type which describes the arguments.</typeparam>
        /// <param name="args">A collection of the arguments used to initialize <paramref name="arguments"/>.</param>
        /// <param name="propertyInfo">A property to be filled with a boolean argument specified in its attribute.</param>
        /// <param name="arguments">An object which contains the argument property.</param>
        /// <returns>A value indicating whether the given property is a boolean argument.</returns>
        private static bool HandleBooleanArgument<T>(IEnumerable<string> args, PropertyInfo propertyInfo, T arguments)
            where T : class, new()
        {
            BooleanArgAttribute booleanArgAttribute = propertyInfo.GetCustomAttribute<BooleanArgAttribute>();
            if (booleanArgAttribute == null)
            {
                return false;
            }
            if (propertyInfo.PropertyType != typeof(bool))
            {
                throw new BooleanArgumentPropertyIsNotBoolException(
                    $"The property {propertyInfo.Name} of the type {typeof(T).FullName} which represents a boolean argument is not of type bool.");
            }
            propertyInfo.SetValue(arguments, BooleanArgumentExists(args, booleanArgAttribute));
            return true;
        }

        /// <summary>
        /// Parses an indexed argument into a given property.
        /// </summary>
        /// <typeparam name="T">The type which describes the arguments.</typeparam>
        /// <param name="args">A collection of the arguments used to initialize <paramref name="arguments"/>.</param>
        /// <param name="propertyInfo">A property to be filled with an indexed argument specified in its attribute.</param>
        /// <param name="arguments">An object which contains the argument property.</param>
        /// <returns>A value indicating whether the given property is an indexed argument.</returns>
        private static bool HandleIndexedArgument<T>(IEnumerable<string> args, PropertyInfo propertyInfo, T arguments)
            where T : class, new()
        {
            IndexedArgAttribute indexedArgAttribute = propertyInfo.GetCustomAttribute<IndexedArgAttribute>();
            if (indexedArgAttribute == null)
            {
                return false;
            }
            string specifiedArgument = args.ElementAtOrDefault(indexedArgAttribute.Index);
            if (specifiedArgument == null)
            {
                throw new ArgumentParsingException(
                    $"A property named {propertyInfo.Name} represents the argument at index {indexedArgAttribute.Index} which does not exist.");
            }
            object specifiedArgumentAsPropertyType;
            try
            {
                specifiedArgumentAsPropertyType = Convert.ChangeType(specifiedArgument, propertyInfo.PropertyType);
            }
            catch (Exception exception)
            {
                throw new ArgumentParsingException(
                    $"A property which represents the argument at index {indexedArgAttribute.Index} is of type {propertyInfo.PropertyType}, which could not be assigned from the argument value \"{specifiedArgument}\"."
                    , exception);
            }
            propertyInfo.SetValue(arguments, specifiedArgumentAsPropertyType);
            return true;
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