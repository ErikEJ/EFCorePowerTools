//------------------------------------------------------------------------------
//<copyright company="Microsoft">
//
//    The MIT License (MIT)
//    
//    Copyright (c) 2015 Microsoft
//    
//    Permission is hereby granted, free of charge, to any person obtaining a copy
//    of this software and associated documentation files (the "Software"), to deal
//    in the Software without restriction, including without limitation the rights
//    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//    copies of the Software, and to permit persons to whom the Software is
//    furnished to do so, subject to the following conditions:
//    
//    The above copyright notice and this permission notice shall be included in all
//    copies or substantial portions of the Software.
//    
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//    SOFTWARE.
//</copyright>
//------------------------------------------------------------------------------
namespace Microsoft.SqlServer.Dac.Extensions.Prototype
{
    using System;
    using Microsoft.SqlServer.TransactSql.ScriptDom;
    using Microsoft.SqlServer.Dac.Model;
    using System.Collections.Generic;
    public interface ISqlModelElement
    {
        ObjectIdentifier Name { get; }
        TSqlObject Element { get; }
        TSqlScript GetAst();
        IEnumerable<TSqlObject> GetChildren();
        IEnumerable<TSqlObject> GetChildren(DacQueryScopes queryScopes);
        object GetMetadata(ModelMetadataClass metadata);
        T GetMetadata<T>(ModelMetadataClass metadata);
        TSqlObject GetParent();
        TSqlObject GetParent(DacQueryScopes queryScopes);
        object GetProperty(ModelPropertyClass property);
        T GetProperty<T>(ModelPropertyClass property);
        IEnumerable<TSqlObject> GetReferenced();
        IEnumerable<TSqlObject> GetReferenced(DacQueryScopes queryScopes);
        IEnumerable<TSqlObject> GetReferenced(ModelRelationshipClass relationshipType);
        IEnumerable<TSqlObject> GetReferenced(ModelRelationshipClass relationshipType, DacQueryScopes queryScopes);
        IEnumerable<ModelRelationshipInstance> GetReferencedRelationshipInstances();
        IEnumerable<ModelRelationshipInstance> GetReferencedRelationshipInstances(DacExternalQueryScopes queryScopes);
        IEnumerable<ModelRelationshipInstance> GetReferencedRelationshipInstances(DacQueryScopes queryScopes);
        IEnumerable<ModelRelationshipInstance> GetReferencedRelationshipInstances(ModelRelationshipClass relationshipType);
        IEnumerable<ModelRelationshipInstance> GetReferencedRelationshipInstances(ModelRelationshipClass relationshipType, DacExternalQueryScopes queryScopes);
        IEnumerable<ModelRelationshipInstance> GetReferencedRelationshipInstances(ModelRelationshipClass relationshipType, DacQueryScopes queryScopes);
        IEnumerable<TSqlObject> GetReferencing();
        IEnumerable<TSqlObject> GetReferencing(DacQueryScopes queryScopes);
        IEnumerable<TSqlObject> GetReferencing(ModelRelationshipClass relationshipType);
        IEnumerable<TSqlObject> GetReferencing(ModelRelationshipClass relationshipType, DacQueryScopes queryScopes);
        IEnumerable<ModelRelationshipInstance> GetReferencingRelationshipInstances();
        IEnumerable<ModelRelationshipInstance> GetReferencingRelationshipInstances(DacQueryScopes queryScopes);
        IEnumerable<ModelRelationshipInstance> GetReferencingRelationshipInstances(ModelRelationshipClass relationshipType);
        IEnumerable<ModelRelationshipInstance> GetReferencingRelationshipInstances(ModelRelationshipClass relationshipType, DacQueryScopes queryScopes);
        string GetScript();
        SourceInformation GetSourceInformation();
        ModelTypeClass ObjectType { get; }
        object this[ModelPropertyClass property] { get; }
        bool TryGetAst(out TSqlScript objectAst);
        bool TryGetScript(out string objectScript);
    }
}
