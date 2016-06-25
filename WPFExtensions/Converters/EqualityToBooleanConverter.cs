﻿using System;
using System.Globalization;
using System.Windows.Data;

namespace WPFExtensions.Converters
{
    public class EqualityToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return object.Equals(value, parameter);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value)
                return parameter;

			//it's false, so don't bind it back
			return Binding.DoNothing;
            //throw new Exception("EqualityToBooleanConverter: It's false, I won't bind back.");
        }
    }
}
