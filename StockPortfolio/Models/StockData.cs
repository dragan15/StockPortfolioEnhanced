using System;

using Xamarin.Forms;

namespace StockPortfolio.Models
{
    public class StockData : ContentPage
    {
        public StockData()
        {
            Content = new StackLayout
            {
                Children = {
                    new Label { Text = "Hello ContentPage" }
                }
            };
        }
    }
}

