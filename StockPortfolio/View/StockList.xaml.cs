using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using CarouselView.FormsPlugin;
using Entry = Microcharts.Entry;
using StockPortfolio.Models;
using Xamarin.Forms;
using Microcharts;
using SkiaSharp;
using System.Diagnostics;

namespace StockPortfolio
{
   


    public partial class StockList : ContentPage
    {
        public string key = "CPM4DDVVKVQQKXKT"; //API KEY for thr alpha vantage api
        private DayData dailydata = new DayData(); //
        private double min = 999999.00; //Set as the highest price holder
        private double max = 000000.00; //Set as the lowest placeholder
        private List<Entry> entries = new List<Entry> { };
        private Entry entry;
        private float temp;
      

    public StockList()
        {
            InitializeComponent();
            // CreateCharts();

        }

        public void FillChart()
        {
            if (GlobalVariables.retreived == true)
            {
               
                for (int i = 0; i < 30; i++)
                {
                    if (i % 2 == 0)
                    {
                        temp = float.Parse(GlobalVariables.StockList[i].The2High);
                        entry = new Entry(temp)
                        {
                            Color = SKColor.Parse("#000000"),
                            Label = "",
                            ValueLabel = GlobalVariables.StockList[i].The2High,
                        };

                        entries.Add(entry);
                        entries.Reverse();
                    }
                }
            }
        }

        async void HandleGetStock(object sender, EventArgs e)
        {
            //USER INPUT
            string UserInput;
            UserInput = GetStock.Text; //User input that is used to retrieve the desired company ancronym 

            //adding the user input to the api endpoint, and converting it to a URI variable
            string StockApiEndpoint = "https://www.alphavantage.co/query?function=TIME_SERIES_DAILY" + "&symbol=" + UserInput + "&apikey=" + key;
            Uri StockApiUri = new Uri(StockApiEndpoint);

            //A class instance that is used to send HTTP requests and receive HTTP responses
            HttpClient client = new HttpClient();

            //gets data from api 
            HttpResponseMessage response = await client.GetAsync(StockApiUri);
            //string
            string jsonContent = await response.Content.ReadAsStringAsync();
            //parse into an object according to data model
            var stockData = StockData.FromJson(jsonContent);

            //if an error wasn't returned
            if (jsonContent != GlobalVariables.ErrorString)
            {
                //convert dictionary to list
                GlobalVariables.StockList = stockData.TimeSeriesDaily.Values.ToList();
                GlobalVariables.DatesList = stockData.TimeSeriesDaily.Keys.ToList();

                List<DayData> DayList = new List<DayData>();

                for (int i = 0; i < 30; i++)
                {
                    //data model equal to api equivalent. 
                    //reminder parsing means to take one form of data and converting it to another
                    //data model defines how information is stoed and retrieced to support solution.
                    dailydata.date = GlobalVariables.DatesList[i];
                    dailydata.high = double.Parse(GlobalVariables.StockList[i].The2High);
                    dailydata.low = double.Parse(GlobalVariables.StockList[i].The3Low);
                    dailydata.close = double.Parse(GlobalVariables.StockList[i].The4Close);

                    //add dailydata to list
                    DayList.Add(dailydata);
                    dailydata = new DayData();
                }

                historyListView.ItemsSource = DayList;

                //populate rest of the fields ins the page
                populatePage();
                GlobalVariables.retreived = true;
                entries.Clear();
                ChartView.Chart = new LineChart { Entries = entries, BackgroundColor = SKColors.Transparent };

                FillChart();
                ChartView.Chart = new LineChart { Entries = entries, BackgroundColor = SKColors.Transparent, };

            }
            else
            {
                await DisplayAlert("ERROR", "Invalid Stock Symbol", "OK");
            }


        }

        private void GetHighAndLow()
        {
           
            for (int i = 0; i < 30; i++)
            {

                if (double.Parse(GlobalVariables.StockList[i].The3Low) <= min)
                {
                    min = double.Parse(GlobalVariables.StockList[i].The3Low);
                }

                if (double.Parse(GlobalVariables.StockList[i].The2High) >= max)
                {
                    max = double.Parse(GlobalVariables.StockList[i].The2High);
                }

            }
            //set labels to found high and low prices.
            lowestLabel.Text = min.ToString();
            highestLabel.Text = max.ToString();

            //reset values for next use.
            min = 999999.00;
            max = 000000.00;
        }

        private void populatePage()
        {

            GetHighAndLow();
        }

        async void OnAlertSimpleClicked(object sender, EventArgs e)
        {
            var action = await DisplayActionSheet("Help?", "Cancel", null, "Stock Info", "Twitter", "Search");
            Debug.WriteLine("Action: " + action);
            if(action == "Stock Info")
            {
                ActionDetails.Text = "Enter Stock SYMBOL and you will be prompted accordingly";
            }

            if (action == "Twitter")
            {
                ActionDetails.Text = "Twitter Stocks are for informative decisions only";
            }

            if (action == "Search")
            {
                ActionDetails.Text = "It's not Google, but it will work";
            }

        }

    }
}
