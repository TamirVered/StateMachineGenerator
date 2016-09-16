using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.ComTypes;
using smg.ExtensionMethods;
using smg.StateDescription.Attributes;
using smg.StateDescription.LogicalRelations;
using smg.StateGeneration.Exceptions;
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
                IsClass = true,
                TypeAttributes = TypeAttributes.Public | TypeAttributes.Class
            };

            typeDeclaration.TypeParameters.AddRange(mStatefulType.GetGenericArguments()
                .Select(x => x.GetCodeTypeParameter())
                .ToArray());

            CodeTypeMemberCollection members = new CodeTypeMemberCollection
            {
                new CodeMemberField(mStatefulType, WRAPPED_FIELD_NAME) {Attributes = MemberAttributes.Family},
                GetStateConstructor()
            };

            foreach (CodeMemberMethod codeMemberMethod in mDecoratorMethods.Select(x => x.GetMemberMethod()))
            {
                members.Add(codeMemberMethod);
            }

            return typeDeclaration;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Gets a constructor to instantiate the state wrapper.
        /// </summary>
        /// <returns>A constructor to instantiate the state wrapper.</returns>
        private CodeConstructor GetStateConstructor()
        {
            CodeConstructor constructor = new CodeConstructor { Attributes = MemberAttributes.Public };

            CodeParameterDeclarationExpression parameter = new CodeParameterDeclarationExpression(new CodeTypeReference(mStatefulType), CONSTRUCTOR_ARGUMENT_NAME);
            CodeFieldReferenceExpression fieldReference = new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), WRAPPED_FIELD_NAME);
            constructor.Parameters.Add(parameter);
            constructor.Statements.Add(new CodeAssignStatement(fieldReference, parameter));

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
            AvailableForAllStatesAttribute availableForAll = method.GetCustomAttribute<AvailableForAllStatesAttribute>();
            AvailableForStatesAttribute availableForStates = method.GetCustomAttribute<AvailableForStatesAttribute>();

            if (availableForAll != null && availableForStates == null)
            {
                throw new InvalidStateRepresentationException($"A method cannot be decorated by both {nameof(AvailableForAllStatesAttribute)} and {nameof(AvailableForStatesAttribute)}.");
            }

            if (availableForAll != null)
            {
                return true;
            }

            if (availableForStates != null)
            {
                Type logicalRelationType = availableForStates.Relation;
                return RelationHelpers.IsPermutationValid(mPermutation, logicalRelationType, availableForStates.States, groupToStates);
            }

            return false;
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