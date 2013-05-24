using System;
using System.Threading;
using System.Windows.Media.Imaging;

namespace Doit.Infrastructure
{
	public static class Utils
	{
		 public static BitmapImage GetFreezedImage(string path)
		 {
			 return GetFreezedImage(new Uri("pack://application:,,,/Images/" + path));
		 }

		 public static BitmapImage GetFreezedImage(Uri path)
		 {
			 var bitmapImage = new BitmapImage(path);
			 bitmapImage.Freeze();
			 return bitmapImage;
		 }

		public static void RunOnStaThread(ThreadStart action)
		{
			var thread = new Thread(action);
			thread.SetApartmentState(ApartmentState.STA);
			thread.Start();
			thread.Join();
		}
	}
}