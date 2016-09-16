using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using smg.StateDescription.Attributes;
using smg.StateDescription.LogicalRelations;
using smg.StateGeneration.Exceptions;

namespace smg.StateGeneration
{
    /// <summary>
    /// Represents a method which decorates a method of a specific <see cref="Type"/> which is in a specific state.
    /// </summary>
    class DecoratorMethod
    {
        #region Constructors

        /// <summary>
        /// Creates a new isntance of <see cref="DecoratorMethod"/>.
        /// </summary>
        /// <param name="methodInfo">The method being decorated.</param>
        /// <param name="type">The stateful <see cref="Type"/> whos method is being decorated.</param>
        /// <param name="groupToStates">Contains the available states of the provided <see cref="Type"/>.</param>
        /// <param name="permutation">A permutation of states of the <see cref="Type"/> whos method is being decorated.</param>
        private DecoratorMethod(MethodInfo methodInfo, Type type, Dictionary<string, string[]> groupToStates, string[] permutation)
        {
            mMethodInfo = methodInfo;
            mType = type;
            mGroupsToStates = groupToStates;
            mPermutation = permutation;
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Initializes a new instance of <see cref="DecoratorMethod"/>.
        /// </summary>
        /// <param name="methodInfo">The method being decorated.</param>
        /// <param name="type">The stateful <see cref="Type"/> whos method is being decorated.</param>
        /// <param name="groupToStates">Contains the available states of the provided <see cref="Type"/>.</param>
        /// <param name="permutation">A permutation of states of the <see cref="Type"/> whos method is being decorated.</param>
        /// <returns>An initialized instance of <see cref="DecoratorMethod"/>.</returns>
        public static DecoratorMethod CreateDecorator(MethodInfo methodInfo, Type type, Dictionary<string, string[]> groupToStates, string[] permutation)
        {
            return new DecoratorMethod(methodInfo, type, groupToStates, permutation);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Provides a <see cref="CodeMemberMethod"/> which represents the decorated method which this <see cref="DecoratorMethod"/> represents.
        /// </summary>
        /// <returns>A <see cref="CodeMemberMethod"/> which represents the decorated method which this <see cref="DecoratorMethod"/> represents.</returns>
        public CodeMemberMethod GetMemberMethod()
        {
            CodeMemberMethod method = new CodeMemberMethod
            {
                Name = mMethodInfo.Name
            };

            method.Parameters.AddRange(mMethodInfo.GetParameters()
                .Select(x => new CodeParameterDeclarationExpression(x.ParameterType, x.Name))
                .ToArray());

            method.TypeParameters.AddRange(mMethodInfo.GetGenericArguments()
                .Select(x => new CodeTypeParameter(x.Name))
                .ToArray());

            method.ReturnType = GetMethodReturnType();

            GenerateMethodBody(method);

            return method;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Generates a method body matching to this instance of <see cref="DecoratorMethod"/> in a given <see cref="CodeMemberMethod"/>.
        /// </summary>
        /// <param name="method">The method for which the code will be generated.</param>
        private void GenerateMethodBody(CodeMemberMethod method)
        {
            CodeFieldReferenceExpression fieldReference = new CodeFieldReferenceExpression(new CodeThisReferenceExpression(),
                StateWrapper.WRAPPED_FIELD_NAME);
            CodeMethodReferenceExpression methodReference = new CodeMethodReferenceExpression(fieldReference, method.Name);
            foreach (CodeTypeParameter codeTypeParameter in method.TypeParameters)
            {
                methodReference.TypeArguments.Add(new CodeTypeReference(codeTypeParameter));
            }

            List<CodeExpression> parameters = new List<CodeExpression>();
            foreach (CodeParameterDeclarationExpression codeParameterDeclarationExpression in method.Parameters)
            {
                parameters.Add(codeParameterDeclarationExpression);
            }
            CodeMethodInvokeExpression methodInvoke = new CodeMethodInvokeExpression(methodReference,
                parameters.ToArray());

            if (mMethodInfo.ReturnType == typeof(void))
            {
                method.Statements.Add(methodInvoke);
                CodeObjectCreateExpression constructorCall = new CodeObjectCreateExpression(method.ReturnType, fieldReference);
                method.Statements.Add(new CodeMethodReturnStatement(constructorCall));
            }
            else
            {
                method.Statements.Add(new CodeMethodReturnStatement(methodInvoke));
            }
        }

        /// <summary>
        /// Provides the return type that the decorator method should have.
        /// </summary>
        /// <returns>The return type that the decorator method should have.</returns>
        private CodeTypeReference GetMethodReturnType()
        {
            if (mMethodInfo.ReturnType != typeof(void))
            {
                return new CodeTypeReference(mMethodInfo.ReturnType);
            }

            IEnumerable<string> statesChanges = mMethodInfo.GetCustomAttributes<ChangeToStateAttribute>()
                .Where(x => RelationHelpers.IsPermutationValid(mPermutation, x.ConditionRelation, x.ConditionStates, mGroupsToStates))
                .Select(x => x.TargetState);

            IEnumerable<string> doesntExists = statesChanges.Where(state => mGroupsToStates.Values.All(group => !group.Contains(state)));

            if (doesntExists.Any())
            {
                throw new InvalidStateRepresentationException($"A method calld '{mMethodInfo.Name}' decoraded by a {nameof(ChangeToStateAttribute)}/s that contain a state to change to which does not exists. States: {string.Join(", ", doesntExists)}");
            }

            var statesFromSameGroup = statesChanges.GroupBy(x => mGroupsToStates.Single(y => y.Value.Contains(x)).Key)
                .Where(x => x.Count() > 1);

            if (statesFromSameGroup.Any())
            {
                throw new InvalidStateRepresentationException($"A method calld '{mMethodInfo.Name}' decoraded by a {nameof(ChangeToStateAttribute)}s that should change to different states from the samge group. States: {string.Join(" And ", statesFromSameGroup.Select(x => $"[{string.Join(", ", x)}]"))}");
            }

            string[] newState = mPermutation.Select(x => ReplaceStateIfNeeded(x, statesChanges)).ToArray();

            string typeName = string.Join(string.Empty, newState) + StateWrapper.NAME_POSTFIX;

            if (mType.IsGenericType)
            {
                return new CodeTypeReference(typeName, mType.GetGenericArguments().Select(x => new CodeTypeReference(x)).ToArray());
            }

            return new CodeTypeReference(typeName);
        }

        /// <summary>
        /// Returns the state from the state changes which should replace the given state if they are from the same group. If no such state was found, returns the original state.
        /// </summary>
        /// <param name="originalState">A state to be changed by the given state changes.</param>
        /// <param name="statesChanges">A collection of states that can possibly replace the given state.</param>
        /// <returns>The state from the state changes which should replace the given state if they are from the same group. If no such state was found, returns the original state.</returns>
        private string ReplaceStateIfNeeded(string originalState, IEnumerable<string> statesChanges)
        {
            string stateToReplace = statesChanges.SingleOrDefault(state => mGroupsToStates.Values
                .Single(group => group.Contains(state))
                .Contains(originalState));

            if (stateToReplace == null)
            {
                return originalState;
            }

            return stateToReplace;
        }
        #endregion

        #region Private Fields

        /// <summary>
        /// The <see cref="Type"/> whos method is being decorated.
        /// </summary>
        private readonly Type mType;

        /// <summary>
        /// Contains the available states of the <see cref="Type"/> whos method is being decorated.
        /// </summary>
        private readonly Dictionary<string, string[]> mGroupsToStates;

        /// <summary>
        /// The method being decorated by this instance of <see cref="DecoratorMethod"/>,
        /// </summary>
        private readonly MethodInfo mMethodInfo;

        /// <summary>
        /// A 
        /// </summary>
        private readonly string[] mPermutation;

        #endregion
    }
}