using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using smg.Common.Exceptions;
using smg.Common.StateDescription.Attributes;
using smg.StateGeneration.ExtensionMethods;

namespace smg.StateGeneration
{
    /// <summary>
    /// Represents one state permutation of the wrapped <see cref="Type"/>.
    /// </summary>
    class StateWrapper
    {
        #region Constants

        /// <summary>
        /// A postfix which will be in the end of the name of the wrapper class represented by a <see cref="StateWrapper"/>.
        /// </summary>
        public const string NAME_POSTFIX = "State";

        /// <summary>
        /// The name of the field that contains the wrapped stateful object.
        /// </summary>
        public const string WRAPPED_FIELD_NAME = "mWrappedInstance";

        /// <summary>
        /// The name of the constructor argument that contains the wrapped stateful object.
        /// </summary>
        public const string CONSTRUCTOR_ARGUMENT_NAME = "statefulObject";

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new instance of <see cref="StateWrapper"/>.
        /// </summary>
        /// <param name="type">The stateful <see cref="Type"/> being wrapped.</param>
        /// <param name="groupToStates">Contains the available states of the provided <see cref="Type"/>.</param>
        /// <param name="permutation">A permutation of states of the type to be represented by the instantiated <see cref="StateWrapper"/>.</param>
        private StateWrapper(Type type, Dictionary<string, string[]> groupToStates, string[] permutation)
        {
            mPermutation = permutation;
            mStatefulType = type;
            foreach (MethodInfo methodInfo in type.GetMethods(BindingFlags.Public | BindingFlags.Instance).Where(x => IsRelevantMethod(x, groupToStates)))
            {
                mDecoratorMethods.Add(DecoratorMethod.CreateDecorator(methodInfo, type, groupToStates, permutation));
            }
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Provides a new instance of <see cref="StateWrapper"/> which represents the given state permutation.
        /// </summary>
        /// <param name="type">The stateful <see cref="Type"/> being wrapped.</param>
        /// <param name="groupToStates">Contains the available states of the provided <see cref="Type"/>.</param>
        /// <param name="permutation">A permutation of states of the type to be represented by the provided <see cref="StateWrapper"/>.</param>
        /// <returns>A new instance of <see cref="StateWrapper"/> which represents the given state permutation.</returns>
        public static StateWrapper CreateWrapper(Type type, Dictionary<string, string[]> groupToStates, IEnumerable<string> permutation)
        {
            StateWrapper wrapper = new StateWrapper(type, groupToStates, permutation.ToArray());
            return wrapper;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Provides a <see cref="CodeTypeDeclaration"/> which represents the wrapper class which this <see cref="StateWrapper"/> represents.
        /// </summary>
        /// <returns>A <see cref="CodeTypeDeclaration"/> which represents the wrapper class which this <see cref="StateWrapper"/> represents.</returns>
        public CodeTypeDeclaration GetTypeDeclaration()
        {
            string className = string.Join(string.Empty, mPermutation) + NAME_POSTFIX;

            CodeTypeDeclaration typeDeclaration = new CodeTypeDeclaration(className)
            {
                TypeAttributes = TypeAttributes.Public | TypeAttributes.Class
            };

            typeDeclaration.TypeParameters.AddRange(mStatefulType.GetGenericArguments()
                .Select(x => x.GetCodeTypeParameter())
                .ToArray());

            CodeTypeMemberCollection members = new CodeTypeMemberCollection
            {
                GetMemberField(typeDeclaration),
                GetStateConstructor()
            };

            AddMemberMethods(members);

            typeDeclaration.Members.AddRange(members);

            return typeDeclaration;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Gets a constructor to instantiate the state wrapper.
        /// </summary>
        /// <returns>A constructor to instantiate the state wrapper.</returns>
        /// <example>
        /// public TypeName(StatefulType statefulObject)
        /// {
        ///     this.mWrappedInstance = statefulObject;
        /// }
        /// </example>
        private CodeConstructor GetStateConstructor()
        {
            CodeRegionDirective constructorsRegionStart = new CodeRegionDirective(CodeRegionMode.Start, "Constructors");
            CodeRegionDirective constructorsRegionEnd = new CodeRegionDirective(CodeRegionMode.End, string.Empty);

            CodeConstructor constructor = new CodeConstructor
            {
                Attributes = MemberAttributes.Public,
                StartDirectives = { constructorsRegionStart },
                EndDirectives = { constructorsRegionEnd }
            };

            CodeParameterDeclarationExpression parameter = new CodeParameterDeclarationExpression(new CodeTypeReference(mStatefulType), CONSTRUCTOR_ARGUMENT_NAME);
            CodeVariableReferenceExpression parameterReference = new CodeVariableReferenceExpression(parameter.Name);
            CodeFieldReferenceExpression fieldReference = new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), WRAPPED_FIELD_NAME);
            constructor.Parameters.Add(parameter);
            constructor.Statements.Add(new CodeAssignStatement(fieldReference, parameterReference));

            return constructor;
        }

        /// <summary>
        /// Determines whether a method is relevant for the state permutation represented by this instance of <see cref="StateWrapper"/>.
        /// </summary>
        /// <param name="method">The method whose relevancy is being checked.</param>
        /// <param name="groupToStates">The state groups for which the permutation will be validated.</param>
        /// <returns>A value indicating whether a method is relevant for the state permutation represented by this instance of <see cref="StateWrapper"/>.</returns>
        private bool IsRelevantMethod(MethodInfo method, Dictionary<string, string[]> groupToStates)
        {
            AvailableForAllStatesAttribute availableForAll = method.GetCustomAttributeReflectionOnly<AvailableForAllStatesAttribute>();
            IEnumerable<AvailableForStatesAttribute> availableForStates = method.GetCustomAttributesReflectionOnly<AvailableForStatesAttribute>();

            if (availableForAll != null && availableForStates.Any())
            {
                throw new InvalidStateRepresentationException($"A method cannot be decorated by both {nameof(AvailableForAllStatesAttribute)} and {nameof(AvailableForStatesAttribute)}.");
            }

            if (availableForAll != null)
            {
                return true;
            }

            if (availableForStates.Any())
            {
                return availableForStates.Any(x => RelationHelpers.IsPermutationValid(mPermutation, x.Relation, x.States, groupToStates));
            }

            return false;
        }

        /// <summary>
        /// Provides the fields region of the generated wrapper class represented by this instance of <see cref="StateWrapper"/>.
        /// </summary>
        /// <param name="typeDeclaration">The<see cref="CodeTypeDeclaration"/> which represents the wrapper class which this <see cref="StateWrapper"/> represents.</param>
        /// <returns>The fields region of the generated wrapper class represented by this instance of <see cref="StateWrapper"/>.</returns>
        private CodeMemberField GetMemberField(CodeTypeDeclaration typeDeclaration)
        {
            List<CodeTypeReference> typeParameters = new List<CodeTypeReference>();
            foreach (CodeTypeParameter codeTypeParameter in typeDeclaration.TypeParameters)
            {
                typeParameters.Add(new CodeTypeReference(codeTypeParameter));
            }

            CodeTypeReference fieldTypeReference = new CodeTypeReference(mStatefulType.Name, typeParameters.ToArray());
            CodeRegionDirective privateFieldsRegionStart = new CodeRegionDirective(CodeRegionMode.Start, "Private Fields");
            CodeRegionDirective privateFieldsRegionEnd = new CodeRegionDirective(CodeRegionMode.End, string.Empty);

            return new CodeMemberField(fieldTypeReference, WRAPPED_FIELD_NAME)
            {
                Attributes = MemberAttributes.Family,
                StartDirectives = { privateFieldsRegionStart },
                EndDirectives = { privateFieldsRegionEnd }
            };
        }

        /// <summary>
        /// Adds the methods region of the generated wrapper class represented by this instance of <see cref="StateWrapper"/> to the given <see cref="CodeTypeMemberCollection"/>.
        /// </summary>
        /// <param name="members">The <see cref="CodeTypeMemberCollection"/> which contains the members of the <see cref="CodeTypeDeclaration"/> which represents the wrapper class which this <see cref="StateWrapper"/> represents.</param>
        private void AddMemberMethods(CodeTypeMemberCollection members)
        {
            CodeRegionDirective publicMethodsRegionStart = new CodeRegionDirective(CodeRegionMode.Start, "Public Methods");
            CodeRegionDirective publicMethodsRegionEnd = new CodeRegionDirective(CodeRegionMode.End, string.Empty);

            int methodIndex = 0;
            foreach (CodeMemberMethod codeMemberMethod in mDecoratorMethods.Select(x => x.GetMemberMethod()))
            {
                if (methodIndex == 0)
                {
                    codeMemberMethod.StartDirectives.Add(publicMethodsRegionStart);
                }
                members.Add(codeMemberMethod);

                methodIndex++;
                if (methodIndex == mDecoratorMethods.Count)
                {
                    codeMemberMethod.EndDirectives.Add(publicMethodsRegionEnd);
                }
            }
        }

        #endregion

        #region Private Fields

        /// <summary>
        /// Constains the state permutation which is represented by this instance of <see cref="StateWrapper"/>.
        /// </summary>
        private readonly string[] mPermutation;

        /// <summary>
        /// Contains the methods decorated by the wrapper class represented by this instance of <see cref="StateWrapper"/>.
        /// </summary>
        private readonly List<DecoratorMethod> mDecoratorMethods = new List<DecoratorMethod>();

        /// <summary>
        /// The stateful <see cref="Type"/> being wrapped by the class represented by this instance of <see cref="StateWrapper"/>.
        /// </summary>
        private Type mStatefulType;

        #endregion
    }
}