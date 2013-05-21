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

namespace SampleApplicationBasic
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        private void btnGenerate_OnClick(object sender, RoutedEventArgs e)
        {
            string[] petNames = new string[] {
                   "Laimo", "Smokey", "Lucy", "Fred", "Boy", "Cute", "Butch", "Alpha"
                };

            /*
             * Appacts
             * Get the instance and call LogEvent(screenName, eventName)
             */
            AppactsPlugin.AnalyticsSingleton.GetInstance().LogEvent("Main", "Generate");

            this.txtResult.Text = petNames[new Random().Next(0, 7)];

        }
    }
}