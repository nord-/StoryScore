using AutoMapper;
using StoryScore.Client.Model;
using System.Windows;

namespace StoryScore.Client
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            Mapper.Initialize(cfg => cfg.AddMaps(new[] { typeof(PlayerViewModel) }));
            Mapper.AssertConfigurationIsValid();
        }
    }
}
