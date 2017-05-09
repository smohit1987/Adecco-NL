// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace AdeccoNL.iOS
{
    [Register ("RecentSearchesCell")]
    partial class RecentSearchesCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel hdKeyword { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel hdLocation { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblKeyword { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblLocation { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (hdKeyword != null) {
                hdKeyword.Dispose ();
                hdKeyword = null;
            }

            if (hdLocation != null) {
                hdLocation.Dispose ();
                hdLocation = null;
            }

            if (lblKeyword != null) {
                lblKeyword.Dispose ();
                lblKeyword = null;
            }

            if (lblLocation != null) {
                lblLocation.Dispose ();
                lblLocation = null;
            }
        }
    }
}