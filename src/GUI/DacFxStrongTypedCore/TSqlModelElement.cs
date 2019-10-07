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
using Microsoft.SqlServer.Dac.Model;
using Microsoft.SqlServer.TransactSql.ScriptDom;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Microsoft.SqlServer.Dac.Extensions.Prototype
{
    public abstract partial class TSqlModelElement : ISqlModelElement
    {
        protected TSqlModelElement()
        {
        }

        public TSqlModelElement(TSqlObject obj, ModelTypeClass typeClass)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("obj");
            }
            else if (obj.ObjectType != typeClass)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                    ModelMessages.InvalidObjectType, obj.ObjectType.Name, typeClass.Name),
                    "typeClass");
            }
            Element = obj; 
        }



        public virtual ObjectIdentifier Name
        {
            get 
            {
                return Element.Name;
            }

        }

        public virtual TSqlObject Element
        {
            get;
            protected set;
        }

        public virtual ModelTypeClass ObjectType { get { return Element.ObjectType; } }

        public object this[ModelPropertyClass property] { get { return Element[property]; } }

        public override bool Equals(object obj)
        {
            return Element.Equals(obj);
        }
        
        public TSqlScript GetAst()
        {
            return Element.GetAst();
        }

        public IEnumerable<TSqlObject> GetChildren()
        {
            return Element.GetChildren();
        }
        public IEnumerable<TSqlObject> GetChildren(DacQueryScopes queryScopes)
        {
            return Element.GetChildren(queryScopes);
        }
        public override int GetHashCode()
        {
            return Element.GetHashCode();
        }

        public T GetMetadata<T>(ModelMetadataClass metadata)
        {
            return Element.GetMetadata<T>(metadata);
        }
        public object GetMetadata(ModelMetadataClass metadata)
        {
            return Element.GetMetadata(metadata);
        }
        public TSqlObject GetParent()
        {
            return Element.GetParent();
        }
        public TSqlObject GetParent(DacQueryScopes queryScopes)
        {
            return Element.GetParent(queryScopes);
        }
        public T GetProperty<T>(ModelPropertyClass property)
        {
            return Element.GetProperty<T>(property);
        }
        public object GetProperty(ModelPropertyClass property)
        {
            return Element.GetProperty(property);
        }
        public IEnumerable<TSqlObject> GetReferenced()
        {
            return Element.GetReferenced();
        }
        public IEnumerable<TSqlObject> GetReferenced(DacQueryScopes queryScopes)
        {
            return Element.GetReferenced(queryScopes);
        }
        public IEnumerable<TSqlObject> GetReferenced(ModelRelationshipClass relationshipType)
        {
            return Element.GetReferenced(relationshipType);
        }

        public IEnumerable<TSqlObject> GetReferenced(ModelRelationshipClass relationshipType, DacQueryScopes queryScopes)
        {
            return Element.GetReferenced(relationshipType, queryScopes);

        }
        public IEnumerable<ModelRelationshipInstance> GetReferencedRelationshipInstances()
        {
            return Element.GetReferencedRelationshipInstances();
        }
        public IEnumerable<ModelRelationshipInstance> GetReferencedRelationshipInstances(DacExternalQueryScopes queryScopes)
        {
            return Element.GetReferencedRelationshipInstances(queryScopes);
        }
        public IEnumerable<ModelRelationshipInstance> GetReferencedRelationshipInstances(DacQueryScopes queryScopes)
        {
            return Element.GetReferencedRelationshipInstances(queryScopes);

        }
        public IEnumerable<ModelRelationshipInstance> GetReferencedRelationshipInstances(ModelRelationshipClass relationshipType)
        {
            return Element.GetReferencedRelationshipInstances(relationshipType);
        }
        public IEnumerable<ModelRelationshipInstance> GetReferencedRelationshipInstances(ModelRelationshipClass relationshipType, DacExternalQueryScopes queryScopes)
        {
            return Element.GetReferencedRelationshipInstances(relationshipType, queryScopes);
        }
        public IEnumerable<ModelRelationshipInstance> GetReferencedRelationshipInstances(ModelRelationshipClass relationshipType, DacQueryScopes queryScopes)
        {
            return Element.GetReferencedRelationshipInstances(relationshipType, queryScopes);
        }
        public IEnumerable<TSqlObject> GetReferencing()
        {
            return Element.GetReferencing();
        }
        public IEnumerable<TSqlObject> GetReferencing(DacQueryScopes queryScopes)
        {
            return Element.GetReferencing(queryScopes);
        }
        public IEnumerable<TSqlObject> GetReferencing(ModelRelationshipClass relationshipType)
        {
            return Element.GetReferencing(relationshipType);
        }
        public IEnumerable<TSqlObject> GetReferencing(ModelRelationshipClass relationshipType, DacQueryScopes queryScopes)
        {
            return Element.GetReferencing(relationshipType, queryScopes);
        }
        public IEnumerable<ModelRelationshipInstance> GetReferencingRelationshipInstances()
        {
            return Element.GetReferencingRelationshipInstances();
        }
        public IEnumerable<ModelRelationshipInstance> GetReferencingRelationshipInstances(DacQueryScopes queryScopes)
        {
            return Element.GetReferencingRelationshipInstances(queryScopes);
        }
        public IEnumerable<ModelRelationshipInstance> GetReferencingRelationshipInstances(ModelRelationshipClass relationshipType)
        {
            return Element.GetReferencingRelationshipInstances(relationshipType);
        }
        public IEnumerable<ModelRelationshipInstance> GetReferencingRelationshipInstances(ModelRelationshipClass relationshipType, DacQueryScopes queryScopes)
        {
            return Element.GetReferencingRelationshipInstances(relationshipType, queryScopes);
        }
        public string GetScript()
        {
            return Element.GetScript();
        }
        public SourceInformation GetSourceInformation()
        {
            return Element.GetSourceInformation();
        }
        public bool TryGetAst(out TSqlScript objectAst)
        {
            return Element.TryGetAst(out objectAst);
        }
        public bool TryGetScript(out string objectScript)
        {
            return Element.TryGetScript(out objectScript);
        }
    }
}
