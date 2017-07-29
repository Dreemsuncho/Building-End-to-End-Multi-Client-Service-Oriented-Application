using System;
using System.Collections.Generic;
using Core.Common.Core;

namespace Core.Common.Tests
{
    internal class TestChild : ObjectBase
    {
        private string _childName = string.Empty;

        public string ChildName
        {
            get { return this._childName; }
            set
            {
                if (this._childName != value)
                {
                    this._childName = value;
                    base.OnPropertyChanged(() => this.ChildName);
                }
            }
        }
    }
}
