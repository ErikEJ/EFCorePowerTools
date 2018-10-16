using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ReverseEngineer20.ReverseEngineer
{
    public class CustomNameOptions
    {
        public IList<Schema> Schemas { get; set; }
    }
}
