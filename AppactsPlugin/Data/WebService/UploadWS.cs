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

using AppactsPlugin.Data.Interface;
using AppactsPlugin.Data.Model;
using System.Collections.Generic;
using AppactsPlugin.Data.Model.Enum;
using System.Text;
using System.Threading;

using AppactsPlugin.External.Xml;

namespace AppactsPlugin.Data.WebService
{
    internal class UploadWS  : IUploadDal
    {
        #region //Private Properties
        private readonly string baseUrl;
        private readonly WebClient webClientStandard;
        private readonly WebClient webClientDevice;
        private readonly Object uploadWebClientStandardSyncLock = new Object();
        private readonly Object uploadWebClientDeviceSyncLock = new Object();

        private WebServiceResponse webServiceResponse;
        private WebServiceResponseObject<Guid> webServiceResponseDevice;
        private Exception webServiceException;
        #endregion

        #region //Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="UploadWS"/> class.
        /// </summary>
        /// <param name="baseUrl">The base URL.</param>
        public UploadWS(string baseUrl)
        {
            this.baseUrl = baseUrl;

            this.webClientStandard = new WebClient();
            this.webClientStandard.OpenReadCompleted += this.webClientStandard_OpenReadCompleted;

            this.webClientDevice = new WebClient();
            this.webClientDevice.OpenReadCompleted += this.webClientDevice_OpenReadCompleted;
        }
        #endregion

        #region //Public Properties
        /// <summary>
        /// Devices the specified account.
        /// </summary>
        /// <param name="account">The account.</param>
        /// <param name="model">The model.</param>
        /// <param name="osVersion">The os version.</param>
        /// <param name="deviceType">Type of the device.</param>
        /// <param name="carrier">The carrier.</param>
        /// <param name="date">The date.</param>
        /// <returns></returns>
        public Guid Device(Guid applicationId, string model, string osVersion, int deviceType, 
            string carrier, String applicationVersion, int timeZoneOffset, string locale, string resolutionWidth, 
            string resolutionHeight, string manufacturer)
        {
            Guid deviceGuid = Guid.Empty;
            WebServiceResponseCodeType responseCode = WebServiceResponseCodeType.GeneralError;

            try
            {
                String url = this.baseUrl + WebServiceType.Device;
                url = this.addToUrl(url, QueryStringKeyType.APPLICATION_GUID, applicationId.ToString());
                url = this.addToUrl(url, QueryStringKeyType.MODEL, model);
                url = this.addToUrl(url, QueryStringKeyType.PLATFORM_TYPE, deviceType.ToString());
                url = this.addToUrl(url, QueryStringKeyType.CARRIER, carrier);
                url = this.addToUrl(url, QueryStringKeyType.OPERATING_SYSTEM, osVersion);
                url = this.addToUrl(url, QueryStringKeyType.VERSION, applicationVersion);
                url = this.addToUrl(url, QueryStringKeyType.TIME_ZONE_OFFSET, timeZoneOffset.ToString());
                url = this.addToUrl(url, QueryStringKeyType.LOCALE, locale);
                url = this.addToUrl(url, QueryStringKeyType.RESOLUTION_WIDTH, resolutionWidth);
                url = this.addToUrl(url, QueryStringKeyType.RESOLUTION_HEIGHT, resolutionHeight);
                url = this.addToUrl(url, QueryStringKeyType.MANUFACTURER, manufacturer);

                #if DEBUG
                System.Diagnostics.Debug.WriteLine(url);
                #endif

                lock (this.uploadWebClientDeviceSyncLock)
                {
                    webClientDevice.OpenReadAsync(new Uri(url));
                    Monitor.Wait(this.uploadWebClientDeviceSyncLock);
                }

                if (this.webServiceException == null 
                    && this.webServiceResponseDevice != null)
                {
                    deviceGuid = this.webServiceResponseDevice.Object;
                    responseCode = this.webServiceResponseDevice.ResponseStatus;
                }
                else
                {
                    throw this.webServiceException;
                }
            }
            catch (Exception ex)
            {
                throw new ExceptionWebServiceLayer(ex);
            }

            return deviceGuid;  
        }

        /// <summary>
        /// Crashes the specified device id.
        /// </summary>
        /// <param name="deviceId">The device id.</param>
        /// <param name="crash">The crash.</param>
        /// <returns></returns>
        public WebServiceResponseCodeType Crash(Guid deviceId, Crash crash)
        {
            KeyValuePair<string, string>[] keyValuePairs = new KeyValuePair<string, string>[] {
                new KeyValuePair<string, string>(QueryStringKeyType.DEVICE_GUID, deviceId.ToString()),
                new KeyValuePair<string, string>(QueryStringKeyType.APPLICATION_GUID, crash.ApplicationId.ToString()),
                new KeyValuePair<string, string>(QueryStringKeyType.DATE_CREATED, crash.DateCreatedFormatted),
                new KeyValuePair<string, string>(QueryStringKeyType.SESSION_ID, crash.SessionId.ToString()),
                new KeyValuePair<string, string>(QueryStringKeyType.VERSION, crash.Version)
            };

            return this.webServiceCall(this.baseUrl, WebServiceType.Crash, keyValuePairs);
        }

        /// <summary>
        /// Errors the specified device id.
        /// </summary>
        /// <param name="deviceId">The device id.</param>
        /// <param name="errorItem">The error item.</param>
        /// <returns></returns>
        public WebServiceResponseCodeType Error(Guid deviceId, ErrorItem errorItem)
        {
            KeyValuePair<string, string>[] keyValuePairs = new KeyValuePair<string, string>[] {
                new KeyValuePair<string, string>(QueryStringKeyType.DEVICE_GUID, deviceId.ToString()),
                new KeyValuePair<string, string>(QueryStringKeyType.APPLICATION_GUID, errorItem.ApplicationId.ToString()),
                new KeyValuePair<string, string>(QueryStringKeyType.DATE_CREATED, errorItem.DateCreatedFormatted),
                new KeyValuePair<string, string>(QueryStringKeyType.DATA, errorItem.Data),
                new KeyValuePair<string, string>(QueryStringKeyType.EVENT_NAME, errorItem.EventName),
                new KeyValuePair<string, string>(QueryStringKeyType.AVAILABLE_FLASH_DRIVE_SIZE, errorItem.AvailableFlashDriveSize.ToString()),
                new KeyValuePair<string, string>(QueryStringKeyType.AVAILABLE_MEMORY_SIZE, errorItem.AvailableMemorySize.ToString()),
                new KeyValuePair<string, string>(QueryStringKeyType.BATTERY, errorItem.Battery.ToString()),
                new KeyValuePair<string, string>(QueryStringKeyType.ERROR_MESSAGE, errorItem.ExceptionMessage),
                new KeyValuePair<string, string>(QueryStringKeyType.SCREEN_NAME, errorItem.ScreenName),
                new KeyValuePair<string, string>(QueryStringKeyType.SESSION_ID, errorItem.SessionId.ToString()),
                new KeyValuePair<string, string>(QueryStringKeyType.VERSION, errorItem.Version)
            };

            return this.webServiceCall(this.baseUrl, WebServiceType.Error, keyValuePairs);
        }

        /// <summary>
        /// Events the specified device id.
        /// </summary>
        /// <param name="deviceId">The device id.</param>
        /// <param name="eventItem">The event item.</param>
        /// <returns></returns>
        public WebServiceResponseCodeType Event(Guid deviceId, EventItem eventItem)
        {
            KeyValuePair<string, string>[] keyValuePairs = new KeyValuePair<string, string>[] {
                new KeyValuePair<string, string>(QueryStringKeyType.DEVICE_GUID, deviceId.ToString()),
                new KeyValuePair<string, string>(QueryStringKeyType.APPLICATION_GUID, eventItem.ApplicationId.ToString()),
                new KeyValuePair<string, string>(QueryStringKeyType.DATE_CREATED, eventItem.DateCreatedFormatted),
                new KeyValuePair<string, string>(QueryStringKeyType.DATA, eventItem.Data),
                new KeyValuePair<string, string>(QueryStringKeyType.EVENT_TYPE, ((int)eventItem.EventType).ToString()),
                new KeyValuePair<string, string>(QueryStringKeyType.EVENT_NAME, eventItem.EventName),
                new KeyValuePair<string, string>(QueryStringKeyType.LENGTH, eventItem.Length.ToString()),
                new KeyValuePair<string, string>(QueryStringKeyType.SCREEN_NAME, eventItem.ScreenName),
                new KeyValuePair<string, string>(QueryStringKeyType.SESSION_ID, eventItem.SessionId.ToString()),
                new KeyValuePair<string, string>(QueryStringKeyType.VERSION, eventItem.Version)
            };

            return this.webServiceCall(this.baseUrl, WebServiceType.Event, keyValuePairs);
        }

        /// <summary>
        /// Feedbacks the specified device id.
        /// </summary>
        /// <param name="deviceId">The device id.</param>
        /// <param name="feedbackItem">The feedback item.</param>
        /// <returns></returns>
        public WebServiceResponseCodeType Feedback(Guid deviceId, FeedbackItem feedbackItem)
        {
            KeyValuePair<string, string>[] keyValuePairs = new KeyValuePair<string, string>[] {
                new KeyValuePair<string, string>(QueryStringKeyType.DEVICE_GUID, deviceId.ToString()),
                new KeyValuePair<string, string>(QueryStringKeyType.APPLICATION_GUID, feedbackItem.ApplicationId.ToString()),
                new KeyValuePair<string, string>(QueryStringKeyType.DATE_CREATED, feedbackItem.DateCreatedFormatted),
                new KeyValuePair<string, string>(QueryStringKeyType.VERSION, feedbackItem.Version),
                new KeyValuePair<string, string>(QueryStringKeyType.SCREEN_NAME, feedbackItem.ScreenName),
                new KeyValuePair<string, string>(QueryStringKeyType.FEEDBACK, feedbackItem.Message),
                new KeyValuePair<string, string>(QueryStringKeyType.FEEDBACK_RATING_TYPE, ((int)feedbackItem.Rating).ToString()),
                new KeyValuePair<string, string>(QueryStringKeyType.SESSION_ID, feedbackItem.SessionId.ToString())
            };

            return this.webServiceCall(this.baseUrl, WebServiceType.Feedback, keyValuePairs); 
        }

        /// <summary>
        /// Systems the error.
        /// </summary>
        /// <param name="deviceId">The device id.</param>
        /// <param name="systemError">The system error.</param>
        /// <returns></returns>
        public WebServiceResponseCodeType SystemError(Guid deviceId, SystemError systemError)
        {
            KeyValuePair<string, string>[] keyValuePairs = new KeyValuePair<string, string>[] {
                new KeyValuePair<string, string>(QueryStringKeyType.DEVICE_GUID, deviceId.ToString()),
                new KeyValuePair<string, string>(QueryStringKeyType.APPLICATION_GUID, systemError.ApplicationId.ToString()),
                new KeyValuePair<string, string>(QueryStringKeyType.DATE_CREATED, systemError.DateCreatedFormatted),
                new KeyValuePair<string, string>(QueryStringKeyType.VERSION, systemError.Version),
                new KeyValuePair<string, string>(QueryStringKeyType.ERROR_MESSAGE, systemError.ExceptionMessage),
                new KeyValuePair<string, string>(QueryStringKeyType.PLATFORM_TYPE, ((int)systemError.DeviceType).ToString()),
                new KeyValuePair<string, string>(QueryStringKeyType.SYSTEM_VERSION, systemError.Version)
            };

            return this.webServiceCall(this.baseUrl, WebServiceType.SystemError, keyValuePairs); 
        }

        /// <summary>
        /// Users the specified device id.
        /// </summary>
        /// <param name="deviceId">The device id.</param>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        public WebServiceResponseCodeType User(Guid deviceId, User user)
        {
            KeyValuePair<string, string>[] keyValuePairs = new KeyValuePair<string, string>[] {
                new KeyValuePair<string, string>(QueryStringKeyType.DEVICE_GUID, deviceId.ToString()),
                new KeyValuePair<string, string>(QueryStringKeyType.APPLICATION_GUID, user.ApplicationId.ToString()),
                new KeyValuePair<string, string>(QueryStringKeyType.DATE_CREATED, user.DateCreatedFormatted),
                new KeyValuePair<string, string>(QueryStringKeyType.VERSION, user.Version),
                new KeyValuePair<string, string>(QueryStringKeyType.AGE, user.Age.ToString()),
                new KeyValuePair<string, string>(QueryStringKeyType.SEX_TYPE, ((int)user.Sex).ToString()),
                new KeyValuePair<string, string>(QueryStringKeyType.SESSION_ID, user.SessionId.ToString())
            };

            return this.webServiceCall(this.baseUrl, WebServiceType.User, keyValuePairs); 
        }

        /// <summary>
        /// Locations the specified device id.
        /// </summary>
        /// <param name="deviceId">The device id.</param>
        /// <param name="applicationId">The application id.</param>
        /// <param name="deviceLocation">The device location.</param>
        /// <returns></returns>
        public WebServiceResponseCodeType Location(Guid deviceId, Guid applicationId, DeviceLocation deviceLocation)
        {
            KeyValuePair<string, string>[] keyValuePairs = new KeyValuePair<string, string>[] {
                new KeyValuePair<string, string>(QueryStringKeyType.LOCATION_LONGITUDE, deviceLocation.Longitude.ToString()),
                new KeyValuePair<string, string>(QueryStringKeyType.LOCATION_LATITUDE, deviceLocation.Latitude.ToString()),
                new KeyValuePair<string, string>(QueryStringKeyType.LOCATION_COUNTRY, deviceLocation.CountryName),
                new KeyValuePair<string, string>(QueryStringKeyType.LOCATION_COUNTRY_CODE, deviceLocation.CountryCode),
                new KeyValuePair<string, string>(QueryStringKeyType.LOCATION_ADMIN, deviceLocation.CountryAdminName),
                new KeyValuePair<string, string>(QueryStringKeyType.LOCATION_ADMIN_CODE, deviceLocation.CountryAdminCode),
                new KeyValuePair<string, string>(QueryStringKeyType.APPLICATION_GUID, applicationId.ToString()),
                new KeyValuePair<string, string>(QueryStringKeyType.DEVICE_GUID, deviceId.ToString())
            };

            return this.webServiceCall(this.baseUrl, WebServiceType.Location, keyValuePairs); 
        }

        /// <summary>
        /// Upgrades the specified device id.
        /// </summary>
        /// <param name="deviceId">The device id.</param>
        /// <param name="applicationId">The application id.</param>
        /// <param name="version">The version.</param>
        /// <returns></returns>
        public WebServiceResponseCodeType Upgrade(Guid deviceId, Guid applicationId, string version)
        {
            KeyValuePair<string, string>[] keyValuePairs = new KeyValuePair<string, string>[] {
                new KeyValuePair<string, string>(QueryStringKeyType.APPLICATION_GUID, applicationId.ToString()),
                new KeyValuePair<string, string>(QueryStringKeyType.DEVICE_GUID, deviceId.ToString()),
                new KeyValuePair<string, string>(QueryStringKeyType.VERSION, version)
            };

            return this.webServiceCall(this.baseUrl, WebServiceType.Upgrade, keyValuePairs); 
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.webClientStandard.OpenReadCompleted -= this.webClientStandard_OpenReadCompleted;
            this.webClientDevice.OpenReadCompleted -= this.webClientDevice_OpenReadCompleted;
        }
        #endregion

        #region //Private Properties
        /// <summary>
        /// Webs the service call.
        /// </summary>
        /// <param name="baseUrl">The base URL.</param>
        /// <param name="serviceUrl">The service URL.</param>
        /// <param name="keyValuePairs">The key value pairs.</param>
        /// <returns></returns>
        private WebServiceResponseCodeType webServiceCall
            (String baseUrl, String serviceUrl, KeyValuePair<string, string>[] keyValuePairs) 
        {
            WebServiceResponseCodeType responseCode = WebServiceResponseCodeType.GeneralError;

            try 
            {
                String url = baseUrl + serviceUrl;

                for (int i = 0; i < keyValuePairs.Length; i++)
                {
                    url = this.addToUrl(url, keyValuePairs[i].Key, this.getValueNotNull(keyValuePairs[i].Value));
                }

                #if DEBUG
                System.Diagnostics.Debug.WriteLine(url); 
                #endif

                lock (this.uploadWebClientStandardSyncLock)
                {
                    webClientStandard.OpenReadAsync(new Uri(url));
                    Monitor.Wait(this.uploadWebClientStandardSyncLock);
                }

                if (this.webServiceException == null 
                    && this.webServiceResponse != null)
                {
                    responseCode = this.webServiceResponse.ResponseStatus;
                }
                else
                {
                    throw this.webServiceException;
                }
            }
            catch(Exception ex) {
                throw new ExceptionWebServiceLayer(ex);
            }

            return responseCode;  
        }

        /// <summary>
        /// Handles the OpenReadCompleted event of the webClient control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Net.OpenReadCompletedEventArgs"/> instance containing the event data.</param>
        private void webClientStandard_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            try
            {
                if (!e.Cancelled && e.Error == null)
                {
                    this.webServiceResponse = Serialization.Deserialize<WebServiceResponse>(e.Result);

                    #if DEBUG
                    System.Diagnostics.Debug.WriteLine("response code:");
                    System.Diagnostics.Debug.WriteLine(this.webServiceResponse.ResponseCode);
                    #endif
                }
            }
            catch (Exception ex)
            {
                this.webServiceException = ex;
            }
            finally
            {
                e.Result.Dispose();
            }
            
            this.webServiceException = e.Error;

            lock (this.uploadWebClientStandardSyncLock)
            {
                Monitor.Pulse(this.uploadWebClientStandardSyncLock);
            }
        }

        /// <summary>
        /// Handles the OpenReadCompleted event of the webClient control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Net.OpenReadCompletedEventArgs"/> instance containing the event data.</param>
        private void webClientDevice_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            try
            {
                if (!e.Cancelled && e.Error == null)
                {
                    this.webServiceResponseDevice = Serialization.Deserialize<WebServiceResponseObject<Guid>>(e.Result);

                    #if DEBUG
                    System.Diagnostics.Debug.WriteLine("device guid:");
                    System.Diagnostics.Debug.WriteLine(this.webServiceResponseDevice.Object);

                    System.Diagnostics.Debug.WriteLine("response code:");
                    System.Diagnostics.Debug.WriteLine(this.webServiceResponseDevice.ResponseCode);
                    #endif
                }
            }
            catch (Exception ex)
            {
                this.webServiceException = ex;
            }
            finally
            {
                e.Result.Dispose();
            }

            this.webServiceException = e.Error;

            lock (this.uploadWebClientDeviceSyncLock)
            {
                Monitor.Pulse(this.uploadWebClientDeviceSyncLock);
            }
        }

        //TODO: ut8 encoder
        /// <summary>
        /// Adds to URL.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        private String addToUrl(String url, String key, String value)
        {
            if (url.IndexOf("?") > 0)
            {
                return url + "&" + key + "=" + HttpUtility.UrlEncode(value);
            }
            return url + "?" + key + "=" + HttpUtility.UrlEncode(value);
        }

        /// <summary>
        /// Gets the value not null.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        private String getValueNotNull(String value)
        {
            if (value != null)
            {
                return value;
            }
            return string.Empty;
        }
        #endregion
    }
}
