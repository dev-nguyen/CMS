using System;
using System.Collections.Generic;

#nullable disable

namespace CMS.Entity
{
    public partial class OptionValue : BaseEntity
    {
        public Guid? OptionId { get; set; }
        public Guid? VariableId { get; set; }
        public Guid? ItemId { get; set; }

        public virtual Item Item { get; set; }
        public virtual OptionSource Option { get; set; }
        public virtual Variable Variable { get; set; }
    }
}
