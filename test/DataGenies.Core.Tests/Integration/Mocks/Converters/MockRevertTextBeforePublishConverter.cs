﻿using System;
using DataGenies.Core.Attributes;
using DataGenies.Core.Behaviours;
using DataGenies.Core.Containers;

namespace DataGenies.Core.Tests.Integration.Mocks.Converters
{
    // [ConverterTemplate]
    // public class MockRevertTextBeforePublishConverter : WrapperBehaviourTemplate
    // {
    //     public override void BehaviourActionWithMessage<T>(Action<T> action, T message)
    //     {
    //         base.BehaviourActionWithMessage(action, message);
    //     }
    //
    //     public override void Execute(IContainer arg)
    //     {
    //         // var testString = Encoding.UTF8.GetString(data);
    //         // var convertedString = new string(testString.Reverse().ToArray());
    //         // return Encoding.UTF8.GetBytes(convertedString);
    //     }
    //
    //     public override BehaviourScope BehaviourScope { get; set; } = BehaviourScope.Message;
    //     public override BehaviourType BehaviourType { get; set; } = BehaviourType.BeforeStart;
    // }
}