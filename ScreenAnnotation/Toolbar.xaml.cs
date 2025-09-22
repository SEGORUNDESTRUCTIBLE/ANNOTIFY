using System;
using System.Windows;
using System.Windows.Controls;

namespace ScreenAnnotation
{
    /// <summary>
    /// Interaction logic for Toolbar.xaml
    /// </summary>
    public partial class Toolbar : UserControl
    {
        public event RoutedEventHandler PenButtonClick;
        public event RoutedEventHandler HighlighterButtonClick;
        public event RoutedEventHandler EraserButtonClick;
        public event RoutedEventHandler UndoButtonClick;
        public event RoutedEventHandler RedoButtonClick;
        public event RoutedEventHandler ClearButtonClick;
        public event RoutedEventHandler ScreenshotButtonClick;
        public event RoutedEventHandler ExitButtonClick;
        public event RoutedPropertyChangedEventHandler<double> ThicknessChanged;
        public event RoutedEventHandler ColorButtonClick;

        public Toolbar()
        {
            InitializeComponent();

            PenButton.Click += (s, e) => PenButtonClick?.Invoke(this, e);
            HighlighterButton.Click += (s, e) => HighlighterButtonClick?.Invoke(this, e);
            EraserButton.Click += (s, e) => EraserButtonClick?.Invoke(this, e);
            UndoButton.Click += (s, e) => UndoButtonClick?.Invoke(this, e);
            RedoButton.Click += (s, e) => RedoButtonClick?.Invoke(this, e);
            ClearButton.Click += (s, e) => ClearButtonClick?.Invoke(this, e);
            ScreenshotButton.Click += (s, e) => ScreenshotButtonClick?.Invoke(this, e);
            ExitButton.Click += (s, e) => ExitButtonClick?.Invoke(this, e);
            ThicknessSlider.ValueChanged += (s, e) => ThicknessChanged?.Invoke(this, e);

            RedColorButton.Click += ColorButton_Click;
            BlueColorButton.Click += ColorButton_Click;
            GreenColorButton.Click += ColorButton_Click;
            BlackColorButton.Click += ColorButton_Click;
        }

        private void ColorButton_Click(object sender, RoutedEventArgs e)
        {
            ColorButtonClick?.Invoke(sender, e);
        }
    }
}
