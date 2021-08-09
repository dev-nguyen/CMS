using System;
using System.Collections.Generic;

#nullable disable

namespace CMS.Entity
{
    public partial class Item
    {
        public Item()
        {
            HyperlinkValues = new HashSet<HyperlinkValue>();
            MoneyValues = new HashSet<MoneyValue>();
            NumberValues = new HashSet<NumberValue>();
            ReferenceValues = new HashSet<ReferenceValue>();
            StringValues = new HashSet<StringValue>();
            TextValues = new HashSet<TextValue>();
        }

        public Guid Id { get; set; }
        public Guid? CatalogId { get; set; }
        public Guid? GroupId { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Modified { get; set; }
        public Guid? Author { get; set; }
        public Guid? Editor { get; set; }
        public bool? Active { get; set; }

        public virtual AppUser AuthorNavigation { get; set; }
        public virtual Category Catalog { get; set; }
        public virtual AppUser EditorNavigation { get; set; }
        public virtual Group Group { get; set; }
        public virtual ICollection<HyperlinkValue> HyperlinkValues { get; set; }
        public virtual ICollection<MoneyValue> MoneyValues { get; set; }
        public virtual ICollection<NumberValue> NumberValues { get; set; }
        public virtual ICollection<ReferenceValue> ReferenceValues { get; set; }
        public virtual ICollection<StringValue> StringValues { get; set; }
        public virtual ICollection<TextValue> TextValues { get; set; }
    }
}
