using System;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

using System.Collections.Generic;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace rMind.Elements
{
    using Storage;
    using Nodes;
    /// <summary>
    /// Starage section
    /// </summary>
    public partial class rMindBaseController : IStorageObject
    {
        public rMindBaseNode GetNodeByIndexPair(int itemIdx, int nodeIdx)
        {
            rMindBaseElement item = null;
            if (m_items.Count > itemIdx)
            {
                item = m_items[itemIdx];
                if (item.Nodes.Count > nodeIdx)
                {
                    return item.Nodes[nodeIdx];
                }
            }
            return null;            
        }
        

        #region Serialize
        public virtual XElement Serialize()
        {           
            var controller = new XElement("controller");
            var attribure = new XAttribute("name", Name);
            controller.Add(attribure);

            controller.Add(ItemsNode());
            controller.Add(WiresNode());

            return controller;
        }

        protected virtual XElement ItemsNode()
        {
            var itemsNode = new XElement("items");
            foreach (var item in m_items.Where(x => x.Storable))
            {
                var itemNode = item.Serialize();
                if (itemNode == null)
                    continue;

                itemsNode.Add(itemNode);
            } 
            return itemsNode;
        }

        protected virtual XElement WiresNode()
        {
            var wiresNode = new XElement("wires");
            foreach (var wire in m_wire_list)
            {
                var wireNode = wire.Serialize();
                if (wireNode == null)
                    continue;

                wiresNode.Add(wireNode);
            }
            return wiresNode;
        }
        #endregion

        #region Deserialize

        public virtual void Deserialize(string xml)
        {
            var node = XElement.Parse(xml);
            Deserialize(node);
        }


        public virtual void Deserialize(XElement node)
        {
            if (node == null)
                return;

            var itemsNode = node.Elements("items").FirstOrDefault();
            DeserializeItems(itemsNode);

            var wiresNode = node.Elements("wires").FirstOrDefault();
            DeserializeWires(wiresNode);
        }

        protected virtual void DeserializeItems(XElement itemsNode)
        {
            if (itemsNode == null)
                return;

            foreach (var itemNode in itemsNode.Elements("item"))
                DeserializeItem(itemNode);                
        }

        protected virtual void DeserializeItem(XElement itemNode)
        {
            rElementType type = rElementType.RET_NONE;
            Enum.TryParse(itemNode.Attribute("type")?.Value, out type);
            var item = CreateElementByElementType(type, true); 
            item.Deserialize(itemNode);
            Draw(item);
            item.Template.UpdateLayout();            
        }

        protected virtual void DeserializeWires(XElement wiresNode)
        {
            if (wiresNode == null)
                return;

            foreach (var wireNode in wiresNode.Elements("wire"))
                DeserializeWire(wireNode);
        }

        protected virtual void DeserializeWire(XElement wireNode)
        {
            var wire = CreateWire();
            wire.Deserialize(wireNode);
        }

        #endregion
    }
}

