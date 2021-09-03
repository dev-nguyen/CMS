using System;
using System.Collections.Generic;

#nullable disable

namespace CMS.Entity
{
    public partial class Variable : BaseEntity
    {
        public Variable()
        {
            DateTimeValues = new HashSet<DateTimeValue>();
            FloatValues = new HashSet<FloatValue>();
            NumberValues = new HashSet<NumberValue>();
            OptionSources = new HashSet<OptionSource>();
            OptionValues = new HashSet<OptionValue>();
            ReferenceSources = new HashSet<ReferenceSource>();
            ReferenceValues = new HashSet<ReferenceValue>();
            StringValues = new HashSet<StringValue>();
            TextValues = new HashSet<TextValue>();
            TrueFalseValues = new HashSet<TrueFalseValue>();
            VariableSetDetails = new HashSet<VariableSetDetail>();

            Created = DateTime.Now.ToUniversalTime();
            Modified = DateTime.Now.ToUniversalTime();
        }

        public string Title { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Modified { get; set; }
        public Guid? AuthorId { get; set; }
        public Guid? EditorId { get; set; }
        public string Description { get; set; }
        public bool? Active { get; set; }
        public Guid? VariableTypeId { get; set; }
        public Guid? SourceId { get; set; }
        public bool? Hidden { get; set; }
        public Guid? SourceTypeId { get; set; }

        public virtual AppUser Author { get; set; }
        public virtual AppUser Editor { get; set; }
        public virtual Category Source { get; set; }
        public virtual ReferenceSourceType SourceType { get; set; }
        public virtual VariableType VariableType { get; set; }
        public virtual ICollection<DateTimeValue> DateTimeValues { get; set; }
        public virtual ICollection<FloatValue> FloatValues { get; set; }
        public virtual ICollection<NumberValue> NumberValues { get; set; }
        public virtual ICollection<OptionSource> OptionSources { get; set; }
        public virtual ICollection<OptionValue> OptionValues { get; set; }
        public virtual ICollection<ReferenceSource> ReferenceSources { get; set; }
        public virtual ICollection<ReferenceValue> ReferenceValues { get; set; }
        public virtual ICollection<StringValue> StringValues { get; set; }
        public virtual ICollection<TextValue> TextValues { get; set; }
        public virtual ICollection<TrueFalseValue> TrueFalseValues { get; set; }
        public virtual ICollection<VariableSetDetail> VariableSetDetails { get; set; }
    }
}
