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
    [Register ("MenuController")]
    partial class MenuController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableView tblView { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (tblView != null) {
                tblView.Dispose ();
                tblView = null;
            }
        }
    }
}