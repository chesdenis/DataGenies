using System;
using System.Collections.Generic;
using DataGenies.Core.Models;

namespace DataGenies.Core.Scanners
{
    public interface IBehaviourTemplatesScanner
    {
        IEnumerable<BehaviourTemplateEntity> ScanTemplates();

        Type FindType(BehaviourTemplateEntity behaviourTemplateEntity);
    }
}