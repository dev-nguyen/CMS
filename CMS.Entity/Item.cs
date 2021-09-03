using System;
using System.Collections.Generic;

#nullable disable

namespace CMS.Entity
{
    public partial class Item : BaseEntity
    {
        public Item()
        {
            DateTimeValues = new HashSet<DateTimeValue>();
            FloatValues = new HashSet<FloatValue>();
            NumberValues = new HashSet<NumberValue>();
            OptionValues = new HashSet<OptionValue>();
            ReferenceValues = new HashSet<ReferenceValue>();
            StringValues = new HashSet<StringValue>();
            TextValues = new HashSet<TextValue>();
            TrueFalseValues = new HashSet<TrueFalseValue>();

            Created = DateTime.Now.ToUniversalTime();
            Modified = DateTime.Now.ToUniversalTime();
        }

        public DateTime? Created { get; set; }
        public DateTime? Modified { get; set; }
        public Guid? AuthorId { get; set; }
        public Guid? EditorId { get; set; }
        public Guid? CategoryId { get; set; }
        public bool? Active { get; set; }

        public virtual AppUser Author { get; set; }
        public virtual Category Category { get; set; }
        public virtual AppUser Editor { get; set; }
        public virtual ICollection<DateTimeValue> DateTimeValues { get; set; }
        public virtual ICollection<FloatValue> FloatValues { get; set; }
        public virtual ICollection<NumberValue> NumberValues { get; set; }
        public virtual ICollection<OptionValue> OptionValues { get; set; }
        public virtual ICollection<ReferenceValue> ReferenceValues { get; set; }
        public virtual ICollection<StringValue> StringValues { get; set; }
        public virtual ICollection<TextValue> TextValues { get; set; }
        public virtual ICollection<TrueFalseValue> TrueFalseValues { get; set; }
    }
}
