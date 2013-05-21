using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AppactsPlugin.Data.Model.Enum;
using AppactsPlugin.Data.Model;

namespace AppactsPlugin.Device.Interface
{
    public interface IDeviceInformation
    {
        DeviceType GetDeviceType();
        long GetFlashDriveSize();
        long GetMemorySize();
        String GetModel();
        String GetPluginVersion();
        int GetPluginVersionCode();
        string GetLocale();
        string GetManufacturer();
        ScreenResolution GetScreenResolution();
    }
}
