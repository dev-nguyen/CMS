﻿using System;
using System.Collections.Generic;

#nullable disable

namespace CMS.Entity
{
    public partial class NumberValue
    {
        public Guid Id { get; set; }
        public Guid? VariableId { get; set; }
        public Guid? ItemId { get; set; }
        public int? Value { get; set; }

        public virtual Item Item { get; set; }
        public virtual Variable Variable { get; set; }
    }
}
