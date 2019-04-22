using System;

using Xamarin.Forms;

namespace StockPortfolio.Models
{
    public class Globals : ContentPage
    {
        public Globals()
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

