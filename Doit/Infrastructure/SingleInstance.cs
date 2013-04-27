﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Ipc;
using System.Runtime.Serialization.Formatters;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using Doit.Native;

namespace Doit.Infrastructure 
{
	/// <summary>
	/// This class checks to make sure that only one instance of 
	/// this application is running at a time.
	/// </summary>
	/// <remarks>
	/// Note: this class should be used with some caution, because it does no
	/// security checking. For example, if one instance of an app that uses this class
	/// is running as Administrator, any other instance, even if it is not
	/// running as Administrator, can activate it with command line arguments.
	/// For most apps, this will not be much of an issue.
	/// </remarks>
	public static class SingleInstance<TApplication> where TApplication : Application, ISingleInstanceApp 
	{
		#region Private Fields

		/// <summary>
		/// String delimiter used in channel names.
		/// </summary>
		private const string Delimiter = ":";

		/// <summary>
		/// Suffix to the channel name.
		/// </summary>
		private const string ChannelNameSuffix = "SingeInstanceIPCChannel";

		/// <summary>
		/// Remote service name.
		/// </summary>
		private const string RemoteServiceName = "SingleInstanceApplicationService";

		/// <summary>
		/// IPC protocol used (string).
		/// </summary>
		private const string IpcProtocol = "ipc://";

		/// <summary>
		/// Application mutex.
		/// </summary>
		private static Mutex singleInstanceMutex;

		/// <summary>
		/// IPC channel for communications.
		/// </summary>
		private static IpcServerChannel channel;

		/// <summary>
		/// List of command line arguments for the application.
		/// </summary>
		private static IList<string> commandLineArgs;

		#endregion

		#region Public Properties

		/// <summary>
		/// Gets list of command line arguments for the application.
		/// </summary>
		public static IList<string> CommandLineArgs
		{
			get { return commandLineArgs; }
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Checks if the instance of the application attempting to start is the first instance. 
		/// If not, activates the first instance.
		/// </summary>
		/// <returns>True if this is the first instance of the application.</returns>
		public static bool InitializeAsFirstInstance(string uniqueName)
		{
			commandLineArgs = GetCommandLineArgs(uniqueName);

			// Build unique application Id and the IPC channel name.
			var applicationIdentifier = uniqueName + Environment.UserName;

			var channelName = string.Concat(applicationIdentifier, Delimiter, ChannelNameSuffix);

			// Create mutex based on unique application Id to check if this is the first instance of the application. 
			bool firstInstance;
			singleInstanceMutex = new Mutex(true, applicationIdentifier, out firstInstance);
			if (firstInstance)
			{
				CreateRemoteService(channelName);
			}
			else
			{
				SignalFirstInstance(channelName, commandLineArgs);
			}

			return firstInstance;
		}

		/// <summary>
		/// Cleans up single-instance code, clearing shared resources, mutexes, etc.
		/// </summary>
		public static void Cleanup()
		{
			if (singleInstanceMutex != null)
			{
				singleInstanceMutex.Close();
				singleInstanceMutex = null;
			}

			if (channel != null)
			{
				ChannelServices.UnregisterChannel(channel);
				channel = null;
			}
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Gets command line args - for ClickOnce deployed applications, command line args may not be passed directly, they have to be retrieved.
		/// </summary>
		/// <returns>List of command line arg strings.</returns>
		private static IList<string> GetCommandLineArgs(string uniqueApplicationName)
		{
			string[] args = null;
			if (AppDomain.CurrentDomain.ActivationContext == null)
			{
				// The application was not clickonce deployed, get args from standard API's
				args = Environment.GetCommandLineArgs();
			}
			else
			{
				// The application was clickonce deployed
				// Clickonce deployed apps cannot recieve traditional commandline arguments
				// As a workaround commandline arguments can be written to a shared location before 
				// the app is launched and the app can obtain its commandline arguments from the 
				// shared location               
				string appFolderPath = Path.Combine(
					Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), uniqueApplicationName);

				string cmdLinePath = Path.Combine(appFolderPath, "cmdline.txt");
				if (File.Exists(cmdLinePath))
				{
					try
					{
						using (TextReader reader = new StreamReader(cmdLinePath, System.Text.Encoding.Unicode))
						{
							args = NativeMethods.CommandLineToArgvW(reader.ReadToEnd());
						}

						File.Delete(cmdLinePath);
					}
					catch (IOException)
					{
					}
				}
			}

			if (args == null)
			{
				args = new string[] { };
			}

			return new List<string>(args);
		}

		/// <summary>
		/// Creates a remote service for communication.
		/// </summary>
		/// <param name="channelName">Application's IPC channel name.</param>
		private static void CreateRemoteService(string channelName)
		{
			BinaryServerFormatterSinkProvider serverProvider = new BinaryServerFormatterSinkProvider();
			serverProvider.TypeFilterLevel = TypeFilterLevel.Full;
			IDictionary props = new Dictionary<string, string>();

			props["name"] = channelName;
			props["portName"] = channelName;
			props["exclusiveAddressUse"] = "false";

			// Create the IPC Server channel with the channel properties
			channel = new IpcServerChannel(props, serverProvider);

			// Register the channel with the channel services
			ChannelServices.RegisterChannel(channel, true);

			// Expose the remote service with the REMOTE_SERVICE_NAME
			IpcRemoteService remoteService = new IpcRemoteService();
			RemotingServices.Marshal(remoteService, RemoteServiceName);
		}

		/// <summary>
		/// Creates a client channel and obtains a reference to the remoting service exposed by the server - 
		/// in this case, the remoting service exposed by the first instance. Calls a function of the remoting service 
		/// class to pass on command line arguments from the second instance to the first and cause it to activate itself.
		/// </summary>
		/// <param name="channelName">Application's IPC channel name.</param>
		/// <param name="args">
		/// Command line arguments for the second instance, passed to the first instance to take appropriate action.
		/// </param>
		private static void SignalFirstInstance(string channelName, IList<string> args)
		{
			IpcClientChannel secondInstanceChannel = new IpcClientChannel();
			ChannelServices.RegisterChannel(secondInstanceChannel, true);

			string remotingServiceUrl = IpcProtocol + channelName + "/" + RemoteServiceName;

			// Obtain a reference to the remoting service exposed by the server i.e the first instance of the application
			IpcRemoteService firstInstanceRemoteServiceReference = (IpcRemoteService)RemotingServices.Connect(typeof(IpcRemoteService), remotingServiceUrl);

			// Check that the remote service exists, in some cases the first instance may not yet have created one, in which case
			// the second instance should just exit
			if (firstInstanceRemoteServiceReference != null)
			{
				// Invoke a method of the remote service exposed by the first instance passing on the command line
				// arguments and causing the first instance to activate itself
				firstInstanceRemoteServiceReference.InvokeFirstInstance(args);
			}
		}

		/// <summary>
		/// Callback for activating first instance of the application.
		/// </summary>
		/// <param name="arg">Callback argument.</param>
		/// <returns>Always null.</returns>
		private static object ActivateFirstInstanceCallback(object arg)
		{
			// Get command line args to be passed to first instance
			IList<string> args = arg as IList<string>;
			ActivateFirstInstance(args);
			return null;
		}

		/// <summary>
		/// Activates the first instance of the application with arguments from a second instance.
		/// </summary>
		/// <param name="args">List of arguments to supply the first instance of the application.</param>
		private static void ActivateFirstInstance(IList<string> args)
		{
			// Set main window state and process command line args
			if (Application.Current == null)
			{
				return;
			}

			((TApplication)Application.Current).SignalExternalCommandLineArgs(args);
		}

		#endregion

		#region Private Classes

		/// <summary>
		/// Remoting service class which is exposed by the server i.e the first instance and called by the second instance
		/// to pass on the command line arguments to the first instance and cause it to activate itself.
		/// </summary>
		private class IpcRemoteService : MarshalByRefObject
		{
			/// <summary>
			/// Activates the first instance of the application.
			/// </summary>
			/// <param name="args">List of arguments to pass to the first instance.</param>
			public void InvokeFirstInstance(IList<string> args)
			{
				if (Application.Current != null)
				{
					// Do an asynchronous call to ActivateFirstInstance function
					Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new DispatcherOperationCallback(SingleInstance<TApplication>.ActivateFirstInstanceCallback), args);
				}
			}

			/// <summary>
			/// Remoting Object's ease expires after every 5 minutes by default. We need to override the InitializeLifetimeService class
			/// to ensure that lease never expires.
			/// </summary>
			/// <returns>Always null.</returns>
			public override object InitializeLifetimeService()
			{
				return null;
			}
		}

		#endregion
	}
}
