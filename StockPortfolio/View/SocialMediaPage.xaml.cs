using XamarinForms.ViewModels;

using Xamarin.Forms;

namespace StockPortfolio
{ 
    public partial class SocialMediaPage : ContentPage
    {
        public SocialMediaPage()
        {
            //setting up the news page here instead of by using the xaml page.
            Title = "Twitter";
            BackgroundColor = Color.White;

            var twitterViewModel = new TwitterViewModel();

            BindingContext = twitterViewModel;

            var label = new Label
            {
                Text = "Tweets",
                TextColor = Color.Gray,
                FontSize = 24
            };

            var dataTemplate = new DataTemplate(() =>
            {
                var screenNameLabel = new Label
                {
                    TextColor = Color.FromHex("#2196F3"),
                    FontSize = 22
                };
                var textLabel = new Label
                {
                    TextColor = Color.Black,
                    FontSize = 18
                };
                var image = new Image
                {
                    WidthRequest = 60,
                    HeightRequest = 60,
                    VerticalOptions = LayoutOptions.Start
                };
                var mediaImage = new Image();

                screenNameLabel.SetBinding(Label.TextProperty, new Binding("ScreenName"));
                textLabel.SetBinding(Label.TextProperty, new Binding("Text"));
                image.SetBinding(Image.SourceProperty, new Binding("ImageUrl"));
                mediaImage.SetBinding(Image.SourceProperty, new Binding("MediaUrl"));

                return new ViewCell
                {
                    View = new StackLayout
                    {
                        Orientation = StackOrientation.Horizontal,
                        Padding = new Thickness(0, 5),
                        Children =
                        {
                            image,
                            new StackLayout
                            {
                                Orientation = StackOrientation.Vertical,
                                Children =
                                {
                                    screenNameLabel,
                                    textLabel,
                                    mediaImage
                                }
                            }
                        }
                    }
                };
            });

            var listView = new ListView
            {
                HasUnevenRows = true
            };

            listView.SetBinding(ListView.ItemsSourceProperty, "Tweets");

            listView.ItemTemplate = dataTemplate;

            Content = new StackLayout
            {
                Padding = new Thickness(5, 10),
                Children =
                {
                    label,
                    listView
                }
            };
        }
    }
}
