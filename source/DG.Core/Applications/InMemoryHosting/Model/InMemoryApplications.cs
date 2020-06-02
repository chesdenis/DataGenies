using DG.Core.Orchestrators;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DG.Core.Applications.InMemoryHosting
{
    public class InMemoryApplications : Dictionary<ApplicationUniqueId, InMemoryApplication>
    {
        public List<InMemoryApplication> GetAllApplication()
        {
            return this.Select(s => s.Value).ToList();
        }
    }
}