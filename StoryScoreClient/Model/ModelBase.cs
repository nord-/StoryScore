using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;

// https://daedtech.com/wpf-and-notifying-property-change/
//
namespace StoryScoreClient.Model
{
    public abstract class ModelBase : INotifyPropertyChanged
    {
        private readonly Dictionary<string, PropertyChangedEventArgs> _argsCache = new Dictionary<string, PropertyChangedEventArgs>();

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void NotifyChange<T>(Expression<Func<T>> propertySelector)
        {
            var myName = propertySelector.GetMemberName();
            if (!string.IsNullOrEmpty(myName))
                NotifyChange(myName);
        }

        protected virtual void NotifyChange(string propertyName)
        {
            if (_argsCache != null)
            {
                if (!_argsCache.ContainsKey(propertyName))
                    _argsCache[propertyName] = new PropertyChangedEventArgs(propertyName);

                NotifyChange(_argsCache[propertyName]);
            }
        }

        private void NotifyChange(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }
    }

    public static class StaticReflection
    {
        public static string GetMemberName<T>(this Expression<T> expression)
        {
            switch (expression.Body)
            {
                case MemberExpression m:
                    return m.Member.Name;
                case UnaryExpression u when u.Operand is MemberExpression m:
                    return m.Member.Name;
                default:
                    throw new NotImplementedException(expression.GetType().ToString());
            }
        }

    }
}
