using System;
using System.Collections.Generic;

#nullable disable

namespace CMS.Entity
{
    public partial class ReferenceValue
    {
        public Guid Id { get; set; }
        public Guid? VariableId { get; set; }
        public Guid? CategoryId { get; set; }
        public Guid? ItemId { get; set; }
        public Guid? Display { get; set; }

        public virtual Category Category { get; set; }
        public virtual Item Item { get; set; }
        public virtual Variable Variable { get; set; }
    }
}
