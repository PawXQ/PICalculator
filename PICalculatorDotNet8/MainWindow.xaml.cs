using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PICalculatorDotNet8
{
    public partial class MainWindow : Window
    {
        private WriteableBitmap _wbitmap;
        private byte[] _pixelBuffer;
        private int _width, _height;
        private int _stride;
        private List<InkParticle> _particles = new List<InkParticle>();
        private Point _lastMousePos;
        private Random _random = new Random();

        private static readonly Color[] TraditionalColors = new Color[]
        {
            Color.FromRgb(46, 68, 83),    // 鐵色 (沉穩的深藍灰)
            Color.FromRgb(119, 150, 154), // 水淺蔥 (清透的淡藍綠)
            Color.FromRgb(165, 91, 83),   // 蘇芳 (帶有灰調的雅緻紅)
            Color.FromRgb(130, 120, 140), // 桔梗鼠 (低飽和的灰紫色)
            Color.FromRgb(40, 40, 40)     // 濃墨 (傳統純墨色)
        };

        // 和紙底色：R=244, G=241, B=225 
        private const byte BgR = 244, BgG = 241, BgB = 225;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel(); // 保持你原來的邏輯
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _width = (int)this.ActualWidth;
            _height = (int)this.ActualHeight;

            _wbitmap = new WriteableBitmap(_width, _height, 96, 96, PixelFormats.Bgr32, null);
            _stride = _width * 4;
            _pixelBuffer = new byte[_stride * _height];

            ClearBuffer();

            InkBackground.Source = _wbitmap;
            _lastMousePos = Mouse.GetPosition(InkBackground);

            CompositionTarget.Rendering += OnRendering;
        }

        private void ClearBuffer()
        {
            for (int i = 0; i < _pixelBuffer.Length; i += 4)
            {
                _pixelBuffer[i] = BgB;
                _pixelBuffer[i + 1] = BgG;
                _pixelBuffer[i + 2] = BgR;
                _pixelBuffer[i + 3] = 255;
            }
        }

        private void InkBackground_MouseMove(object sender, MouseEventArgs e)
        {
            Point currentPos = e.GetPosition(InkBackground);
            double dx = currentPos.X - _lastMousePos.X;
            double dy = currentPos.Y - _lastMousePos.Y;
            double dist = Math.Sqrt(dx * dx + dy * dy);

            if (dist > 2)
            {
                // 放慢顏色切換的速度，讓同一筆畫維持相近的色系，不會變成彩虹碎紙機
                int colorIndex = (Environment.TickCount / 1500) % TraditionalColors.Length;
                Color brushColor = TraditionalColors[colorIndex];

                int pCount = Math.Min((int)(dist / 2), 8);
                for (int i = 0; i < pCount; i++)
                {
                    _particles.Add(new InkParticle
                    {
                        X = currentPos.X + (_random.NextDouble() - 0.5) * 12,
                        Y = currentPos.Y + (_random.NextDouble() - 0.5) * 12,
                        Vx = dx * 0.08 + (_random.NextDouble() - 0.5) * 2, // 降低噴濺速度，更柔和
                        Vy = dy * 0.08 + (_random.NextDouble() - 0.5) * 2,
                        Life = 1.0f,
                        Size = (float)(_random.NextDouble() * 8 + 3), // 筆觸稍微放大
                        Decay = (float)(_random.NextDouble() * 0.006 + 0.003),
                        R = brushColor.R,
                        G = brushColor.G,
                        B = brushColor.B
                    });
                }
            }
            _lastMousePos = currentPos;
        }

        private void OnRendering(object sender, EventArgs e)
        {
            if (_width != (int)this.ActualWidth || _height != (int)this.ActualHeight) return;

            FadeBuffer();

            for (int i = _particles.Count - 1; i >= 0; i--)
            {
                var p = _particles[i];
                p.Update();

                if (p.Life <= 0)
                {
                    _particles.RemoveAt(i);
                    continue;
                }

                // 傳入粒子的獨立顏色
                DrawInkSplash(p.X, p.Y, p.Size, p.Life, p.R, p.G, p.B);
            }

            _wbitmap.WritePixels(new Int32Rect(0, 0, _width, _height), _pixelBuffer, _stride, 0);
        }

        // 修改後的淡出邏輯：保證絕對能恢復原本的底色
        private void FadeBuffer()
        {
            for (int i = 0; i < _pixelBuffer.Length; i += 4)
            {
                // 使用 FadeColor 方法，加快淡出速度 (0.05)，並避免殘影
                _pixelBuffer[i] = FadeColor(_pixelBuffer[i], BgB, 0.05);     // Blue
                _pixelBuffer[i + 1] = FadeColor(_pixelBuffer[i + 1], BgG, 0.05); // Green
                _pixelBuffer[i + 2] = FadeColor(_pixelBuffer[i + 2], BgR, 0.05); // Red
            }
        }

        // 淡出漸變計算：強制每次至少恢復 1 單位，解決漸變卡在整數除法造成的灰色殘影
        private byte FadeColor(byte current, byte target, double rate)
        {
            if (current == target) return current;
            int diff = target - current;
            int step = (int)(diff * rate);

            // 如果差距太小，乘上 rate 變成 0，我們強制它移動 1 步
            if (step == 0) step = Math.Sign(diff);

            return (byte)(current + step);
        }

        private void DrawInkSplash(double cx, double cy, double radius, float opacity, byte rColor, byte gColor, byte bColor)
        {
            int r = (int)radius + 3;
            int minX = Math.Max(0, (int)(cx - r));
            int maxX = Math.Min(_width - 1, (int)(cx + r));
            int minY = Math.Max(0, (int)(cy - r));
            int maxY = Math.Min(_height - 1, (int)(cy + r));

            for (int y = minY; y <= maxY; y++)
            {
                for (int x = minX; x <= maxX; x++)
                {
                    double dx = x - cx;
                    double dy = y - cy;
                    double dist = Math.Sqrt(dx * dx + dy * dy);

                    if (dist < radius + 2)
                    {
                        float edgeFactor = 1.0f - (float)(dist / (radius + 2));
                        if (edgeFactor < 0) edgeFactor = 0;

                        // 降低一點濃度，讓顏色更像水彩的清透感
                        float finalAlpha = opacity * edgeFactor * 0.25f;

                        int idx = (y * _width + x) * 4;

                        // 將之前的黑色換成了粒子的明亮顏色
                        _pixelBuffer[idx] = (byte)(_pixelBuffer[idx] * (1 - finalAlpha) + bColor * finalAlpha); // B
                        _pixelBuffer[idx + 1] = (byte)(_pixelBuffer[idx + 1] * (1 - finalAlpha) + gColor * finalAlpha); // G
                        _pixelBuffer[idx + 2] = (byte)(_pixelBuffer[idx + 2] * (1 - finalAlpha) + rColor * finalAlpha); // R
                    }
                }
            }
        }

        // 輔助函式：將 HSV (色相、飽和度、明度) 轉換為 RGB 顏色
        private Color HsvToRgb(double h, double s, double v)
        {
            int hi = Convert.ToInt32(Math.Floor(h / 60)) % 6;
            double f = h / 60 - Math.Floor(h / 60);
            v = v * 255;
            byte v_byte = (byte)v;
            byte p = (byte)(v * (1 - s));
            byte q = (byte)(v * (1 - f * s));
            byte t = (byte)(v * (1 - (1 - f) * s));

            if (hi == 0) return Color.FromRgb(v_byte, t, p);
            else if (hi == 1) return Color.FromRgb(q, v_byte, p);
            else if (hi == 2) return Color.FromRgb(p, v_byte, t);
            else if (hi == 3) return Color.FromRgb(p, q, v_byte);
            else if (hi == 4) return Color.FromRgb(t, p, v_byte);
            else return Color.FromRgb(v_byte, p, q);
        }
    }

    public class InkParticle
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Vx { get; set; }
        public double Vy { get; set; }
        public float Life { get; set; }
        public float Size { get; set; }
        public float Decay { get; set; }

        // 新增屬性：儲存該粒子的專屬 RGB 顏色
        public byte R { get; set; }
        public byte G { get; set; }
        public byte B { get; set; }

        public void Update()
        {
            X += Vx;
            Y += Vy;
            Vx *= 0.90;
            Vy *= 0.90;
            Life -= Decay;
            Size += 0.15f;
        }
    }
}