using DG.Core.Attributes;
using DG.Core.Model.Enums;
using DG.Core.Model.Output;

namespace DG.Core.Tests.MockApplications
{
    [Application]
    internal class SampleApplicationWithReportStateB
    {
        [ReportState]
        public StateReport ReportStateSampleFunction()
        {
            return new StateReport()
            {
                Status = Status.Started,
            };
        }
    }
}