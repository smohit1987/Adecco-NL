// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace AdeccoNL.iOS
{
    [Register ("RefineCell")]
    partial class RefineCell
    {
        [Outlet]
        //[GeneratedCode ("iOS Designer", "1.0")]
        //public UIKit.UIImageView imageView { get; set; }

        //  [Outlet]
        //[GeneratedCode ("iOS Designer", "1.0")]
        public UIKit.UILabel label { get; set; }

        [Outlet]
        //[GeneratedCode ("iOS Designer", "1.0")]
        public UIKit.UIButton checkBoxButton { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (checkBoxButton != null) {
                checkBoxButton.Dispose ();
                checkBoxButton = null;
            }

            if (label != null) {
                label.Dispose ();
                label = null;
            }
        }
    }
}