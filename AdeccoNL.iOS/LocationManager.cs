using System;
using CoreLocation;
using UIKit;

namespace AdeccoNL.iOS
{

	public class LocationManager
	{
		protected CLLocationManager locationManager;
		public event EventHandler<LocationUpdatedEventArgs> LocationUpdated = delegate { };

		public LocationManager()
		{
			this.locationManager = new CLLocationManager();
			this.locationManager.PausesLocationUpdatesAutomatically = true;

			// iOS 8 has additional permissions requirements
			if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
			{
				locationManager.RequestAlwaysAuthorization(); 
			}

			if (UIDevice.CurrentDevice.CheckSystemVersion(9, 0))
			{
				locationManager.AllowsBackgroundLocationUpdates = true;
			}
		}

		public CLLocationManager LocMgr
		{
			get { return this.locationManager; }
		}

		public void StartLocationUpdates()
		{
			if (CLLocationManager.LocationServicesEnabled)
			{
				//set the desired accuracy, in meters
				//this.locationManager.DesiredAccuracy = 1;
				this.locationManager.DesiredAccuracy = CLLocation.AccuracyThreeKilometers;

				this.locationManager.DistanceFilter = 1;

				this.locationManager.LocationsUpdated += (object sender, CLLocationsUpdatedEventArgs e) =>
				{
	  			// fire our custom Location Updated event
	 			 LocationUpdated(this, new LocationUpdatedEventArgs(e.Locations[e.Locations.Length - 1]));
				};
				this.locationManager.StartUpdatingLocation();

				//LocationUpdated += PrintLocation;

			}
		}
		public void StopLocationUpdates()
		{
			this.locationManager.StopUpdatingLocation();

			//if (this.locationManager != null)
			//{
			//	this.locationManager.LocationsUpdated -= LocationManager_LocationsUpdated;
			//	//this.locationManager = null;
			//}
		}

		private static void LocationManager_LocationsUpdated(object sender, CLLocationsUpdatedEventArgs e)
		{
			return;
		}
		//This will keep going in the background and the foreground
		public void PrintLocation(object sender, LocationUpdatedEventArgs e)
		{

			CLLocation location = e.Location;
			Console.WriteLine("Altitude: " + location.Altitude + " meters");
			Console.WriteLine("Longitude: " + location.Coordinate.Longitude);
			Console.WriteLine("Latitude: " + location.Coordinate.Latitude);
			Console.WriteLine("Course: " + location.Course);
			Console.WriteLine("Speed: " + location.Speed);

			//this.locationManager.StopUpdatingLocation();
			// stop location updates until timer calls StartUpdatingLocation
		}
	}

	public class LocationUpdatedEventArgs : EventArgs
	{
		CLLocation location;

		public LocationUpdatedEventArgs(CLLocation location)
		{
			this.location = location;
		}

		public CLLocation Location
		{
			get { return location; }
		}
	}

	}