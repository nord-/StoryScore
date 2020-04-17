using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Media.Animation;
using Newtonsoft.Json;

namespace StoryScore.Display.Services
{
    public class AdsService
    {
        private readonly Options          _options;
        private string[] _ads;
        private int _displayAd = 0;

        private readonly Timer _rollAdsTimer = new Timer(5000);

        public event Action<string> AdSourceChanged;

        public AdsService(Options options)
        {
            _options = options;

            _rollAdsTimer.Elapsed += RollAdsTimer_Elapsed;
        }

        private void RollAdsTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            AdSourceChanged?.Invoke(Path.Combine(_options.FileStorePath, _ads[++_displayAd]));
            if (_displayAd == _ads.Length - 1) _displayAd = -1;
        }

        public void ShowAds(string adsToShowJson)
        {
            _ads = JsonConvert.DeserializeObject<string[]>(adsToShowJson);
            _displayAd = 0;
            AdSourceChanged?.Invoke(Path.Combine(_options.FileStorePath, _ads[_displayAd]));

            _rollAdsTimer.Start();
        }

        public void HideAds()
        {
            _rollAdsTimer.Stop();
            AdSourceChanged?.Invoke(string.Empty);
        }
    }
}