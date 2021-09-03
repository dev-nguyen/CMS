using System;
using System.Collections.Generic;

#nullable disable

namespace CMS.Entity
{
    public partial class ReferenceSource : BaseEntity
    {
        public ReferenceSource()
        {
            ReferenceValues = new HashSet<ReferenceValue>();
        }

        public Guid? VariableId { get; set; }
        public Guid? SourceId { get; set; }

        public virtual Category Source { get; set; }
        public virtual Variable Variable { get; set; }
        public virtual ICollection<ReferenceValue> ReferenceValues { get; set; }
    }
}
