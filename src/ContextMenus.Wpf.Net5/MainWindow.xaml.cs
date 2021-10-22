using System;
using System.Windows;
using System.Windows.Threading;
using CefSharp;
using CefSharp.Wpf;

namespace ContextMenus.Wpf.Net5 {

	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window {
		public MainWindow() {
			var settings = new CefSettings();
			Cef.Initialize(settings);

			InitializeComponent();

			new DispatcherTimer(TimeSpan.FromSeconds(5), DispatcherPriority.Normal,
				(s, e) => {
					((DispatcherTimer)s)?.Stop();
					Browser.MenuHandler = new SearchContextMenuHandler();
					Browser.Address = "github.com";
				}, Dispatcher).Start();
		}
	}

	public class SearchContextMenuHandler : IContextMenuHandler {

		/// <inheritdoc />
		public void OnBeforeContextMenu(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame,
			IContextMenuParams parameters,
			IMenuModel model) {
			model.Clear();
			model.AddItem(CefMenuCommand.Find, "Search in Biing");
			model.SetEnabledAt(0, !string.IsNullOrWhiteSpace(parameters.SelectionText));
			model.AddItem(CefMenuCommand.CustomFirst+1, "Search in Google");
			model.SetEnabledAt(1, !string.IsNullOrWhiteSpace(parameters.SelectionText));
		}

		/// <inheritdoc />
		public bool OnContextMenuCommand(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame,
			IContextMenuParams parameters,
			CefMenuCommand commandId, CefEventFlags eventFlags) {
			switch (commandId) {
				case CefMenuCommand.Find: {
					var searchAddress = "https://www.bing.com/search?q=" + parameters.SelectionText;
					frame.ExecuteJavaScriptAsync($"window.open('{searchAddress}', '_blank')");
					return true;
				}
				case CefMenuCommand.CustomFirst + 1: {
					var searchAddress = "https://www.google.com/search?q=" + parameters.SelectionText;
					frame.ExecuteJavaScriptAsync($"window.open('{searchAddress}', '_blank')");
					return true;
				}
				default:
					return false;
			}
		}

		/// <inheritdoc />
		public void OnContextMenuDismissed(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame) {

		}

		/// <inheritdoc />
		public bool RunContextMenu(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame,
			IContextMenuParams parameters,
			IMenuModel model, IRunContextMenuCallback callback) {
			return false;
		}
	}

}
