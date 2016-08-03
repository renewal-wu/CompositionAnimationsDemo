using Windows.UI.Composition;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;

namespace CompositionAnimationsSample.DemoPages
{
    public sealed partial class DemoPage1 : Page
    {
        private Compositor compositor;

        public DemoPage1()
        {
            //var visual = ElementCompositionPreview.GetElementVisual(this);
            //compositor = visual.Compositor;

            this.InitializeComponent();

            var visual = ElementCompositionPreview.GetElementVisual(this);
            compositor = visual.Compositor;

            ////////////////////////////////////////////////////////////////////////////////////////////////
            //
            // 不需要等 Control.Loaded ，就可以取到他的 Visual
            // 可見 XAML visual tree 與 Composition tree 的產生順序沒有先後之分
            //
            ////////////////////////////////////////////////////////////////////////////////////////////////

            // 試著取得 [沒有被加至 Visual Tree 的元件] 的 Visual 與 Compositor
            // 會有問題嗎???
            //var textBlock = new TextBlock();
            //var textBlockVisual = ElementCompositionPreview.GetElementVisual(textBlock);
            //var textBlockCompositor = textBlockVisual.Compositor;
        }
    }
}
