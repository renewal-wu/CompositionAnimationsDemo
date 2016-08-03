using System;
using System.Numerics;
using System.Threading.Tasks;
using Windows.UI.Composition;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;

namespace CompositionAnimationsSample.DemoPages
{
    public sealed partial class DemoPage6 : Page
    {
        private Compositor compositor;
        private CompositionPropertySet propertySet;

        public DemoPage6()
        {
            this.InitializeComponent();

            SurfaceLoader.Initialize(ElementCompositionPreview.GetElementVisual(this).Compositor);

            compositor = ElementCompositionPreview.GetElementVisual(this).Compositor;

            StartExpressionAnimation();
        }

        private async void StartExpressionAnimation()
        {
            // 將 containerVisual 與 spriteVisual 新增至 TargetGrid 中
            var containerVisual = compositor.CreateContainerVisual();
            var spriteVisual = compositor.CreateSpriteVisual();
            containerVisual.Children.InsertAtTop(spriteVisual);
            ElementCompositionPreview.SetElementChildVisual(TargetGrid, containerVisual);

            // 讀圖
            var targetBrush = await GenerateCompositionBrush();

            // 將圖片讀進 spriteVisual
            spriteVisual.Brush = targetBrush;
            spriteVisual.Size = new Vector2(300f, 300f);
            spriteVisual.CenterPoint = new Vector3(150f, 150f, 0f);

            // 產生 PropertySet，並定義 Option1、Option2
            propertySet = compositor.CreatePropertySet();
            propertySet.InsertScalar("Option1", 0.0f);
            propertySet.InsertScalar("Option2", 0.0f);

            //// 取得 propertySet 內的屬性
            // float currentOption1;
            // propertySet.TryGetScalar("Option1", out currentOption1);

            // Opacity 會根據 Option1 呈現線性動畫
            var expressionAnimation = compositor.CreateExpressionAnimation();
            expressionAnimation.SetReferenceParameter("PropertySet", propertySet);
            expressionAnimation.Expression = "PropertySet.Option1";
            spriteVisual.StartAnimation("Opacity", expressionAnimation);

            // Scale.X 與 Scale.Y 會根據 Option2 呈現平方根動畫
            expressionAnimation = compositor.CreateExpressionAnimation();
            expressionAnimation.SetReferenceParameter("PropertySet", propertySet);
            expressionAnimation.Expression = "Square(PropertySet.Option2)";
            spriteVisual.StartAnimation("Scale.X", expressionAnimation);
            spriteVisual.StartAnimation("Scale.Y", expressionAnimation);
        }

        private async Task<CompositionBrush> GenerateCompositionBrush()
        {
            var surface = await SurfaceLoader.LoadFromUri(new Uri("ms-appx:///Assets/KKBOX.png"));
            return compositor.CreateSurfaceBrush(surface);
        }

        private void SetOption(float optionX, float optionY)
        {
            var scalarAnimation = compositor.CreateScalarKeyFrameAnimation();
            scalarAnimation.Duration = TimeSpan.FromMilliseconds(800);
            scalarAnimation.InsertKeyFrame(1f, optionX);
            propertySet.StartAnimation("Option1", scalarAnimation);

            scalarAnimation = compositor.CreateScalarKeyFrameAnimation();
            scalarAnimation.Duration = TimeSpan.FromMilliseconds(800);
            scalarAnimation.InsertKeyFrame(1f, optionY);
            propertySet.StartAnimation("Option2", scalarAnimation);
        }

        private void Button_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            SetOption(1f, 1f);
        }

        private void Button_Click_1(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            SetOption(0f, 0f);
        }
    }
}
