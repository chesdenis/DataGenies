using System.Collections;
using System.Collections.Generic;

namespace DataGenies.Core.Models
{
    public class BindingReference
    {
        public int PublisherId { get; set; }
        public int ReceiverId { get; set; }
        
        public string PublisherInstanceName { get; set; }
        public string ReceiverInstanceName { get; set; }
        
        public string CurrentInstanceName { get; set; }
        public string CurrentTemplateName { get; set; }

        public string ExchangeName => $"{PublisherInstanceName}_{ReceiverInstanceName}";
        public string QueueName => $"{CurrentTemplateName}->{CurrentInstanceName}";

        public string PublisherTemplateName { get; set; }
        public string ReceiverTemplateName { get; set; }

        public string RoutingKey { get; set; }
    }

    public class ConnectedPublishers : List<BindingReference>
    {
        
    }

    public class ConnectedReceivers : List<BindingReference>
    {
        
    }

    public class BindingNetwork
    {
        public int ApplicationInstanceEntityId { get; set; }
        public ConnectedPublishers Publishers { get; set; }
        public ConnectedReceivers Receivers { get; set; }
    }
}