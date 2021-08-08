using System;
using System.Collections.Generic;

#nullable disable

namespace CMS.Entity
{
    public partial class VariableGroup
    {
        public Guid GroupId { get; set; }
        public Guid VariableId { get; set; }

        public virtual Group Group { get; set; }
        public virtual Variable Variable { get; set; }
    }
}
