using System;
using System.Collections.Generic;

#nullable disable

namespace CMS.Entity
{
    public partial class Category
    {
        public Category()
        {
            Items = new HashSet<Item>();
            ReferenceValues = new HashSet<ReferenceValue>();
        }

        public Guid Id { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Modified { get; set; }
        public Guid? Author { get; set; }
        public Guid? Editor { get; set; }
        public bool? Active { get; set; }

        public virtual ICollection<Item> Items { get; set; }
        public virtual ICollection<ReferenceValue> ReferenceValues { get; set; }
    }
}
