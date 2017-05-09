using System;
using CoreAnimation;
using CoreGraphics;
using Foundation;
using UIKit;


namespace AdeccoNL.iOS
{
	public sealed class BadgeBarButtonItem : UIBarButtonItem
	{
		private UILabel _badge;
		private string _badgeValue;
		private UIColor _badgeBGColor;
		private UIColor _badgeTextColor;
		private UIFont _badgeFont;
		private nfloat _badgePadding;
		private nfloat _badgeMinSize;
		private nfloat _badgeOriginX;
		private nfloat _badgeOriginY;
		private bool _shouldHideBadgeAtZero;
		private bool _shouldAnimateBadge;

		public string BadgeValue
		{
			get
			{
				return _badgeValue;
			}
			set
			{
				_badgeValue = value;
				SetBadgeValue(value);
			}
		}

		public UIColor BadgeBGColor
		{
			get
			{
				return _badgeBGColor;
			}
			set
			{
				_badgeBGColor = value;

				if (_badge != null)
				{
					RefreshBadge();
				}
			}
		}

		public UIColor BadgeTextColor
		{
			get
			{
				return _badgeTextColor;
			}
			set
			{
				_badgeTextColor = value;

				if (_badge != null)
				{
					RefreshBadge();
				}
			}
		}

		public UIFont BadgeFont
		{
			get
			{
				return _badgeFont;
			}
			set
			{
				_badgeFont = value;

				if (_badge != null)
				{
					RefreshBadge();
				}
			}
		}

		public nfloat BadgePadding
		{
			get
			{
				return _badgePadding;
			}
			set
			{
				_badgePadding = value;

				if (_badge != null)
				{
					UpdateBadgeFrame();
				}
			}
		}

		public nfloat BadgeMinSize
		{
			get
			{
				return _badgeMinSize;
			}
			set
			{
				_badgeMinSize = value;

				if (_badge != null)
				{
					UpdateBadgeFrame();
				}
			}
		}

		public nfloat BadgeOriginX
		{
			get
			{
				return _badgeOriginX;
			}
			set
			{
				_badgeOriginX = value;

				if (_badge != null)
				{
					UpdateBadgeFrame();
				}
			}
		}

		public nfloat BadgeOriginY
		{
			get
			{
				return _badgeOriginY;
			}
			set
			{
				_badgeOriginY = value;

				if (_badge != null)
				{
					UpdateBadgeFrame();
				}
			}
		}

		public bool ShouldHideBadgeAtZero
		{
			get
			{
				return _shouldHideBadgeAtZero;
			}
			set
			{
				_shouldHideBadgeAtZero = value;
				if (_badge != null)
				{
					UpdateBadgeFrame();
				}
			}
		}

		public bool ShouldAnimateBadge
		{
			get
			{
				return _shouldAnimateBadge;
			}
			set
			{
				_shouldAnimateBadge = value;

				if (_badge != null)
				{
					UpdateBadgeFrame();
				}
			}
		}

		public BadgeBarButtonItem(UIButton customButton)
		{
			CustomView = customButton;
			if (CustomView != null)
			{
				Initializer();
			}
		}

		private static UILabel DuplicateLabel(UILabel labelToCopy)
		{
			var duplicateLabel = new UILabel(labelToCopy.Frame)
			{
				Text = labelToCopy.Text,
				Font = labelToCopy.Font
			};

			return duplicateLabel;
		}

		private void Initializer()
		{
			BadgeBGColor = UIColor.Red;
			BadgeTextColor = UIColor.White;
			BadgeFont = UIFont.SystemFontOfSize(14);
			BadgePadding = 6;
			BadgeMinSize = 8;
			BadgeOriginX = 7;
			BadgeOriginY = -9;
			ShouldHideBadgeAtZero = true;
			ShouldAnimateBadge = true;
			CustomView.ClipsToBounds = false;
		}

		private void RefreshBadge()
		{
			_badge.TextColor = BadgeTextColor;
			_badge.BackgroundColor = BadgeBGColor;
			_badge.Font = BadgeFont;
		}

		private void UpdateBadgeFrame()
		{
			var frameLabel = DuplicateLabel(_badge);

			frameLabel.SizeToFit();

			var expectedLabelSize = frameLabel.Frame.Size;

			var minHeight = expectedLabelSize.Height;

			minHeight = (minHeight < BadgeMinSize) ? BadgeMinSize : expectedLabelSize.Height;
			var minWidth = expectedLabelSize.Width;
			var padding = BadgePadding;

			minWidth = (minWidth < minHeight) ? minHeight : expectedLabelSize.Width;
			_badge.Frame = new CGRect(BadgeOriginX, BadgeOriginY, minWidth + padding, minHeight + padding);
			_badge.Layer.CornerRadius = (minHeight + padding) / 2;
			_badge.Layer.MasksToBounds = true;
		}

		private void UpdateBadgeValueAnimated(bool animated)
		{
			if (animated && ShouldAnimateBadge && _badge.Text != BadgeValue)
			{
				var animation = new CABasicAnimation
				{
					KeyPath = @"transform.scale",
					From = NSObject.FromObject(1.5),
					To = NSObject.FromObject(1),
					Duration = 0.2,
					TimingFunction = new CAMediaTimingFunction(0.4f, 1.3f, 1f, 1f)
				};
				_badge.Layer.AddAnimation(animation, @"bounceAnimation");
			}

			_badge.Text = BadgeValue;

			var duration = animated ? 0.2 : 0;
			UIView.Animate(duration, UpdateBadgeFrame);
		}

		private void RemoveBadge()
		{
			if (_badge != null)
			{
				UIView.AnimateNotify(0.15f, 0.0F,
				UIViewAnimationOptions.CurveEaseIn,
				() =>
				{
					_badge.Transform = CGAffineTransform.MakeScale(0.1f, 0.1f);
				},
				completed =>
				{
					_badge.RemoveFromSuperview();
					_badge = null;
				}
				);
			}
		}

		private void SetBadgeValue(string badgeValue)
		{
			_badgeValue = badgeValue;

			if (string.IsNullOrEmpty(badgeValue) || (badgeValue == @"0" && ShouldHideBadgeAtZero))
			{
				RemoveBadge();
			}
			else if (_badge == null)
			{
				_badge = new UILabel(new CGRect(BadgeOriginX, BadgeOriginY, 20, 20))
				{
					TextColor = BadgeTextColor,
					BackgroundColor = BadgeBGColor,
					Font = BadgeFont,
					TextAlignment = UITextAlignment.Center,
				};

				CustomView.AddSubview(_badge);
				UpdateBadgeValueAnimated(false);
			}
			else
			{
				UpdateBadgeValueAnimated(true);
			}
		}

	}
}

