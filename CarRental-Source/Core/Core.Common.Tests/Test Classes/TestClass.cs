using System;
using FluentValidation;
using Core.Common.Core;

namespace Core.Common.Tests
{
    internal class TestClass : ObjectBase
    {
        private string _cleanProp = string.Empty;
        private string _dirtyProp = string.Empty;
        private string _stringProp = string.Empty;
        private TestChild _child = new TestChild();
        //private CollectionBase<TestChild> _children = new CollectionBase<TestChild>();
        private TestChild _notNavigableChild = new TestChild();

        public string CleanProp
        {
            get { return this._cleanProp; }
            set
            {
                if (this._cleanProp != value)
                {
                    this._cleanProp = value;
                    base.OnPropertyChanged(() => this.CleanProp, false);
                }
            }
        }

        public string DirtyProp
        {
            get { return this._dirtyProp; }
            set
            {
                if (this._dirtyProp != value)
                {
                    this._dirtyProp = value;
                    base.OnPropertyChanged(() => this.DirtyProp);
                }
            }
        }

        public string StringProp
        {
            get { return this._stringProp; }
            set
            {
                if (this._stringProp != value)
                {
                    this._stringProp = value;
                    base.OnPropertyChanged(() => this.StringProp, false);
                }
            }
        }

        public TestChild Child
        {
            get { return this._child; }
        }

        [NotNavigable]
        public TestChild NotNavigableChild
        {
            get { return this._notNavigableChild; }
        }

        protected override IValidator GetValidator()
        {
            return new TestClassValidator();
        }

        private class TestClassValidator : AbstractValidator<TestClass>
        {
            public TestClassValidator()
            {
                base.RuleFor(x => x.StringProp).NotEmpty();
            }
        }

        //public CollectionBase<TestChild> Children
        //{
        //    get { return this._children; }
        //}
    }
}
