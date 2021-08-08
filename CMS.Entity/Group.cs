using System;
using System.Collections.Generic;

#nullable disable

namespace CMS.Entity
{
    public partial class Group
    {
        public Group()
        {
            Items = new HashSet<Item>();
            VariableGroups = new HashSet<VariableGroup>();
        }

        public Guid Id { get; set; }
        public string Title { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Modified { get; set; }
        public Guid? Author { get; set; }
        public Guid? Editor { get; set; }
        public bool? Active { get; set; }

        public virtual ICollection<Item> Items { get; set; }
        public virtual ICollection<VariableGroup> VariableGroups { get; set; }
    }
}
