using System;
using UIKit;
using CoreLocation;
using MapKit;
using System.Collections.Generic;


namespace AdeccoNL.iOS
{
	partial class MapViewController : UIViewController
	{
		MyMapDelegate mapDel;
		UISearchController searchController;
		CLLocationManager locationManager = new CLLocationManager();

		public List<Branch> _branchList { get; set; }


		public MapViewController(IntPtr handle) : base(handle)
		{

		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();


			this.NavigationController.NavigationBar.TitleTextAttributes = new UIStringAttributes()
			{
				ForegroundColor = UIColor.White
			};

			this.Title = Translations.Bl_Map;

			locationManager.RequestWhenInUseAuthorization();

			// set map type and show user location
		    map.MapType = MKMapType.Standard;
			//map.ShowsUserLocation = true;
			map.Bounds = View.Bounds;

			// set map center and region
			//const double lat = 52.132633;
			//const double lon = 5.2912659999999505;


			double lat = Convert.ToDouble(Constants.Latitude); //52.132633;
			double lon = Convert.ToDouble(Constants.Longitude); //.2912659999999505;

			// set the map delegate
			mapDel = new MyMapDelegate();
			map.Delegate = mapDel;


			foreach (Branch aBranch in this._branchList)
			{
				lat = Convert.ToDouble(aBranch.Latitude);
				lon = Convert.ToDouble(aBranch.Longitude);

				// add an annotation
				map.AddAnnotation(new MKPointAnnotation
				{
					Title = aBranch.BranchName,
					Coordinate = new CLLocationCoordinate2D(Convert.ToDouble(aBranch.Latitude), Convert.ToDouble(aBranch.Longitude))
	
				});

			}

			var mapCenter = new CLLocationCoordinate2D(lat, lon);
			var mapRegion = MKCoordinateRegion.FromDistance(mapCenter, 200000, 200000);
			map.CenterCoordinate = mapCenter;
			            
			//map.Region = mapRegion;
			//[myMapView setRegion:adjustedRegion animated:YES];

			map.SetRegion(mapRegion, true);

			 //add an overlay
			//var circleOverlay = MKCircle.Circle(mapCenter, 5000);
			//map.AddOverlay(circleOverlay);

		}

		class MyMapDelegate : MKMapViewDelegate
		{
			string pId = "PinAnnotation";

			public override MKAnnotationView GetViewForAnnotation(MKMapView mapView, IMKAnnotation annotation)
			{
				MKAnnotationView anView;

				if (annotation is MKUserLocation)
					return null;

				else {

					// show pin annotation
					anView = (MKPinAnnotationView)mapView.DequeueReusableAnnotation(pId);

					if (anView == null)
						anView = new MKPinAnnotationView(annotation, pId);

					((MKPinAnnotationView)anView).PinColor = MKPinAnnotationColor.Red;
					anView.CanShowCallout = true;
				}

				return anView;
			}

			public override MKOverlayView GetViewForOverlay(MKMapView mapView, IMKOverlay overlay)
			{
				var circleOverlay = overlay as MKCircle;
				var circleView = new MKCircleView(circleOverlay);
				circleView.FillColor = UIColor.Red;
				circleView.Alpha = 0.4f;
				return circleView;
			}
		}
	}
}

