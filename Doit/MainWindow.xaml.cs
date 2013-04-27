using System;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media.Animation;
using Application = System.Windows.Application;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;

namespace Doit
{
	public partial class MainWindow
	{
		private readonly MainWindowViewModel _mainWindowViewModel;

		private bool _altKeyPressed;

		public MainWindow()
		{
			InitializeComponent();

			_mainWindowViewModel = MainWindowViewModel.Instance;

			_mainWindowViewModel.ChangeVisibilityRequested += (sender, args) => 
			{
				if (args.IsVisible)
				{
					ShowMe();
				}
				else
				{
					HideMe();
				}
			};

			DataContext = _mainWindowViewModel;

			Opacity = 0d;

			Left = (Screen.PrimaryScreen.Bounds.Width - Width) * 0.5;
			Top = (Screen.PrimaryScreen.Bounds.Height - Height) * 0.2;
		}

		public void HideMe()
		{
			_mainWindowViewModel.Query = string.Empty;
			_mainWindowViewModel.Actions.Clear();
			_mainWindowViewModel.AccumulatedActions.Clear();

			var duration = TimeSpan.FromMilliseconds(200d);

			var doubleAnimation = new DoubleAnimation
			{
				Duration = duration,
				To = 0d,
				EasingFunction = new PowerEase { EasingMode = EasingMode.EaseOut }
			};

			Storyboard.SetTargetProperty(doubleAnimation, new PropertyPath(OpacityProperty));

			var objectAnimationUsingKeyFrames = new ObjectAnimationUsingKeyFrames();
			objectAnimationUsingKeyFrames.KeyFrames.Add(new DiscreteObjectKeyFrame(Visibility.Hidden, KeyTime.FromTimeSpan(duration)));

			Storyboard.SetTargetProperty(objectAnimationUsingKeyFrames, new PropertyPath(VisibilityProperty));

			var storyboard = new Storyboard();
			storyboard.Children.Add(doubleAnimation);
			storyboard.Children.Add(objectAnimationUsingKeyFrames);

			storyboard.Completed += (sender, args) => _mainWindowViewModel.Query = string.Empty;
			storyboard.Begin(this);
		}

		public void ShowMe()
		{
			_mainWindowViewModel.UpdateLastActiveWindowHandle();

			var duration = TimeSpan.FromMilliseconds(200d);

			var doubleAnimation = new DoubleAnimation
			{
				Duration = duration,
				To = 1d,
				EasingFunction = new PowerEase { EasingMode = EasingMode.EaseIn }
			};

			Storyboard.SetTargetProperty(doubleAnimation, new PropertyPath(OpacityProperty));

			var objectAnimationUsingKeyFrames = new ObjectAnimationUsingKeyFrames();
			objectAnimationUsingKeyFrames.KeyFrames.Add(new DiscreteObjectKeyFrame(Visibility.Visible, KeyTime.FromTimeSpan(TimeSpan.Zero)));

			Storyboard.SetTargetProperty(objectAnimationUsingKeyFrames, new PropertyPath(VisibilityProperty));

			var storyboard = new Storyboard();
			storyboard.Children.Add(objectAnimationUsingKeyFrames);
			storyboard.Children.Add(doubleAnimation);

			storyboard.Completed += (sender, args) =>
			{
				Activate();
				_mainWindowViewModel.UpdateActions();
				InputBox.Focus();
			};
			storyboard.Begin(this);
		}

		protected override void OnChildDesiredSizeChanged(UIElement child)
		{
			AnimateWindowHeight();
			base.OnChildDesiredSizeChanged(child);
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			if (((App)Application.Current).StartHidden)
			{
				HideMe();
			}
			else
			{
				// Set the window height for first time with animation
				////AnimateWindowHeight();
				ShowMe();
			}
		}

		private void InputBox_OnPreviewKeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Return)
			{
				if (_mainWindowViewModel.SelectedAction == null)
				{
					_mainWindowViewModel.ExecuteActionWhenAvailable = true;
				}
				else
				{
					if ((e.KeyStates & KeyStates.Down) == KeyStates.Down)
					{
						_mainWindowViewModel.ExecuteAction(_mainWindowViewModel.SelectedAction.Action);
					}
				}

				return;
			}

			if (e.Key == Key.Tab)
			{
				e.Handled = true;

				if (_mainWindowViewModel.SelectedAction == null)
				{
					if (string.IsNullOrEmpty(_mainWindowViewModel.Query))
					{
						_mainWindowViewModel.UpdateActions();
					}
				}
				else
				{
					_mainWindowViewModel.AccumulatedActions.Add(_mainWindowViewModel.SelectedAction.Action);
					_mainWindowViewModel.Query = string.Empty;
				}

				return;
			}

			if (e.Key == Key.Back && string.IsNullOrEmpty(_mainWindowViewModel.Query) && _mainWindowViewModel.AccumulatedActions.Count > 0)
			{
				_mainWindowViewModel.AccumulatedActions.RemoveAt(_mainWindowViewModel.AccumulatedActions.Count - 1);

				_mainWindowViewModel.UpdateActions();

				return;
			}

			if (e.Key == Key.Escape)
			{
				HideMe();

				return;
			}

			var itemCount = _mainWindowViewModel.Actions.Count;

			if (itemCount > 0)
			{
				int index;

				if (e.Key == Key.Down)
				{
					index = ActionsListBox.SelectedIndex + 1;
				}
				else if (e.Key == Key.Up)
				{
					index = ActionsListBox.SelectedIndex - 1;
				}
				else
				{
					return;
				}

				if (index < 0)
				{
					index += itemCount;
				}

				ActionsListBox.SelectedIndex = index % itemCount;
				ActionsListBox.ScrollIntoView(_mainWindowViewModel.SelectedAction);
			}
		}

		private void AnimateWindowHeight()
		{
			BeginInit();

			////setting SizeToContent of window to Height get you the exact value of window height required to display completely
			////SizeToContent = SizeToContent.Height;
			////var height = ActualHeight;
			////Debug.WriteLine("ActionsListBox.DesiredSize: " + ActionsListBox.DesiredSize);
			////Debug.WriteLine("height: " + height);
			////SizeToContent = SizeToContent.Manual;

			// Run the animation code at backgroud for smoothness
			Dispatcher.BeginInvoke(new Action(AnimateWindowHeightCore), null);

			EndInit();
		}

		private void AnimateWindowHeightCore()
		{
			var heightAnimation = new DoubleAnimation
			{
				Duration = new Duration(TimeSpan.FromSeconds(0.3)), 
				EasingFunction = new PowerEase { EasingMode = EasingMode.EaseOut },
				From = ActualHeight, 
				To = 62 + (52 * _mainWindowViewModel.Actions.Count), 
				FillBehavior = FillBehavior.HoldEnd,
			};

			BeginAnimation(HeightProperty, heightAnimation);
		}

		private void MainWindow_OnPreviewKeyDown(object sender, KeyEventArgs e)
		{
			if (_altKeyPressed)
			{
				var i = e.SystemKey - Key.D0;

				if (i == 0)
				{
					i = 10;
				}

				if (i > 0 && i <= 10 && i <= _mainWindowViewModel.Actions.Count)
				{
					var actionItem = _mainWindowViewModel.Actions[i - 1];
					_mainWindowViewModel.SelectedAction = actionItem;
					_mainWindowViewModel.ExecuteAction(actionItem.Action);

					e.Handled = true;
				}
			}

			if ((e.SystemKey == Key.LeftAlt || e.SystemKey == Key.RightAlt) && !_altKeyPressed)
			{
				_altKeyPressed = true;
			}
		}

		private void MainWindow_OnPreviewKeyUp(object sender, KeyEventArgs e)
		{
			if (e.SystemKey == Key.LeftAlt || e.SystemKey == Key.RightAlt)
			{
				_altKeyPressed = false;
			}
		}
	}
}