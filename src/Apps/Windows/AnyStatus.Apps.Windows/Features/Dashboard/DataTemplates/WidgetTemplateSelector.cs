using AnyStatus.API.Widgets;
using System.Windows;
using System.Windows.Controls;

namespace AnyStatus.Apps.Windows.Features.Dashboard.DataTemplates
{
    public class WidgetTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container) =>
            container is FrameworkElement element ? (DataTemplate)element.FindResource(GetTemplateKey(item)) : null;

        private static string GetTemplateKey(object widget) => widget switch
        {
            FolderWidget _ => "FolderTemplate",
            TextLabelWidget _ => "LabelTemplate",
            IMetricWidget _ => "MetricTemplate",
            IPipeline _ => "PipelineTemplate",
            _ => "WidgetTemplate"
        };
    }
}
