using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace CMS.Entity
{
    public class AppUser:IdentityUser<Guid>
    {
        public virtual ICollection<Category> CategoryAuthors { get; set; }
        public virtual ICollection<Category> CategoryEditors { get; set; }
        public virtual ICollection<Item> ItemAuthors { get; set; }
        public virtual ICollection<Item> ItemEditors { get; set; }
        public virtual ICollection<Variable> VariableAuthors { get; set; }
        public virtual ICollection<Variable> VariableEditors { get; set; }
        public virtual ICollection<VariableSet> VariableSetAuthors { get; set; }
        public virtual ICollection<VariableSet> VariableSetEditors { get; set; }
        public virtual ICollection<VariableType> VariableTypeAuthors { get; set; }
        public virtual ICollection<VariableType> VariableTypeEditors { get; set; }
    }
}
