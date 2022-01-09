using System;

namespace Microsoft.EntityFrameworkCore
{
    public class OutputParameter<TValue>
    {
        private bool _valueSet = false;

#pragma warning disable S1104 // Fields should not have public accessibility
        public TValue _value;
#pragma warning restore S1104 // Fields should not have public accessibility

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
