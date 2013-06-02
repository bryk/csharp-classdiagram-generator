using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VelocityDb;
using System.Runtime.Serialization;

namespace PersistenceLayer
{
    [DataContract]
    public abstract class Persistable : OptimizedPersistable
    {
        [DataMember]
        public new UInt32 Id { get; set; }

        public Persistable()
        {
            Id = 0;
        }

        public abstract void Merge(Persistable another);
    }
}
