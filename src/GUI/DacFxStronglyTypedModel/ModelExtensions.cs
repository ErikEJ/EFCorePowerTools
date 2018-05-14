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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Dac.Model;

namespace Microsoft.SqlServer.Dac.Extensions.Prototype
{   
    public partial class TSqlTable
    {
        public IEnumerable<ISqlIndex> Indexes
        {
            get 
            {
                foreach (var element in Element.GetReferencing(Index.IndexedObject))
                {
                    yield return (ISqlIndex)TSqlModelElement.AdaptInstance(element);
                }
            }
        }

        public IEnumerable<TSqlForeignKeyConstraint> ForeignKeyConstraints
        {
            get
            {
                foreach (var element in Element.GetReferencing(ForeignKeyConstraint.Host))
                {
                    yield return (TSqlForeignKeyConstraint)TSqlModelElement.AdaptInstance(element);
                }
            }
        }

        public IEnumerable<TSqlPrimaryKeyConstraint> PrimaryKeyConstraints
        {
            get 
            {
                foreach (var element in Element.GetReferencing(PrimaryKeyConstraint.Host))
                {
                    yield return (TSqlPrimaryKeyConstraint)TSqlModelElement.AdaptInstance(element);
                }
            }
        }

        public IEnumerable<TSqlDefaultConstraint> DefaultConstraints
        {
            get
            {
                foreach (var element in Element.GetReferencing(DefaultConstraint.Host))
                {
                    yield return (TSqlDefaultConstraint)TSqlModelElement.AdaptInstance(element);
                }
            }
        }

        public IEnumerable<TSqlCheckConstraint> CheckConstraints
        {
            get
            {
                foreach (var element in Element.GetReferencing(CheckConstraint.Host))
                {
                    yield return (TSqlCheckConstraint)TSqlModelElement.AdaptInstance(element);
                }
            }
        }

        public IEnumerable<TSqlUniqueConstraint> UniqueConstraints
        {
            get
            {
                foreach (var element in Element.GetReferencing(UniqueConstraint.Host))
                {
                    yield return (TSqlUniqueConstraint)TSqlModelElement.AdaptInstance(element);
                }
            }
        }

        /// <summary>
        /// Returns all constraints for the table
        /// </summary>
        public IEnumerable<ISqlModelElement> AllConstraints
        {
            get
            {
                foreach(var constraint in ForeignKeyConstraints)
                {
                    yield return constraint;
                }
                foreach (var constraint in PrimaryKeyConstraints)
                {
                    yield return constraint;
                }
                foreach(var constraint in UniqueConstraints)
                {
                    yield return constraint;
                }
                foreach (var constraint in CheckConstraints)
                {
                    yield return constraint;
                }
                foreach (var constraint in DefaultConstraints)
                {
                    yield return constraint;
                }
            }
        }
        public IEnumerable<TSqlDmlTrigger> Triggers
        {
            get
            {
                foreach(var element in Element.GetReferencing(DmlTrigger.TriggerObject))
                {
                    yield return (TSqlDmlTrigger)TSqlModelElement.AdaptInstance(element);
                }
            }
        }

    }
    public partial class TSqlFileTable
    {
        public IEnumerable<ISqlIndex> Indexes
        {
            get
            {
                foreach (var element in Element.GetReferencing(Index.IndexedObject))
                {
                    yield return (ISqlIndex)TSqlModelElement.AdaptInstance(element);
                }
            }
        }

        public IEnumerable<TSqlDmlTrigger> Triggers
        {
            get
            {
                foreach (var element in Element.GetReferencing(DmlTrigger.TriggerObject))
                {
                    yield return (TSqlDmlTrigger)TSqlModelElement.AdaptInstance(element);
                }
            }

        }
    }

    public partial class TSqlTableValuedFunction
    {
        public IEnumerable<ISqlIndex> Indexes
        {
            get
            {
                foreach (var element in Element.GetReferencing(Index.IndexedObject))
                {
                    yield return (ISqlIndex)TSqlModelElement.AdaptInstance(element);
                }
            }
        }
    }

    public partial class TSqlView
    {
        public IEnumerable<ISqlIndex> Indexes
        {
            get
            {
                foreach (var element in Element.GetReferencing(Index.IndexedObject))
                {
                    yield return (ISqlIndex)TSqlModelElement.AdaptInstance(element);
                }
            }
        }

        public IEnumerable<TSqlDmlTrigger> Triggers
        {
            get
            {
                foreach (var element in Element.GetReferencing(DmlTrigger.TriggerObject))
                {
                    yield return (TSqlDmlTrigger)TSqlModelElement.AdaptInstance(element);
                }
            }

        }
    }

    public partial class TSqlTableType
    {
        public IEnumerable<TSqlTableTypePrimaryKeyConstraint> PrimaryKeyConstraints
        {
            get
            {

                foreach (var element in Constraints.OfType<TSqlTableTypePrimaryKeyConstraint>())
                {
                    yield return element;
                }
            }
        }

        public IEnumerable<TSqlTableTypeDefaultConstraint> DefaultConstraints
        {
            get
            {
                foreach (var element in Constraints.OfType<TSqlTableTypeDefaultConstraint>())
                {
                    yield return element;
                }
            }
        }

        public IEnumerable<TSqlTableTypeCheckConstraint> CheckConstraints
        {
            get
            {
                foreach (var element in Constraints.OfType<TSqlTableTypeCheckConstraint>())
                {
                    yield return element;
                }
            }
        }

        public IEnumerable<TSqlTableTypeUniqueConstraint> UniqueConstraints
        {
            get
            {
                foreach (var element in this.Constraints.OfType<TSqlTableTypeUniqueConstraint>())
                {
                    yield return element;
                }
            }
        }
    }
}
