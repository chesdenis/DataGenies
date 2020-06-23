using System;
using System.Collections.Generic;
using System.Text;

namespace DG.Core.Model.Dto
{
    public class PropertyDTO
    {
        public string PropertyTypeName { get; set; }

        public string PropertyName { get; set; }

        public object ProperyValue { get; set; }

        public int NestingLevel { get; set; }

        public string ParentKey { get; set; }
    }
}
