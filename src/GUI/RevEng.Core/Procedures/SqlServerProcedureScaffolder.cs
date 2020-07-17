using Microsoft.EntityFrameworkCore.Scaffolding;
using RevEng.Core.Procedures.Model;
using RevEng.Core.Procedures.Scaffolding;

namespace RevEng.Core.Procedures
{
    public class SqlServerProcedureScaffolder : IProcedureScaffolder
    {
        private readonly IProcedureModelFactory procedureModelFactory;

        public SqlServerProcedureScaffolder(IProcedureModelFactory procedureModelFactory)
        {
            this.procedureModelFactory = procedureModelFactory;
        }

        public ScaffoldedModel ScaffoldModel(string connectionString, ProcedureScaffolderOptions procedureScaffolderOptions)
        {
            //TODO Generate code and file names here!

            return null;
        }
    }
}
