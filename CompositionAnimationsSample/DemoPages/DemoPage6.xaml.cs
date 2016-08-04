using System;
using System.Numerics;
using System.Threading.Tasks;
using Windows.UI.Composition;
using Windows.UI.Composition.Interactions;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Input;

namespace CompositionAnimationsSample.DemoPages
{
    public sealed partial class DemoPage6 : Page
    {
        private Visual rootVisual;
        private Compositor compositor;
        private InteractionTracker tracker;
        private VisualInteractionSource interactionSource;

        public DemoPage6()
        {
            this.InitializeComponent();

            SurfaceLoader.Initialize(ElementCompositionPreview.GetElementVisual(this).Compositor);

            rootVisual = ElementCompositionPreview.GetElementVisual(this);
            compositor = rootVisual.Compositor;

            AddInteractableImage();
        }

        private async void AddInteractableImage()
        {
            // 將 containerVisual 與 spriteVisual 新增至 DemoPage6 中
            var containerVisual = compositor.CreateContainerVisual();
            var spriteVisual = compositor.CreateSpriteVisual();
            containerVisual.Children.InsertAtTop(spriteVisual);
            ElementCompositionPreview.SetElementChildVisual(this, containerVisual);

            // 讀圖
            var targetBrush = await GenerateCompositionBrush();

            // 將圖片讀進 spriteVisual
            spriteVisual.Brush = targetBrush;
            spriteVisual.Size = new Vector2(300f, 300f);

            // 建立互動來源 (必須指定互動範圍區域)
            interactionSource = VisualInteractionSource.Create(rootVisual);

            // 設定互動方式
            interactionSource.PositionYSourceMode = InteractionSourceMode.EnabledWithInertia;

            // 建立 InteractionTracker
            tracker = InteractionTracker.Create(compositor);

            // 設定位移位置的最大值與最小值
            // 類似 ScrollViewer.ScrollableHeight, ScrollViewer.ScrollableWidth
            tracker.MaxPosition = new Vector3(0, 300f, 0);
            tracker.MinPosition = new Vector3();

            // 將互動來源加至 tracker
            tracker.InteractionSources.Add(interactionSource);

            // 用 tracker 來設定 animation
            const int topMargin = 100;
            var offsetYAnimation = compositor.CreateExpressionAnimation("-tracker.Position.Y + topMargin");
            offsetYAnimation.SetReferenceParameter("tracker", tracker);
            offsetYAnimation.SetScalarParameter("topMargin", topMargin);
            spriteVisual.StartAnimation("Offset.Y", offsetYAnimation);

            //=======================================================
            // 讓 interactionSource 支援 Scale

            //tracker.MaxScale = 3.0f;
            //tracker.MinScale = 0.5f;

            //interactionSource.ScaleSourceMode = InteractionSourceMode.EnabledWithoutInertia;

            //var scaleAnimation = compositor.CreateExpressionAnimation("tracker.Scale");
            //scaleAnimation.SetReferenceParameter("tracker", tracker);
            //spriteVisual.StartAnimation("Scale.X", scaleAnimation);
            //spriteVisual.StartAnimation("Scale.Y", scaleAnimation);

            //var maxYAnimation = compositor.CreateExpressionAnimation("spriteVisual.Size.Y * spriteVisual.Scale.Y");
            //maxYAnimation.SetReferenceParameter("spriteVisual", spriteVisual);
            //tracker.StartAnimation("MaxPosition.Y", maxYAnimation);
        }

        private async Task<CompositionBrush> GenerateCompositionBrush()
        {
            var surface = await SurfaceLoader.LoadFromUri(new Uri("ms-appx:///Assets/KKBOX.png"));
            return compositor.CreateSurfaceBrush(surface);
        }

        private void Pointer_Pressed(object sender, PointerRoutedEventArgs e)
        {
            if (e.Pointer.PointerDeviceType == Windows.Devices.Input.PointerDeviceType.Touch)
            {
                interactionSource.TryRedirectForManipulation(e.GetCurrentPoint(RootContainer));
            }
        }
    }
}
