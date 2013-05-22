appacts-sdk-wp7
===============

#Methods#
For basic integration, you can use the following methods:

void Start(Assembly assembly, String applicationId, String serverUrl);
void LogEvent(String screenName, String eventName);
void Stop();



##Sample Usage##

###App.xaml###
      // Code to execute when the application is launching (eg, from Start)
        // This code will not execute when the application is reactivated
        private void Application_Launching(object sender, LaunchingEventArgs e)
        {
            /*
             * Appacts
             * Application is launching lets start the session
             */
            AppactsPlugin.AnalyticsSingleton.GetInstance().Start(System.Reflection.Assembly.GetExecutingAssembly(),
                "9baa4776-ed8f-42ec-b7dc-94f1e2a8ac87", "http://yourserver.com/api/");

        }

        // Code to execute when the application is activated (brought to foreground)
        // This code will not execute when the application is first launched
        private void Application_Activated(object sender, ActivatedEventArgs e)
        {
            /*
             * Appacts
             * Application has came back to the foreground, lets start the session
             */
            AppactsPlugin.AnalyticsSingleton.GetInstance().Start(System.Reflection.Assembly.GetExecutingAssembly(),
                "9baa4776-ed8f-42ec-b7dc-94f1e2a8ac87", "http://yourserver.com/api/");
        }

        // Code to execute when the application is deactivated (sent to background)
        // This code will not execute when the application is closing
        private void Application_Deactivated(object sender, DeactivatedEventArgs e)
        {
            /*
             * Appacts
             * Application has gone in to the background lets stop the session
             */
            AppactsPlugin.AnalyticsSingleton.GetInstance().Stop();
        }
        
        
###Page###
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

