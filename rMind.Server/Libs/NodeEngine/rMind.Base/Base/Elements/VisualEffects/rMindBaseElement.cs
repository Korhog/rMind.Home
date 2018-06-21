using System;
using System.Linq;
using System.Collections.Generic;

using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Xaml.Input;

using Newtonsoft.Json;

namespace rMind.Elements
{
    using Draw;
    using Types;
    using Theme;
    using Storage;
    using ColorContainer;
    using Nodes;
    using Input;
    using Windows.UI.Xaml.Hosting;
    using System.Numerics;
    using Windows.UI.Composition;

    /// <summary>
    /// [visual] Base controller element 
    /// </summary>   
    public partial class rMindBaseElement : rMindBaseItem, IDrawContainer, IStorageObject, IInteractElement
    {
        public virtual void AddEffect()
        {
#warning еффекты пока работают через анус, поэтому не используем
            return;
            var shadowHost = Template;

            Visual hostVisual = ElementCompositionPreview.GetElementVisual(shadowHost);
            Compositor compositor = hostVisual.Compositor;

            // Create a drop shadow
            var dropShadow = compositor.CreateDropShadow();
            dropShadow.Color = Colors.Black;
            dropShadow.BlurRadius = 5.0f;
            dropShadow.Offset = new Vector3(15.0f, 15.0f, 0.0f);
            // Associate the shape of the shadow with the shape of the target element
            dropShadow.Mask = m_shadow_mask.GetAlphaMask();

            // Create a Visual to hold the shadow
            var shadowVisual = compositor.CreateSpriteVisual();
            shadowVisual.Shadow = dropShadow;

            // Add the shadow as a child of the host in the visual tree
            ElementCompositionPreview.SetElementChildVisual(shadowHost, shadowVisual);

            // Make sure size of shadow host and shadow visual always stay in sync
            var bindSizeAnimation = compositor.CreateExpressionAnimation("hostVisual.Size");
            bindSizeAnimation.SetReferenceParameter("hostVisual", hostVisual);

            shadowVisual.StartAnimation("Size", bindSizeAnimation);
        }
    }
}
