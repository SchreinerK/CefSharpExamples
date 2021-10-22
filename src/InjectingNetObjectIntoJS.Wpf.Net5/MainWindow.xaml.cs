using System;
using System.IO;
using System.Windows;
using System.Windows.Threading;
using CefSharp;
using CefSharp.Wpf;

namespace IncectingNetObjectIntoJS.Wpf.Net5
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			var settings = new CefSettings{};
			Cef.Initialize(settings);

			InitializeComponent();

			new DispatcherTimer(TimeSpan.FromSeconds(6), DispatcherPriority.Normal,
				(s, e) => {
					((DispatcherTimer)s)?.Stop();
					InitializeBrowser();
				}, Dispatcher).Start();
		}

		private void InitializeBrowser() {

			// NOTE: Obsolete. see https://github.com/cefsharp/CefSharp/issues/2990 for details on migrating your code.
			// Browser.RegisterJsObject("dotNetMessage", new DotNetMessage());
			// CefSharpSettings.WcfEnabled = true; <-- seems obsolete
			// Browser.JavascriptObjectRepository.Settings.LegacyBindingEnabled = true;
			// ^- System.Exception: 'JavascriptBindingSettings.LegacyBindingEnabled can no longer be modified, settings must be changed before the underlying browser has been created.'
			Browser.JavascriptObjectRepository.Register("dotNetMessage", new DotNetMessage(), options: BindingOptions.DefaultBinder);


			if (!Browser.IsBrowserInitialized) {
				Browser.IsBrowserInitializedChanged += (s, e) => {
					if (Browser.IsBrowserInitialized) LoadPage();
				};
			}
			else {
				LoadPage();
			}
		}

		private void LoadPage() {
			Browser.LoadHtml(File.ReadAllText("index.html"));
		}
	}

	public class DotNetMessage {

		public void Show(string message) {
			MessageBox.Show(message, "Message from JavaScript");
		}
	}
}
