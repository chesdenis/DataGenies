﻿using System;
using System.Linq;
using System.Text;
using DataGenies.Core.Attributes;
using DataGenies.Core.Behaviours;
using DataGenies.Core.Containers;
using DataGenies.Core.Extensions;

namespace DataGenies.Core.Tests.Integration.Mocks.Converters
{
//     [ConverterTemplate]
//     public class MockRevertTextAfterReceiveConverter : WrapperBehaviour
//     {
//         // public override void BehaviourAction<T>(Action<T> action, T arg) where T : class
//         // {
//         //     var typedArg = arg as byte[];
//         //     
//         //     var testString = Encoding.UTF8.GetString(typedArg);
//         //     var convertedString = new string(testString.Reverse().ToArray()).ToBytes() as T;
//         //
//         //     action(convertedString);
//         // }
//
//         // public override void Execute(IContainer arg)
//         // {
//         //     // arg.Resolve<>()
//         //     //
//         //     // var testString = Encoding.UTF8.GetString(data);
//         //     // var convertedString = new string(testString.Reverse().ToArray());
//         //     // return Encoding.UTF8.GetBytes(convertedString);
//         // }
//
//         public override BehaviourScope BehaviourScope { get; set; } = BehaviourScope.Message;
//         public override BehaviourType BehaviourType { get; set; } = BehaviourType.Wrapper;
//     }
}