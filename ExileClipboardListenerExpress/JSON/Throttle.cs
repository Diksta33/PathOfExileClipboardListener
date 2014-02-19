using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Threading;

namespace ExileClipboardListener.JSON
{
    public delegate void ThottledEventHandler(object sender, ThottledEventArgs e);
    public class ThottledEventArgs : EventArgs
    {
        public TimeSpan WaitTime { get; private set; }
        public ThottledEventArgs(TimeSpan waitTime)
        {
            WaitTime = waitTime;
        }
    }
    public sealed class RequestThrottle
    {
        private readonly Queue<DateTime> _requestTimes = new Queue<DateTime>();

        public event ThottledEventHandler Throttled;

        private RequestThrottle()
        {
            ThrottleWindowTime = new TimeSpan(0, 0, 1, 0);
            ThrottleWindowCount = 15;
            MaxPendingRequests = 15;
        }

        public static RequestThrottle Instance
        {
            get { return Nested.Instance; }
        }

        /// <summary>
        ///   The maximum number of allowed pending request.
        /// 
        ///   The throttle window will keep us in compliance with the 
        ///   letter of the law, but testing has shown that a large 
        ///   number of outstanding requests result in a cascade of 
        ///   (500) errors that does not stop. 
        /// 
        ///   So we will block while there are > MaxPendingRequests 
        ///   regardless of throttle window.
        /// 
        ///   Defaults to 15 which has proven to be reliable.
        /// </summary>
        public int MaxPendingRequests { get; set; }

        /// <summary>
        ///   If you are interested in monitoring
        /// </summary>
        public int OutstandingRequests { get; private set; }

        /// <summary>
        ///   The quantitive portion (xxx) of the of 30 requests per 5 seconds
        ///   Defaults to published guidelines of 5 seconds
        /// </summary>
        public int ThrottleWindowCount { get; set; }

        /// <summary>
        ///   The temporal portion (yyy) of the of 30 requests per 5 seconds
        ///   Defaults to the published guidelines of 30
        /// </summary>
        public TimeSpan ThrottleWindowTime { get; set; }


        /// <summary>
        ///   This decrements the outstanding request count.
        /// 
        ///   This MUST MUST MUST be called when a request has 
        ///   completed regardless of status.
        /// 
        ///   If a request fails, it may be wise to delay calling 
        ///   this, e.g. cool down, for a few seconds, before 
        ///   reissuing the request.
        /// </summary>
        public void Complete()
        {
            OutstandingRequests--;
        }

        /// <summary>
        ///   Create a WebRequest. This method will block if too many
        ///   outstanding requests are pending or the throttle window
        ///   threshold has been reached.
        /// </summary>
        /// <param name = "uri"></param>
        /// <returns></returns>
        public WebRequest Create(Uri uri)
        {
            lock (typeof(ThrottleLock))
            {
                // note: we could use a list of WeakReferences and 
                // may do so at a later date, but for now, this
                // works just fine as long as you call .Complete
                OutstandingRequests++;

                while (OutstandingRequests > MaxPendingRequests)
                {
                    using (var throttleGate = new AutoResetEvent(false))
                    {
                        Debug.WriteLine("Max number requests reached, waiting");
                        throttleGate.WaitOne(100);
                    }
                }

                if (_requestTimes.Count == ThrottleWindowCount)
                {
                    // pull the earliest request of the bottom
                    DateTime tail = _requestTimes.Dequeue();
                    // calculate the interval between now (head) and tail
                    // to determine if we need to chill out for a few millisecons

                    TimeSpan waitTime = (ThrottleWindowTime - (DateTime.Now - tail));

                    if (waitTime.TotalMilliseconds > 0)
                    {
                        Trace.WriteLine("waiting:\t" + waitTime + "\t" + uri.AbsoluteUri);
                        using (var throttleGate = new AutoResetEvent(false))
                        {
                            if (Throttled != null)
                                Throttled(this, new ThottledEventArgs(waitTime));

                            Debug.WriteLine("Approaching Threshold, Just Chillin like a Villain on Penicillin " + waitTime.TotalSeconds);
                            throttleGate.WaitOne(waitTime);
                        }
                    }
                }

                // good to go. 
                _requestTimes.Enqueue(DateTime.Now);
                return WebRequest.Create(uri);
            }
        }


        public WebRequest Create(string url)
        {
            return Create(new Uri(url));
        }

        private class ThrottleLock
        {
        }

        internal class Nested
        {
            internal static readonly RequestThrottle Instance = new RequestThrottle();
        }
    }
}
