using AutoMapper;
using StoryScore.Client.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
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
