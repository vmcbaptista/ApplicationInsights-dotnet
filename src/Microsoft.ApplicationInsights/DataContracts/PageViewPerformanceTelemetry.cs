﻿namespace Microsoft.ApplicationInsights.DataContracts
{
    using System;
    using System.Collections.Generic;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.External;

    /// <summary>
    /// Telemetry type used to track page load performance.
    /// </summary>
    public sealed class PageViewPerformanceTelemetry : ITelemetry, ISupportProperties, ISupportSampling
    {
        internal const string TelemetryName = "PageView";

        internal readonly string BaseType = typeof(PageViewPerformanceTelemetry).Name;

        internal readonly PageViewPerfData Data;
        private readonly TelemetryContext context;

        private double? samplingPercentage;

        /// <summary>
        /// Initializes a new instance of the <see cref="PageViewPerformanceTelemetry"/> class.
        /// </summary>
        public PageViewPerformanceTelemetry()
        {
            this.Data = new PageViewPerfData();
            this.context = new TelemetryContext(this.Data.properties);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PageViewPerformanceTelemetry"/> class with the
        /// specified <paramref name="pageName"/>.
        /// </summary>
        /// <exception cref="ArgumentException">The <paramref name="pageName"/> is null or empty string.</exception>
        public PageViewPerformanceTelemetry(string pageName) : this()
        {
            this.Name = pageName;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PageViewPerformanceTelemetry"/> class by cloning an existing instance.
        /// </summary>
        /// <param name="source">Source instance of <see cref="PageViewPerformanceTelemetry"/> to clone from.</param>
        private PageViewPerformanceTelemetry(PageViewPerformanceTelemetry source)
        {
            this.Data = source.Data.DeepClone();
            this.context = source.context.DeepClone(this.Data.properties);
        }

        /// <summary>
        /// Gets or sets date and time when event was recorded.
        /// </summary>
        public DateTimeOffset Timestamp { get; set; }

        /// <summary>
        /// Gets or sets the value that defines absolute order of the telemetry item.
        /// </summary>
        public string Sequence { get; set; }

        /// <summary>
        /// Gets the context associated with the current telemetry item.
        /// </summary>
        public TelemetryContext Context
        {
            get { return this.context; }
        }

        /// <summary>
        /// Gets or sets the name of the page.
        /// </summary>
        public string Name
        {
            get { return this.Data.name; }
            set { this.Data.name = value; }
        }

        /// <summary>
        /// Gets or sets the page view Uri.
        /// </summary>
        public Uri Url
        {
            get
            {
                if (this.Data.url.IsNullOrWhiteSpace())
                {
                    return null;
                }

                return new Uri(this.Data.url, UriKind.RelativeOrAbsolute);
            }

            set
            {
                if (value == null)
                {
                    this.Data.url = null;
                }
                else
                {
                    this.Data.url = value.ToString();
                }
            }
        }

        /// <summary>
        /// Gets or sets the page view duration.
        /// </summary>
        public TimeSpan Duration
        {
            get { return Utils.ValidateDuration(this.Data.duration); }
            set { this.Data.duration = value.ToString(); }
        }

        /// <summary>
        /// Gets or sets the page DOM processing time.
        /// </summary>
        public TimeSpan DomProcessing
        {
            get { return Utils.ValidateDuration(this.Data.domProcessing); }
            set { this.Data.domProcessing = value.ToString(); }
        }

        /// <summary>
        /// Gets or sets the page loading total time.
        /// </summary>
        public TimeSpan PerfTotal
        {
            get { return Utils.ValidateDuration(this.Data.perfTotal); }
            set { this.Data.perfTotal = value.ToString(); }
        }

        /// <summary>
        /// Gets or sets the page load network time.
        /// </summary>
        public TimeSpan NetworkConnect
        {
            get { return Utils.ValidateDuration(this.Data.networkConnect); }
            set { this.Data.networkConnect = value.ToString(); }
        }

        /// <summary>
        /// Gets or sets the page load send request time.
        /// </summary>
        public TimeSpan SentRequest
        {
            get { return Utils.ValidateDuration(this.Data.sentRequest); }
            set { this.Data.sentRequest = value.ToString(); }
        }

        /// <summary>
        /// Gets or sets the page load recieve response duration.
        /// </summary>
        public TimeSpan ReceivedResponse
        {
            get { return Utils.ValidateDuration(this.Data.receivedResponse); }
            set { this.Data.receivedResponse = value.ToString(); }
        }

        /// <summary>
        /// Gets a dictionary of custom defined metrics.
        /// <a href="https://go.microsoft.com/fwlink/?linkid=525722#properties">Learn more</a>
        /// </summary>
        public IDictionary<string, double> Metrics
        {
            get { return this.Data.measurements; }
        }

        /// <summary>
        /// Gets a dictionary of application-defined property names and values providing additional information about this page view.
        /// <a href="https://go.microsoft.com/fwlink/?linkid=525722#properties">Learn more</a>
        /// </summary>
        public IDictionary<string, string> Properties
        {
            get { return this.Data.properties; }
        }

        /// <summary>
        /// Gets or sets data sampling percentage (between 0 and 100).
        /// Should be 100/n where n is an integer. <a href="https://go.microsoft.com/fwlink/?linkid=832969">Learn more</a>
        /// </summary>
        double? ISupportSampling.SamplingPercentage
        {
            get { return this.samplingPercentage; }
            set { this.samplingPercentage = value; }
        }

        /// <summary>
        /// Deeply clones a <see cref="PageViewTelemetry"/> object.
        /// </summary>
        /// <returns>A cloned instance.</returns>
        public ITelemetry DeepClone()
        {
            return new PageViewPerformanceTelemetry(this);
        }

        /// <summary>
        /// Sanitizes the properties based on constraints.
        /// </summary>
        void ITelemetry.Sanitize()
        {
            this.Name = this.Name.SanitizeName();
            this.Name = Utils.PopulateRequiredStringValue(this.Name, "name", typeof(PageViewTelemetry).FullName);
            this.Properties.SanitizeProperties();
            this.Metrics.SanitizeMeasurements();
            this.Url = this.Url.SanitizeUri();
        }
    }
}
