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

    public class rMindVerticalLine : rMindLine
    {
        public rMindVerticalLine(rMindQuadContainer parent) : base(parent)
        {
            m_line_orientation = rMindLineOrientation.Vertical;
        }

        public List<rMindBaseNode> TopNodes { get { return NodesA; } }
        public List<rMindBaseNode> BottomNodes { get { return NodesB; } }

        public rMindBaseNode AddTopNode()
        {
            int currentCount = m_parent.VLines.Max(line => line.TopNodes.Count);
            if (TopNodes.Count + 1 > currentCount)
            {
                /*
                 * если количество верхних узлов равно максимальному количеству по линиям
                 * добавляем новую строку.
                 */
                m_parent.Template.RowDefinitions.Add(new RowDefinition() {
                    Height = GridLength.Auto,
                    MinHeight = 24
                });

                foreach (var line in m_parent.VLines)
                    line.ShiftNodes(1);

                foreach (var line in m_parent.HLines)
                {
                    foreach (var n in line.LeftNodes.Union(line.RightNodes))
                        n.Row += 1;
                }      
            } 
            
            var node = m_parent.CreateNode();

            int idx = currentCount - TopNodes.Count - 1;
            //if (idx == 0)
            //    ShiftNodes(1);

            node.SetCell(m_parent.GetLineIndex(this), idx > 0 ? idx : 0);
            node.NodeOrientation = rMindNodeOriantation.Top;
            TopNodes.Add(node);

            m_parent.UpdateBase();

            return node;
        }

        public rMindBaseNode AddBottomNode()
        {
            int currentCount = m_parent.VLines.Max(line => line.BottomNodes.Count);
            if (BottomNodes.Count + 1 > currentCount)
            {
                m_parent.Template.RowDefinitions.Add(new RowDefinition() {
                    Height = GridLength.Auto,
                    MinHeight = 24
                });
            }

            var node = m_parent.CreateNode();
            var offset = m_parent.GetNodeOffset();
            var idx = offset.Row + offset.HLines + BottomNodes.Count;    

            node.SetCell(m_parent.GetLineIndex(this), idx);
            node.NodeOrientation = rMindNodeOriantation.Bottom;
            BottomNodes.Add(node);

            return node;
        }

        public override void ShiftNodes(int offset)
        {
            foreach (var node in TopNodes.Union(BottomNodes))
                node.Row += 1;
        }

        protected override void RemoveANode(rMindBaseNode node)
        {
            var outMax = m_parent.VLines.Where(x => x != this).Max(x => x.TopNodes.Count);
            if (TopNodes.Count > outMax)
            {
                /* Если количество занимаемых колонок больше, чем у остальных строк
                 * Нужно удалять колонку грида и сдвинуть все узлы. Кроме своих.
                 */
                var nodes = m_parent.VLines.Where(x => x != this).Select(x => x as rMindLine)
                    .Union(m_parent.HLines.Select(x => x as rMindLine))
                    .Select(x => x.NodesA.Union(x.NodesB));

                foreach (var arr in nodes)
                {
                    foreach (var n in arr)
                    {
                        n.Row--;
                    }
                }

                // Среди своих узлов удаляем только те, что правее
                var offsetNodes = TopNodes.Union(BottomNodes).Where(x => x.Row > node.Row);
                foreach (var n in offsetNodes)
                    n.Row--;

                if (m_parent.Template.RowDefinitions.Count > 1)
                {
                    m_parent.Template.RowDefinitions.Remove(
                        m_parent.Template.RowDefinitions.Last()
                    );
                }
            }
            else
            {
                var offsetNodes = TopNodes.Where(x => x.Row < node.Row);
                foreach (var n in offsetNodes)
                    n.Row++;
            }

            TopNodes.Remove(node);
            m_parent.UpdateBase();
            m_parent.RemoveNode(node);
        }

        protected override void RemoveBNode(rMindBaseNode node)
        {
            var outMax = m_parent.VLines.Where(x => x != this).Max(x => x.BottomNodes.Count);
            if (BottomNodes.Count > outMax)
            {
                if (m_parent.Template.RowDefinitions.Count > 1)
                {
                    m_parent.Template.RowDefinitions.Remove(
                        m_parent.Template.RowDefinitions.Last()
                    );
                }
            }

            var offsetNodes = BottomNodes.Where(x => x.Row > node.Row);
            foreach (var n in offsetNodes)
                n.Row--;

            BottomNodes.Remove(node);
            m_parent.RemoveNode(node);
        }
    }
}
