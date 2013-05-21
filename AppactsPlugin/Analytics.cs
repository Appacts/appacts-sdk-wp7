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
using Microsoft.Phone.Net.NetworkInformation;

using AppactsPlugin.Interface;
using AppactsPlugin.Data.Interface;
using AppactsPlugin.Data.Sql;
using AppactsPlugin.Data.WebService;
using AppactsPlugin.Device.Interface;
using System.Collections.Generic;
using AppactsPlugin.Data.Model;
using AppactsPlugin.Device;
using AppactsPlugin.Data.Model.Enum;
using System.Threading;
using System.Net.NetworkInformation;
using System.ComponentModel;
using System.Device.Location;
using System.Reflection;

namespace AppactsPlugin
{
    internal class Analytics : IAnalytics
    {
        private const string connectonString = "appacts.sdf";

        private string baseUrl = string.Empty;		  

        private Guid applicationId;
        private string applicationVersion;
        private Guid sessionId;

        private IStorageDal iStorageDal;
        private IUploadDal iUploadDal;

        private IDeviceInformation iDeviceInformation;
        private IDeviceDynamicInformation iDeviceDynamicInformation;
        private IPlatform iPlatform;

        private readonly List<Session> sessionsScreenOpen = new List<Session>();
        private readonly List<Session> sessionsContentLoading = new List<Session>();

        private bool authenticationFailure = false;
        private bool databaseExists = false;
        private bool itemsWaitingToBeUploaded = true;
        private int numberOfItemsWaitingToBeUploaded = 0;
        private OptStatusType optStatusType = OptStatusType.OptIn;
        private UploadType uploadType;

        private bool userProcessed = false;
        private bool deviceLocationProcessed = false;
        private bool upgradedProcessed = false;

        private Object threadUploadLock = new Object();
        private Object threadIsUploadingLock = new Object();
        private bool threadIsUploading = false;
        private Session session = null;
        private bool threadUploadInterrupted = false;
        private bool stopped = false;
        private bool started = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="Analytics"/> class.
        /// </summary>
        /// <param name="accountId">The account id.</param>
        /// <param name="applicationId">The application id.</param>
        /// <param name="applicationVersion">The application version.</param>
        public Analytics()
        {

        }

        /// <summary>
        /// Starts the specified assembly.
        /// </summary>
        /// <param name="baseUrl">base url i.e. the url of the API http://yoursite.com/api/ </param>
        /// <param name="assembly">The assembly.</param>
        /// <param name="applicationId">The application id.</param>
        /// <param name="uploadType">Type of the upload.</param>
        public void Start(string baseUrl, Assembly assembly, string applicationId, UploadType uploadType)
        {
            if (!this.started)
            {
                if (String.IsNullOrEmpty(baseUrl))
                {
                    throw new Exception("You need to specify baseUrl, i.e. your server api url http://yoursite.com/api/");
                }

                this.baseUrl = baseUrl;

                string applicationVersion = null;
                try
                {
                    AssemblyName assemblyName = new AssemblyName(assembly.FullName);
                    applicationVersion = assemblyName.Version.ToString();
                }
                catch
                {
                    applicationVersion = "0";
                }

                this.init(new Guid(applicationId), uploadType, applicationVersion);

                this.threadUploadInterrupted = false;
                this.started = true;
                this.stopped = false;

                if (uploadType == UploadType.WhileUsingAsync)
                {
                    this.UploadWhileUsingAsync();
                }
            }
        }

        /// <summary>
        /// Starts the specified assembly.
        /// </summary>
        /// <param name="baseUrl">base url i.e. the url of the API http://yoursite.com/api/ </param>
        /// <param name="assembly">The assembly.</param>
        /// <param name="applicationId">The application id.</param>
        public void Start(string baseUrl, Assembly assembly, string applicationId)
        {
            this.Start(baseUrl, assembly, applicationId, UploadType.WhileUsingAsync);
        }

        /// <summary>
        /// Uploads the while using async.
        /// </summary>
        public void UploadWhileUsingAsync()
        {
            if(this.uploadType != UploadType.WhileUsingAsync) 
            {
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    NetworkChange.NetworkAddressChanged +=
                        new NetworkAddressChangedEventHandler(networkChanged);
                });

                this.uploadType = UploadType.WhileUsingAsync;
            }

            this.uploadIntelligent();
        }

        /// <summary>
        /// Uploads the manual.
        /// </summary>
        public void UploadManual()
        {
            this.uploadIntelligent();
        }

        /// <summary>
        /// Logs the error.
        /// </summary>
        /// <param name="screenName">Name of the screen.</param>
        /// <param name="eventName">Name of the event.</param>
        /// <param name="data">The data.</param>
        /// <param name="ex">The ex.</param>
        public void LogError(string screenName, string eventName, string data, ExceptionDescriptive ex)
        {
            if (this.started && this.databaseExists)
            {
                try
                {
                    ErrorItem errorItem = new ErrorItem(this.applicationId, screenName, data,
                        this.iDeviceDynamicInformation.GetDeviceGeneralInformation(), eventName, ex, this.sessionId, this.applicationVersion);
                    this.iStorageDal.Save(errorItem);

                    this.setItemsWaitingToBeUploaded();

                    if (this.uploadType == UploadType.WhileUsingAsync)
                    {
                        this.uploadIntelligent();
                    }
                }
                catch (ExceptionDatabaseLayer exceptionDatabaseLayer)
                {
                    this.logSystemError(exceptionDatabaseLayer);
                }
            }
        }

        /// <summary>
        /// Logs the event.
        /// </summary>
        /// <param name="screenName">Name of the screen.</param>
        /// <param name="eventName">Name of the event.</param>
        /// <param name="data">The data.</param>
        public void LogEvent(string screenName, string eventName, string data)
        {
            if (this.started && this.databaseExists && this.optStatusType == OptStatusType.OptIn)
            {
                try
                {
                    EventItem eventItem = new EventItem(this.applicationId, screenName, data,
                        EventType.Event, eventName, 0, this.sessionId, this.applicationVersion);
                    this.iStorageDal.Save(eventItem);

                    this.setItemsWaitingToBeUploaded();
                    if (this.uploadType == UploadType.WhileUsingAsync)
                    {
                        this.uploadIntelligent();
                    }

                }
                catch (ExceptionDatabaseLayer ex)
                {
                    this.logSystemError(ex);
                }
            }
        }

        /// <summary>
        /// Logs the event.
        /// </summary>
        /// <param name="screenName">Name of the screen.</param>
        /// <param name="eventName">Name of the event.</param>
        public void LogEvent(string screenName, string eventName)
        {
            this.LogEvent(screenName, eventName, null);
        }

        /// <summary>
        /// Logs the feedback.
        /// </summary>
        /// <param name="screenName">Name of the screen.</param>
        /// <param name="ratingType">Type of the rating.</param>
        /// <param name="comment">The comment.</param>
        public void LogFeedback(string screenName, RatingType ratingType, string comment)
        {
            try
            {
                FeedbackItem feedbackItem = new FeedbackItem(this.applicationId, screenName,
                    comment, ratingType, this.sessionId, this.applicationVersion);

                this.iStorageDal.Save(feedbackItem);

                this.setItemsWaitingToBeUploaded();
                if (this.uploadType == UploadType.WhileUsingAsync)
                {
                    this.uploadIntelligent();
                }
            }
            catch (ExceptionDatabaseLayer ex)
            {
                this.logSystemError(ex);
                throw ex;
            }
        }

        /// <summary>
        /// Screens the open.
        /// </summary>
        /// <param name="screenName">Name of the screen.</param>
        public void ScreenOpen(string screenName)
        {
             if(this.started && this.databaseExists && this.optStatusType == OptStatusType.OptIn) {
                try {
            	    Session session = new Session(screenName);

        		    lock(this.sessionsScreenOpen) {
                        if (!this.sessionsScreenOpen.Contains(session))
                        {
                            this.sessionsScreenOpen.Add(session);
	        			
		                    EventItem eventItem = new EventItem(this.applicationId, screenName, null, 
		                        EventType.ScreenOpen, 0, this.sessionId, this.applicationVersion);

		                    this.iStorageDal.Save(eventItem);
		                
		                    this.setItemsWaitingToBeUploaded();

		                    if(this.uploadType == UploadType.WhileUsingAsync) {
		                        this.uploadIntelligent();
		                    }
	            	    }
        		    }
                } catch(ExceptionDatabaseLayer ex) {
                    this.logSystemError(ex);
                }
            }
        }

        /// <summary>
        /// Screens the closed.
        /// </summary>
        /// <param name="screenName">Name of the screen.</param>
        public void ScreenClosed(string screenName)
        {
            if(this.started && this.databaseExists && this.optStatusType == OptStatusType.OptIn) {
                try {
                
                    long miliSeconds = 0;
                    int index = -1;
                
                    lock(this.sessionsScreenOpen) {
	                    for(int i = 0; i < this.sessionsScreenOpen.Count; i++) {
	                        Session session = (Session)this.sessionsScreenOpen[i];
	                        if(session.Name == screenName) {
	                            index = i;
	                            miliSeconds = session.End();
	                            break;
	                        }
	                    }
	               
	                    if(index != -1) {
	                        this.sessionsScreenOpen.RemoveAt(index);
	                    
		                    EventItem eventItem = new EventItem(this.applicationId, screenName, null, 
		                        EventType.ScreenClosed, miliSeconds, this.sessionId, this.applicationVersion);
		                    this.iStorageDal.Save(eventItem);
		                
		                    this.setItemsWaitingToBeUploaded();
		                    if(this.uploadType == UploadType.WhileUsingAsync) {
		                        this.uploadIntelligent();
		                    }                    
	                    }
                    }
                } catch(ExceptionDatabaseLayer ex) {
                    this.logSystemError(ex);
                }
            }
        }

        /// <summary>
        /// Contents the loading.
        /// </summary>
        /// <param name="screenName">Name of the screen.</param>
        /// <param name="contentName">Name of the content.</param>
        public void ContentLoading(String screenName, string contentName)
        {
            if(this.started && 
                this.databaseExists && this.optStatusType == OptStatusType.OptIn) {
                try {
            	    Session session = new Session(String.Concat(screenName, contentName));
            	
             	    lock(this.sessionsContentLoading) {
                        if (!this.sessionsContentLoading.Contains(session))
                        {
                            this.sessionsContentLoading.Add(session);
	            		
		                    EventItem eventItem = new EventItem(this.applicationId, screenName, null, 
		                        EventType.ContentLoading, contentName, 0, this.sessionId, this.applicationVersion);

		                    this.iStorageDal.Save(eventItem);
		                
		                    this.setItemsWaitingToBeUploaded();
		                    if(this.uploadType == UploadType.WhileUsingAsync) {
		                        this.uploadIntelligent();
		                    }
	            	    }
             	    }
                } catch(ExceptionDatabaseLayer ex) {
                    this.logSystemError(ex);
                }
            }
        }

        /// <summary>
        /// Contents the loaded.
        /// </summary>
        /// <param name="screenName">Name of the screen.</param>
        /// <param name="contentName">Name of the content.</param>
        public void ContentLoaded(String screenName, string contentName)
        {
            if(this.started && 
                this.databaseExists && this.optStatusType == OptStatusType.OptIn) {
                try {
             
                    long miliSeconds = 0;
                    int index = -1;
                
                    lock(this.sessionsContentLoading) {

                        string sessionName = string.Concat(screenName, contentName);

                        for (int i = 0; i < this.sessionsContentLoading.Count; i++)
                        {
                            Session session = (Session)this.sessionsContentLoading[i];
                            if (session.Name == sessionName)
                            {
	                            index = i;
	                            miliSeconds = session.End();
	                            break;
	                        }
	                    }
	                
	                    if(index != -1) {
                            this.sessionsContentLoading.RemoveAt(index);
	                    
		                     EventItem eventItem = new EventItem(this.applicationId, screenName, null, 
		                        EventType.ContentLoaded, contentName, miliSeconds, this.sessionId, this.applicationVersion);
		                    this.iStorageDal.Save(eventItem);
		                
		                    this.setItemsWaitingToBeUploaded();
		                    if(this.uploadType == UploadType.WhileUsingAsync) {
		                        this.uploadIntelligent();
		                    }                   
	                    }
                    }
                } catch(ExceptionDatabaseLayer ex) {
                    this.logSystemError(ex);
                }
            }
        }

        /// <summary>
        /// Sets the user information.
        /// </summary>
        /// <param name="age">The age.</param>
        /// <param name="sexType">Type of the sex.</param>
        public void SetUserInformation(int age, SexType sexType)
        {
            try
            {
                User user = new User(age, sexType, StatusType.Pending, this.applicationId, this.sessionId, this.applicationVersion);
                this.iStorageDal.Save(user);

                this.setItemsWaitingToBeUploaded();
                if (this.uploadType == UploadType.WhileUsingAsync)
                {
                    this.uploadIntelligent();
                }
            }
            catch (ExceptionDatabaseLayer ex)
            {
                this.logSystemError(ex);
                throw ex;
            }
        }

        /// <summary>
        /// Determines whether [is user information set].
        /// </summary>
        /// <returns>
        ///   <c>true</c> if [is user information set]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsUserInformationSet()
        {
            bool isUserInformationSet = true;

            if (this.started && this.databaseExists)
            {
                try
                {
                    if (this.iStorageDal.GetUser(this.applicationId) == null)
                    {
                        isUserInformationSet = false;
                    }
                }
                catch (ExceptionDatabaseLayer ex)
                {
                    this.logSystemError(ex);
                }
            }

            return isUserInformationSet;
        }

        /// <summary>
        /// Sets the opt status.
        /// </summary>
        /// <param name="optStatusType">Type of the opt status.</param>
        public void SetOptStatus(Data.Model.Enum.OptStatusType optStatusType)
        {
            try
            {
                this.optStatusType = optStatusType;
                if (this.started && this.databaseExists)
                {
                    AppactsPlugin.Data.Model.ApplicationMeta application = this.iStorageDal.GetApplication(this.applicationId);
                    application.OptStatus = optStatusType;
                    this.iStorageDal.Update(application);
                }
            }
            catch (ExceptionDatabaseLayer ex)
            {
                this.logSystemError(ex);
            }
        }

        /// <summary>
        /// Gets the opt status.
        /// </summary>
        /// <returns></returns>
        public OptStatusType GetOptStatus()
        {
            OptStatusType optStatusType = OptStatusType.None;
            try
            {
                if (this.started && this.databaseExists)
                {
                    AppactsPlugin.Data.Model.ApplicationMeta application = this.iStorageDal.GetApplication(this.applicationId);
                    optStatusType = application.OptStatus;
                }
            }
            catch (ExceptionDatabaseLayer ex)
            {
                this.logSystemError(ex);
            }
            return optStatusType;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Stop()
        {
            if (this.started)
            {
                try
                {
                    this.threadUploadInterrupted = true;
                    NetworkChange.NetworkAddressChanged -= this.networkChanged;

                    this.iUploadDal.Dispose();
                    this.iDeviceDynamicInformation.Dispose();
                }
                catch (Exception) { }

                if (this.databaseExists)
                {
                    try
                    {
                        EventItem eventItem = new EventItem(this.applicationId, null,
                            null, EventType.ApplicationClose, null, this.session.End(), this.sessionId, this.applicationVersion);
                        this.iStorageDal.Save(eventItem);

                        AppactsPlugin.Data.Model.ApplicationMeta application = this.iStorageDal.GetApplication(this.applicationId);
                        application.State = ApplicationStateType.Close;
                        this.iStorageDal.Update(application);

                    }
                    catch (ExceptionDatabaseLayer ex)
                    {
                        this.logSystemError(ex);
                    }

                    try
                    {
                        lock (this.threadIsUploadingLock)
                        {
                            if (this.threadIsUploading)
                            {
                                while (this.threadIsUploading)
                                {
                                    Monitor.Wait(this.threadIsUploadingLock);
                                }
                            }
                        }
                    }
                    catch (Exception)
                    {

                    }
                    finally
                    {
                        System.Diagnostics.Debug.WriteLine("analytics stop");
                        this.iStorageDal.Dispose();

                    }
                }

                this.stopped = true;
                this.started = false;
            }
        }

        #region //Private Properties
        /// <summary>
        /// Inits this instance.
        /// </summary>
        private void init(Guid applicationId, UploadType uploadType, string applicationVersion)
        {
            this.applicationId = applicationId;
            this.applicationVersion = applicationVersion;
            this.sessionId = Guid.NewGuid();

            this.iStorageDal = new StorageSql(connectonString);
            this.iUploadDal = new UploadWS(baseUrl);

            this.iDeviceInformation = new DeviceInformation();
            this.iDeviceDynamicInformation = new DeviceDynamicInformation();
            this.iPlatform = new Platform();

            try
            {
                this.initDatabase();

                this.databaseExists = true;
            }
            catch (ExceptionDatabaseLayer) { }


            if (this.databaseExists)
            {
                AppactsPlugin.Data.Model.ApplicationMeta applicationMeta = null;
                bool applicationInitialSetup = false;

                try
                {
                    applicationMeta = this.iStorageDal.GetApplication(this.applicationId);

                    if (applicationMeta == null)
                    {
                        applicationMeta = new ApplicationMeta(this.applicationId, ApplicationStateType.Close, DateTime.Now, OptStatusType.OptIn);

                        this.iStorageDal.Save(applicationMeta);
                        applicationInitialSetup = true;
                    }
                }
                catch (ExceptionDatabaseLayer ex)
                {
                    this.logSystemError(ex);
                }

                try
                {
                    this.optStatusType = applicationMeta.OptStatus;

                    if (applicationMeta.State == ApplicationStateType.Open)
                    {
                        this.iStorageDal.Save(new Crash(this.applicationId, applicationMeta.SessionId, this.applicationVersion));
                    }

                    this.iStorageDal.Save(new EventItem(this.applicationId, null, null,
                        EventType.ApplicationOpen, 0, this.sessionId, this.applicationVersion));

                    applicationMeta.SessionId = this.sessionId;
                    applicationMeta.State = ApplicationStateType.Open;

                    if (applicationMeta.Version == null || applicationMeta.Version != this.applicationVersion)
                    {
                        applicationMeta.Version = this.applicationVersion;
                        applicationMeta.Upgraded = !applicationInitialSetup;
                    }

                    this.iStorageDal.Update(applicationMeta);

                    //#if DEBUG
                   // throw new ExceptionDatabaseLayer(new Exception("Random test"));
                    //#endif
                }
                catch (ExceptionDatabaseLayer exceptionDatabaseLayer)
                {
                    this.logSystemError(exceptionDatabaseLayer);
                }
            }

            this.session = new Session();
        }

        /// <summary>
        /// Inits the database.
        /// </summary>
        private void initDatabase()
        {
            #if DEBUG
            //clean database everytime
            //this.iStorageDal.Delete();
            #endif

            if (!this.iStorageDal.Exists())
            {
                this.iStorageDal.Create();
            }

            int pluginVersionCurrent = this.iDeviceInformation.GetPluginVersionCode();
            int pluginVersionOld = this.iStorageDal.GetSchemaVersion();
            if (this.iStorageDal.UpdateSchema(pluginVersionCurrent, pluginVersionOld))
            {
                this.iStorageDal.UpdateSchemaVersion(pluginVersionCurrent);
            }
        }

        /// <summary>
        /// Logs the system error.
        /// </summary>
        /// <param name="exceptionDatabaseLayer">The exception database layer.</param>
        private void logSystemError(ExceptionDescriptive exceptionDescriptive)
        {
            try
            {
                if (this.databaseExists)
                {
                    #if DEBUG
                    System.Diagnostics.Debug.WriteLine("logSystemError");
                    System.Diagnostics.Debug.WriteLine(exceptionDescriptive.Message);
                    System.Diagnostics.Debug.WriteLine(exceptionDescriptive.Source);
                    System.Diagnostics.Debug.WriteLine(exceptionDescriptive.StackTrace);
                    #endif

                    this.iStorageDal.Save(new SystemError(this.applicationId, exceptionDescriptive,
                        new AnalyticsSystem(this.iDeviceInformation.GetDeviceType(), this.iDeviceInformation.GetPluginVersion()), this.sessionId,
                            this.applicationVersion));

                    this.setItemsWaitingToBeUploaded();
                }
            }
            catch (ExceptionDatabaseLayer exceptionDatabaseLayer) 
            {
                #if DEBUG
                System.Diagnostics.Debug.WriteLine(exceptionDatabaseLayer.Message);
                System.Diagnostics.Debug.WriteLine(exceptionDatabaseLayer.Source);
                System.Diagnostics.Debug.WriteLine(exceptionDatabaseLayer.StackTrace);
                #endif
            }
        }

        /// <summary>
        /// Uploads the intelligent.
        /// </summary>
        private void uploadIntelligent()
        {
            if (this.itemsWaitingToBeUploaded && Microsoft.Phone.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable()
                && !this.authenticationFailure && this.optStatusType == OptStatusType.OptIn && !this.threadUploadInterrupted
                    && this.databaseExists && this.started)
            {
                lock (this.threadIsUploadingLock)
                {
                    if (!this.threadIsUploading)
                    {
                        #if DEBUG
                        System.Diagnostics.Debug.WriteLine("Starting a new upload thread");
                        #endif

                        this.threadIsUploading = true;
                        ThreadPool.QueueUserWorkItem(new WaitCallback(upload));
                    }
                }
            }
        }

        /// <summary>
        /// Networks the changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void networkChanged(Object sender, EventArgs e)
        {
            if (Microsoft.Phone.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
            {
                this.uploadIntelligent();
            }
        }

        /// <summary>
        /// Uploads the specified state info.
        /// </summary>
        /// <param name="stateInfo">The state info.</param>
        private void upload(Object stateInfo)
        {
            Guid deviceId = Guid.Empty; 
    	    bool exceptionWasRaised = false;
        
            if(!this.threadUploadInterrupted) {

	            try {
                    AppactsPlugin.Data.Model.DeviceLocation device = this.iStorageDal.GetDeviceLocation();

                    if (device == null)
                    {
                        deviceId = this.iUploadDal.Device
                            (this.applicationId, iDeviceInformation.GetModel(), iPlatform.GetOS(), 
                            (int)DeviceType.WindowsPhone, iPlatform.GetCarrier(), 
                            this.applicationVersion, 
                            (DateTime.Now - DateTime.UtcNow).Hours, 
                            this.iDeviceInformation.GetLocale(), 
                            this.iDeviceInformation.GetScreenResolution().Width.ToString(),
                            this.iDeviceInformation.GetScreenResolution().Height.ToString(), 
                            this.iDeviceInformation.GetManufacturer());

                        if (deviceId == Guid.Empty)
                        {
                            this.authenticationFailure = true;
                            return;
                        }

                        device = new Data.Model.DeviceLocation(deviceId);
                        this.iStorageDal.Save(device);
                    }
                    else
                    {
                        deviceId = device.Id;

                        if (!this.upgradedProcessed)
                        {
                            this.upgradedProcessed = this.uploadUpgraded(deviceId);
                        }
                    }
	            } catch(ExceptionDatabaseLayer ex) {
	                #if DEBUG
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    System.Diagnostics.Debug.WriteLine("Save Device");
                    #endif
	                exceptionWasRaised = true;
	            } catch(ExceptionWebServiceLayer ex) { 
	                #if DEBUG
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    System.Diagnostics.Debug.WriteLine("Save Device");
                    #endif
	                exceptionWasRaised = true;
	            }   
            }
        
    	    //remember current count before we start processing items, so later we can
            //find out if we need to call upload thread again
            int numberOfItemsWaitingToBeUploadedBefore = this.numberOfItemsWaitingToBeUploaded;
        
            if(!this.threadUploadInterrupted && !exceptionWasRaised) {
	            try {
	                this.uploadSystemError(deviceId);
	            } catch(ExceptionDatabaseLayer ex) {
	                #if DEBUG
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    System.Diagnostics.Debug.WriteLine("uploadSystemError");
                    #endif
	                exceptionWasRaised = true;
	            } catch(ExceptionWebServiceLayer ex) { 
	                #if DEBUG
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                     System.Diagnostics.Debug.WriteLine("uploadSystemError");
                    #endif
	                exceptionWasRaised = true;
	            }   
            }
        
            if(!this.threadUploadInterrupted && !exceptionWasRaised) {
	            try {
	                this.uploadCrash(deviceId);
	            } catch(ExceptionDatabaseLayer ex) {
	                #if DEBUG
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                     System.Diagnostics.Debug.WriteLine("uploadSystemError");
                    #endif
	                exceptionWasRaised = true;
	            } catch(ExceptionWebServiceLayer ex) { 
	                #if DEBUG
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    System.Diagnostics.Debug.WriteLine("uploadCrash");
                    #endif
	                exceptionWasRaised = true;
	            }   
            }
       
            if(!this.threadUploadInterrupted && !exceptionWasRaised && !this.userProcessed) {
	            try {
                    this.userProcessed =  this.uploadUser(deviceId);
	            } catch(ExceptionDatabaseLayer ex) {
	                #if DEBUG
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    System.Diagnostics.Debug.WriteLine("uploadUser");
                    #endif
	                exceptionWasRaised = true;
	            } catch(ExceptionWebServiceLayer ex) { 
	                #if DEBUG
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    System.Diagnostics.Debug.WriteLine("uploadUser");
                    #endif
	                exceptionWasRaised = true;
	            }
            }
            
            if(!this.threadUploadInterrupted && !exceptionWasRaised) {
	            try {
	                this.uploadError(deviceId);
	            } catch(ExceptionDatabaseLayer ex) {
	                #if DEBUG
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    System.Diagnostics.Debug.WriteLine("uploadError");
                    #endif
	                exceptionWasRaised = true;
	            } catch(ExceptionWebServiceLayer ex) { 
	                #if DEBUG
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    System.Diagnostics.Debug.WriteLine("uploadError");
                    #endif
	                exceptionWasRaised = true;
	            }  
            }
       
            if(!this.threadUploadInterrupted && !exceptionWasRaised) {
	            try {
	                this.uploadEvent(deviceId);
	            } catch(ExceptionDatabaseLayer ex) {
	                #if DEBUG
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    System.Diagnostics.Debug.WriteLine("uploadEvent");
                    #endif
	                exceptionWasRaised = true;
	            } catch(ExceptionWebServiceLayer ex) { 
	                #if DEBUG
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    System.Diagnostics.Debug.WriteLine("uploadEvent");
                    #endif
	                exceptionWasRaised = true;
	            }  
            }
        
            if(!this.threadUploadInterrupted && !exceptionWasRaised) {
	            try {
	                this.uploadFeedback(deviceId);
	            } catch(ExceptionDatabaseLayer ex) {
	                #if DEBUG
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    System.Diagnostics.Debug.WriteLine("uploadFeedback");
                    #endif
	                exceptionWasRaised = true;
	            } catch(ExceptionWebServiceLayer ex) { 
	                #if DEBUG
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    System.Diagnostics.Debug.WriteLine("uploadFeedback");
                    #endif
	                exceptionWasRaised = true;
	            }  
            }
        
            if(!this.threadUploadInterrupted && !exceptionWasRaised && !this.deviceLocationProcessed) {
	            try {
	                this.deviceLocationProcessed = this.uploadDeviceLocation(deviceId);
	            } catch(ExceptionDatabaseLayer ex) {
	                #if DEBUG
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    System.Diagnostics.Debug.WriteLine("uploadDeviceLocation");
                    #endif
	                exceptionWasRaised = true;
	            } catch(ExceptionWebServiceLayer ex) { 
	                #if DEBUG
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    System.Diagnostics.Debug.WriteLine("uploadDeviceLocation");
                    #endif
	                exceptionWasRaised = true;
	            }   
            }
        
            if(!exceptionWasRaised) {
        	    this.uploadSuccesfullyCompleted(numberOfItemsWaitingToBeUploadedBefore);
            }
        
            lock(this.threadIsUploadingLock) {
                this.threadIsUploading = false;

                #if DEBUG
                System.Diagnostics.Debug.WriteLine("Ending upload thread");
                #endif

                Monitor.PulseAll(this.threadIsUploadingLock);
            }
        }

        /// <summary>
        /// Uploads the succesfully completed.
        /// </summary>
        /// <param name="numberOfItemsWaitingToBeUploadedBefore">The number of items waiting to be uploaded before.</param>
        private void uploadSuccesfullyCompleted(int numberOfItemsWaitingToBeUploadedBefore)
        {
            if (!this.threadUploadInterrupted)
            {
                if (numberOfItemsWaitingToBeUploadedBefore == this.numberOfItemsWaitingToBeUploaded)
                {
                    this.itemsWaitingToBeUploaded = false;
                }
            }
        }

        /// <summary>
        /// Sets the items waiting to be uploaded.
        /// </summary>
        private void setItemsWaitingToBeUploaded()
        {
            this.numberOfItemsWaitingToBeUploaded++;
            this.itemsWaitingToBeUploaded = true;
        }

        /// <summary>
        /// Uploads the system error.
        /// </summary>
        /// <param name="deviceId">The device id.</param>
        private void uploadSystemError(Guid deviceId) 
        {
            SystemError systemError = null;
            do
            {
                systemError = this.iStorageDal.GetSystemError(this.applicationId);
            
                if(systemError != null) {
                    WebServiceResponseCodeType responseCode = this.iUploadDal.SystemError(deviceId, systemError);
                    
                    if(responseCode == WebServiceResponseCodeType.Ok) {
                        this.iStorageDal.Remove(systemError);
                    } else if(responseCode == WebServiceResponseCodeType.InactiveAccount 
                        || responseCode ==  WebServiceResponseCodeType.InactiveApplication) {
                	    this.authenticationFailure = true;
                        return;
                    } else if(responseCode == WebServiceResponseCodeType.GeneralError) {
                        return;
                    }
                }
            } while(systemError != null && !this.threadUploadInterrupted);
        }
    
        /// <summary>
        /// Uploads the crash.
        /// </summary>
        /// <param name="deviceId">The device id.</param>
        private void uploadCrash(Guid deviceId) 
        {
            Crash crash = null;
            do
            {
                crash = this.iStorageDal.GetCrash(applicationId);
            
                if(crash != null) {
                    WebServiceResponseCodeType responseCode = this.iUploadDal.Crash(deviceId, crash);
                
                    if(responseCode == WebServiceResponseCodeType.Ok) {
                        this.iStorageDal.Remove(crash);
                    } else if(responseCode == WebServiceResponseCodeType.InactiveAccount 
                        || responseCode ==  WebServiceResponseCodeType.InactiveApplication) {
                	    this.authenticationFailure = true;
                        return;
                    } else if(responseCode == WebServiceResponseCodeType.GeneralError) {
                        return;
                    }
                }
            } while(crash != null && !this.threadUploadInterrupted);
        }
    
        /// <summary>
        /// Uploads the user.
        /// </summary>
        /// <param name="deviceId">The device id.</param>
        private bool uploadUser(Guid deviceId) 
        {
           User user = this.iStorageDal.GetUser(this.applicationId);

           if(user != null && user.Status == StatusType.Pending) {
                WebServiceResponseCodeType responseCode = this.iUploadDal.User(deviceId, user);
    
                if(responseCode == WebServiceResponseCodeType.Ok) {
                    user.Status = StatusType.Processed;
                    this.iStorageDal.Update(user);
                    return true;
                } else if(responseCode == WebServiceResponseCodeType.InactiveAccount 
                    || responseCode ==  WebServiceResponseCodeType.InactiveApplication) {
            	    this.authenticationFailure = true;
                    return false;
                }
           }
           else if (user == null)
           {
               return false;
           }

           return true;
        }
    
        /// <summary>
        /// Uploads the error.
        /// </summary>
        /// <param name="deviceId">The device id.</param>
        private void uploadError(Guid deviceId) 
        {
            ErrorItem errorItem = null;
            do
            {
                errorItem = this.iStorageDal.GetErrorItem(this.applicationId);
            
                if(errorItem != null) {
        
                    WebServiceResponseCodeType responseCode = this.iUploadDal.Error(deviceId, errorItem);
                    
                    if(responseCode == WebServiceResponseCodeType.Ok) {
                        this.iStorageDal.Remove(errorItem);
                    } else if(responseCode == WebServiceResponseCodeType.InactiveAccount 
                        || responseCode ==  WebServiceResponseCodeType.InactiveApplication) {
                	    this.authenticationFailure = true;
                        return;
                    } else if(responseCode == WebServiceResponseCodeType.GeneralError) {
                        return;
                    }
                }
            } while(errorItem != null && !this.threadUploadInterrupted);
        }
    
        /// <summary>
        /// Uploads the event.
        /// </summary>
        /// <param name="deviceId">The device id.</param>
        private void uploadEvent(Guid deviceId) 
        {
            EventItem eventItem = null;
            do
            {
                eventItem = this.iStorageDal.GetEventItem(this.applicationId);
            
                if(eventItem != null) {
                    WebServiceResponseCodeType responseCode = this.iUploadDal.Event(deviceId, eventItem);
                    
                    if(responseCode == WebServiceResponseCodeType.Ok) {
                        this.iStorageDal.Remove(eventItem);
                    } else if(responseCode == WebServiceResponseCodeType.InactiveAccount 
                        || responseCode ==  WebServiceResponseCodeType.InactiveApplication) {
                	    this.authenticationFailure = true;
                        return;
                    } else if(responseCode == WebServiceResponseCodeType.GeneralError) {
                        return;
                    }
              
                }
            } while(eventItem != null && !this.threadUploadInterrupted);
        }
    
        /// <summary>
        /// Uploads the feedback.
        /// </summary>
        /// <param name="deviceId">The device id.</param>
        private void uploadFeedback(Guid deviceId) 
        {
            FeedbackItem feedbackItem = null;
            do
            {
                feedbackItem = this.iStorageDal.GetFeedbackItem(this.applicationId);
            
                if(feedbackItem != null) {
                    WebServiceResponseCodeType responseCode = this.iUploadDal.Feedback(deviceId, feedbackItem);

                    if(responseCode == WebServiceResponseCodeType.Ok) {
                        this.iStorageDal.Remove(feedbackItem);
                    } else if(responseCode == WebServiceResponseCodeType.InactiveAccount 
                        || responseCode ==  WebServiceResponseCodeType.InactiveApplication) {
                	    this.authenticationFailure = true;
                        return;
                    } else if(responseCode == WebServiceResponseCodeType.GeneralError) {
                        return;
                    }
                }
            } while(feedbackItem != null && !this.threadUploadInterrupted);
        }

        /// <summary>
        /// Uploads the device location.
        /// </summary>
        /// <param name="deviceId">The device id.</param>
        private bool uploadDeviceLocation(Guid deviceId) 
        {
            DeviceLocation deviceLocationProcessed = this.iStorageDal.GetDeviceLocation(StatusType.Processed);
        
            if(deviceLocationProcessed == null) {
           
                DeviceLocation deviceLocationPending = this.iStorageDal.GetDeviceLocation(StatusType.Pending);
            
                if(deviceLocationPending == null) {
                    try {

                        GeoCoordinate geoCoordinate = this.iDeviceDynamicInformation.GetDeviceLocation();

                        if (geoCoordinate != null)
                        {
                            deviceLocationPending = this.iStorageDal.GetDeviceLocation();
                            deviceLocationPending.Longitude = geoCoordinate.Longitude;
                            deviceLocationPending.Latitude = geoCoordinate.Latitude;
                            deviceLocationPending.Status = StatusType.Pending;

                            this.iStorageDal.Update(deviceLocationPending);
                        }
                    } catch(Exception ex) {
                        #if DEBUG
                        System.Diagnostics.Debug.WriteLine(ex.Message);
                        #endif
                        deviceLocationPending = null;
                    }
                }

                if (deviceLocationPending != null)
                {
                    WebServiceResponseCodeType responseCode = this.iUploadDal.Location(deviceId, applicationId, deviceLocationPending);
                    if (responseCode == WebServiceResponseCodeType.Ok)
                    {
                        deviceLocationPending.Status = StatusType.Processed;
                        this.iStorageDal.Update(deviceLocationPending);
                        return true;
                    }
                    else if (responseCode == WebServiceResponseCodeType.InactiveAccount || responseCode == WebServiceResponseCodeType.InactiveApplication)
                    {
                        this.authenticationFailure = true;
                        return true;
                    }
                    else if (responseCode == WebServiceResponseCodeType.GeneralError)
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        private bool uploadUpgraded(Guid deviceId)
        {
            ApplicationMeta applicationMeta = this.iStorageDal.GetApplication(this.applicationId);

            if (applicationMeta.Upgraded)
            {
                WebServiceResponseCodeType responseCode = this.iUploadDal.Upgrade(deviceId, applicationId, applicationMeta.Version);

                if (responseCode == WebServiceResponseCodeType.Ok)
                {
                    applicationMeta.Upgraded = false;
                    this.iStorageDal.Update(applicationMeta);
                    return true;
                }
                else if (responseCode == WebServiceResponseCodeType.InactiveAccount
                  || responseCode == WebServiceResponseCodeType.InactiveApplication)
                {
                    this.authenticationFailure = true;
                    return false;
                }
                else if (responseCode == WebServiceResponseCodeType.GeneralError)
                {
                    return false;
                }
            }

            return true;
        }
        #endregion
    }
}
