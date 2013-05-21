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

using Microsoft.Phone.Controls;

using AppactsPlugin.Interface;
using AppactsPlugin.Data.Model.Enum;

namespace SampleApplication.App_Base
{
    public abstract class PhoneApplicationPageBase : PhoneApplicationPage
    {
        #region //Private Properties
        /// <summary>
        /// Gets or sets the name of the page.
        /// </summary>
        /// <value>
        /// The name of the page.
        /// </value>
        public string PageName { get; set; }
        #endregion

        #region //Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="PhoneApplicationPageBase"/> class.
        /// </summary>
        /// <param name="pageName">Name of the page.</param>
        /// <example>http://blogs.msdn.com/b/jschaffe/archive/2011/03/03/creating-a-custom-base-page-for-windows-phone-7.aspx</example>
        public PhoneApplicationPageBase(string pageName)
        {
            this.PageName = pageName;

            this.Loaded += this.onLoaded;
        } 
        #endregion

        #region //Public Methods
        /// <summary>
        /// Ons the loaded.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        protected virtual void onLoaded(object sender, RoutedEventArgs e)
        {
            if (NavigationContext.QueryString.ContainsKey("removeBackEntry") && 
                bool.Parse(NavigationContext.QueryString["removeBackEntry"]))
            {
                NavigationService.RemoveBackEntry();
            }
        }

        /// <summary>
        /// Called when a page becomes the active page in a frame.
        /// </summary>
        /// <param name="e">An object that contains the event data.</param>
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            AppactsPlugin.AnalyticsSingleton.GetInstance().ScreenOpen(this.PageName);
            base.OnNavigatedTo(e);
        }


        /// <summary>
        /// Called when a page is no longer the active page in a frame.
        /// </summary>
        /// <param name="e">An object that contains the event data.</param>
        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            AppactsPlugin.AnalyticsSingleton.GetInstance().ScreenClosed(this.PageName);
            base.OnNavigatedFrom(e);
        }
        #endregion
    }
}
