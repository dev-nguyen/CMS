using System;
using System.Collections.Generic;

#nullable disable

namespace CMS.Entity
{
    public partial class VariableType
    {
        public VariableType()
        {
            Variables = new HashSet<Variable>();
        }

        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Modified { get; set; }
        public Guid? Author { get; set; }
        public Guid? Editor { get; set; }
        public bool? Active { get; set; }

        public virtual AppUser AuthorNavigation { get; set; }
        public virtual AppUser EditorNavigation { get; set; }
        public virtual ICollection<Variable> Variables { get; set; }
    }
}
