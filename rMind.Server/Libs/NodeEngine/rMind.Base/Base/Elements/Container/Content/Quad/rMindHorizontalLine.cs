using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace rMind.Content.Quad
{
    using Nodes;

    public class rMindHorizontalLine : rMindLine
    {
        public rMindHorizontalLine(rMindQuadContainer parent) : base(parent)
        {
            m_line_orientation = rMindLineOrientation.Horizontal;
        }

        public List<rMindBaseNode> LeftNodes { get { return NodesA; } }
        public List<rMindBaseNode> RightNodes { get { return NodesB; } }

        public rMindBaseNode AddLeftNode()
        {
            int currentCount = m_parent.HLines.Max(line => line.LeftNodes.Count);
            if (LeftNodes.Count + 1 > currentCount)
            {
                /*
                 * если количество верхних узлов равно максимальному количеству по линиям
                 * добавляем новую строку.
                 */
                m_parent.Template.ColumnDefinitions.Add(new ColumnDefinition() {
                    Width = GridLength.Auto,
                    MinWidth = 24
                });

                foreach (var line in m_parent.HLines)
                    line.ShiftNodes(1);

                foreach (var line in m_parent.VLines)
                {
                    foreach (var n in line.TopNodes.Union(line.BottomNodes))
                        n.Column += 1;
                }                
            }

            var node = m_parent.CreateNode();

            int idx = currentCount - LeftNodes.Count - 1;
            node.SetCell(idx > 0 ? idx : 0, m_parent.GetLineIndex(this));
            node.NodeOrientation = rMindNodeOriantation.Left;
            LeftNodes.Add(node);

            m_parent.UpdateBase();

            return node;
        }

        public rMindBaseNode AddRightNode()
        {
            int currentCount = m_parent.HLines.Max(line => line.RightNodes.Count);
            if (RightNodes.Count + 1 > currentCount)
            {
                m_parent.Template.ColumnDefinitions.Add(new ColumnDefinition() {
                    Width = GridLength.Auto,
                    MinWidth = 24
                });
            }

            var node = m_parent.CreateNode();

            var offset = m_parent.GetNodeOffset();
            node.SetCell(offset.Column + offset.VLines + RightNodes.Count, m_parent.GetLineIndex(this));
            node.NodeOrientation = rMindNodeOriantation.Right;
            RightNodes.Add(node);

            return node;
        }

        public override void ShiftNodes(int offset)
        {
            foreach (var node in LeftNodes.Union(RightNodes))
                node.Column += 1;
        }

        protected override void RemoveANode(rMindBaseNode node)
        {
            var outMax = m_parent.HLines.Where(x => x != this).Max(x => x.LeftNodes.Count);            
            if (LeftNodes.Count > outMax)
            {
                /* Если количество занимаемых колонок больше, чем у остальных строк
                 * Нужно удалять колонку грида и сдвинуть все узлы. Кроме своих.
                 */
                var nodes = m_parent.HLines.Where(x => x != this).Select(x => x as rMindLine)
                    .Union(m_parent.VLines.Select(x => x as rMindLine))
                    .Select(x => x.NodesA.Union(x.NodesB));

                foreach(var arr in nodes)
                {
                    foreach(var n in arr)
                    {
                        n.Column--;
                    }
                }

                // Среди своих узлов удаляем только те, что правее
                var offsetNodes = LeftNodes.Union(RightNodes).Where(x => x.Column > node.Column);
                foreach (var n in offsetNodes)
                    n.Column--;

                if (m_parent.Template.ColumnDefinitions.Count > 1)
                {
                    m_parent.Template.ColumnDefinitions.Remove(
                        m_parent.Template.ColumnDefinitions.Last()
                    );
                }
            }
            else
            {
                var offsetNodes = LeftNodes.Where(x => x.Column < node.Column);
                foreach (var n in offsetNodes)
                    n.Column++;
            }

            LeftNodes.Remove(node);
            m_parent.UpdateBase();
            m_parent.RemoveNode(node);
        }

        protected override void RemoveBNode(rMindBaseNode node)
        {
            var outMax = m_parent.HLines.Where(x => x != this).Max(x => x.RightNodes.Count);
            if (RightNodes.Count > outMax)
            {
                if (m_parent.Template.ColumnDefinitions.Count > 1)
                {
                    m_parent.Template.ColumnDefinitions.Remove(
                        m_parent.Template.ColumnDefinitions.Last()
                    );
                }
            }

            var offsetNodes = RightNodes.Where(x => x.Column > node.Column);
            foreach (var n in offsetNodes)
                n.Column--;

            RightNodes.Remove(node);
            m_parent.RemoveNode(node);
        }
    }

}
