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
using AppactsPlugin.Data.Model;

namespace SampleApplication
{
    public partial class PageDemographic : PhoneApplicationPageBase
    {
        #region //Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="PageDemographic"/> class.
        /// </summary>
        public PageDemographic()
            : base("Demographic")
        {
            InitializeComponent();
        }
        #endregion

        #region //Private Properties
        /// <summary>
        /// Handles the Click event of the btnNext control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            int age = -1;
            SexType? sexType = null;

            ListBoxItem listBoxItemAge = this.lstAge.SelectedItem as ListBoxItem;
            if (listBoxItemAge != null)
            {
                age = int.Parse((string)listBoxItemAge.Content);
            }

            ListBoxItem listBoxItemGender = this.lstGender.SelectedItem as ListBoxItem;
            if (listBoxItemGender != null)
            {
                sexType = (SexType)Enum.Parse(typeof(SexType), (string)listBoxItemGender.Content, true);
            }

            if (age != -1 && sexType.HasValue)
            {
                try
                {
                    AppactsPlugin.AnalyticsSingleton.GetInstance().SetUserInformation(age, sexType.Value);
                }
                catch (ExceptionDatabaseLayer ex)
                {
                    //TODO: handle the error, db is not working
                }

                NavigationService.Navigate(new Uri("/PageDog.xaml?removeBackEntry=true", UriKind.Relative));
            }
            else
            {
                //TODO: warn user that information needs to be filled in
            }
        }

        /// <summary>
        /// Handles the Click event of the btnSkip control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void btnSkip_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/PageDog.xaml?removeBackEntry=true", UriKind.Relative));
        } 
        #endregion
    }
}