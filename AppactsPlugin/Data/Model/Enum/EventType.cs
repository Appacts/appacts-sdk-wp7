using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace AppactsPlugin.Data.Model.Enum
{
    internal enum EventType
    {
        ApplicationOpen = 1,
        ApplicationClose = 2,
        Error = 3,
        Event = 4,
        Feedback = 5,
        ScreenClosed = 6,
        ContentLoaded = 7,
        ContentLoading = 8,
        ScreenOpen = 9
    }
}
