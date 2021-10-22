using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using CefSharp;
using CefSharp.Wpf;

namespace ExecutingJavaScript {

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
					Browser.Address = "github.com";
				}, Dispatcher).Start();
		}

		private async  void ExecuteJSButton_Click(object sender, RoutedEventArgs e) {
			if (Browser.CanExecuteJavascriptInMainFrame && !string.IsNullOrWhiteSpace(ScriptTextBox.Text)) {
				var response = await Browser.EvaluateScriptAsync(ScriptTextBox.Text);
				if (response != null) {
					MessageBox.Show(response.Result.ToString(), "JavaScript Result");
				}
			}
		}
	}

}
