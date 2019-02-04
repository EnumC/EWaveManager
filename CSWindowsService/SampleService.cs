/****************************** Module Header ******************************\
* Module Name:  SampleService.cs
* Project:      EWaveManager
* Copyright (c) Eric Qian.
* 
* 
* This source is all rights reserved.
* See https://enumc.com.
* 
* THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
* EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED 
* WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
\***************************************************************************/

#region Using directives
using System.Diagnostics;
using System.ServiceProcess;
using System.Threading;
#endregion


namespace CSWindowsService
{
    public partial class SampleService : ServiceBase
    {
        public SampleService()
        {
            InitializeComponent();
            this.CanStop = false;
            this.stopping = false;
            this.stoppedEvent = new ManualResetEvent(false);
        }


        /// <summary>
        /// The function is executed when a Start command is sent to the 
        /// service by the SCM or when the operating system starts (for a 
        /// service that starts automatically). It specifies actions to take 
        /// when the service starts. In this code sample, OnStart logs a 
        /// service-start message to the Application log, and queues the main 
        /// service function for execution in a thread pool worker thread.
        /// </summary>
        /// <param name="args">Command line arguments</param>
        /// <remarks>
        /// A service application is designed to be long running. Therefore, 
        /// it usually polls or monitors something in the system. The 
        /// monitoring is set up in the OnStart method. However, OnStart does 
        /// not actually do the monitoring. The OnStart method must return to 
        /// the operating system after the service's operation has begun. It 
        /// must not loop forever or block. To set up a simple monitoring 
        /// mechanism, one general solution is to create a timer in OnStart. 
        /// The timer would then raise events in your code periodically, at 
        /// which time your service could do its monitoring. The other 
        /// solution is to spawn a new thread to perform the main service 
        /// functions, which is demonstrated in this code sample.
        /// </remarks>
        protected override void OnStart(string[] args)
        {
            // Log a service start message to the Application log.
            this.eventLog1.WriteEntry("CSWindowsService in OnStart.");

            // Queue the main service function for execution in a worker thread.
            ThreadPool.QueueUserWorkItem(new WaitCallback(ServiceWorkerThread));
        }


        /// <summary>
        /// The method performs the main function of the service. It runs on 
        /// a thread pool worker thread.
        /// </summary>
        /// <param name="state"></param>
        private void ServiceWorkerThread(object state)
        {
            // Periodically check if the service is stopping.
            while (!this.stopping)
            {

                Thread.Sleep(2000);
                Process[] pname = Process.GetProcessesByName("JFCLI");
                if (pname.Length == 0)
                {
                    // this.eventLog1.WriteEntry("jfcli is not running.");
                    Process.Start("shutdown.exe", "-s -f -t 00");
                }
                else
                {
                    // this.eventLog1.WriteEntry("jfcli is running.");
                }
            }

            // Signal the stopped event.
            this.stoppedEvent.Set();
        }


        /// <summary>
        /// The function is executed when a Stop command is sent to the 
        /// service by SCM. It specifies actions to take when a service stops 
        /// running. In this code sample, OnStop logs a service-stop message 
        /// to the Application log, and waits for the finish of the main 
        /// service function.
        /// </summary>
        protected override void OnStop()
        {
            // Log a service stop message to the Application log.
            this.eventLog1.WriteEntry("CSWindowsService is stopped.");

            // Indicate that the service is stopping and wait for the finish 
            // of the main service function (ServiceWorkerThread).
            this.stopping = true;
            this.stoppedEvent.WaitOne();
        }


        private bool stopping;
        private ManualResetEvent stoppedEvent;
    }
}