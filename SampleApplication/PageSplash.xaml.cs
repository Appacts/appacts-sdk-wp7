using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;

using SampleApplication.App_Base;
using System.Threading;
using AppactsPlugin.Data.Model;
using AppactsPlugin.Data.Model.Enum;

namespace SampleApplication
{
    public partial class PageSplash : PhoneApplicationPageBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PageSplash"/> class.
        /// </summary>
        public PageSplash()
            : base("Splash")
        {
            InitializeComponent();

            Thread thread = new Thread(() => work());
            thread.Start();
        }

        /// <summary>
        /// Works this instance.
        /// </summary>
        private void work()
        {
            AppactsPlugin.AnalyticsSingleton.GetInstance().ContentLoading(this.PageName, "work");

            try
            {
                //call your webservice, do some data processing before showing application to the user
            }
            catch (Exception ex)
            {
                AppactsPlugin.AnalyticsSingleton.GetInstance().LogError(this.PageName, "Loading splash page", null, new ExceptionDescriptive(ex));
            }

            AppactsPlugin.AnalyticsSingleton.GetInstance().ContentLoaded(this.PageName, "work");

            if (AppactsPlugin.AnalyticsSingleton.GetInstance().GetOptStatus() == OptStatusType.None)
            {
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    NavigationService.Navigate(new Uri("/PageTermsAndConditions.xaml?removeBackEntry=true", UriKind.Relative));
                });

            }
            else
            {
                if (AppactsPlugin.AnalyticsSingleton.GetInstance().IsUserInformationSet())
                {
                    Deployment.Current.Dispatcher.BeginInvoke(() =>
                    {
                        NavigationService.Navigate(new Uri("/PageDog.xaml?removeBackEntry=true", UriKind.Relative));
                    });

                }
                else
                {
                    Deployment.Current.Dispatcher.BeginInvoke(() =>
                    {
                        NavigationService.Navigate(new Uri("/PageDemographic.xaml?removeBackEntry=true", UriKind.Relative));
                    });
                }
            }
        }
    }
}