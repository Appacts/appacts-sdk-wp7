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
using AppactsPlugin.Data.Model;

namespace SampleApplication
{
    public partial class PageCat : PhoneApplicationPageBase
    {
        #region //Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="PageCat"/> class.
        /// </summary>
        public PageCat()
            : base("Cat")
        {
            InitializeComponent();
        } 
        #endregion

        #region //Private Properties
        /// <summary>
        /// Handles the Click event of the btnDog control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void btnDog_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/PageDog.xaml?removeBackEntry=true", UriKind.Relative));
        }

        /// <summary>
        /// Handles the Click event of the btnGiveFeedback control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void btnGiveFeedback_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri(String.Concat("/PageFeedback.xaml?pageName=", this.PageName), UriKind.Relative));
        }

        /// <summary>
        /// Handles the Click event of the btnGenerate control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void btnGenerate_Click(object sender, RoutedEventArgs e)
        {
            string[] petNames = new string[] {
                    "Lilly", "Bella", "Hun", "Queen", "Sleepy", "Cute", "PoP", "Beta"
                };

            AppactsPlugin.AnalyticsSingleton.GetInstance().LogEvent(this.PageName, "Generate");

            try
            {
                //app will throw exception on purpose
                this.txtPetName.Text = petNames[new Random().Next(0, 12)];
            }
            catch (Exception ex)
            {
                AppactsPlugin.AnalyticsSingleton.GetInstance().LogError(this.PageName, "Generate", null, new ExceptionDescriptive(ex));
            }
        } 
        #endregion
    }
}