﻿using DataGenies.Core.Attributes;
using DataGenies.Core.Behaviours;
using DataGenies.Core.Containers;

namespace DataGenies.Core.Tests.Integration.Mocks.Converters
{
    [ConverterTemplate]
    public class MockRevertTextAfterReceiveConverter : BehaviourAfterStart
    {
        public override void Execute(IContainer arg)
        {
            // arg.Resolve<>()
            //
            // var testString = Encoding.UTF8.GetString(data);
            // var convertedString = new string(testString.Reverse().ToArray());
            // return Encoding.UTF8.GetBytes(convertedString);
        }

        public override BehaviourScope BehaviourScope { get; set; } = BehaviourScope.Message;
        public override BehaviourType BehaviourType { get; set; } = BehaviourType.AfterStart;
    }
}