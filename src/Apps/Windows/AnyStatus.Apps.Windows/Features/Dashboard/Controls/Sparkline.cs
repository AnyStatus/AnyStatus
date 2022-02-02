using ScottPlot;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace AnyStatus.Apps.Windows.Features.Dashboard.Controls
{
    public class Sparkline : UserControl
    {
        private double[] _ys;
        private bool _autoAxis;
        private readonly WpfPlot _plot;

        public Sparkline()
        {
            Loaded += OnLoaded;
            Unloaded += OnUnloaded;
            Content = _plot = Initialize();
        }

        private WpfPlot Initialize()
        {
            WpfPlot plot = new();

            plot.Configuration.Pan = false;
            plot.Configuration.Zoom = false;
            plot.Configuration.ScrollWheelZoom = false;
            plot.Configuration.DoubleClickBenchmark = false;

            plot.Plot.Grid(false);
            plot.Plot.Frameless();
            plot.Plot.Layout(padding: 6);
            plot.Plot.Palette = Palette.Dark;
            plot.Plot.XAxis.Ticks(false, false);
            plot.Plot.YAxis.Ticks(false, false);
            plot.Plot.Style(Color.Empty, Color.Empty);

            return plot;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            Loaded -= OnLoaded;

            _ys = new double[Size];

            var signal = _plot.Plot.AddSignal(_ys);
            signal.MarkerSize = 0;
            signal.Color = Color.Green;

            if (MaxValue.HasValue || MinValue.HasValue)
            {
                _plot.Plot.SetAxisLimits(xMin: 0, xMax: Size, yMin: MinValue, yMax: MaxValue);
            }
            else
            {
                _autoAxis = true;
            }

            Render();
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            Unloaded -= OnUnloaded;

            BindingOperations.ClearAllBindings(this);

            _plot.Plot.Clear();
        }

        private void Render()
        {
            if (_ys is null || Values is null)
            {
                return;
            }

            for (int i = Values.Count; i > 0 && Values.Count - i < Size; i--)
            {
                _ys[^(Values.Count - i + 1)] = Values[i - 1];
            }

            if (_autoAxis)
            {
                _plot.Plot.AxisAuto();
            }

            _plot.Render();
        }

        #region Properties

        public int Size
        {
            get => (int)GetValue(SizeProperty);
            set => SetValue(SizeProperty, value);
        }

        public double? MaxValue
        {
            get => (double?)GetValue(MaxValueProperty);
            set => SetValue(MaxValueProperty, value);
        }

        public double? MinValue
        {
            get => (double?)GetValue(MinValueProperty);
            set => SetValue(MinValueProperty, value);
        }

        public ObservableCollection<double> Values
        {
            get => (ObservableCollection<double>)GetValue(ValuesProperty);
            set => SetValue(ValuesProperty, value);
        }

        public static readonly DependencyProperty MaxValueProperty =
            DependencyProperty.Register(nameof(MaxValue), typeof(double?), typeof(Sparkline));

        public static readonly DependencyProperty MinValueProperty =
            DependencyProperty.Register(nameof(MinValue), typeof(double?), typeof(Sparkline));

        public static readonly DependencyProperty SizeProperty =
            DependencyProperty.Register(nameof(Size), typeof(int), typeof(Sparkline));

        public static readonly DependencyProperty ValuesProperty =
            DependencyProperty.Register(nameof(Values), typeof(ObservableCollection<double>), typeof(Sparkline),
                new FrameworkPropertyMetadata(default(ObservableCollection<double>),
                    new PropertyChangedCallback(OnPropertyChangedCallback)));

        private static void OnPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue is not null)
            {
                ((ObservableCollection<double>)e.OldValue).CollectionChanged -= ((Sparkline)d).CollectionChanged;
            }

            if (e.NewValue is not null)
            {
                ((ObservableCollection<double>)e.NewValue).CollectionChanged += ((Sparkline)d).CollectionChanged;
            }
        }

        private void CollectionChanged(object sender, NotifyCollectionChangedEventArgs e) => Application.Current?.Dispatcher.Invoke(() => Render());

        #endregion
    }
}
