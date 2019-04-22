using System;

using Xamarin.Forms;

namespace StockPortfolio.Models
{
    public class DayModel : ContentPage
    {
        public DayModel()
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

