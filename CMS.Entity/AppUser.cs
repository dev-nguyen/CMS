using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace CMS.Entity
{
    public class AppUser : IdentityUser<Guid>
    {
        public AppUser()
        {
            CategoryAuthorNavigations = new HashSet<Category>();
            CategoryEditorNavigations = new HashSet<Category>();
            GroupAuthorNavigations = new HashSet<Group>();
            GroupEditorNavigations = new HashSet<Group>();
            ItemAuthorNavigations = new HashSet<Item>();
            ItemEditorNavigations = new HashSet<Item>();
            VariableAuthorNavigations = new HashSet<Variable>();
            VariableEditorNavigations = new HashSet<Variable>();
            VariableTypeAuthorNavigations = new HashSet<VariableType>();
            VariableTypeEditorNavigations = new HashSet<VariableType>();
        }

        public virtual ICollection<Category> CategoryAuthorNavigations { get; set; }
        public virtual ICollection<Category> CategoryEditorNavigations { get; set; }
        public virtual ICollection<Group> GroupAuthorNavigations { get; set; }
        public virtual ICollection<Group> GroupEditorNavigations { get; set; }
        public virtual ICollection<Item> ItemAuthorNavigations { get; set; }
        public virtual ICollection<Item> ItemEditorNavigations { get; set; }
        public virtual ICollection<Variable> VariableAuthorNavigations { get; set; }
        public virtual ICollection<Variable> VariableEditorNavigations { get; set; }
        public virtual ICollection<VariableType> VariableTypeAuthorNavigations { get; set; }
        public virtual ICollection<VariableType> VariableTypeEditorNavigations { get; set; }
    }
}
