using System;
using System.Windows;
using System.Windows.Threading;
using CefSharp;
using CefSharp.Wpf;

namespace WorkingWithPlugins.Wpf.Net5 {

	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window {

		private ChromiumWebBrowser _browserView;

		public MainWindow() {
			var settings = new CefSettings();
			Cef.Initialize(settings);

			InitializeComponent();

			new DispatcherTimer(TimeSpan.FromSeconds(5), DispatcherPriority.Normal, (s, e) => {
				((DispatcherTimer)s)?.Stop();
				InitializeBrowser();
				DisplayPlugins();				
			}, Dispatcher).Start();
		}

		private void InitializeBrowser() {
			if (_browserView != null) {
				BrowserPanel.Children.Remove(_browserView);
				_browserView.Dispose();
			}

			var contextHandler = new RequestContextHandler {
				DisplayPdf = ViewPdfCheckBox.IsChecked == true
			};
			_browserView = new ChromiumWebBrowser {
				RequestContext = new RequestContext(contextHandler)
			};

			_browserView.Address = "www.pdf995.com/samples/pdf.pdf";
			BrowserPanel.Children.Add(_browserView);
		}

		public async void DisplayPlugins() {
			var plugins = await Cef.GetPlugins();
			PluginsListbox.ItemsSource = plugins;
		}


		private void RefreshButton_Click(object sender, RoutedEventArgs e) {
			InitializeBrowser();
		}
	}

	public class RequestContextHandler : IRequestContextHandler {

		public bool DisplayPdf { get; set; } = true;

		/// <inheritdoc />
		public void OnRequestContextInitialized(IRequestContext requestContext) {
			
		}

		/// <inheritdoc />
		public bool OnBeforePluginLoad(string mimeType, string url, bool isMainFrame, string topOriginUrl, WebPluginInfo pluginInfo,
			ref PluginPolicy pluginPolicy) {

			if (pluginInfo.Name.ToLowerInvariant().Contains("pdf")) {
				pluginPolicy = DisplayPdf ? PluginPolicy.Allow : PluginPolicy.Disable;
				return true;
			}

			return false;
		}

		/// <inheritdoc />
		public IResourceRequestHandler GetResourceRequestHandler(IBrowser browser, IFrame frame, IRequest request, bool isNavigation,
			bool isDownload, string requestInitiator, ref bool disableDefaultHandling) {
			return null;
		}
	}

}
