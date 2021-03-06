﻿using Esri.ArcGISRuntime.Layers;
using Esri.ArcGISRuntime.Symbology;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ArcGISRuntimeSDKDotNet_DesktopSamples.Samples
{
    /// <summary>
    /// Sample shows how to create a SimpleRenderer for a graphics layer in XAML and code. User added points will be added to the graphics layer and rendered using the GraphicsLayer Renderer.
    /// </summary>
    /// <title>Simple Renderer</title>
	/// <category>Symbology</category>
	public partial class SimpleRendererSample : UserControl
    {
        private Random _random = new Random();

        /// <summary>Construct Simple Renderer sample control</summary>
        public SimpleRendererSample()
        {
            InitializeComponent();

            mapView.ExtentChanged += mapView_ExtentChanged;
        }

        // Start map interaction
        private async void mapView_ExtentChanged(object sender, EventArgs e)
        {
            mapView.ExtentChanged -= mapView_ExtentChanged;
            await AcceptPointsAsync();
        }

        // Change the graphics layer renderer to a new SimpleRenderer
        private void ChangeRendererButton_Click(object sender, RoutedEventArgs e)
        {
            graphicsLayer.Renderer = new SimpleRenderer() { Symbol = GetRandomSymbol() };
        }

        // Accept user map clicks and add points to the graphics layer (use the default symbol from renderer)
        private async Task AcceptPointsAsync()
        {
            try
            {
                while (mapView.Extent != null)
                {
                    var point = await mapView.Editor.RequestPointAsync();
                    graphicsLayer.Graphics.Add(new Graphic(point));
                }
            }
            catch (TaskCanceledException)
            {
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Simple Renderer Sample");
            }
        }

        // Utility: Generate a random simple marker symbol
        private SimpleMarkerSymbol GetRandomSymbol()
        {
            return new SimpleMarkerSymbol()
            {
                Size = 15,
                Color = GetRandomColor(),
                Style = (SimpleMarkerStyle)_random.Next(0, 6)
            };
        }
        
        // Utility function: Generate a random System.Windows.Media.Color
        private Color GetRandomColor()
        {
            var colorBytes = new byte[3];
            _random.NextBytes(colorBytes);
            return Color.FromRgb(colorBytes[0], colorBytes[1], colorBytes[2]);
        }
    }
}
