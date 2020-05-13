﻿namespace DG.Core.Orchestrators
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using DG.Core.Model.Output;

    public interface IApplicationOrchestrator
    {
        IEnumerable<StateReport> GetInstanceState(string application, string instanceName);

        IEnumerable<PropertyInfo> GetSettingsProperties(string application, string instanceName);

        void CollectPossibleApplicationTypes();

        void Register(string application, string instanceName);
        
        void UnRegister(string application, string instanceName);

        void BuildInstance(string application, string instanceName, string propertiesAsJson, int count = 1);

        void Start(string application, string instanceName);

        void Stop(string application, string instanceName);
    }
}