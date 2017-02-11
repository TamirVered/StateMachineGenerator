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
    }
}