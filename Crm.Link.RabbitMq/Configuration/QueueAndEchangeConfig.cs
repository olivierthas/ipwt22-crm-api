namespace Crm.Link.RabbitMq.Configuration
{
    public static class QueueAndEchangeConfig
    {
        public static Dictionary<string, string[]> Euhhh { get; set; } = new Dictionary<string, string[]>
        {
            {"CrmAccount", new[] { "PlanningAccount", "FrontAccount" } },
            {"CrmSession", new[] { "PlanningSession", "FrontSession" } },
            {"CrmAccountSession", new[] { "PlanningSession", "FrontSession" } },
            {"None", new[] { "CrmAccount", "CrmSession", "CrmAccountSession"} }

        };
    }
}
