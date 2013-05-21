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
    public partial class PageFeedback : PhoneApplicationPageBase
    {
        #region //Private Properties
        private string pageNameNavigateFrom;
        #endregion

        #region //Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="PageFeedback"/> class.
        /// </summary>
        public PageFeedback()
            : base("Feedback")
        {
            InitializeComponent();
            this.Loaded += this.PageFeedback_Loaded;
        }

        /// <summary>
        /// Handles the Loaded event of the PageFeedback control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        public void PageFeedback_Loaded(object sender, RoutedEventArgs e)
        {
            this.pageNameNavigateFrom = NavigationContext.QueryString["pageName"];
            this.txtUserMessage.Text = string.Format(this.txtUserMessage.Text, this.pageNameNavigateFrom);
        }
        #endregion


        #region //Private Properties
        /// <summary>
        /// Handles the Click event of the btnSend control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
            RatingType? ratingType = null;

            ListBoxItem listBoxItemRating = this.lstRating.SelectedItem as ListBoxItem;
            if (listBoxItemRating != null)
            {
                ratingType = (RatingType)int.Parse((string)listBoxItemRating.Content);
            }

            try
            {
                AppactsPlugin.AnalyticsSingleton.GetInstance().LogFeedback(this.pageNameNavigateFrom, ratingType.Value, this.txtFeedback.Text);
            }
            catch (AppactsPlugin.Data.Model.ExceptionDatabaseLayer)
            {
                //todo: handle the error
            }

            NavigationService.GoBack();
        }

        /// <summary>
        /// Handles the Click event of the btnCancel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        } 
        #endregion
    }
}