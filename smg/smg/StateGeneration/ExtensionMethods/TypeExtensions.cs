using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            typeParameter.Constraints.AddRange(type.GetGenericParameterConstraints()
                .Select(x => new CodeTypeReference(x))
                .ToArray());

            typeParameter.HasConstructorConstraint = (type.GetConstructor(Type.EmptyTypes) != null);

            return typeParameter;
        }
    }
}
