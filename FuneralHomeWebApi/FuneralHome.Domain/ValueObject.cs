using BaseLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuneralHome.Domain
{
    /// <summary>
    /// Value object base
    /// </summary>
    public abstract class ValueObject
    {
        public abstract Result IsValid();
        public override abstract bool Equals(object? other);
        public override abstract int GetHashCode();
    }
}