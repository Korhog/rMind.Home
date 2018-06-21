using System;
using System.Collections.Generic;
using System.Linq;

using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Xaml.Controls;
using System.Threading.Tasks;

namespace rMind.Content
{
    using Row;
    using Nodes;
    using Elements;
    using System.Xml.Linq;

    /// <summary>
    /// Контейнер с возможностью добавлять строки с данными
    /// </summary>
    public partial class rMindRowContainer : rMindBaseElement
    {
        #region Serialize
        public override XElement Serialize()
        {
            var node = base.Serialize();
            var rowsNode = SerializeRows();
            if (rowsNode != null)
                node.Add(rowsNode);

            return node;
        }

        protected virtual XElement SerializeRows()
        {
            if (m_rows.Count == 0)
                return null;

            var rowsNode = new XElement("rows");
            foreach (var row in m_rows)
            {
                var rowNode = row.Serialize();
                rowsNode.Add(rowNode);
            }

            return rowsNode;
        }
        #endregion

        #region Deserialize
        public override void Deserialize(XElement node)
        {
            var rowsNode = node.Element("rows");
            DeserializeRows(rowsNode);

            base.Deserialize(node);
        }

        protected virtual void DeserializeRows(XElement rowsNode)
        {
            if (rowsNode == null)
                return;

            foreach(var rowNode in rowsNode.Elements("row"))
            {
                RowType type = RowType.None;
                Enum.TryParse(rowNode.Attribute("rtype")?.Value, out type);

                switch (type)
                {
                    case RowType.Row:
                        var row = new Row.rMindRow();
                        row.Deserialize(rowNode);
                        AddRow(row);
                        break;

                    case RowType.Separator:
                        AddSeparator();
                        break;
                }
            }
        }
        #endregion
    }
}
