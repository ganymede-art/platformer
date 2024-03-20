using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class UserInterfaceStatics
{
    public static bool AreAllWidgetsEnabled(IUserInterfaceWidget[] widgets)
        => widgets.All(x => x.Status == UserInterfaceWidgetStatus.Enabled);

    public static bool AreAllWidgetsDisabled(IUserInterfaceWidget[] widgets)
        => widgets.All(x => x.Status == UserInterfaceWidgetStatus.Disabled);
}