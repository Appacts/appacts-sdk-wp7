using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AppactsPlugin.Data.Model;
using AppactsPlugin.Data.Model.Enum;
using System.Reflection;

namespace AppactsPlugin.Interface
{
    public interface IAnalytics
    {
        /// <summary>
        /// Starts the specified assembly.
        /// </summary>
        /// <param name="baseUrl">base url i.e. the url of the API http://yoursite.com/api/ </param>
        /// <param name="assembly">The assembly.</param>
        /// <param name="applicationId">The application id.</param>
        /// <param name="uploadType">Type of the upload.</param>
        void Start(string baseUrl, Assembly assembly, string applicationId, UploadType uploadType);

        /// <summary>
        /// Starts the specified assembly.
        /// </summary>
        /// <param name="baseUrl">base url i.e. the url of the API http://yoursite.com/api/ </param>
        /// <param name="assembly">The assembly.</param>
        /// <param name="applicationId">The application id.</param>
        void Start(string baseUrl, Assembly assembly, string applicationId);

        /// <summary>
        /// Log Error from your application
        /// </summary>
        /// <param name="screenName">is a screen name that you want to log error against</param>
        /// <param name="eventName">is a event name that you want to log error against</param>
        /// <param name="data">is data that you want to log error against</param>
        /// <param name="ex">ExceptionDescriptive is an Exception that you want to log error against</param>
        void LogError(String screenName, String eventName, String data, ExceptionDescriptive ex);
    
        /// <summary>
        /// Log Event from your application
        /// </summary>
        /// <param name="screenName">screenName is a screen name that you want to log event against</param>
        /// <param name="eventName">eventName is a event name that you want to log event against</param>
        /// <param name="data">data is data that you want to log event against</param>
        /// <see cref="/>
        void LogEvent(String screenName, String eventName, String data);

        /// <summary>
        ///  Log Event from your application
        /// </summary>
        /// <param name="screenName">is a screen name that you want to log event against</param>
        /// <param name="eventName">is a event name that you want to log event against</param>
        void LogEvent(String screenName, String eventName);
    
        /// <summary>
        /// Log Feedback from your application
        /// </summary>
        /// <param name="screenName">is a screen name that you want to log feedback against</param>
        /// <param name="ratingType">is a rating 1 - 5 see RatingType enum</param>
        /// <param name="comment">is a comment that user made about your screen</param>
        /// <exception cref="ExceptionDatabaseLayer">this will notify you that users feedback was not stored, so your application can handle this situation</exception>
        /// 
        void LogFeedback(String screenName, RatingType ratingType, String comment);
    
        /// <summary>
        /// Screen Open, specify screen that just opened, this helps us to start timer on to measure how long user stays on this screen, soon as users is finished with this screen call ScreenClosed
        /// </summary>
        /// <param name="screenName">Name of the screen.</param>
        void ScreenOpen(String screenName);

        /// <summary>
        ///Screen Closed, specify the screen that has just closed, this helps us calculate time spent on the screen
        /// </summary>
        /// <param name="screenName">Name of the screen.</param>
        void ScreenClosed(String screenName);

        /// <summary>
        /// Content Loading, specify content that has started to load, this helps us to start timer to measure how long content takes to load, soon as content has loaded call ContentLoaded
        /// </summary>
        /// <param name="screenName">Name of the screen.</param>
        /// <param name="contentName">Name of the content.</param>
        void ContentLoading(String screenName, String contentName);

        /// <summary>
        /// Content Loaded, specify the content that has just loaded, this helps us calculate time it took to load
        /// </summary>
        /// <param name="screenName">Name of the screen.</param>
        /// <param name="contentName">Name of the content.</param>
        void ContentLoaded(String screenName, String contentName);
    
        /// <summary>
        /// Set User Information, when you want to log away demographic information use this to log age and sex
        /// </summary>
        /// <param name="age">age that user has entered in your application</param>
        /// <param name="sexType">sexType that user has entered in your application see SexType enum</param>
        /// <exception cref="ExceptionDatabaseLayer">this will notify you that users information was not stored, so your application can handle this situation</exception>
        void SetUserInformation(int age, SexType sexType);

        /// <summary>
        /// Is User Information Set, use this to check whether information was already saved return boolean weather or not we have already asked user for demographic information
        /// </summary>
        /// <returns>
        ///   <c>true</c> if [is user information set]; otherwise, <c>false</c>.
        /// </returns>
        bool IsUserInformationSet();
    
        /// <summary>
        /// Set Opt Status, by OptStatusType.OptOut we will not log any data, by OptStatusType.OptIn we will log all data
        /// </summary>
        /// <param name="optStatusType">optStatusType, by default user is set to OptStatusType.OptIn</param>
        void SetOptStatus(OptStatusType optStatusType);
    
        /// <summary>
        /// Get Opt Status, when nothing was set OptStatusType.None will be required i.e. 0 when you receive OptStatusType.None you will know that this is a new user you will be able to give user an option to OptStatusType.OptOut
        /// </summary>
        /// <returns>OptStatusType what is OptStatusType currently set to</returns>
        OptStatusType GetOptStatus();
    
        /// <summary>
        /// For not heavy applications, i.e. forms, utilities, we recommend you use this methodology this enables automatic upload we check network coverage, usage, and when is the best time to upload data to our severs 
        /// </summary>
        void UploadWhileUsingAsync();
    
        /// <summary>
        /// For heavy applications i.e. games we recommend you use this upload methodology using this you can easily log many events and then when you are ready call this method to log  everything in one go, however there might not be network coverage so data might not be uploaded for  a while
        /// </summary>
        void UploadManual();

        /// <summary>
        /// Stops this instance.
        /// </summary>
        void Stop();
    }
}
