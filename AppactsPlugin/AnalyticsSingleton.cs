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

using AppactsPlugin.Interface;
using AppactsPlugin.Data.Model.Enum;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace AppactsPlugin
{
    public sealed class AnalyticsSingleton
    {
        private static IAnalytics iAnalytics;
        private static readonly Object objectToLock = new Object();

        /// <summary>
        /// Get Analytics Instance
        /// </summary>
        /// <param name="accountId">accountId, when you setup your app this would be generated for you.</param>
        /// <param name="applicationId">applicationId, when you setup your app this would be generated for you</param>
        /// <param name="applicationVersion"> applicationVersion, specify version of your app here, make sure you don't forget to update this</param>
        /// <param name="uploadType">uploadType how data is going to be uploaded, UploadType.WhileUsingAsync | UploadType.Manual</param>
        /// <returns>iAnalytics new or cached instance</returns>
        /// <see cref="Integration Guidelines SDK Document"/>
        public static IAnalytics GetInstance()
        {
            lock (objectToLock)
            {
                if (iAnalytics == null)
                {
                    #if DEBUG
                    System.Diagnostics.Debug.WriteLine("created new analytics instance");
                    #endif

                    iAnalytics = new Analytics();
                }
            }
        
            return iAnalytics;
        }
    }
}
