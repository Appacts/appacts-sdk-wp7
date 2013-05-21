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

using AppactsPlugin.Data.Model.Enum;
using System.Xml.Serialization;

namespace AppactsPlugin.Data.Model
{
    [XmlRoot("HttpHandlerBase")]
    public class WebServiceResponse
    {
        /// <summary>
        /// Gets or sets the response.
        /// </summary>
        /// <value>
        /// The response.
        /// </value>
        [XmlElement("ResponseCode")]
        public int ResponseCode { get; set; }

        /// <summary>
        /// Gets the response status.
        /// </summary>
        [XmlIgnore]
        public WebServiceResponseCodeType ResponseStatus
        {
            get
            {
                return (WebServiceResponseCodeType)this.ResponseCode;
            }
        }
    }
}
