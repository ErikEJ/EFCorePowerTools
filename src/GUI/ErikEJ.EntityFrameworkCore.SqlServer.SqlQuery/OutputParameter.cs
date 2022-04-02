using System;

namespace Microsoft.EntityFrameworkCore
{
    /// <summary>
    /// Output paramter.
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    public class OutputParameter<TValue>
    {
        private bool _valueSet;
        private TValue _value;

        /// <summary>
        /// The resulting value of the parameter.
        /// </summary>
        public TValue Value
        {
            get
            {
                if (!_valueSet)
                    throw new InvalidOperationException("Value not set.");

                return _value;
            }
        }

        internal void SetValue(object value)
        {
            _valueSet = true;

            _value = null == value || Convert.IsDBNull(value) ? default(TValue) : (TValue)value;
        }
    }
}
