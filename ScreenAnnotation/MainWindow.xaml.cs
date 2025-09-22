using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
// Note: To use System.Drawing and System.Windows.Forms, you may need to add a reference.
// For .NET 6, add the System.Drawing.Common and System.Windows.Forms NuGet packages.
using System.Drawing;
using SDImaging = System.Drawing.Imaging; // Alias to avoid conflict
using SWForms = System.Windows.Forms;     // Alias for clarity

namespace ScreenAnnotation
{
    public partial class MainWindow : Window
    {
        private HwndSource _source;
        private const int HOTKEY_ID = 9000;

        public MainWindow()
        {
            InitializeComponent();
            // Set initial drawing attributes
            inkCanvas.DefaultDrawingAttributes.Color = Colors.Red;
            inkCanvas.DefaultDrawingAttributes.Width = 5;
            inkCanvas.DefaultDrawingAttributes.Height = 5;
            inkCanvas.EditingMode = InkCanvasEditingMode.Ink;
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            var helper = new WindowInteropHelper(this);
            _source = HwndSource.FromHwnd(helper.Handle);
            _source.AddHook(HwndHook);
            RegisterHotKey();
        }

        protected override void OnClosed(EventArgs e)
        {
            _source.RemoveHook(HwndHook);
            _source = null;
            UnregisterHotKey();
            base.OnClosed(e);
        }

        private void RegisterHotKey()
        {
            var helper = new WindowInteropHelper(this);
            if (!HotkeyHelper.RegisterHotKey(helper.Handle, HOTKEY_ID, HotkeyHelper.Modifiers.Ctrl | HotkeyHelper.Modifiers.Alt, HotkeyHelper.Keys.A))
            {
                MessageBox.Show("Failed to register hotkey. It might be in use by another application.", "Hotkey Error");
            }
        }

        private void UnregisterHotKey()
        {
            var helper = new WindowInteropHelper(this);
            HotkeyHelper.UnregisterHotKey(helper.Handle, HOTKEY_ID);
        }

        private IntPtr HwndHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            const int WM_HOTKEY = 0x0312;
            if (msg == WM_HOTKEY && wParam.ToInt32() == HOTKEY_ID)
            {
                ToggleVisibility();
                handled = true;
            }
            return IntPtr.Zero;
        }

        private void ToggleVisibility()
        {
            if (this.IsVisible)
            {
                this.Hide();
            }
            else
            {
                // Get the screen where the mouse is.
                var mousePosition = SWForms.Control.MousePosition;
                var screen = SWForms.Screen.FromPoint(mousePosition);

                // Move the window to the correct screen before showing
                this.WindowState = WindowState.Normal;
                this.Left = screen.WorkingArea.Left;
                this.Top = screen.WorkingArea.Top;

                this.Show();
                this.WindowState = WindowState.Maximized;
                this.Activate();
            }
        }

        private void PenButton_Click(object sender, RoutedEventArgs e)
        {
            inkCanvas.EditingMode = InkCanvasEditingMode.Ink;
            var attr = inkCanvas.DefaultDrawingAttributes;
            attr.IsHighlighter = false;
            var color = attr.Color;
            attr.Color = Color.FromArgb(255, color.R, color.G, color.B);
        }

        private void HighlighterButton_Click(object sender, RoutedEventArgs e)
        {
            inkCanvas.EditingMode = InkCanvasEditingMode.Ink;
            var attr = inkCanvas.DefaultDrawingAttributes;
            attr.IsHighlighter = true;
            var color = attr.Color;
            attr.Color = Color.FromArgb(128, color.R, color.G, color.B);
        }

        private void EraserButton_Click(object sender, RoutedEventArgs e)
        {
            inkCanvas.EditingMode = InkCanvasEditingMode.EraseByStroke;
        }

        private void ColorButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button != null && button.Background is SolidColorBrush brush)
            {
                var newColor = brush.Color;
                var attr = inkCanvas.DefaultDrawingAttributes;
                attr.Color = newColor;

                if (attr.IsHighlighter)
                {
                    attr.Color = Color.FromArgb(128, newColor.R, newColor.G, newColor.B);
                }
                else
                {
                    attr.Color = Color.FromArgb(255, newColor.R, newColor.G, newColor.B);
                }
            }
        }

        private void ThicknessSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (inkCanvas != null)
            {
                var attr = inkCanvas.DefaultDrawingAttributes;
                attr.Width = e.NewValue;
                attr.Height = e.NewValue;
            }
        }

        private void UndoButton_Click(object sender, RoutedEventArgs e)
        {
            if (inkCanvas.CanUndo)
            {
                inkCanvas.Undo();
            }
        }

        private void RedoButton_Click(object sender, RoutedEventArgs e)
        {
            if (inkCanvas.CanRedo)
            {
                inkCanvas.Redo();
            }
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            inkCanvas.Strokes.Clear();
        }

        private async void ScreenshotButton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            await Task.Delay(250); // Ensure window is hidden

            try
            {
                var helper = new WindowInteropHelper(this);
                var screen = SWForms.Screen.FromHandle(helper.Handle);
                var bounds = screen.Bounds;

                using (var screenshot = new Bitmap(bounds.Width, bounds.Height, SDImaging.PixelFormat.Format32bppArgb))
                {
                    using (var graphics = Graphics.FromImage(screenshot))
                    {
                        graphics.CopyFromScreen(bounds.X, bounds.Y, 0, 0, bounds.Size, CopyPixelOperation.SourceCopy);
                    }

                    var rtb = new RenderTargetBitmap((int)inkCanvas.ActualWidth, (int)inkCanvas.ActualHeight, 96d, 96d, PixelFormats.Pbgra32);
                    rtb.Render(inkCanvas);

                    var encoder = new PngBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(rtb));

                    using (var stream = new MemoryStream())
                    {
                        encoder.Save(stream);
                        stream.Position = 0;
                        using (var inkBitmap = new Bitmap(stream))
                        {
                            using (var graphics = Graphics.FromImage(screenshot))
                            {
                                graphics.DrawImage(inkBitmap, 0, 0);
                            }
                        }
                    }

                    var dialog = new SaveFileDialog
                    {
                        FileName = $"Capture_{DateTime.Now:yyyyMMdd_HHmmss}.png",
                        Filter = "PNG Image|*.png"
                    };

                    if (dialog.ShowDialog() == true)
                    {
                        screenshot.Save(dialog.FileName, SDImaging.ImageFormat.Png);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Could not take screenshot. Error: {ex.Message}", "Error");
            }
            finally
            {
                this.Show();
            }
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
