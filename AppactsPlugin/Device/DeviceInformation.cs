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
using AppactsPlugin.Device.Interface;
using AppactsPlugin.Data.Model.Enum;
using System.Reflection;

using Microsoft.Phone.Info;
using System.Text;
using System.Globalization;
using System.Device.Location;
using AppactsPlugin.Data.Model;

namespace AppactsPlugin.Device
{
    internal class DeviceInformation : IDeviceInformation
    {
        private readonly ScreenResolution screenResolution;

        public DeviceInformation()
        {
            this.screenResolution = new ScreenResolution((int)Application.Current.Host.Content.ActualWidth, (int)Application.Current.Host.Content.ActualHeight);
        }

        public DeviceType GetDeviceType()
        {
            return DeviceType.WindowsPhone;
        }

        public long GetFlashDriveSize()
        {
            return -1;
        }

        public long GetMemorySize()
        {
            return DeviceStatus.DeviceTotalMemory;
        }

        public string GetModel()
        {
            return DeviceStatus.DeviceName;
        }

        public string GetPluginVersion()
        {
            string pluginVersion = null;

            try
            {
                AssemblyName assemblyName = new AssemblyName(Assembly.GetCallingAssembly().FullName);
                pluginVersion = assemblyName.Version.ToString();
            }
            catch (Exception)
            { }

            return pluginVersion;
        }

        public int GetPluginVersionCode()
        {
            return int.Parse(this.GetPluginVersion().Replace(".", ""));
        }

        public string GetLocale()
        {
            return CultureInfo.CurrentCulture.Name.Replace('-','_');
        }

        public string GetManufacturer()
        {
            return Microsoft.Phone.Info.DeviceStatus.DeviceManufacturer;
        }

        public ScreenResolution GetScreenResolution()
        {
            return this.screenResolution;
        }
    }
}
