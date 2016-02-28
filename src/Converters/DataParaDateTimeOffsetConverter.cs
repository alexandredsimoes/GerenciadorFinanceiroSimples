using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace GerenciadorFinanceiroSimples.Converters
{
    public class DateTimeParaDateTimeOffsetConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null) return DateTimeOffset.MinValue;
            var data = DateTimeOffset.Parse(value.ToString());

            return data;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value == null) return DateTime.Now;
            var data = value as DateTimeOffset?;

            return data.Value.DateTime;
        }
    }
}
