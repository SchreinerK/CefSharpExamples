using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using CefSharp;
using CefSharp.Wpf;

namespace СefSharpRemoteDebugging.Wpf.Net5 {

	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window {

		public MainWindow() {
			var settings = new CefSettings {
				RemoteDebuggingPort = 8080
			};
			Cef.Initialize(settings);

			InitializeComponent();
			DataContext = this;

			LeftBrowser.FrameLoadEnd += LeftBrowser_FrameLoadEnd;
			LeftBrowser.LoadError += LeftBrowser_LoadError;

			// == NOT WORKING, only blank page displayed ==
			//LeftBrowser.Address = "github.com";
			// Loaded += (s,e) => LeftBrowser.Address = "github.com";
			// Dispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle,
			// 	new Action(() => LeftBrowser.Address = "github.com"));
			// Loaded += (s,e) => Dispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle,
			// 	new Action(() => LeftBrowser.Address = "github.com"));
			// == END NOT WORKING ==
			
			// == WORKS but waiting is BAD ==
			new DispatcherTimer(TimeSpan.FromSeconds(3), DispatcherPriority.Normal, (s, e) => {
				((DispatcherTimer)s)?.Stop();
				LeftBrowser.Address = "github.com";
			}, Dispatcher).Start();
		}

		private void Dispatch(Action action) {
			Dispatcher.BeginInvoke(action);
		}

		private void LeftBrowser_FrameLoadEnd(object sender, FrameLoadEndEventArgs e) {
			Dispatcher.BeginInvoke(new Action(() => {
				RightBrowser.Address = "localhost:8080";
			}));
		}

		private void LeftBrowser_LoadError(object sender, LoadErrorEventArgs e) {
			LeftBrowser.LoadHtml($@"<html>{e.ErrorText}</html>",e.FailedUrl, Encoding.UTF8, true);
		}

		private void Address_KeyUp(object sender, KeyEventArgs e) {
			if (e.Key == Key.Enter) LeftBrowser.Address = ((TextBox)sender).Text;
		}
	}
 }
