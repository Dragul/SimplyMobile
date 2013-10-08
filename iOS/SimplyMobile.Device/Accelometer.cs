using System;
using MonoTouch.UIKit;
using SimplyMobile.Core;

namespace SimplyMobile.Device
{
	public static partial class Accelometer
	{
		public static double UpdateInterval
		{
			get { return UIAccelerometer.SharedAccelerometer.UpdateInterval; }
			set { UIAccelerometer.SharedAccelerometer.UpdateInterval = value; }
		}

		static partial void StartMonitoring()
		{
			UIAccelerometer.SharedAccelerometer.Acceleration += HandleAcceleration;
		}

		static void HandleAcceleration (object sender, UIAccelerometerEventArgs e)
		{
			readingAvailable.Invoke (sender, new AccelometerStatus(e.Acceleration.X, e.Acceleration.Y, e.Acceleration.Z));
		}

		static partial void StopMonitoring()
		{
			UIAccelerometer.SharedAccelerometer.Acceleration -= HandleAcceleration;
		}
	}
}

