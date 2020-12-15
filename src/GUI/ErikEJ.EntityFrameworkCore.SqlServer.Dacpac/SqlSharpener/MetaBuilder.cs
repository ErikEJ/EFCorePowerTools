using System;
using System.Collections.Generic;
using System.Linq;
using SqlSharpener.Model;
using dac = Microsoft.SqlServer.Dac.Model;

namespace SqlSharpener
{
    /// <summary>
    /// Creates a model from the specified sql files.
    /// </summary>
    [Serializable]
    public class MetaBuilder
    {
        private IEnumerable<Procedure> _procedures;
        private bool _modelLoaded = false;

        /// <summary>
        /// Objects representing the meta data parsed from stored procedures in the model.
        /// </summary>
        /// <value>
        /// The procedures.
        /// </value>
        public IEnumerable<Procedure> Procedures
        {
            get
            {
                if (!_modelLoaded) throw new InvalidOperationException("Load the model");
                return _procedures;
            }
        }

        /// <summary>
        /// Parses the specified TSqlModel
        /// </summary>
        /// <param name="model">The model.</param>
        public void LoadModel(dac.TSqlModel model)
        {
            _procedures = model.GetObjects(dac.DacQueryScopes.UserDefined, dac.Procedure.TypeClass)
                .Select(sqlProc => new Procedure(sqlProc)).ToList();
            _modelLoaded = true;
        }
    }
}