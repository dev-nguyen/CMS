using System;
using System.Collections.Generic;

#nullable disable

namespace CMS.Entity
{
    public partial class ReferenceSourceType : BaseEntity
    {
        public ReferenceSourceType()
        {
            Variables = new HashSet<Variable>();
        }

        public string Title { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Variable> Variables { get; set; }
    }
}
