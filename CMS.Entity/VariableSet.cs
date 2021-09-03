using System;
using System.Collections.Generic;

#nullable disable

namespace CMS.Entity
{
    public partial class VariableSet : BaseEntity
    {
        public VariableSet()
        {
            Categories = new HashSet<Category>();
            VariableSetDetails = new HashSet<VariableSetDetail>();
            Created = DateTime.Now.ToUniversalTime();
            Modified = DateTime.Now.ToUniversalTime();
        }

        public string Title { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Modified { get; set; }
        public Guid? AuthorId { get; set; }
        public Guid? EditorId { get; set; }
        public bool? Active { get; set; }

        public virtual AppUser Author { get; set; }
        public virtual AppUser Editor { get; set; }
        public virtual ICollection<Category> Categories { get; set; }
        public virtual ICollection<VariableSetDetail> VariableSetDetails { get; set; }
    }
}
