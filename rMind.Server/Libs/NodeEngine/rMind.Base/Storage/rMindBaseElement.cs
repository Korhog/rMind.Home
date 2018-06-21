using rMind.Draw;

using System;
using System.Xml.Linq;

namespace rMind.Elements
{
    using Storage;
    using ColorContainer;
    using Nodes;
    /// <summary>
    /// Base scheme element 
    /// </summary>
    public partial class rMindBaseElement : rMindBaseItem, IDrawContainer, IStorageObject
    {
        public long Index { get { return GetController()?.GetIndexOfElement(this) ?? -1; } }

        public virtual XElement Serialize()
        {
            var itemNode = new XElement("item");
            // attributes
            itemNode.Add(new XAttribute("type", m_element_type.ToString()));
            itemNode.Add(new XAttribute("x", Math.Round(Position.X)));
            itemNode.Add(new XAttribute("y", Math.Round(Position.Y)));

            itemNode.Add(OptionsNode());

            if (m_inner_controller != null)
            {
                itemNode.Add(m_inner_controller.Serialize());
            }

            return itemNode;
        }

        protected virtual XElement OptionsNode()
        {
            var optionsNode = new XElement("options");
            return optionsNode;
        }

        public virtual void Deserialize(XElement node)
        {
            var sX = node.Attribute("x")?.Value;
            var sY = node.Attribute("y")?.Value;

            var x = 0.0;
            var y = 0.0;

            double.TryParse(sX, out x);
            double.TryParse(sY, out y);

            SetPosition(x, y);

            var controllerNode = node.Element("controller");
            if (controllerNode != null)            
                InnerController.Deserialize(controllerNode);
            

            var optionsNode = node.Element("options");
            if (optionsNode != null)
                DeserializeOptions(optionsNode);

        }

        protected virtual void DeserializeOptions(XElement optionsNode)
        {

        }
    }
}
