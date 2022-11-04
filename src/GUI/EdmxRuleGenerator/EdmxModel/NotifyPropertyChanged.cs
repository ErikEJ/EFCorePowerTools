using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace EdmxRuleGenerator.EdmxModel;

[DataContract]
public abstract class NotifyPropertyChanged : INotifyPropertyChanged
{
    /// <summary>
    ///   <para>Occurs when a property value changes.</para>
    /// </summary>
    public event PropertyChangedEventHandler PropertyChanged;

    /// <summary>
    ///   <para>Returns the name of a property identified by a lambda expression.</para>
    /// </summary>
    /// <param name="expression">A lambda expression selecting the property.</param>
    /// <typeparam name="T"></typeparam>
    /// <returns>The name of the property accessed by expression.</returns>
    public static string GetPropertyName<T>(Expression<Func<T>> expression)
    {
        return GetPropertyName((LambdaExpression)expression);
    }

    internal static string GetPropertyName(LambdaExpression expression)
    {
        if (expression.Body is not MemberExpression body)
        {
            throw new ArgumentException("MemberExpression is expected in expression.Body", nameof(expression));
        }

        var member = body.Member;
        return member.Name;
    }


    protected bool SetProperty<T>(ref T storage, T value, Expression<Func<T>> expression, Action changedCallback)
    {
        return SetProperty(ref storage, value, GetPropertyName(expression), changedCallback);
    }

    protected bool SetProperty<T>(ref T storage, T value, Expression<Func<T>> expression)
    {
        return SetProperty(ref storage, value, expression, null);
    }


    protected void RaisePropertyChanged<T>(Expression<Func<T>> expression)
    {
        RaisePropertyChanged(GetPropertyName(expression));
    }

    protected void RaisePropertiesChanged<T1, T2>(
        Expression<Func<T1>> expression1,
        Expression<Func<T2>> expression2)
    {
        RaisePropertyChanged(expression1);
        RaisePropertyChanged(expression2);
    }

    protected void RaisePropertiesChanged<T1, T2, T3>(
        Expression<Func<T1>> expression1,
        Expression<Func<T2>> expression2,
        Expression<Func<T3>> expression3)
    {
        RaisePropertyChanged(expression1);
        RaisePropertyChanged(expression2);
        RaisePropertyChanged(expression3);
    }

    protected bool SetProperty<T>(ref T storage, T value, string propertyName, Action changedCallback)
    {
        if (EqualityComparer<T>.Default.Equals(storage, value))
        {
            return false;
        }

        storage = value;
        RaisePropertyChanged(propertyName);
        changedCallback?.Invoke();
        return true;
    }

    protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
    {
        return SetProperty(ref storage, value, propertyName, null);
    }

    protected bool SetProperty<T>(ref T storage, T value, Action changedCallback,
        [CallerMemberName] string propertyName = null)
    {
        GuardPropertyName(propertyName);
        return SetProperty(ref storage, value, propertyName, changedCallback);
    }

    /// <summary> Set Value with callback carrying the BEFORE value </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="storage"></param>
    /// <param name="value"></param>
    /// <param name="changedCallback">callback carrying the BEFORE value</param>
    /// <param name="propertyName"></param>
    /// <returns></returns>
    protected bool SetProperty<T>(ref T storage, T value, Action<T> changedCallback,
        [CallerMemberName] string propertyName = null)
    {
        var before = storage;
        return SetProperty(ref storage, value, () => changedCallback(before), propertyName);
    }

    private static void GuardPropertyName(string propertyName)
    {
        if (string.IsNullOrEmpty(propertyName))
        {
            throw new ArgumentNullException(nameof(propertyName));
        }
    }

    protected void RaisePropertyChanged(string propertyName)
    {
        var propertyChanged = PropertyChanged;
        propertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected void RaisePropertyChanged()
    {
        RaisePropertiesChanged(null);
    }

    protected void RaisePropertiesChanged(params string[] propertyNames)
    {
        if (propertyNames == null || propertyNames.Length == 0)
        {
            RaisePropertyChanged(string.Empty);
        }
        else
        {
            foreach (var propertyName in propertyNames)
            {
                RaisePropertyChanged(propertyName);
            }
        }
    }
}
