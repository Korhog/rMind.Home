using System.Linq;
using System.Collections.Generic;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

using rMind.Types;

namespace rMind.Elements
{
    using Types;
    using Nodes;

    public partial class rMindBaseController : Storage.IStorageObject
    {
        List<KeyValuePair<Vector2, rMindBaseNode>> m_baked_nodes;        
        /// <summary>
        /// Create new wire
        /// </summary>
        public List<KeyValuePair<Vector2, rMindBaseNode>> BakedNodes {
            get
            {
                if (m_baked_nodes == null)
                    m_baked_nodes = new List<KeyValuePair<Vector2, rMindBaseNode>>();
                return m_baked_nodes;
            }
        }
        
        // Bske nodes for magnet
        public void BakeNodes(rMindBaseNode root)
        {
            BakedNodes.Clear();

            // Собираем все ноды
            var nodes = m_items
                .SelectMany(x => x.Nodes)
                .Where(node => node.CanAttach && node.Parent != root.Parent)
                .Where(node => node.ConnectionType == root.ConnectionType)
                .Where(node => node.NodeType != root.NodeType)
                .Select(node => new KeyValuePair<Vector2, rMindBaseNode>(node.GetOffset(), node));
            
            BakedNodes.AddRange(nodes);
        }

        public virtual rMindBaseWire CreateWire()
        {
            var wire = new rMindBaseWire(this);
            wire.A.Translate(new Vector2(20, 50));
            wire.B.Translate(new Vector2(10, 10));

            Draw(wire);

            m_wire_list.Add(wire);
            return wire;
        }

        protected virtual void Draw(rMindBaseWire wire)
        {
            if (m_subscribed)
            {
                m_canvas.Children.Add(wire.Line);
                m_canvas.Children.Add(wire.A.Template);
                m_canvas.Children.Add(wire.B.Template);
            }
        }

        public virtual void RemoveWire(rMindBaseWire wire)
        {
            if (m_subscribed)
            {
                m_canvas.Children.Remove(wire.A.Template);
                m_canvas.Children.Remove(wire.B.Template);
                m_canvas.Children.Remove(wire.Line);
            }

            m_wire_list.Remove(wire);
        }

        public void SetDragWireDot(rMindBaseWireDot item, PointerRoutedEventArgs e)
        {
            m_items_state.DragedWireDot = item;
            if (item == null)
                return;

            var node = item.ReverseDot.Node;
            if (node == null)
                return;

            item.Detach();
            BakeNodes(node);

            var p = e.GetCurrentPoint(m_canvas);
            item.SetPosition(new Vector2(p.Position.X, p.Position.Y));
        }
    }
}

