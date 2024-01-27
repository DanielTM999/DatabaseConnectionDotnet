using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseConnection.core
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class PropName: Attribute
    {
        public string Name { get;}

        public PropName(string name)
        {
            Name = name;
        }
    }
}
