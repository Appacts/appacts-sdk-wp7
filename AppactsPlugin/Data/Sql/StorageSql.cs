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

using System.Linq;
using System.Data.Linq;
using Microsoft.Phone.Data.Linq;

using AppactsPlugin.Data.Interface;
using AppactsPlugin.Data.Model;
using System.Threading;

namespace AppactsPlugin.Data.Sql
{
    internal class StorageSql : DataContext, IStorageDal
    {
        #region //Tables
        public Table<AppactsPlugin.Data.Model.ApplicationMeta> Application;
        public Table<DeviceLocation> DeviceLocation;
        public Table<Crash> Crash;
        public Table<ErrorItem> ErrorItem;
        public Table<EventItem> EventItem;
        public Table<FeedbackItem> FeedbackItem;
        public Table<SystemError> SystemError;
        public Table<User> User;
        #endregion

        #region //Private Properties
        public static AutoResetEvent threadQueue = new AutoResetEvent(true); 
        #endregion

        #region //Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="StorageSql"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public StorageSql(string connectionString)
            : base(String.Concat("Data Source=isostore:/", connectionString))
        {
            this.Log = Console.Out;
        } 
        #endregion

        #region //Public Properties
        /// <summary>
        /// Deletes this instance.
        /// </summary>
        public void Delete()
        {
            try
            {
                threadQueue.WaitOne();

                base.DeleteDatabase();
            }
            catch (Exception ex)
            {
                throw new ExceptionDatabaseLayer(ex);
            }
            finally
            {
                threadQueue.Set();
            }
        }

        /// <summary>
        /// Existses this instance.
        /// </summary>
        public bool Exists()
        {
            bool exists;

            try
            {
                threadQueue.WaitOne();

                exists = base.DatabaseExists();
            }
            catch (Exception ex)
            {
                throw new ExceptionDatabaseLayer(ex);
            }
            finally
            {
                threadQueue.Set();
            }
            
            return exists;
        }

        /// <summary>
        /// Creates this instance.
        /// </summary>
        public void Create()
        {
            try
            {
                threadQueue.WaitOne();

                base.CreateDatabase();
            }
            catch (Exception ex)
            {
                throw new ExceptionDatabaseLayer(ex);
            }
            finally
            {
                threadQueue.Set();
            }
        }

        /// <summary>
        /// Gets the crash.
        /// </summary>
        /// <param name="applicationId">The application id.</param>
        /// <returns></returns>
        public Crash GetCrash(Guid applicationId)
        {
            Crash crash;

            try
            {
                threadQueue.WaitOne();

                crash = this.Crash.Where(x => x.ApplicationId == applicationId)
                    .FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new ExceptionDatabaseLayer(ex);
            }
            finally
            {
                threadQueue.Set();
            }

            return crash;
        }

        /// <summary>
        /// Gets the error item.
        /// </summary>
        /// <param name="applicationId">The application id.</param>
        /// <returns></returns>
        public ErrorItem GetErrorItem(Guid applicationId)
        {
            ErrorItem errorItem;

            try
            {
                threadQueue.WaitOne();

                errorItem = this.ErrorItem
                    .Where(x => x.ApplicationId == applicationId)
                    .FirstOrDefault();

            }
            catch (Exception ex)
            {
                throw new ExceptionDatabaseLayer(ex);
            }
            finally
            {
                threadQueue.Set();
            }

            return errorItem;
        }

        /// <summary>
        /// Gets the event item.
        /// </summary>
        /// <param name="applicationId">The application id.</param>
        /// <returns></returns>
        public EventItem GetEventItem(Guid applicationId)
        {
            EventItem eventItem;

            try
            {
                threadQueue.WaitOne();

                eventItem = this.EventItem
                    .Where(x => x.ApplicationId == applicationId)
                    .FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new ExceptionDatabaseLayer(ex);
            }
            finally
            {
                threadQueue.Set();
            }

            return eventItem;
        }

        /// <summary>
        /// Gets the feedback item.
        /// </summary>
        /// <param name="applicationId">The application id.</param>
        /// <returns></returns>
        public FeedbackItem GetFeedbackItem(Guid applicationId)
        {
            FeedbackItem feedBackItem;

            try
            {
                threadQueue.WaitOne();

                feedBackItem = this.FeedbackItem
                    .Where(x => x.ApplicationId == applicationId)
                    .FirstOrDefault();

            }
            catch (Exception ex)
            {
                throw new ExceptionDatabaseLayer(ex);
            }
            finally
            {
                threadQueue.Set();
            }

            return feedBackItem;
        }

        /// <summary>
        /// Gets the system error.
        /// </summary>
        /// <param name="applicationId">The application id.</param>
        /// <returns></returns>
        public SystemError GetSystemError(Guid applicationId)
        {
            SystemError systemError;

            try
            {
                threadQueue.WaitOne();

                systemError = this.SystemError
                    .Where(x => x.ApplicationId == applicationId)
                    .FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new ExceptionDatabaseLayer(ex);
            }
            finally
            {
                threadQueue.Set();
            }

            return systemError;
        }

        /// <summary>
        /// Removes the specified event item.
        /// </summary>
        /// <param name="eventItem">The event item.</param>
        public void Remove(EventItem eventItem)
        {
            try
            {
                threadQueue.WaitOne();

                this.EventItem.DeleteOnSubmit(eventItem);
                this.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw new ExceptionDatabaseLayer(ex);
            }
            finally
            {
                threadQueue.Set();
            }
        }

        /// <summary>
        /// Removes the specified feedback item.
        /// </summary>
        /// <param name="feedbackItem">The feedback item.</param>
        public void Remove(FeedbackItem feedbackItem)
        {
            try
            {
                threadQueue.WaitOne();
                this.FeedbackItem.DeleteOnSubmit(feedbackItem);
                this.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw new ExceptionDatabaseLayer(ex);
            }
            finally
            {
                threadQueue.Set();
            }
        }

        /// <summary>
        /// Removes the specified error item.
        /// </summary>
        /// <param name="errorItem">The error item.</param>
        public void Remove(ErrorItem errorItem)
        {
            try
            {
                threadQueue.WaitOne();

                this.ErrorItem.DeleteOnSubmit(errorItem);
                this.SubmitChanges();

            }
            catch (Exception ex)
            {
                throw new ExceptionDatabaseLayer(ex);
            }
            finally
            {
                threadQueue.Set();
            }
        }

        /// <summary>
        /// Removes the specified system error.
        /// </summary>
        /// <param name="systemError">The system error.</param>
        public void Remove(SystemError systemError)
        {
            try
            {
                threadQueue.WaitOne();

                this.SystemError.DeleteOnSubmit(systemError);
                this.SubmitChanges();

            }
            catch (Exception ex)
            {
                throw new ExceptionDatabaseLayer(ex);
            }
            finally
            {
                threadQueue.Set();
            }
        }

        /// <summary>
        /// Removes the specified crash.
        /// </summary>
        /// <param name="crash">The crash.</param>
        public void Remove(Crash crash)
        {
            try
            {
                threadQueue.WaitOne();

                this.Crash.DeleteOnSubmit(crash);
                this.SubmitChanges();

            }
            catch (Exception ex)
            {
                throw new ExceptionDatabaseLayer(ex);
            }
            finally
            {
                threadQueue.Set();
            }
        }

        /// <summary>
        /// Saves the specified device location.
        /// </summary>
        /// <param name="deviceLocation">The device location.</param>
        public void Save(DeviceLocation deviceLocation)
        {
            try
            {
                threadQueue.WaitOne();

                this.DeviceLocation.InsertOnSubmit(deviceLocation);

                this.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw new ExceptionDatabaseLayer(ex);
            }
            finally
            {
                threadQueue.Set();
            }
        }

        /// <summary>
        /// Saves the specified event item.
        /// </summary>
        /// <param name="eventItem">The event item.</param>
        public void Save(EventItem eventItem)
        {
            try
            {
                threadQueue.WaitOne();

                this.EventItem.InsertOnSubmit(eventItem);
                this.SubmitChanges();

            }
            catch (Exception ex)
            {
                throw new ExceptionDatabaseLayer(ex);
            }
            finally
            {
                threadQueue.Set();
            }
        }

        /// <summary>
        /// Saves the specified error item.
        /// </summary>
        /// <param name="errorItem">The error item.</param>
        public void Save(ErrorItem errorItem)
        {
            try
            {
                threadQueue.WaitOne();
                this.ErrorItem.InsertOnSubmit(errorItem);
                this.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw new ExceptionDatabaseLayer(ex);
            }
            finally
            {
                threadQueue.Set();
            }
        }

        /// <summary>
        /// Saves the specified feedback item.
        /// </summary>
        /// <param name="feedbackItem">The feedback item.</param>
        public void Save(FeedbackItem feedbackItem)
        {
            try
            {
                threadQueue.WaitOne();
                this.FeedbackItem.InsertOnSubmit(feedbackItem);
                this.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw new ExceptionDatabaseLayer(ex);
            }
            finally
            {
                threadQueue.Set();
            }
        }

        /// <summary>
        /// Saves the specified system error.
        /// </summary>
        /// <param name="systemError">The system error.</param>
        public void Save(SystemError systemError)
        {
            try
            {
                threadQueue.WaitOne();

                this.SystemError.InsertOnSubmit(systemError);
                this.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw new ExceptionDatabaseLayer(ex);
            }
            finally
            {
                threadQueue.Set();
            }
        }

        /// <summary>
        /// Saves the specified crash.
        /// </summary>
        /// <param name="crash">The crash.</param>
        public void Save(Crash crash)
        {
            try
            {
                threadQueue.WaitOne();

                this.Crash.InsertOnSubmit(crash);
                this.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw new ExceptionDatabaseLayer(ex);
            }
            finally
            {
                threadQueue.Set();
            }
        }

        /// <summary>
        /// Gets the application.
        /// </summary>
        /// <param name="applicationId">The application id.</param>
        /// <returns></returns>
        public Model.ApplicationMeta GetApplication(Guid applicationId)
        {
            Model.ApplicationMeta application;

            try
            {
                threadQueue.WaitOne();

                application = this.Application.Where(x => x.Id == applicationId).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new ExceptionDatabaseLayer(ex);
            }
            finally
            {
                threadQueue.Set();
            }

            return application;
        }

        /// <summary>
        /// Saves the specified application.
        /// </summary>
        /// <param name="application">The application.</param>
        public void Save(Model.ApplicationMeta application)
        {
            try
            {
                threadQueue.WaitOne();

                this.Application.InsertOnSubmit(application);
                this.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw new ExceptionDatabaseLayer(ex);
            }
            finally
            {
                threadQueue.Set();
            }
        }

        /// <summary>
        /// Updates the specified application.
        /// </summary>
        /// <param name="application">The application.</param>
        public void Update(Model.ApplicationMeta application)
        {
            try
            {
                threadQueue.WaitOne();

                this.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw new ExceptionDatabaseLayer(ex);
            }
            finally
            {
                threadQueue.Set();
            }
        }

        /// <summary>
        /// Gets the device location.
        /// </summary>
        /// <returns></returns>
        public DeviceLocation GetDeviceLocation()
        {
            DeviceLocation deviceLocation;

            try
            {
                threadQueue.WaitOne();

                deviceLocation = this.DeviceLocation.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new ExceptionDatabaseLayer(ex);
            }
            finally
            {
                threadQueue.Set();
            }

            return deviceLocation;
        }

        /// <summary>
        /// Gets the device location.
        /// </summary>
        /// <param name="statusType">Type of the status.</param>
        /// <returns></returns>
        public DeviceLocation GetDeviceLocation(Model.Enum.StatusType statusType)
        {
            DeviceLocation deviceLocation;

            try
            {
                threadQueue.WaitOne();

                deviceLocation = this.DeviceLocation
                    .Where(x => x.Status == statusType)
                    .FirstOrDefault();

            }
            catch (Exception ex)
            {
                throw new ExceptionDatabaseLayer(ex);
            }
            finally
            {
                threadQueue.Set();
            }

            return deviceLocation;
        }

        /// <summary>
        /// Updates the specified device.
        /// </summary>
        /// <param name="device">The device.</param>
        public void Update(DeviceLocation device)
        {
            try
            {
                threadQueue.WaitOne();

                this.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw new ExceptionDatabaseLayer(ex);
            }
            finally
            {
                threadQueue.Set();
            }
        }

        /// <summary>
        /// Gets the user.
        /// </summary>
        /// <param name="applicationId">The application id.</param>
        /// <returns></returns>
        public User GetUser(Guid applicationId)
        {
            User user;

            try
            {
                threadQueue.WaitOne();

                user = this.User
                    .Where(x => x.ApplicationId == applicationId)
                    .FirstOrDefault();

            }
            catch (Exception ex)
            {
                throw new ExceptionDatabaseLayer(ex);
            }
            finally
            {
                threadQueue.Set();
            }

            return user;
        }

        /// <summary>
        /// Saves the specified user.
        /// </summary>
        /// <param name="user">The user.</param>
        public void Save(User user)
        {
            try
            {
                threadQueue.WaitOne();

                this.User.InsertOnSubmit(user);

                this.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw new ExceptionDatabaseLayer(ex);
            }
            finally
            {
                threadQueue.Set();
            }
        }

        /// <summary>
        /// Updates the specified user.
        /// </summary>
        /// <param name="user">The user.</param>
        public void Update(User user)
        {
            try
            {
                threadQueue.WaitOne();

                this.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw new ExceptionDatabaseLayer(ex);
            }
            finally
            {
                threadQueue.Set();
            }
        }

        /// <summary>
        /// Gets the schema version.
        /// </summary>
        /// <returns></returns>
        public int GetSchemaVersion()
        {
            return this.CreateDatabaseSchemaUpdater().DatabaseSchemaVersion;
        }

        /// <summary>
        /// Updates the schema.
        /// </summary>
        /// <example>
        /// http://www.windowsphonegeek.com/articles/Windows-Phone-Local-Database-Schema-Upgrade-Part1---Adding-new-columns
        /// </example>
        /// <param name="applicationId">The application id.</param>
        /// <param name="applicationVersion">The application version.</param>
        /// <param name="pluginVersionCurrent">The plugin version current.</param>
        /// <param name="pluginVersionOld">The plugin version old.</param>
        /// <returns></returns>
        public bool UpdateSchema(int pluginVersionCurrent, int pluginVersionOld)
        {
            if (pluginVersionCurrent > pluginVersionOld)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Updates the schema version.
        /// </summary>
        /// <param name="pluginVersionCurrent">The plugin version current.</param>
        public void UpdateSchemaVersion(int pluginVersionCurrent)
        {
            DatabaseSchemaUpdater databaseSchemaUpdater = this.CreateDatabaseSchemaUpdater();
            databaseSchemaUpdater.DatabaseSchemaVersion = pluginVersionCurrent;
            databaseSchemaUpdater.Execute();
        }

        /// <summary>
        /// Releases all resources used by the <see cref="T:System.Data.Linq.DataContext"/>.
        /// </summary>
        public new void Dispose()
        {
            try
            {
                threadQueue.WaitOne();

                base.Dispose();
            }
            catch (Exception ex)
            {
                throw new ExceptionDatabaseLayer(ex);
            }
            finally
            {
                threadQueue.Set();
            }
        }
        #endregion
    }
}
