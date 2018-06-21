using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Controls;

namespace rMind.Content.Row
{
    using Storage;
    using Nodes;
    using System.Xml.Linq;

    public enum RowType
    {
        None,
        Row,
        Separator
    }

    public interface IRow : IStorageObject
    {
        RowType RowType { get; }
    }

    public class rMindRowSeparator : IRow
    {
        public RowType RowType { get { return RowType.Separator; } }

        public void Deserialize(XElement node)
        {
            throw new System.NotImplementedException();
        }

        public XElement Serialize()
        {
            var node = new XElement("row");
            node.Add(new XAttribute("rtype", RowType));
            return node;
        }
    }

    public partial class rMindRow : IRow
    {
        public new RowType RowType { get { return RowType.Row; } }

        public rMindNodeConnectionType InputNodeType { get; set; } = rMindNodeConnectionType.Container;
        public rMindNodeConnectionType OutputNodeType { get; set; } = rMindNodeConnectionType.Container;

        public rMindBaseNode InputNode { get; set; }
        public rMindBaseNode OutputNode { get; set; }

        Button m_delete_button;

        public Button DeleteButton {
            get
            {
                if (m_delete_button == null)
                {
                    m_delete_button = CreateDeleteButton();
                }

                return m_delete_button;
            }
        }

        public FrameworkElement Content { get; set; }

        private Button CreateDeleteButton()
        {             
            var deleteButton = new rMind.BaseControls.Buttons.RoundButton()
            {
                Content = new FontIcon()
                {
                    FontFamily = new FontFamily("Segoe MDL2 Assets"),
                    Glyph = "\uE106",
                    Foreground = new SolidColorBrush(Colors.White),
                    FontSize = 10
                },
                Radius = 30,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Center,
                Background = new SolidColorBrush(Colors.Red),

                Width = 24,
                Height = 24,
                CornerRadius = new CornerRadius(12),

                Margin = new Thickness(2),
                Tag = this
            };
            Grid.SetColumn(deleteButton, 1);
            return deleteButton;            
        }

        public virtual void SetVisibility(bool state)
        {
            InputNode?.SetVisibility(state);
            OutputNode?.SetVisibility(state);
            if (Content != null)
            {
                Content.Visibility = state ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        public virtual void SetRowIndex(int index)
        {
            if (m_delete_button != null) Grid.SetRow(m_delete_button, index);
            InputNode?.SetCell(InputNode.Column, index);
            OutputNode?.SetCell(OutputNode.Column, index);

            if (Content != null) Grid.SetRow(Content, index);
        }       
    }
}
