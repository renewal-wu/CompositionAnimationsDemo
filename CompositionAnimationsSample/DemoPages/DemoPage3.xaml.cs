using System;
using System.Numerics;
using Windows.UI;
using Windows.UI.Composition;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;

namespace CompositionAnimationsSample.DemoPages
{
    public sealed partial class DemoPage3 : Page
    {
        private Visual demoRectangleVisual;
        private Compositor compositor;

        public DemoPage3()
        {
            this.InitializeComponent();

            demoRectangleVisual = ElementCompositionPreview.GetElementVisual(DemoRectangle);
            compositor = demoRectangleVisual.Compositor;

            this.Loaded += DemoPage3_Loaded;
        }

        private void DemoPage3_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            //demoRectangleVisual.Opacity = 0;
            StartOpacityKeyFrameAnimation();
            //demoRectangleVisual.Opacity = 0;
        }

        private void StartOpacityKeyFrameAnimation()
        {
            var scalarAnimation = compositor.CreateScalarKeyFrameAnimation();
            scalarAnimation.Duration = TimeSpan.FromMilliseconds(1000);
            scalarAnimation.IterationBehavior = AnimationIterationBehavior.Forever;
            scalarAnimation.InsertKeyFrame(0.0f, 0.0f);
            scalarAnimation.InsertKeyFrame(1.0f, 1.0f);

            demoRectangleVisual.StartAnimation("Opacity", scalarAnimation);
        }
    }
}
