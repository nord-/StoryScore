using AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace StoryScore.Client.Model
{
    public class ObservableProperty<T> : ObservablePropertyBase
    {
        private T _value;

        public ObservableProperty() { }
        public ObservableProperty(T value)
        {
            Value = value;
        }

        public T Value
        {
            get => _value;
            set { _value = value; OnPropertyChanged(); }
        }

        public static bool operator ==(ObservableProperty<T> self, T other)
        {
            return self._value.Equals(other);
        }

        public static bool operator !=(ObservableProperty<T> self, T other)
        {
            return !self._value.Equals(other);
        }
    }

    public abstract class ObservablePropertyBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public class ObservablePropertyMappings : Profile
    {
        public ObservablePropertyMappings()
        {
            //CreateMap(typeof(ObservableProperty<int>), typeof(int))
            //.ConvertUsing(typeof(ObsConverter<>));
            //CreateMap(typeof(ObservableProperty<string>), typeof(string))
            //.ConvertUsing(typeof(ObsConverter<>));
            CreateMap(typeof(ObservableProperty<>), typeof(object))
                .ConvertUsing(typeof(ObsConverter<>));
            CreateMap(typeof(object), typeof(ObservableProperty<>))
                .ConvertUsing(typeof(ObsConverterTo<>));
        }

        private class ObsConverter<T> : ITypeConverter<ObservableProperty<T>,object>
        {
            public object Convert(ObservableProperty<T> source, object destination, ResolutionContext context)
            {
                return source.Value;
            }
        }

        private class ObsConverterTo<T> : ITypeConverter<object, ObservableProperty<T>>
        {
            public ObservableProperty<T> Convert(object source, ObservableProperty<T> destination, ResolutionContext context)
            {
                return new ObservableProperty<T>((T)source);
            }
        }
    }
}
