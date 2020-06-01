using DG.Core.Scanners;
using System;
using System.Collections.Generic;

namespace DG.Core.Orchestrators
{
    public class ApplicationTypesCollection : List<Type>
    {
        private readonly IApplicationTypesScanner applicationTypesScanner;

        public ApplicationTypesCollection(IApplicationTypesScanner applicationTypesScanner)
        {
            this.applicationTypesScanner = applicationTypesScanner;
        }

        public void CollectApplicationTypes()
        {
            this.Clear();
            this.AddRange(this.applicationTypesScanner.Scan());
        }
    }
}