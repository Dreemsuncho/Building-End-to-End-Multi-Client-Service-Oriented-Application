using Core.Common.Contracts;
using Core.Common.Extensions;
using Core.Common.Utils;
using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Core.Common.Core
{
    public abstract class ObjectBase : NotificationObject, IDirtyCapable, IExtensibleDataObject, IDataErrorInfo
    {
        protected bool isDirty = false;
        protected IValidator validator = null;
        protected IEnumerable<ValidationFailure> validationErrors = null;

        public ObjectBase()
        {
            this.validator = this.GetValidator();
            this.Validate();
        }


        #region IExtensibleDataObject Members

        public ExtensionDataObject ExtensionData { get; set; }

        #endregion


        #region IDirtyCapable members

        [NotNavigable]
        public virtual bool IsDirty
        {
            get { return this.isDirty; }
            protected set
            {
                this.isDirty = value;
                OnPropertyChanged(() => this.IsDirty, false);
            }
        }


        /// <summary>
        /// Walks the object graph and check if is any dirty object.
        /// </summary>
        public virtual bool IsAnythingDirty()
        {
            bool result = false;

            WalkObjectGraph(
            o =>
            {
                if (o.IsDirty)
                    result = true;

                return result;
            }, 
            coll => { });

            return result;
        }

        /// <summary>
        /// Walks the object graph and get all dirty objects.
        /// </summary>
        public IList<IDirtyCapable> GetDirtyObjects()
        {
            List<IDirtyCapable> dirtyObjects = new List<IDirtyCapable>();

            WalkObjectGraph(
            o =>
            {
                if (o.IsDirty)
                    dirtyObjects.Add(o);

                return false;
            }, coll => { });

            return dirtyObjects;
        }

        /// <summary>
        /// Walks the object graph and cleaning any dirty object.
        /// </summary>
        public void CleanAll()
        {
            WalkObjectGraph(
            o =>
            {
                if (o.IsDirty)
                    o.IsDirty = false;
                return false;
            }, coll => { });
        }

        #endregion


        #region Protected methods

        protected void WalkObjectGraph(Func<ObjectBase, bool> snippetForObject,
                                       Action<IList> snippetForCollection,
                                       params string[] exemptProperties)
        {
            List<ObjectBase> visited = new List<ObjectBase>();
            Action<ObjectBase> walk = null;

            List<string> exemptions = new List<string>();
            if (exemptProperties != null)
                exemptions = exemptProperties.ToList();

            walk = (o) =>
            {
                if (o != null && !visited.Contains(o))
                {
                    visited.Add(o);

                    bool exitWalk = snippetForObject.Invoke(o);

                    if (!exitWalk)
                    {
                        PropertyInfo[] properties = o.GetBrowsableProperties();
                        foreach (PropertyInfo property in properties)
                        {
                            if (!exemptions.Contains(property.Name))
                            {
                                if (property.PropertyType.IsSubclassOf(typeof(ObjectBase)))
                                {
                                    ObjectBase obj = (ObjectBase)(property.GetValue(o, null));
                                    walk(obj);
                                }
                                else
                                {
                                    IList coll = property.GetValue(o, null) as IList;
                                    if (coll != null)
                                    {
                                        snippetForCollection.Invoke(coll);

                                        foreach (object item in coll)
                                        {
                                            if (item is ObjectBase)
                                                walk((ObjectBase)item);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };

            walk(this);
        }

        #endregion


        #region Property change notification

        protected void OnPropertyChanged<T>(Expression<Func<T>> propertyExpression, bool makeDirty = true)
        {
            base.OnPropertyChanged(propertyExpression);

            if (makeDirty)
                IsDirty = true;

            Validate();
        }

        #endregion


        #region Validation

        protected virtual IValidator GetValidator()
        {
            return null;
        }

        [NotNavigable]
        public IEnumerable<ValidationFailure> ValidationErrors
        {
            get { return this.validationErrors; }
        }

        public void Validate()
        {
            if (this.validator != null)
            {
                ValidationResult results = this.validator.Validate(this);
                this.validationErrors = results.Errors;
            }
        }

        [NotNavigable]
        public virtual bool IsValid
        {
            get
            {
                if (this.validationErrors != null && 
                    this.validationErrors.Count() > 0)
                    return false;
                else
                    return true;
            }
        }

        #endregion


        #region IDataErrorInfo members

        string IDataErrorInfo.Error
        {
            get { return string.Empty; }
        }

        string IDataErrorInfo.this[string columnName]
        {
            get
            {
                StringBuilder errors = new StringBuilder();

                if (this.validationErrors != null && 
                    this.validationErrors.Count() > 0)
                {
                    foreach (ValidationFailure validationError in this.validationErrors)
                    {
                        if (validationError.PropertyName == columnName)
                            errors.AppendLine(validationError.ErrorMessage);
                    }
                }

                return errors.ToString();
            }
        }

        #endregion
    }
}
