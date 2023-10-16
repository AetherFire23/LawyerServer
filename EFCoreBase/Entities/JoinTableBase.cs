using EFCoreBase.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JWTAuth.Entities;
internal abstract class JoinTableBase<T1, T2> : EntityBase
{
    public Guid LeftId { get; set; }
    public T1 LeftEntity { get; set; }

    public Guid RightId { get; set; }
    public T2 RightEntity { get; set; }
}
