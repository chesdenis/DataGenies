using System;
using System.Collections.Generic;
using DataGenies.Core.Models;

namespace DataGenies.Core.Scanners
{
    public interface IApplicationTemplatesScanner
    {
        IEnumerable<ApplicationTemplateEntity> ScanTemplates();

        Type FindType(ApplicationTemplateEntity applicationTemplateEntity);
    }
}