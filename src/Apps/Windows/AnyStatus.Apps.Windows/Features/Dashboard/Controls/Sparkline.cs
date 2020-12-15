using ScottPlot;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

//todo: get background from Dependency Property

namespace AnyStatus.Apps.Windows.Features.Dashboard.Controls
{
    public class Sparkline : UserControl
    {
        private double[] _ys;

        private readonly WpfPlot _plotUserControl;

        public Sparkline()
        {
            Unloaded += OnUnloaded;

            _plotUserControl = new WpfPlot();
            _plotUserControl.Configure(recalculateLayoutOnMouseUp: false, enablePanning: false, enableRightClickMenu: false, enableRightClickZoom: false, enableScrollWheelZoom: false);
            _plotUserControl.plt.Grid(false);
            _plotUserControl.plt.Frame(false);
            _plotUserControl.plt.Ticks(false, false);
            _plotUserControl.plt.Style(figBg: Color.Empty, dataBg: Color.Empty);

            Content = _plotUserControl;
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            Unloaded -= OnUnloaded;

            BindingOperations.ClearAllBindings(this);

            _plotUserControl.plt.Clear();
        }

        private static void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is Sparkline sparkline)
            {
                sparkline.Render();
            }
        }

        private void Render()
        {
            if (_ys is null)
            {
                _ys = new double[Size];

                if (Values != null)
                {
                    var previousValues = Values.ToArray();

                    if (previousValues.Length > Size)
                    {
                        Array.Copy(previousValues, previousValues.Length - Size - 1, _ys, 0, Size);
                    }
                    else
                    {
                        Array.Copy(previousValues, 0, _ys, Size - previousValues.Length, previousValues.Length);
                    }
                }

                var sig = _plotUserControl.plt.PlotSignal(_ys);

                sig.markerSize = 0;
                sig.penHD = new Pen(Color.Green, 1.7f); //ColorTranslator.FromHtml("#4CAF50")
            }
            else
            {
                Array.Copy(_ys, 1, _ys, 0, Size - 1); // "scroll" the whole chart to the left

                _ys[^1] = Value; // place the newest data point at the end
            }

            _plotUserControl.plt.AxisAuto();

            _plotUserControl.plt.TightenLayout(padding: 6);

            _plotUserControl.Render(skipIfCurrentlyRendering: true, recalculateLayout: false, lowQuality: false);
        }

        #region Properties

        public int Size
        {
            get => (int)GetValue(SizeProperty);
            set => SetValue(SizeProperty, value);
        }

        public double MaxValue
        {
            get => (double)GetValue(MaxValueProperty);
            set => SetValue(MaxValueProperty, value);
        }

        public double Value
        {
            get => (double)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        public IEnumerable<double> Values
        {
            get => (IEnumerable<double>)GetValue(ValuesProperty);
            set => SetValue(ValuesProperty, value);
        }

        public static readonly DependencyProperty MaxValueProperty =
            DependencyProperty.Register(nameof(MaxValue), typeof(double), typeof(Sparkline));

        public static readonly DependencyProperty SizeProperty =
            DependencyProperty.Register(nameof(Size), typeof(int), typeof(Sparkline));

        public static readonly DependencyProperty ValuesProperty =
            DependencyProperty.Register(nameof(Values), typeof(IEnumerable<double>), typeof(Sparkline));

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(nameof(Value), typeof(double), typeof(Sparkline),
                new FrameworkPropertyMetadata(default(double), new PropertyChangedCallback(OnValueChanged)));

        #endregion
    }
}
