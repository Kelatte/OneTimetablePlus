using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media.Effects;
using System.IO;
using System.Windows.Media;
using System.Windows;

namespace OneTimetablePlus.Resources
{
    class ChangeColorEffect : ShaderEffect
    {
        private const string _kshaderAsBase64 = @"AAP///7/KwBDVEFCHAAAAH8AAAAAA///AgAAABwAAAAAAQAAeAAAAEQAAAADAAAAAQACAEwAAAAAAAAAXAAAAAIAAAABAAIAaAAAAAAAAABpbnB1dACrqwQADAABAAEAAQAAAAAAAAB0YXJnZXRDb2xvcgABAAMAAQAEAAEAAAAAAAAAcHNfM18wAE1pY3Jvc29mdCAoUikgSExTTCBTaGFkZXIgQ29tcGlsZXIgMTAuMQCrUQAABQEAD6DcJDSoAAAAAAAAAAAAAAAAHwAAAgUAAIAAAAOQHwAAAgAAAJAACA+gQgAAAwAAD4AAAOSQAAjkoAIAAAMBAAGAAAD/gAEAAKBYAAAEAAgHgAEAAIAAAOSgAADkgAEAAAIACAiAAAD/gP//AAA=";
        private static readonly PixelShader _shader;

        static ChangeColorEffect()
        {
            _shader = new PixelShader() { };
            _shader.SetStreamSource(new MemoryStream(Convert.FromBase64String(_kshaderAsBase64)));
            //_shader.UriSource = new Uri("/2.ps", UriKind.Relative);
            //_shader.UriSource = new Uri("pack://application:,,,/2.ps", UriKind.Absolute);
        }

        public ChangeColorEffect()
        {
            PixelShader = _shader;
            UpdateShaderValue(InputProperty);
            UpdateShaderValue(TargetColorProperty);

        }

        public Brush Input
        {
            get { return (Brush)GetValue(InputProperty); }
            set { SetValue(InputProperty, value); }
        }

        public static readonly DependencyProperty InputProperty =
            ShaderEffect.RegisterPixelShaderSamplerProperty("Input", typeof(ChangeColorEffect), 0);

        public Color TargetColor
        {
            get { return (Color)GetValue(TargetColorProperty); }
            set { SetValue(TargetColorProperty, value); }
        }

        public static readonly DependencyProperty TargetColorProperty =
            DependencyProperty.Register("TargetColor", typeof(Color), typeof(ChangeColorEffect),
                    new UIPropertyMetadata(System.Windows.Media.Colors.Black, PixelShaderConstantCallback(0)));
    }
}
