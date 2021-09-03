using System;
using System.Collections.Generic;

#nullable disable

namespace CMS.Entity
{
    public partial class ReferenceValue : BaseEntity
    {
        public Guid? VariableId { get; set; }
        public Guid? ItemId { get; set; }
        public Guid? ReferenceSourceId { get; set; }

        public virtual Item Item { get; set; }
        public virtual ReferenceSource ReferenceSource { get; set; }
        public virtual Variable Variable { get; set; }
    }
}
