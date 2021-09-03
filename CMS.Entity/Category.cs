using System;
using System.Collections.Generic;

#nullable disable

namespace CMS.Entity
{
    public partial class Category: BaseEntity 
    {
        public Category()
        {
            Items = new HashSet<Item>();
            ReferenceSources = new HashSet<ReferenceSource>();
            Variables = new HashSet<Variable>();

            Created = DateTime.Now.ToUniversalTime();
            Modified = DateTime.Now.ToUniversalTime();
        }

        public string Title { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Modified { get; set; }
        public Guid? AuthorId { get; set; }
        public Guid? EditorId { get; set; }
        public bool? Active { get; set; }
        public Guid? VariableSetId { get; set; }

        public virtual AppUser Author { get; set; }
        public virtual AppUser Editor { get; set; }
        public virtual VariableSet VariableSet { get; set; }
        public virtual ICollection<Item> Items { get; set; }
        public virtual ICollection<ReferenceSource> ReferenceSources { get; set; }
        public virtual ICollection<Variable> Variables { get; set; }
    }
}
