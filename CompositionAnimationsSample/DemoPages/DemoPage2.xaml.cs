using System;
using System.Numerics;
using Windows.UI;
using Windows.UI.Composition;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;

namespace CompositionAnimationsSample.DemoPages
{
    public sealed partial class DemoPage2 : Page
    {
        private Visual rootVisual;
        private Compositor compositor;

        public DemoPage2()
        {
            this.InitializeComponent();

            rootVisual = ElementCompositionPreview.GetElementVisual(this);
            compositor = rootVisual.Compositor;
        }

        /// <summary>
        /// 最基礎的 KeyFrameAnimation 範例
        /// </summary>
        private void StartBasicKeyFrameAnimation()
        {
            var vector3Animation = compositor.CreateVector3KeyFrameAnimation();
            vector3Animation.Duration = TimeSpan.FromMilliseconds(500);
            vector3Animation.IterationBehavior = AnimationIterationBehavior.Forever;
            vector3Animation.InsertKeyFrame(0.0f, new Vector3(1.0f, 1.0f, 0.0f));
            vector3Animation.InsertKeyFrame(0.5f, new Vector3(3.0f, 5.0f, 0.0f));

            var easingFunction = compositor.CreateLinearEasingFunction();
            vector3Animation.InsertKeyFrame(1.0f, new Vector3(1.0f, 1.0f, 0.0f), easingFunction);

            rootVisual.StartAnimation("Scale", vector3Animation);

            // KeyFrameAnimation 這麼多種，要怎麼知道該產生哪種 KeyFrameAnimation?

            // Live Demo
            // 對 rootVisual 的 Opacity 做動畫
        }

        /// <summary>
        /// 顏色動畫
        /// </summary>
        private void StartColorKeyFrameAnimation()
        {
            var containerVisual = compositor.CreateContainerVisual();
            ElementCompositionPreview.SetElementChildVisual(this, containerVisual);

            var colorBrush = compositor.CreateColorBrush(Colors.Red);
            var colorAnimation = compositor.CreateColorKeyFrameAnimation();
            colorAnimation.Duration = TimeSpan.FromMilliseconds(1000);
            colorAnimation.IterationBehavior = AnimationIterationBehavior.Count;
            colorAnimation.IterationCount = 2;
            colorAnimation.InsertKeyFrame(0.0f, Colors.Red);
            colorAnimation.InsertKeyFrame(1.0f, Colors.Yellow);

            var spriteVisual = compositor.CreateSpriteVisual();
            spriteVisual.Brush = colorBrush;
            spriteVisual.Size = new Vector2(100, 100);
            containerVisual.Children.InsertAtTop(spriteVisual);

            colorBrush.StartAnimation("Color", colorAnimation);

            //// 對 spriteVisual 的 Scale 做動畫
            //// 下列程式碼與範例一的程式碼幾乎一模一樣
            //var vector3Animation = compositor.CreateVector3KeyFrameAnimation();
            //vector3Animation.Duration = TimeSpan.FromMilliseconds(500);
            //vector3Animation.IterationBehavior = AnimationIterationBehavior.Forever;
            //vector3Animation.InsertKeyFrame(0.0f, new Vector3(1.0f, 1.0f, 0.0f));
            //vector3Animation.InsertKeyFrame(0.5f, new Vector3(3.0f, 5.0f, 0.0f));

            //var easingFunction = compositor.CreateLinearEasingFunction();
            //vector3Animation.InsertKeyFrame(1.0f, new Vector3(1.0f, 1.0f, 0.0f), easingFunction);

            //containerVisual.StartAnimation("Scale", vector3Animation);
        }

        #region Button click events
        private void Button_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            StartBasicKeyFrameAnimation();
        }

        private void Button_Click_1(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            StartColorKeyFrameAnimation();
        }
        #endregion
    }
}
