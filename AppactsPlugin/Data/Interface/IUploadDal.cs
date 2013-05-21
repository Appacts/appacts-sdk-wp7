using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using AppactsPlugin.Data.Model;
using AppactsPlugin.Data.Model.Enum;

namespace AppactsPlugin.Data.Interface
{
    internal interface IUploadDal : IDisposable
    {
        /// <summary>
        /// Crashes the specified device id.
        /// </summary>
        /// <param name="deviceId">The device id.</param>
        /// <param name="crash">The crash.</param>
        /// <returns></returns>
        WebServiceResponseCodeType Crash(Guid deviceId, Crash crash);

        /// <summary>
        /// Devices the specified account.
        /// </summary>
        /// <param name="applicationId">The application id.</param>
        /// <param name="model">The model.</param>
        /// <param name="osVersion">The os version.</param>
        /// <param name="deviceType">Type of the device.</param>
        /// <param name="carrier">The carrier.</param>
        /// <param name="applicationVersion">The application version.</param>
        /// <param name="timeZoneOffset">The time zone offset.</param>
        /// <returns></returns>
        Guid Device(Guid applicationId, String model,  String osVersion, int deviceType,
            String carrier, String applicationVersion, int timeZoneOffset, string locale, string resolutionWidth,
            string resolutionHeight, string manufacturer);

        /// <summary>
        /// Errors the specified device id.
        /// </summary>
        /// <param name="deviceId">The device id.</param>
        /// <param name="errorItem">The error item.</param>
        /// <returns></returns>
        WebServiceResponseCodeType Error(Guid deviceId, ErrorItem errorItem);

        /// <summary>
        /// Events the specified device id.
        /// </summary>
        /// <param name="deviceId">The device id.</param>
        /// <param name="eventItem">The event item.</param>
        /// <returns></returns>
        WebServiceResponseCodeType Event(Guid deviceId, EventItem eventItem);

        /// <summary>
        /// Feedbacks the specified device id.
        /// </summary>
        /// <param name="deviceId">The device id.</param>
        /// <param name="feedbackItem">The feedback item.</param>
        /// <returns></returns>
        WebServiceResponseCodeType Feedback(Guid deviceId, FeedbackItem feedbackItem);

        /// <summary>
        /// Systems the error.
        /// </summary>
        /// <param name="deviceId">The device id.</param>
        /// <param name="systemError">The system error.</param>
        /// <returns></returns>
        WebServiceResponseCodeType SystemError(Guid deviceId, SystemError systemError);

        /// <summary>
        /// Users the specified device id.
        /// </summary>
        /// <param name="deviceId">The device id.</param>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        WebServiceResponseCodeType User(Guid deviceId, User user);

        /// <summary>
        /// Locations the specified device id.
        /// </summary>
        /// <param name="deviceId">The device id.</param>
        /// <param name="applicationId">The application id.</param>
        /// <param name="deviceLocation">The device location.</param>
        /// <returns></returns>
        WebServiceResponseCodeType Location(Guid deviceId, Guid applicationId, DeviceLocation deviceLocation);

        /// <summary>
        /// Upgrades the specified device id.
        /// </summary>
        /// <param name="deviceId">The device id.</param>
        /// <param name="applicationId">The application id.</param>
        /// <param name="version">The version.</param>
        /// <returns></returns>
        WebServiceResponseCodeType Upgrade(Guid deviceId, Guid applicationId, String version);
    }
}
