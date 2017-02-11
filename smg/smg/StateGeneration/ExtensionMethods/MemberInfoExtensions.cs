using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;

namespace smg.StateGeneration.ExtensionMethods
{
    /// <summary>
    /// Provides extension methods to <see cref="MemberInfo"/>.
    /// </summary>
    static class MemberInfoExtensions
    {
        /// <summary>
        /// Resolves a non-reflection-only version attributes of a reflection-only <see cref="MemberInfo"/>.
        /// </summary>
        /// <typeparam name="T">The <see cref="Type"/> of the attributes to be resolved.</typeparam>
        /// <param name="memberInfo">The <see cref="MemberInfo"/> of which the attributes will be resolved.</param>
        /// <returns>A non-reflection-only version attributes of a reflection-only <see cref="MemberInfo"/>.</returns>
        public static IEnumerable<T> GetCustomAttributesReflectionOnly<T>(this MemberInfo memberInfo)
        {
            return memberInfo.GetCustomAttributesData()
                .Where(customAttributeData => customAttributeData.AttributeType.GUID == typeof(T).GUID)
                .Select(customAttributeData => (T)Activator.CreateInstance(typeof(T), customAttributeData.ConstructorArguments
                    .Select(argument => argument.GetActualArgument())
                    .ToArray()));
        }

        /// <summary>
        /// Resolves a non-reflection-only version attribute of a reflection-only <see cref="MemberInfo"/>.
        /// </summary>
        /// <typeparam name="T">The <see cref="Type"/> of the attribute to be resolved.</typeparam>
        /// <param name="memberInfo">The <see cref="MemberInfo"/> of which the attribute will be resolved.</param>
        /// <returns>A non-reflection-only version attribute of a reflection-only <see cref="MemberInfo"/>.</returns>
        public static T GetCustomAttributeReflectionOnly<T>(this MemberInfo memberInfo)
        {
            IEnumerable<T> attributes = memberInfo.GetCustomAttributesReflectionOnly<T>();
            if (attributes.Skip(1).Any())
            {
                throw new AmbiguousMatchException($"More than one attribute of the requested type ('{typeof(T).FullName}') was found.");
            }
            return attributes.FirstOrDefault();
        }
    }
}