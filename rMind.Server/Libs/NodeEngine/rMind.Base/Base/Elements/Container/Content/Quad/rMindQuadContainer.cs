using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace rMind.Content
{
    using Elements;
    using Quad;

    public struct NodeOffset
    {
        public int Column;
        public int Row;

        public int VLines;
        public int HLines;

    }
    /// <summary>
    /// Контейнер с нодами, по 4-ем сторонам
    /// </summary>
    public class rMindQuadContainer : rMindBaseElement
    {
        protected List<rMindVerticalLine> m_vertical_lines;
        protected List<rMindHorizontalLine> m_horizontal_lines;

        public List<rMindVerticalLine> VLines { get { return m_vertical_lines; }}
        public List<rMindHorizontalLine> HLines { get { return m_horizontal_lines; } }

        public rMindQuadContainer(rMindBaseController parent) : base(parent)
        {
            m_vertical_lines = new List<rMindVerticalLine>();
            m_horizontal_lines = new List<rMindHorizontalLine>();
        }

        public override void Init()
        {
            base.Init();
            Template.ColumnDefinitions[0].MinWidth = 24;
            Template.RowDefinitions[0].MinHeight = 24;            
        }

        /// <summary>
        /// Возвращает 
        /// </summary>
        public NodeOffset GetNodeOffset()
        {
            return new NodeOffset
            {
                Row = m_vertical_lines.Count == 0 ? 0 : m_vertical_lines.Max(x => x.TopNodes.Count),
                Column = m_horizontal_lines.Count == 0 ? 0 : m_horizontal_lines.Max(x => x.LeftNodes.Count),
                HLines = m_horizontal_lines.Count,
                VLines = m_vertical_lines.Count                
            };
        }

        public rMindLine CreateLine<T>()
        {
            rMindLine line = null;
            if (typeof(T) == typeof(rMindHorizontalLine))
            {
                line = CreateHorizontalLine();
            }

            if (typeof(T) == typeof(rMindVerticalLine))
                line = CreateVerticalLine();

            UpdateBase();
            return line;
        }       

        protected virtual rMindVerticalLine CreateVerticalLine()
        {
            var line = new rMindVerticalLine(this);

            bool first = m_horizontal_lines.Count == 0 && m_vertical_lines.Count == 0;
            if (!first)
            {
                Template.ColumnDefinitions.Add(new ColumnDefinition() {
                    Width = GridLength.Auto,
                    MinWidth = 24,
                });               

                foreach (var l in HLines)
                {
                    foreach (var node in l.RightNodes)
                        node.Column += 1;
                }
            }

            m_vertical_lines.Add(line);
            return line;
        }

        protected virtual rMindHorizontalLine CreateHorizontalLine()
        {
            var line = new rMindHorizontalLine(this);

            bool first = m_horizontal_lines.Count == 0 && m_vertical_lines.Count == 0;
            if(!first)
            {
                Template.RowDefinitions.Add(new RowDefinition() {
                    Height = GridLength.Auto,
                    MinHeight = 24
                });

                foreach (var l in VLines)
                {
                    foreach (var node in l.BottomNodes)
                        node.Row += 1;
                }
            }           

            m_horizontal_lines.Add(line);
            return line;
        }

        public void RemoveLine(rMindLine line)
        {
            if (line is rMindHorizontalLine)
            {
                RemoveHorizontalLine(line as rMindHorizontalLine);
            }

            if (line is rMindVerticalLine)
            {
                RemoveVerticalLine(line as rMindVerticalLine);
            }
        }

        protected virtual void OffsetNodes(bool row, int idx)
        {
            var nodes = VLines.Select(x => x as rMindLine)
                .Union(HLines.Select(x => x as rMindLine))
                .Select(x => x.NodesA.Union(x.NodesB));

            nodes = row ?
                nodes.Select(x => x.Where(y => y.Row > idx)) :
                nodes.Select(x => x.Where(y => y.Column > idx));

            foreach (var arr in nodes)
            {
                foreach (var n in arr)
                {
                    if (row)
                        n.Row--;
                    else
                        n.Column--;
                }
            }
        }

        protected virtual void RemoveVerticalLine(rMindVerticalLine line)
        {
            var removeNodes = line.TopNodes.Union(line.BottomNodes).ToList();
            var colIdx = GetLineIndex(line);
            RemoveNodes(removeNodes);
            // Собираем узлы ниже
            OffsetNodes(false, colIdx);
            if (Template.ColumnDefinitions.Count > 1)
                Template.ColumnDefinitions.Remove(
                    Template.ColumnDefinitions.Last()
                );
            VLines.Remove(line);
        }

        protected virtual void RemoveHorizontalLine(rMindHorizontalLine line)
        {
            var removeNodes = line.LeftNodes.Union(line.RightNodes).ToList();
            var rowIdx = GetLineIndex(line);
            RemoveNodes(removeNodes);
            // Собираем узлы ниже
            OffsetNodes(true, rowIdx);
            if (Template.RowDefinitions.Count > 1)
                Template.RowDefinitions.Remove(
                    Template.RowDefinitions.Last()
                );
            HLines.Remove(line);
        }

        public virtual int GetLineIndex(rMindLine line)
        {
            if (line is rMindVerticalLine)
            {
                return m_vertical_lines.IndexOf(line as rMindVerticalLine) + GetNodeOffset().Column;
            }

            if (line is rMindHorizontalLine)
            {
                return m_horizontal_lines.IndexOf(line as rMindHorizontalLine) + GetNodeOffset().Row;
            }

            return 0;
        }

        public void UpdateBase()
        {
            var offset = GetNodeOffset();
            Grid.SetColumn(m_base, offset.Column);
            Grid.SetRow(m_base, offset.Row);

            Grid.SetColumnSpan(m_base, offset.VLines == 0 ? 1 : offset.VLines);
            Grid.SetRowSpan(m_base, offset.HLines == 0 ? 1 : offset.HLines);
        }
    }
}
