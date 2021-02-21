namespace Thrift.Net.Compilation.Types
{
    using System;
    using Thrift.Net.Compilation.Symbols;

    /// <summary>
    /// Used to resolve types.
    /// </summary>
    public class TypeResolver
    {
        // TODO: Should this return an ITypeResolutionResult instead?
        public ITypeReference Resolve(string typeName, ISymbol parent)
        {
            if (BaseType.TryResolve(typeName, out var baseType))
            {
                return new TypeReference(baseType);
            }

            var userTypeDefinition = parent.ResolveType(typeName);
            if (userTypeDefinition != null)
            {
                return new TypeReference(new UserType(userTypeDefinition));
            }

            return TypeReference.Unresolved;
        }
    }
}