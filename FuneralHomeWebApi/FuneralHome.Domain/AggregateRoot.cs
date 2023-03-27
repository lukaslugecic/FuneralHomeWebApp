using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuneralHome.Domain
{
    /// <summary>
    /// Domain aggregate root base
    /// </summary>
    /// <typeparam name="TPrimKey"></typeparam>
    public abstract class AggregateRoot<TPrimKey> : Entity<TPrimKey>
    {

        protected AggregateRoot(TPrimKey id) : base(id)
        {
        }
    }
}