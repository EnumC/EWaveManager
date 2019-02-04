/****************************** Module Header ******************************\
* Module Name:  Program.cs
* Project:      EWaveManager
* Copyright (c) Eric Qian.
* 
* The file defines the entry point of the application.
* 
* This source is all rights reserved.
* 
* THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
* EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED 
* WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
\***************************************************************************/

#region Using directives
using System.ServiceProcess;
#endregion


namespace CSWindowsService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            ServiceBase service = new SampleService();
            ServiceBase.Run(service);
        }
    }
}