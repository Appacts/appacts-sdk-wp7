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
    public enum WebServiceResponseCodeType
    {
        Ok = 100,
        InactiveAccount = 101,
        InactiveApplication = 102,
        NoDevice = 103,
        GeneralError = 104
    }
}
