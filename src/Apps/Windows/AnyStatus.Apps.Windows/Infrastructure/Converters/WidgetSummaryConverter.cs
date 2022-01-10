using AnyStatus.API.Widgets;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;

namespace AnyStatus.Apps.Windows.Infrastructure.Converters
{
    public class WidgetSummaryConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            foreach (var value in values)
            {
                if (value is Widget widget)
                {
                    var stats = new Dictionary<string, int>();

                    Count(widget, stats);

                    return stats;
                }
            }

            return null;
        }

        private void Count(IWidget widget, Dictionary<string, int> stats)
        {
            if (widget is null)
            {
                return;
            }

            if (widget is not FolderWidget && widget.Status is not null)
            {
                if (stats.ContainsKey(widget.Status))
                {
                    stats[widget.Status]++;
                }
                else
                {
                    stats.Add(widget.Status, 1);
                }
            }

            foreach (var child in widget)
            {
                Count(child, stats);
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
