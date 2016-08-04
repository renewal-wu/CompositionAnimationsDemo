using SamplesCommon;
using System;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Windows.UI.Composition;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;

namespace CompositionAnimationsSample.DemoPages
{
    public sealed partial class DemoPage5 : Page
    {
        private Compositor compositor;

        public DemoPage5()
        {
            this.InitializeComponent();

            SurfaceLoader.Initialize(ElementCompositionPreview.GetElementVisual(this).Compositor);

            compositor = ElementCompositionPreview.GetElementVisual(this).Compositor;

            StartExpressionAnimation();
            //ParallaxScrollingDemoListView.Loaded += ParallaxScrollingDemoListView_Loaded;
        }

        private async void StartExpressionAnimation()
        {
            // 將 containerVisual 與 spriteVisual 新增至 DemoPage5 中
            var containerVisual = compositor.CreateContainerVisual();
            var spriteVisual = compositor.CreateSpriteVisual();
            containerVisual.Children.InsertAtTop(spriteVisual);
            ElementCompositionPreview.SetElementChildVisual(this, containerVisual);

            // 讀圖
            var targetBrush = await GenerateCompositionBrush();

            // 將圖片讀進 spriteVisual
            spriteVisual.Brush = targetBrush;
            spriteVisual.Size = new Vector2(300f, 300f);

            // 取得 ScrollViewer PropertySet
            CompositionPropertySet scrollProperties = ElementCompositionPreview.GetScrollViewerManipulationPropertySet(TargetScrollViewer);

            // Expression animation 規則
            // 捲軸向下垂直捲動的距離 > TopBreak 時，物件垂直位移為 [0]
            // 上述條件不符時，物件垂直位移為 [TopBreak] - [捲軸向下垂直捲動的距離]

            // ***捲軸向下垂直捲動距離為負數，越往下捲越小
            // 故以 [-ScrollManipulation.Translation.Y] 表示 [捲軸向下垂直捲動的距離]
            const float topBreak = 500;

            var expressionAnimation = compositor.CreateExpressionAnimation();
            expressionAnimation.SetScalarParameter("TopBreak", topBreak);
            expressionAnimation.SetReferenceParameter("ScrollManipulation", scrollProperties);
            expressionAnimation.Expression = "-ScrollManipulation.Translation.Y > TopBreak ? 0 : TopBreak - (-ScrollManipulation.Translation.Y)";
            spriteVisual.StartAnimation("Offset.Y", expressionAnimation);
        }

        private async Task<CompositionBrush> GenerateCompositionBrush()
        {
            var surface = await SurfaceLoader.LoadFromUri(new Uri("ms-appx:///Assets/KKBOX.png"));
            return compositor.CreateSurfaceBrush(surface);
        }

        private void ParallaxScrollingDemoListView_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            StartParallaxScrollingAnimation();
        }

        /// <summary>
        /// 視差捲動動畫
        /// </summary>
        private async void StartParallaxScrollingAnimation()
        {
            TargetScrollViewer.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            ParallaxScrollingContainer.Visibility = Windows.UI.Xaml.Visibility.Visible;

            // 將 containerVisual 與 spriteVisual 新增至 DemoImageContainer 中
            var containerVisual = compositor.CreateContainerVisual();
            var spriteVisual = compositor.CreateSpriteVisual();
            containerVisual.Children.InsertAtTop(spriteVisual);
            ElementCompositionPreview.SetElementChildVisual(DemoImageContainer, containerVisual);

            // 讀圖
            var targetBrush = await GenerateCompositionBrush();

            // 將圖片讀進 spriteVisual
            spriteVisual.Brush = targetBrush;
            spriteVisual.Size = new Vector2(300f, 300f);

            // 取得 ListView 內的 ScrollViewer
            ParallaxScrollingDemoListView.UpdateLayout();
            var scrollViewer = ParallaxScrollingDemoListView.GetDescendantsOfType<ScrollViewer>().FirstOrDefault();
            if (scrollViewer == null)
            {
                return;
            }

            // 取得 ScrollViewer PropertySet
            CompositionPropertySet scrollProperties = ElementCompositionPreview.GetScrollViewerManipulationPropertySet(TargetScrollViewer);

            // 視差捲動
            var expressionAnimation = compositor.CreateExpressionAnimation();
            expressionAnimation.SetReferenceParameter("ScrollManipulation", scrollProperties);
            expressionAnimation.Expression = "-ScrollManipulation.Translation.Y * 0.2";
            spriteVisual.StartAnimation("Offset.Y", expressionAnimation);
        }
    }
}
