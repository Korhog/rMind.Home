using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rMind.Content.Quad
{
    using Nodes;

    public enum rMindLineOrientation
    {
        Vertical,
        Horizontal
    }

    public class rMindLine
    {
        protected rMindQuadContainer m_parent;

        protected List<rMindBaseNode> m_a_node_list;
        protected List<rMindBaseNode> m_b_node_list;

        public List<rMindBaseNode> NodesA
        {
            get
            {
                if (m_a_node_list == null)
                {
                    m_a_node_list = new List<rMindBaseNode>();
                }
                return m_a_node_list;
            }
        }
        public List<rMindBaseNode> NodesB
        {
            get
            {
                if (m_b_node_list == null)
                {
                    m_b_node_list = new List<rMindBaseNode>();
                }
                return m_b_node_list;
            }
        }

        protected rMindLineOrientation m_line_orientation = rMindLineOrientation.Horizontal;

        public rMindLine(rMindQuadContainer parent)
        {
            m_parent = parent;
        }

        /// <summary>
        /// Устанавливает кольчиство строк\колонок между А и Б нодами
        /// </summary>
        /// <param name="size"></param>
        public virtual void SetContentLength(int size) { }

        public virtual void ShiftNodes(int offset) { }

        public void RemoveNode(rMindBaseNode node)
        {
            if (NodesA.Contains(node))
            {
                RemoveANode(node);
                return;
            }

            if (NodesB.Contains(node))
            {
                RemoveBNode(node);
            }
        }

        protected virtual void RemoveANode(rMindBaseNode node) { }

        protected virtual void RemoveBNode(rMindBaseNode node) { }

    }
}
