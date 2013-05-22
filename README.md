Integration Examples
===============

##Basic Integration##


###Methods###
For basic integration, you can use the following methods:

      void Start(Assembly assembly, String applicationId, String serverUrl);
      void LogEvent(String screenName, String eventName);
      void Stop();



####Sample Usage####

#####App.xaml#####
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
        
        
#####Page#####
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


##Advanced Integration##

###Methods###

For advanced integration, you can use the following methods:

      void LogError(String screenName, String eventName, String data, ExceptionDescriptive ex);
      void LogEvent(String screenName, String eventName, String data);
      void LogFeedback(String screenName, RatingType ratingType, String comment) 
      void ScreenOpen(String screenName);
      void ScreenClosed(String screenName);
      void ContentLoading(String screenName, String contentName);   
      void ContentLoaded(String screenName, String contentName);
      void SetUserInformation(int age, SexType sexType);
      boolean IsUserInformationSet();
      void SetOptStatus(OptStatusType optStatusType);
      int GetOptStatus();
      void UploadWhileUsingAsync();
      void UploadManual();

####Getting an instance####

When your application is opened you need to obtain a new instance of IAnalytics. You can do this easily by using AnalyticsSingleton. For example:

######Import#####

using AppactsPlugin.Interface;


#####Methods#####

      IAnalytics AnalyticsSingleton.GetInstance()


#####Sample#####

      IAnalytics iAnalytics = AnalyticsSingleton.GetInstance();


#####Optional#####

We suggest that you add an abstract base class into your application so that you have a common area from where everything derives. For example:

      public abstract class PhoneApplicationPageBase : PhoneApplicationPage
      {
          #region //Protected Properties
          protected string PageName { get; set; }
          #endregion
      
          #region //Constructor
          public PhoneApplicationPageBase(string pageName)
          {
              this.PageName = pageName;
          } 
          #endregion
      
          #region //Public Methods
          protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
          {
              AnalyticsSingleton.GetInstance().ScreenOpen(this.PageName);
              base.OnNavigatedTo(e);
          }
      
          protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
          {
              AnalyticsSingleton.GetInstance().ScreenClosed(this.PageName);
              base.OnNavigatedFrom(e);
          }
          #endregion
      }

####Session Management####

Fantastic thing about Windows Phone 7+ framework is that all of the application life-cycle event handlers can be found in one place. You need to update app.xaml to call relevant methods.

#####Import######

      using AppactsPlugin.Interface;


#####Methods#####

      void IAnalytics.Start(System.Reflection.Assembly executingAssembly, String applicationId, String serverUrl)
      void IAnalytics.Stop()


#####Sample - App.xaml#####

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
        
Note: Please make sure that .Start & .Stop is always called from main thread, these two methods need to hook in to network coverage event handler.

###Opt In or Opt out?###

Every app is different. Your business needs to make a decision about how it wants to deal with its users. You can be really nice and ask your users whether or not they would like to participate in your customer experience improvement program. If they say yes you can use our services and log their experience. Alternatively you can make them opt in automatically by accepting your terms and conditions. Either way here is how you control opt in/ out in the terms and conditions scenario:

#####Import#####

      using AppactsPlugin.Data.Model;
      using AppactsPlugin.Data.Model.Enum;


#####Methods#####

      int GetOptStatus();
      void SetOptStatus(OptStatusType optStatusType);


#####Sample - GetOptStatus#####

      if (AnalyticsSingleton.GetInstance().GetOptStatus() == OptStatusType.None)
      {
          Deployment.Current.Dispatcher.BeginInvoke(() =>
          {
              NavigationService.Navigate(new Uri("/PageTermsAndConditions.xaml?removeBackEntry=true", UriKind.Relative));
          });
      
      }


#####Sample - SetOptStatus#####

      private void btnAgree_Click(object sender, RoutedEventArgs e)
      {
          AnalyticsSingleton.GetInstance().SetOptStatus(OptStatusType.OptIn);
          AnalyticsSingleton.GetInstance().LogEvent(this.PageName, "Agree");
          NavigationService.Navigate(new Uri("/PageDemographic.xaml?removeBackEntry=true", UriKind.Relative));
      }


####Demographics####

To improve your app you need to know who is using it, how old they are, what their gender is & where they are from. We have made it easy for you to capture this information:

#####Import#####

      using AppactsPlugin.Data.Model.Enum;
      using AppactsPlugin.Data.Model;


#####Methods#####

      IsUserInformationSet();
      void SetUserInformation(int age, SexType sexType);


#####Sample - IsUserInformationSet#####

      if (AnalyticsSingleton.GetInstance().IsUserInformationSet())
      {
          Deployment.Current.Dispatcher.BeginInvoke(() =>
          {
              NavigationService.Navigate(new Uri("/PageDog.xaml?removeBackEntry=true", UriKind.Relative));
          });
      
      }


#####Sample - SetUserInformation#####
      try
      {
          AnalyticsSingleton.GetInstance().SetUserInformation(age, sexType);
      }
      catch (Exception ex)
      {
          //TODO: handle the error, db is not working
      }
      
Note: Our plugin throws exception if it can't save users information. Normally this happens when user’s storage card is not present, it was corrupt, or device is full. As we throw an error you can notify a user that there was an issue or just handle it using in your app as per your business requirements.


####Logging and uploading your customers experience####

#####Import#####

      using AppactsPlugin.Data.Model;
      using AppactsPlugin.Data.Model.Enum;


#####Methods#####

      void LogError(String screenName, String eventName, String data, ExceptionDescriptive ex);
      void LogEvent(String screenName, String eventName, String data);
      void LogFeedback(String screenName, RatingType ratingType, String comment);
      void ScreenOpen(String screenName);
      void ScreenClosed(String screenName);
      void ContentLoading(String screenName, String contentName);   
      void ContentLoaded(String screenName, String contentName);
      void UploadWhileUsingAsync();
      void UploadManual();


#####Sample - LogError#####

      try
      {
          //app will throw exception on purpose
          this.txtPetName.Text = petNames[new Random().Next(0, 12)];
      }
      catch (Exception ex)
      {
          AnalyticsSingleton.GetInstance().LogError(this.PageName, "Generate", null, new ExceptionDescriptive(ex));
      }


#####Sample - LogEvent#####

      private void btnGenerate_Click(object sender, RoutedEventArgs e)
      {
          AnalyticsSingleton.GetInstance().LogEvent(this.PageName, "Generate");
      }


#####Sample - LogFeedback#####

      try
      {
          AnalyticsSingleton.GetInstance().LogFeedback(this.pageNameNavigateFrom, ratingType.Value, this.txtFeedback.Text);
      }
      catch (AppactsPlugin.Data.Model.ExceptionDatabaseLayer)
      {
          //todo: handle the error
      }
Note: Our plugin throws exception if it can't save users information. Normally this happens when user’s storage card is not present, it was corrupt, or device is full. As we throw an error you can notify a user that there was an issue or just handle it using in your app as per your business requirements.



#####Sample - ScreenOpen & ScreenClosed#####

      protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
      {
          AnalyticsSingleton.GetInstance().ScreenOpen(this.PageName);
          base.OnNavigatedTo(e);
      }
      
      protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
      {
          AnalyticsSingleton.GetInstance().ScreenClosed(this.PageName);
          base.OnNavigatedFrom(e);
      }


Note: Screen names need to be unique for each screen. We collect screen names in our plugin so that when Screen Closed is called we can calculate how long the user was on the screen for. You can use ScreenOpen many times but it will only register each unique screen name once.

#####Sample - ContentLoading & ContentLoaded#####

      AnalyticsSingleton.GetInstance().ContentLoading(this.PageName, "Some Content");
      
      try
      {
          //call your webservice, do some data processing before showing application to the user
      }
      catch (Exception ex)
      {
          AnalyticsSingleton.GetInstance().LogError(this.PageName, "Loading splash page", null, new ExceptionDescriptive(ex));
      }
      
      AnalyticsSingleton.GetInstance().ContentLoaded(this.PageName, "Some Content");
      

#####Sample - UploadWhileUsingAsync & UploadManual#####

We have created two methods for two different scenarios:

*UploadWhileUsingAsync – use this when you are creating a light application, i.e. utilities, forms, etc.. Using this method we will take care of all data uploading. As soon as the user creates an event we will try and upload this event to our servers and present it to you in your reports. The aim of this approach is to prevent waiting and obtain data straight away. Using this approach is recommended by our team as this will monitor network coverage, event queues and it will do its best to get data to our servers immediately.

*UploadManual – use this when you have a very event heavy application i.e. game. Using this method you will need to raise the upload event manually when you are ready. This is a very light approach and popular among some app makers, however data might not be uploaded to our servers for days/ weeks (depending on the app use) therefore statistics will be delayed.

*UploadWhileUsingAsync & UploadManual – you could always use both together. You can specify that you want to upload manually and later call UploadWhileUsingAsync. The example below will demonstrate this.

#####UploadWhileUsingAsync#####

      AnalyticsSingleton.GetInstance().Start(System.Reflection.Assembly.GetExecutingAssembly(), "95f33abd-9111-424b-a19b-9982c4e8c36f", "http://yourserver.com/api/", UploadType.WhileUsingAsync)


By specifying Upload Type While Using Async during the initial singleton request, the plugin will automatically start uploading data while a user is using the app.

#####UploadManual######

      AnalyticsSingleton.GetInstance().Start(System.Reflection.Assembly.GetExecutingAssembly(), "95f33abd-9111-424b-a19b-9982c4e8c36f", "http://yourserver.com/api/", UploadType.Manual)

By specifying Upload Type Manual during the initial singleton request, the plugin will not upload any data. It will just collect it and you will need to manually trigger either “UploadManual” or “UploadWhileUsingAsync” i.e.

      private void btnGenerate_Click(object sender, RoutedEventArgs e)
      {
          AnalyticsSingleton.GetInstance().LogEvent(this.PageName, "Generate");
          AnalyticsSingleton.GetInstance().UploadManual();
      }


You have to manually trigger upload if you want an upload to take place at a certain point. As mentioned before it has many draw backs, although it must be used in heavy data collection scenarios.

      private void btnGenerate_Click(object sender, RoutedEventArgs e)
      {
          AnalyticsSingleton.GetInstance().LogEvent(this.PageName, "Generate");
          AnalyticsSingleton.GetInstance().UploadWhileUsingAsync();
      }


You can call UploadWhileUsingAsync manually later if you called your singleton with “UploadType.Manual”. This can be useful in different scenarios but this approach should be rarely used.
