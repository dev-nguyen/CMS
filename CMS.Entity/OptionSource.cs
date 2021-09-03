using System;
using System.Collections.Generic;

#nullable disable

namespace CMS.Entity
{
    public partial class OptionSource : BaseEntity
    {
        public OptionSource()
        {
            OptionValues = new HashSet<OptionValue>();
        }

        public Guid? VariableId { get; set; }
        public string Value { get; set; }

        public virtual Variable Variable { get; set; }
        public virtual ICollection<OptionValue> OptionValues { get; set; }
    }
}
