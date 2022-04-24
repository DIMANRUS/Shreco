using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Shreco.Pages
{	
	public partial class ScannerPage : ContentPage
	{	
		public ScannerPage ()
		{
			InitializeComponent ();

		}

		public delegate void ScanMethods(string result);
		public event ScanMethods OnScanResult;

		async void Button_Clicked(System.Object sender, System.EventArgs e)
        {
			await Navigation.PopModalAsync();
        }

        void ZXingScannerView_OnScanResult(ZXing.Result result)
        {
			OnScanResult?.Invoke(result.ToString());
        }
    }
}

