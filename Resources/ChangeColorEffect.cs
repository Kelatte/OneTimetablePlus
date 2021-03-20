using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media.Effects;
using System.IO;
using System.Drawing;
using System.Windows;

namespace OneTimetablePlus.Resources
{
    class ChangeColorEffect : ShaderEffect
    {
        private const string _kshaderAsBase64 = @"AAP///7/HwBDVEFCHAAAAE8AAAAAA///AQAAABwAAAAAAQAASAAAADAAAAADAAAAAQACADgAAAAAAAAAaW5wdXQAq6sEAAwAAQABAAEAAAAAAAAAcHNfM18wAE1pY3Jvc29mdCAoUikgSExTTCBTaGFkZXIgQ29tcGlsZXIgMTAuMQCrUQAABQAAD6AAAIA/AAAAAAAAAAAAAAAAHwAAAgUAAIAAAAOQHwAAAgAAAJAACA+gQgAAAwAAD4AAAOSQAAjkoAEAAAIACAuAAADkgAEAAAIACASAAAAAoP//AAA=";
        private static readonly PixelShader _shader;
    
        static ChangeColorEffect()
        {
            _shader = new PixelShader();
            _shader.SetStreamSource(new MemoryStream(Convert.FromBase64String(_kshaderAsBase64)));
        }

        public ChangeColorEffect()
        {
            PixelShader = _shader;
            UpdateShaderValue(InputProperty);
        }

        public Brush Input
        {
            get { return (Brush)GetValue(InputProperty); }
            set { SetValue(InputProperty, value); }
        }

        public static readonly DependencyProperty InputProperty =
            ShaderEffect.RegisterPixelShaderSamplerProperty("Input", typeof(ChangeColorEffect), 0);

    }
}
