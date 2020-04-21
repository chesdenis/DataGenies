using DG.Core.Attributes;

namespace DG.Core.Tests.MockApplications
{
    [Application]
    internal class SampleApplicationWithReportStateA
    {
        [ReportState]
        public string ReportStateSampleFunction()
        {
            return "{'Status':'0'}";
        }
    }
}