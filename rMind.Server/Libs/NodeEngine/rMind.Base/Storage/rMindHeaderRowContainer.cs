using Windows.UI;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Xaml.Controls;


namespace rMind.Content
{
    using Theme;
    using System.Xml.Linq;

    public partial class rMindHeaderRowContainer : rMindRowContainer
    {
        public override void Deserialize(XElement node)
        {            
            base.Deserialize(node);
        }

        protected override void DeserializeOptions(XElement optionsNode)
        {            
            var color = ColorContainer.rMindColors.Deserialize(optionsNode.Attribute("color")?.Value);
            AccentColor = color;

            var expanded = false;
            bool.TryParse(optionsNode.Attribute("expanded")?.Value, out expanded);
            m_expanded = !expanded;
            Expand();

            base.DeserializeOptions(optionsNode);
        }

        public override XElement Serialize()
        {
            var node = base.Serialize();
            return node;
        }
    }
}
