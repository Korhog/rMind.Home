using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace rMind.Elements.Debug
{ 
    public class rMindDebugContainer : rMindBaseElement
    {
        public rMindDebugContainer(rMindBaseController parent) : base(parent)
        {

        }

        public override void Init()
        {
            base.Init();
            m_base.Width = 60;

            Template.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });

            Template.ColumnDefinitions.Add(new ColumnDefinition());
            Template.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });

            //Template.Padding = new Windows.UI.Xaml.Thickness(2);

            CreateNode().SetCell(0, 0);
            CreateNode().SetCell(0, 1);
            CreateNode().SetCell(2, 0);
            CreateNode().SetCell(2, 1);

            Grid.SetColumnSpan(m_base, 3);
            Grid.SetRowSpan(m_base, 2);
        }                
    }
}
