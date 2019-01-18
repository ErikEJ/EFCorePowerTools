using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Design.Internal;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.DependencyInjection;

namespace ReverseEngineer20
{
    public class DesignTimeServicesBuilder
    {
        private readonly IOperationReporter _reporter;
        private readonly Assembly _assembly;

        public DesignTimeServicesBuilder(string assemblyPath, IOperationReporter reporter)
        {
            _reporter = reporter;
            _assembly = Assembly.LoadFrom(assemblyPath);    
        }

        public virtual IServiceProvider Build(IServiceCollection services)
        {
            ConfigureUserServices(services);
            return services.BuildServiceProvider();
        }

        private void ConfigureUserServices(IServiceCollection services)
        {
            _reporter.WriteVerbose(DesignStrings.FindingDesignTimeServices(_assembly.GetName().Name));
            var types = GetLoadableTypes(_assembly);
            var designTimeServicesType = types
                .Where(t => typeof(IDesignTimeServices).GetTypeInfo().IsAssignableFrom(t)).Select(t => t.AsType())
                .FirstOrDefault();
            if (designTimeServicesType == null)
            {
                _reporter.WriteVerbose(DesignStrings.NoDesignTimeServices);
                return;
            }
            _reporter.WriteVerbose(DesignStrings.UsingDesignTimeServices(designTimeServicesType.ShortDisplayName()));
            ConfigureDesignTimeServices(designTimeServicesType, services);
        }

        private void ConfigureDesignTimeServices(Type designTimeServicesType, IServiceCollection services)
        {
            Debug.Assert(designTimeServicesType != null, "designTimeServicesType is null.");

            var designTimeServices = (IDesignTimeServices)Activator.CreateInstance(designTimeServicesType);
            designTimeServices.ConfigureDesignTimeServices(services);
        }

        private IEnumerable<TypeInfo> GetLoadableTypes(Assembly assembly)
        {
            if (assembly == null) throw new ArgumentNullException(nameof(assembly));
            try
            {
                return assembly.DefinedTypes;
            }
            catch (ReflectionTypeLoadException e)
            {
                return e.Types.Where(t => t != null).Cast<TypeInfo>();
            }
        }
    }
}