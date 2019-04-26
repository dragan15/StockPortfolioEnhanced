using System.Collections.Generic;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using LinqToTwitter;
using tweetData;

namespace XamarinForms.ViewModels
{
    public class TwitterViewModel : INotifyPropertyChanged
    {

        private List<Tweet> _tweets;
        public List<Tweet> Tweets
        {
            get { return _tweets; }
            set
            {
                if (_tweets == value)
                    return;

                _tweets = value;
                OnPropertyChanged();
            }
        }

        public TwitterViewModel()
        {
            InitTweetsAsync();
        }

        public async Task InitTweetsAsync()
        {

            var auth = new ApplicationOnlyAuthorizer()
            {
                CredentialStore = new InMemoryCredentialStore
                {
                    // Please use this link to create a new app in Twitter
                    // https://apps.twitter.com/app/new
                    // so you can get ConsumerKey and ConsumerSecret
                    //ConsumerKey = "******",
                    //ConsumerSecret = "*******",
                    ConsumerKey = "OGrfiKBcA20Db49T6g0Ci9nC9",
                    ConsumerSecret = "5MoQR8nqNfmk2DgyzklhAHFgHcDDlogDf2zvJlLSe4WAjWO8qz",
                },
            };

            await auth.AuthorizeAsync(); //Ask for authorization

            var ctx = new TwitterContext(auth);

            var tweets = await
                (from tweet
                 in ctx.Status
                 where tweet.Type == StatusType.User
                        && tweet.ScreenName == "Jelenamandrapa"
                       && tweet.Count == 30
                 select tweet)
                .ToListAsync();


            //once information is obtained, map data to Twitter model. all info stored in tweets property
            Tweets = (from tweet
                      in tweets
                      select new Tweet
                      {
                          StatusID = tweet.StatusID,
                          ScreenName = tweet.User.ScreenNameResponse,
                          Text = tweet.Text,
                          ImageUrl = tweet.User.ProfileImageUrl,
                          MediaUrl = tweet?.Entities?.MediaEntities?.FirstOrDefault()?.MediaUrl
                      })
                .ToList();

            //obtain tweets from hashtags
            Search searchResponse = await
                (from search in ctx.Search
                 where search.Type == SearchType.Search &&
                       search.Query == "StockMarket"
                       && search.Count == 100
                 select search)
                .SingleAsync();

            Tweets =
                (from tweet in searchResponse.Statuses
                 select new Tweet
                 {
                     StatusID = tweet.StatusID,
                     ScreenName = tweet.User.ScreenNameResponse,
                     Text = tweet.Text,
                     ImageUrl = tweet.User.ProfileImageUrl
                 })
                .ToList();
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}