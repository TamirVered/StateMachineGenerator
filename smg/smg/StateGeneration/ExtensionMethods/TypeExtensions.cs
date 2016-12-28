using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace smg.StateGeneration.ExtensionMethods
{
    /// <summary>
    /// Provides extension methods to <see cref="Type"/>.
    /// </summary>
    static class TypeExtensions
    {
        /// <summary>
        /// Provides a <see cref="CodeTypeParameter"/> which describes a given <see cref="Type"/>.
        /// </summary>
        /// <param name="type">The type to create <see cref="CodeTypeParameter"/> according to.</param>
        /// <returns>A <see cref="CodeTypeParameter"/> which describes a given <see cref="Type"/>.</returns>
        public static CodeTypeParameter GetCodeTypeParameter(this Type type)
        {
            CodeTypeParameter typeParameter = new CodeTypeParameter(type.Name);

            Type[] genericConstraints = type.GetGenericParameterConstraints();
            if (genericConstraints.Any())
            {
                typeParameter.Constraints.AddRange(genericConstraints
                    .Select(x => x.ContainsGenericParameters ? x.WithGenericTypeArguments() : new CodeTypeReference(x))
                    .ToArray());
            }

            typeParameter.HasConstructorConstraint = type.GenericParameterAttributes.HasFlag(GenericParameterAttributes.DefaultConstructorConstraint);

            return typeParameter;
        }

        /// <summary>
        /// Recursively resolves a reference to a type with its generic type arguments.
        /// </summary>
        /// <param name="type">The type whos reference will be resolved.</param>
        /// <returns>A resolved reference to a type with its generic type arguments.</returns>
        public static CodeTypeReference WithGenericTypeArguments(this Type type)
        {
            if (!type.ContainsGenericParameters)
            {
                return new CodeTypeReference(type);
            }

            List<CodeTypeReference> genericArguments = new List<CodeTypeReference>();
            foreach (Type genericTypeArgument in type.GenericTypeArguments)
            {
                genericArguments.Add(genericTypeArgument.WithGenericTypeArguments());
            }

            return new CodeTypeReference(type.Name, genericArguments.ToArray());
        }
    }
}
