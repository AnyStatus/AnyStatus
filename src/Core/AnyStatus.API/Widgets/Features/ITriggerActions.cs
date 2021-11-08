using System.Collections.Generic;

namespace AnyStatus.API.Widgets
{
    interface ITriggerActions
    {
        ICollection<IActionTrigger> ActionTriggers { get; set; }
    }
}
