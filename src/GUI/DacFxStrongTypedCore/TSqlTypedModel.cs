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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.SqlServer.Dac.Extensions.Prototype
{
    public sealed class TSqlTypedModel: IDisposable
    {
        TSqlModel model;

        public TSqlTypedModel(TSqlModel model)
        {
            this.model = model;
        }

        public TSqlTypedModel(string filename)
        {
            model = new TSqlModel(filename);
        }

        public TSqlTypedModel(SqlServerVersion modelTargetVersion, TSqlModelOptions modelCreationOptions)
        {
            model = new TSqlModel(modelTargetVersion, modelCreationOptions);
        }

        public TSqlTypedModel(string filename, DacSchemaModelStorageType modelStorageType)
        {
            model = new TSqlModel(filename, modelStorageType);
        }
        
        public IEnumerable<T> GetObjects<T>(DacQueryScopes queryScope) where T : ISqlModelElement
        {
            // Map the System.Type to a set of ModelType classes that extend the type or implement the interface
            foreach( ModelTypeClass modelType in UtilityMethods.GetModelElementTypes(typeof(T)))
            {
                foreach(var element in model.GetObjects(queryScope, modelType))
                {
                    // Adapt instance with strongly-typed wrapper.
                    yield return (T)TSqlModelElement.AdaptInstance(element);
                }
            }           
        }

        public IEnumerable<T> GetObjects<T>(ObjectIdentifier id, DacQueryScopes queryScope) where T : ISqlModelElement
        {
            // Map the System.Type to a set of ModelType classes that extend the type or implement the interface
            foreach (ModelTypeClass modelType in UtilityMethods.GetModelElementTypes(typeof(T)))
            {
                foreach (var element in model.GetObjects(modelType,id, queryScope))
                {
                    // Adapt instance with strongly-typed wrapper.
                    yield return (T)TSqlModelElement.AdaptInstance(element);
                }
            }
        }

        public T GetObject<T>(ObjectIdentifier id, DacQueryScopes queryScope) where T : ISqlModelElement
        {
            return GetObjects<T>(id, queryScope).FirstOrDefault();
        }

        // Summary:
        //     Adds objects to the model based on the contents of a TSql Script string.
        //     The script should consist of valid TSql DDL statements.  Objects added using
        //     this method cannot be updated or deleted at a later point as update/delete
        //     requires a script name to be specified when adding the objects. If this is
        //     a requirement use the Microsoft.SqlServer.Dac.Model.TSqlModel.AddOrUpdateObjects(System.String,System.String,Microsoft.SqlServer.Dac.Model.TSqlObjectOptions)
        //     method instead.
        //
        // Parameters:
        //   inputScript:
        //     Script containing TSql DDL statements
        //
        // Exceptions:
        //   System.ArgumentNullException:
        //     If the supplied inputScript is null.
        //
        //   System.Runtime.Remoting.RemotingException:
        //     If communication with the Microsoft.SqlServer.Dac.Model.TSqlObjectService
        //     fails.
        public void AddObjects(string inputScript)
        {
            model.AddObjects(inputScript);
        }
        //
        // Summary:
        //     Adds objects to the model based on the contents of a Microsoft.SqlServer.TransactSql.ScriptDom.TSqlScript
        //     object. The script should be valid TSql with no parse errors. Objects added
        //     using this method cannot be updated or deleted at a later point as update/delete
        //     requires a script name to be specified when adding the objects. If this is
        //     a requirement use the Microsoft.SqlServer.Dac.Model.TSqlModel.AddOrUpdateObjects(Microsoft.SqlServer.TransactSql.ScriptDom.TSqlScript,System.String,Microsoft.SqlServer.Dac.Model.TSqlObjectOptions)
        //     method instead.
        //
        // Parameters:
        //   inputScript:
        //     The Microsoft.SqlServer.TransactSql.ScriptDom.TSqlScript to add
        //
        // Exceptions:
        //   System.ArgumentNullException:
        //     If the supplied inputScript is null.
        //
        //   System.Runtime.Remoting.RemotingException:
        //     If communication with the Microsoft.SqlServer.Dac.Model.TSqlObjectService
        //     fails.
        public void AddObjects(TSqlScript inputScript)
        {
            model.AddObjects(inputScript);
        }
        //
        // Summary:
        //     Add Objects to the model based on the contents of a TSql Script string, plus
        //     additional metadata defined by a Microsoft.SqlServer.Dac.Model.TSqlObjectOptions
        //     object The script should consist of valid TSql DDL statements. Objects added
        //     using this method cannot be updated or deleted at a later point as update/delete
        //     requires a script name to be specified when adding the objects. If this is
        //     a requirement use the Microsoft.SqlServer.Dac.Model.TSqlModel.AddOrUpdateObjects(System.String,System.String,Microsoft.SqlServer.Dac.Model.TSqlObjectOptions)
        //     method instead.
        //
        // Parameters:
        //   inputScript:
        //     Script containing TSql DDL statements
        //
        //   options:
        //     Options defining how to interpret the script
        //
        // Exceptions:
        //   System.ArgumentNullException:
        //     If the supplied inputScript is null.
        //
        //   System.Runtime.Remoting.RemotingException:
        //     If communication with the Microsoft.SqlServer.Dac.Model.TSqlObjectService
        //     fails.
        public void AddObjects(string inputScript, TSqlObjectOptions options)
        {
            model.AddObjects(inputScript, options);
        }

        //
        // Summary:
        //     Add Objects to the model based on the contents of a Microsoft.SqlServer.TransactSql.ScriptDom.TSqlScript
        //     object, plus additional metadata defined by a Microsoft.SqlServer.Dac.Model.TSqlObjectOptions
        //     object The script should be valid TSql with no parse errors. Objects added
        //     using this method cannot be updated or deleted at a later point as update/delete
        //     requires a script name to be specified when adding the objects. If this is
        //     a requirement use the Microsoft.SqlServer.Dac.Model.TSqlModel.AddOrUpdateObjects(Microsoft.SqlServer.TransactSql.ScriptDom.TSqlScript,System.String,Microsoft.SqlServer.Dac.Model.TSqlObjectOptions)
        //     method instead.
        //
        // Parameters:
        //   inputScript:
        //     The Microsoft.SqlServer.TransactSql.ScriptDom.TSqlScript to add
        //
        //   options:
        //     Options defining how to interpret the script
        //
        // Exceptions:
        //   System.ArgumentNullException:
        //     If the supplied inputScript is null.
        //
        //   System.Runtime.Remoting.RemotingException:
        //     If communication with the Microsoft.SqlServer.Dac.Model.TSqlObjectService
        //     fails.
        public void AddObjects(TSqlScript inputScript, TSqlObjectOptions options)
        {
            model.AddObjects(inputScript, options);
        }

        //
        // Summary:
        //     Adds or updates the objects defined for a specified sourceName with the objects
        //     defined in the inputScript. If any objects were previously added with the
        //     same sourceName these will be completely replaced The object definitions
        //     are based on the contents of a TSql Script string plus additional metadata
        //     defined by a Microsoft.SqlServer.Dac.Model.TSqlObjectOptions object The script
        //     should consist of valid TSql DDL statements.
        //
        // Parameters:
        //   inputScript:
        //     Script containing TSql DDL statements
        //
        //   sourceName:
        //     A name to identify the inputScript, for example a fileName such as "MyTable.sql"
        //     or simply an alias like "dbo.Table". Scripts are cached and TSqlObjects are
        //     linked to the source name.  Any future Update/Delete operations will remove
        //     all existing objects with the same script name and replace them with the
        //     new objects.
        //
        //   options:
        //     Options defining how to interpret the script
        //
        // Exceptions:
        //   System.ArgumentNullException:
        //     If the supplied inputScript is null.
        //
        //   System.ArgumentException:
        //     If the supplied sourceName is null or whitespace, or if it ends in ".xsd".
        //     Note: source names ending in ".xsd" are currently not supported. These relate
        //     to Xml Schema Collections and adding of these is currently not supported
        //     in the public API.
        //
        //   System.Runtime.Remoting.RemotingException:
        //     If communication with the Microsoft.SqlServer.Dac.Model.TSqlObjectService
        //     fails.
        public void AddOrUpdateObjects(string inputScript, string sourceName, TSqlObjectOptions options)
        {
            model.AddOrUpdateObjects(inputScript, sourceName, options);
        }

        //
        // Summary:
        //     Adds or updates the objects defined for a specified sourceName with the objects
        //     defined in the inputScript. If any objects were previously added with the
        //     same sourceName these will be completely replaced The object definitions
        //     are based on the contents of a Microsoft.SqlServer.TransactSql.ScriptDom.TSqlScript
        //     object plus additional metadata defined by a Microsoft.SqlServer.Dac.Model.TSqlObjectOptions
        //     object The script should consist of valid TSql DDL statements.
        //
        // Parameters:
        //   inputScript:
        //     Script containing TSql DDL statements
        //
        //   sourceName:
        //     A name to identify the inputScript, for example a fileName such as "MyTable.sql"
        //     or simply an alias like "dbo.Table". Scripts are cached and TSqlObjects are
        //     linked to the source name.  Any future Update/Delete operations will remove
        //     all existing objects with the same script name and replace them with the
        //     new objects.
        //
        //   options:
        //     Options defining how to interpret the script
        //
        // Exceptions:
        //   System.ArgumentNullException:
        //     If the supplied inputScript is null.
        //
        //   System.ArgumentException:
        //     If the supplied sourceName is null or whitespace, or if it ends in ".xsd".
        //     Note: source names ending in ".xsd" are currently not supported. These relate
        //     to Xml Schema Collections and adding of these is currently not supported
        //     in the public API.
        //
        //   System.Runtime.Remoting.RemotingException:
        //     If communication with the Microsoft.SqlServer.Dac.Model.TSqlObjectService
        //     fails.
        public void AddOrUpdateObjects(TSqlScript inputScript, string sourceName, TSqlObjectOptions options)
        {
            model.AddOrUpdateObjects(inputScript, sourceName, options);
        }

        //
        // Summary:
        //     Copies the Microsoft.SqlServer.Dac.Model.DatabaseOptions for the model to
        //     a Microsoft.SqlServer.Dac.Model.TSqlModelOptions object.  This is useful
        //     if you wish to duplicate the options for a model when creating a new model.
        //
        // Returns:
        //     Microsoft.SqlServer.Dac.Model.TSqlModelOptions with settings matching the
        //     database options of the model
        public TSqlModelOptions CopyModelOptions()
        {
            return model.CopyModelOptions();
        }

        //
        // Summary:
        //     Deletes any objects that were added to the model with the specified sourceName.
        //
        // Parameters:
        //   sourceName:
        //     A name to identify the source to be deleted, for example a fileName such
        //     as "MyTable.sql" or simply an alias like "dbo.Table". Scripts are cached
        //     and TSqlObjects are linked to the source name.
        //
        // Exceptions:
        //   System.ArgumentException:
        //     If the supplied sourceName is null or whitespace, or if it ends in ".xsd".
        //     Note: source names ending in ".xsd" are currently not supported. These relate
        //     to Xml Schema Collections and adding of these is currently not supported
        //     in the public API.
        //
        //   System.Runtime.Remoting.RemotingException:
        //     If communication with the Microsoft.SqlServer.Dac.Model.TSqlObjectService
        //     fails.
        public void DeleteObjects(string sourceName)
        {
            model.DeleteObjects(sourceName);
        }


        public void Dispose()
        {
            if(this.model != null)
            {
                model.Dispose();
                model = null;
            }
        }
    }
}
