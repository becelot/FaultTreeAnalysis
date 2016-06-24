using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace FaultTreeAnalysis.GUI.Converters
{
	public class VisibilityConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (!(value is bool))
			{
				return Visibility.Visible;
			}

			if ((bool)value)
			{
				return Visibility.Visible;
			}

			return Visibility.Hidden;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
