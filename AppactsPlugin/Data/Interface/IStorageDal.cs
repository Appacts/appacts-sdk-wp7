using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AppactsPlugin.Data.Model;
using AppactsPlugin.Data.Model.Enum;

namespace AppactsPlugin.Data.Interface
{
    internal interface IStorageDal
    {
        /// <summary>
        /// Existses this instance.
        /// </summary>
        bool Exists();

        /// <summary>
        /// Creates this instance.
        /// </summary>
        void Create();

        /// <summary>
        /// Deletes this instance.
        /// </summary>
        void Delete();

        /// <summary>
        /// Gets the crash.
        /// </summary>
        /// <param name="applicationId">The application id.</param>
        /// <returns></returns>
        Crash GetCrash(Guid applicationId);

        /// <summary>
        /// Gets the error item.
        /// </summary>
        /// <param name="applicationId">The application id.</param>
        /// <returns></returns>
        ErrorItem GetErrorItem(Guid applicationId);

        /// <summary>
        /// Gets the event item.
        /// </summary>
        /// <param name="applicationId">The application id.</param>
        /// <returns></returns>
        EventItem GetEventItem(Guid applicationId);

        /// <summary>
        /// Gets the feedback item.
        /// </summary>
        /// <param name="applicationId">The application id.</param>
        /// <returns></returns>
        FeedbackItem GetFeedbackItem(Guid applicationId);

        /// <summary>
        /// Gets the system error.
        /// </summary>
        /// <param name="applicationId">The application id.</param>
        /// <returns></returns>
        SystemError GetSystemError(Guid applicationId);

        /// <summary>
        /// Removes the specified event item.
        /// </summary>
        /// <param name="eventItem">The event item.</param>
        void Remove(EventItem eventItem);

        /// <summary>
        /// Removes the specified feedback item.
        /// </summary>
        /// <param name="feedbackItem">The feedback item.</param>
        void Remove(FeedbackItem feedbackItem);

        /// <summary>
        /// Removes the specified error item.
        /// </summary>
        /// <param name="errorItem">The error item.</param>
        void Remove(ErrorItem errorItem);

        /// <summary>
        /// Removes the specified system error.
        /// </summary>
        /// <param name="systemError">The system error.</param>
        void Remove(SystemError systemError);

        /// <summary>
        /// Removes the specified crash.
        /// </summary>
        /// <param name="crash">The crash.</param>
        void Remove(Crash crash);

        /// <summary>
        /// Saves the specified device location.
        /// </summary>
        /// <param name="deviceLocation">The device location.</param>
        void Save(DeviceLocation deviceLocation);

        /// <summary>
        /// Saves the specified event item.
        /// </summary>
        /// <param name="eventItem">The event item.</param>
        void Save(EventItem eventItem);

        /// <summary>
        /// Saves the specified error item.
        /// </summary>
        /// <param name="errorItem">The error item.</param>
        void Save(ErrorItem errorItem);

        /// <summary>
        /// Saves the specified feedback item.
        /// </summary>
        /// <param name="feedbackItem">The feedback item.</param>
        void Save(FeedbackItem feedbackItem);

        /// <summary>
        /// Saves the specified system error.
        /// </summary>
        /// <param name="systemError">The system error.</param>
        void Save(SystemError systemError);

        /// <summary>
        /// Saves the specified crash.
        /// </summary>
        /// <param name="crash">The crash.</param>
        void Save(Crash crash);

        /// <summary>
        /// Gets the application.
        /// </summary>
        /// <param name="applicationId">The application id.</param>
        /// <returns></returns>
        AppactsPlugin.Data.Model.ApplicationMeta GetApplication(Guid applicationId);

        /// <summary>
        /// Saves the specified application.
        /// </summary>
        /// <param name="application">The application.</param>
        void Save(AppactsPlugin.Data.Model.ApplicationMeta application);

        /// <summary>
        /// Updates the specified application.
        /// </summary>
        /// <param name="application">The application.</param>
        void Update(AppactsPlugin.Data.Model.ApplicationMeta application);

        /// <summary>
        /// Gets the device location.
        /// </summary>
        /// <returns></returns>
        AppactsPlugin.Data.Model.DeviceLocation GetDeviceLocation();

        /// <summary>
        /// Gets the device location.
        /// </summary>
        /// <param name="statusType">Type of the status.</param>
        /// <returns></returns>
        AppactsPlugin.Data.Model.DeviceLocation GetDeviceLocation(StatusType statusType);

        /// <summary>
        /// Updates the specified device.
        /// </summary>
        /// <param name="device">The device.</param>
        void Update(AppactsPlugin.Data.Model.DeviceLocation device);

        /// <summary>
        /// Gets the user.
        /// </summary>
        /// <param name="applicationId">The application id.</param>
        /// <returns></returns>
        User GetUser(Guid applicationId);

        /// <summary>
        /// Saves the specified user.
        /// </summary>
        /// <param name="user">The user.</param>
        void Save(User user);

        /// <summary>
        /// Updates the specified user.
        /// </summary>
        /// <param name="user">The user.</param>
        void Update(User user);

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        void Dispose();

        /// <summary>
        /// Gets the schema version.
        /// </summary>
        /// <returns></returns>
        int GetSchemaVersion();

        /// <summary>
        /// Updates the schema.
        /// </summary>
        /// <param name="applicationId">The application id.</param>
        /// <param name="applicationVersion">The application version.</param>
        /// <param name="pluginVersionCurrent">The plugin version current.</param>
        /// <param name="pluginVersionOld">The plugin version old.</param>
        /// <returns></returns>
        bool UpdateSchema(int pluginVersionCurrent, int pluginVersionOld);

        /// <summary>
        /// Updates the schema version.
        /// </summary>
        /// <param name="pluginVersionCurrent">The plugin version current.</param>
        void UpdateSchemaVersion(int pluginVersionCurrent);
    }
}
