using System;

namespace Microsoft.EntityFrameworkCore
{
    /// <summary>
    /// Output paramter.
    /// </summary>
    /// <typeparam name="TValue"> The type.</typeparam>
    public class OutputParameter<TValue>
    {
        private bool valueSet;
        private TValue value;

        /// <summary>
        /// Gets the resulting value of the parameter.
        /// </summary>
        public TValue Value
        {
            get
            {
                if (!valueSet)
                {
                    throw new InvalidOperationException("Value not set.");
                }

                return value;
            }
        }

        internal void SetValue(object value)
        {
            valueSet = true;

            this.value = value == null || Convert.IsDBNull(value) ? default(TValue) : (TValue)value;
        }
    }
}
