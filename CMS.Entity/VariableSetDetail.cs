using System;
using System.Collections.Generic;

#nullable disable

namespace CMS.Entity
{
    public partial class VariableSetDetail
    {
        public Guid VariableSetId { get; set; }
        public Guid VariableId { get; set; }

        public virtual Variable Variable { get; set; }
        public virtual VariableSet VariableSet { get; set; }
    }
}
