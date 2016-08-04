using Microsoft.Graphics.Canvas.Effects;
using System;
using System.Numerics;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Composition;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;

namespace CompositionAnimationsSample.DemoPages
{
    public sealed partial class DemoPage4 : Page
    {
        private Visual rootVisual;
        private Compositor compositor;

        public DemoPage4()
        {
            this.InitializeComponent();

            // SurfaceLoader 是微軟幫我們寫好的元件
            // 可以透過它，以 Win2D 的方式讀圖
            // *** 用之前要 call Initialize(); 不用了要 call Uninitialize();
            SurfaceLoader.Initialize(ElementCompositionPreview.GetElementVisual(this).Compositor);

            rootVisual = ElementCompositionPreview.GetElementVisual(this);
            compositor = rootVisual.Compositor;

            ReadImageBySurfaceLoader();
            //StartGraphicsEffect();
            //StartMultipleGraphicsEffect();
        }

        /// <summary>
        /// 用 SurfaceLoader 讀圖
        /// </summary>
        private async void ReadImageBySurfaceLoader()
        {
            // 將 containerVisual 與 spriteVisual 新增至 DemoPage4 中
            var containerVisual = compositor.CreateContainerVisual();
            var spriteVisual = compositor.CreateSpriteVisual();
            containerVisual.Children.InsertAtTop(spriteVisual);
            ElementCompositionPreview.SetElementChildVisual(this, containerVisual);

            // 讀圖
            var targetBrush = await GenerateCompositionBrush();

            // 將圖片讀進 spriteVisual
            spriteVisual.Brush = targetBrush;
            spriteVisual.Size = new Vector2(300f, 300f);
        }

        private async Task<CompositionBrush> GenerateCompositionBrush()
        {
            var surface = await SurfaceLoader.LoadFromUri(new Uri("ms-appx:///Assets/KKBOX.png"));
            return compositor.CreateSurfaceBrush(surface);
        }

        /// <summary>
        /// 用 SurfaceLoader 讀圖，並跑高斯模糊動畫
        /// </summary>
        private async void StartGraphicsEffect()
        {
            // 將 containerVisual 與 spriteVisual 新增至 DemoPage4 中
            var containerVisual = compositor.CreateContainerVisual();
            var spriteVisual = compositor.CreateSpriteVisual();
            containerVisual.Children.InsertAtTop(spriteVisual);
            ElementCompositionPreview.SetElementChildVisual(this, containerVisual);

            // 新增效果元件
            var blurEffect = new GaussianBlurEffect
            {
                Name = "blur",
                BorderMode = EffectBorderMode.Hard,
                BlurAmount = 0,
                Source = new CompositionEffectSourceParameter("source")
            };
            var blurEffectFactory = compositor.CreateEffectFactory(blurEffect, new[] { "blur.BlurAmount" });
            var effectBrush = blurEffectFactory.CreateBrush();

            // 讀圖
            var targetBrush = await GenerateCompositionBrush();

            // 將圖片指定給效果元件，並命名為 "source"
            effectBrush.SetSourceParameter("source", targetBrush);

            // 將效果讀進 spriteVisual
            spriteVisual.Brush = effectBrush;
            spriteVisual.Size = new Vector2(300f, 300f);

            // 對效果元件的 "blur.BlurAmount" 屬性做動畫
            ScalarKeyFrameAnimation blurAnimation = compositor.CreateScalarKeyFrameAnimation();
            blurAnimation.InsertKeyFrame(0.0f, 0.0f);
            blurAnimation.InsertKeyFrame(0.5f, 100.0f);
            blurAnimation.InsertKeyFrame(1.0f, 0.0f);
            blurAnimation.Duration = TimeSpan.FromSeconds(4);
            blurAnimation.IterationBehavior = AnimationIterationBehavior.Forever;
            effectBrush.StartAnimation("blur.BlurAmount", blurAnimation);
        }

        /// <summary>
        /// 用 SurfaceLoader 讀圖，並跑高斯模糊 + 飽和度動畫
        /// </summary>
        private async void StartMultipleGraphicsEffect()
        {
            // 將 containerVisual 與 spriteVisual 新增至 DemoPage4 中
            var containerVisual = compositor.CreateContainerVisual();
            var spriteVisual = compositor.CreateSpriteVisual();
            containerVisual.Children.InsertAtTop(spriteVisual);
            ElementCompositionPreview.SetElementChildVisual(this, containerVisual);

            // 新增 GaussianBlurEffect
            var blurEffect = new GaussianBlurEffect
            {
                Name = "blur",
                BorderMode = EffectBorderMode.Hard,
                BlurAmount = 0,

                // 新增 SaturationEffect
                Source = new SaturationEffect()
                {
                    Name = "saturation",
                    Saturation = 0,
                    Source = new CompositionEffectSourceParameter("source")
                }
            };

            var blurEffectFactory = compositor.CreateEffectFactory(blurEffect, new[] { "blur.BlurAmount", "saturation.Saturation" });
            var effectBrush = blurEffectFactory.CreateBrush();

            // 讀圖
            var targetBrush = await GenerateCompositionBrush();

            // 將圖片指定給效果元件，並命名為 "source"
            effectBrush.SetSourceParameter("source", targetBrush);

            // 將效果讀進 spriteVisual
            spriteVisual.Brush = effectBrush;
            spriteVisual.Size = new Vector2(300f, 300f);

            // 對效果元件的 "blur.BlurAmount" 屬性做動畫
            ScalarKeyFrameAnimation blurAnimation = compositor.CreateScalarKeyFrameAnimation();
            blurAnimation.InsertKeyFrame(0.0f, 0.0f);
            blurAnimation.InsertKeyFrame(0.5f, 100.0f);
            blurAnimation.InsertKeyFrame(1.0f, 0.0f);
            blurAnimation.Duration = TimeSpan.FromSeconds(4);
            blurAnimation.IterationBehavior = AnimationIterationBehavior.Forever;
            effectBrush.StartAnimation("blur.BlurAmount", blurAnimation);

            // 對效果元件的 "saturation.Saturation" 屬性做動畫
            ScalarKeyFrameAnimation saturationAnimation = compositor.CreateScalarKeyFrameAnimation();
            saturationAnimation.InsertKeyFrame(0.0f, 0.0f);
            saturationAnimation.InsertKeyFrame(1.0f, 1.0f);
            saturationAnimation.Duration = TimeSpan.FromSeconds(4);
            saturationAnimation.IterationBehavior = AnimationIterationBehavior.Forever;
            effectBrush.StartAnimation("saturation.Saturation", saturationAnimation);
        }
    }
}
