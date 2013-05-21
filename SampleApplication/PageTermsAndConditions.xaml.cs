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
using AppactsPlugin.Data.Model.Enum;

namespace SampleApplication
{
    public partial class PageTermsAndConditions : PhoneApplicationPageBase
    {
        #region //Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="PageTermsAndConditions"/> class.
        /// </summary>
        public PageTermsAndConditions()
            : base("Terms and Conditions")
        {
            InitializeComponent();
        }
        #endregion

        #region //Private Properties
        /// <summary>
        /// Handles the Click event of the btnAgree control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        /// <see cref="http://blogs.msdn.com/b/ptorr/archive/2010/08/01/exiting-a-windows-phone-application.aspx"/>
        private void btnAgree_Click(object sender, RoutedEventArgs e)
        {
            AppactsPlugin.AnalyticsSingleton.GetInstance().SetOptStatus(OptStatusType.OptIn);
            AppactsPlugin.AnalyticsSingleton.GetInstance().LogEvent(this.PageName, "Agree", null);
            NavigationService.Navigate(new Uri("/PageDemographic.xaml?removeBackEntry=true", UriKind.Relative));
        } 
        #endregion
    }
}