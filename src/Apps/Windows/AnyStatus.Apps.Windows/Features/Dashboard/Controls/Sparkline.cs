using ScottPlot;
using System;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace AnyStatus.Apps.Windows.Features.Dashboard.Controls
{
    public class Sparkline : UserControl
    {
        private double[] _ys;
        private WpfPlot _plot;

        public Sparkline()
        {
            Loaded += OnLoaded;
            Unloaded += OnUnloaded;

            Initialize();

            Content = _plot;
        }

        private void Initialize()
        {
            _plot = new();

            _plot.Configuration.Pan = false;
            _plot.Configuration.Zoom = false;
            _plot.Configuration.ScrollWheelZoom = false;
            _plot.Configuration.DoubleClickBenchmark = false;

            _plot.Plot.Grid(false);
            _plot.Plot.Frameless();
            _plot.Plot.Palette = Palette.Dark;
            _plot.Plot.XAxis.Ticks(false, false);
            _plot.Plot.YAxis.Ticks(false, false);
            _plot.Plot.Style(Color.Empty, Color.Empty);
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            Loaded -= OnLoaded;

            _ys = new double[Size];

            var signal = _plot.Plot.AddSignal(_ys);

            signal.MarkerSize = 0;
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            Unloaded -= OnUnloaded;

            BindingOperations.ClearAllBindings(this);

            _plot.Plot.Clear();
        }

        private static object CoerceValueCallback(DependencyObject sender, object baseValue)
        {
            ((Sparkline)sender).Render((double)baseValue);

            return baseValue;
        }

        private void Render(double value)
        {
            if (_ys is null)
            {
                return;
            }

            ShiftLeft(ref _ys);

            _ys[^1] = value;

            _plot.Plot.AxisAuto();

            _plot.Render();
        }

        private void ShiftLeft(ref double[] ys) => Array.Copy(ys, 1, ys, 0, Size - 1);

        #region Props

        public double Value
        {
            get => (double)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        public double[] Values
        {
            get => (double[])GetValue(ValuesProperty);
            set => SetValue(ValuesProperty, value);
        }

        public int Size
        {
            get => (int)GetValue(SizeProperty);
            set => SetValue(SizeProperty, value);
        }

        public static readonly DependencyProperty SizeProperty =
            DependencyProperty.Register(nameof(Size), typeof(int), typeof(Sparkline));

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(nameof(Value), typeof(double), typeof(Sparkline),
                new PropertyMetadata(default(double), null, CoerceValueCallback));

        public static readonly DependencyProperty ValuesProperty =
           DependencyProperty.Register(nameof(Values), typeof(double[]), typeof(Sparkline),
               new PropertyMetadata(default(double[]), null, null));

        #endregion
    }
}
