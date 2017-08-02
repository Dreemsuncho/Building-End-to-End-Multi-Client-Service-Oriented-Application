using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using Core.Common.Utils;

namespace Core.Common.Core
{
    public abstract class NotificationObject : INotifyPropertyChanged
    {
        private event PropertyChangedEventHandler _propertyChanged;

        protected readonly List<PropertyChangedEventHandler> propertyChangedSubscribers = new List<PropertyChangedEventHandler>();

        public event PropertyChangedEventHandler PropertyChanged
        {
            add
            {
                if (!this.propertyChangedSubscribers.Contains(value))
                {
                    this._propertyChanged += value;
                    this.propertyChangedSubscribers.Add(value);
                }
            }
            remove
            {
                this._propertyChanged -= value;
                this.propertyChangedSubscribers.Remove(value);
            }
        }

        private void _OnPropertyChanged(string propertyName)
        {
            if (this._propertyChanged != null)
                this._propertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void OnPropertyChanged<T>(Expression<Func<T>> propertyExpression)
        {
            string propertyName = PropertySupport.ExtractPropertyName(propertyExpression);
            this._OnPropertyChanged(propertyName);
        }
    }
}
