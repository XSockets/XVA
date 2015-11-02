using System;
using System.Collections.Generic;
using PollingDbForUpdates.Core.Interfaces.Validation;
	
namespace PollingDbForUpdates.Core.Common.Validation
{
    public class ValidationContainer<T,TA> : IValidationContainer<T,TA>
    {
        public IDictionary<string, IList<string>> ValidationErrors { get; private set; }
        public T Entity { get; private set; }
        public TA EntityViewModel { get; private set; }
        public bool IsValid
        {
            get { return this.ValidationErrors.Count == 0; }
        }

        public ValidationContainer(IDictionary<string, IList<string>> validationErrors, T entity)
        {
            this.ValidationErrors = validationErrors;
            Entity = entity;
        }

        public void AddError(string key, string value)
        {
            if (!ValidationErrors.ContainsKey(key))
                ValidationErrors.Add(key, new List<string>());
            ValidationErrors[key].Add(value);
        }

        public void SetViewModel()
        {
            this.EntityViewModel = (TA)Activator.CreateInstance(typeof (TA), this.Entity, true);
            this.Entity = default(T);
        }
    }
}
