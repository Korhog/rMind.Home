using System;
using System.Linq;
using Windows.UI.Xaml.Controls;


namespace rMind.Elements
{

    /// <summary>
    /// Base scheme controller : Menu section
    /// </summary>
    public partial class rMindBaseController : Storage.IStorageObject
    {
        protected virtual void InitMenu()
        {
            if (m_canvas == null)
            {
                return;
            }

            //m_flyout = new MenuFlyout();

            m_flyout = new MenuFlyout();
            m_flyout.Opening += OnFlyout;

            
            var item = new MenuFlyoutItem()
            {
                Text = "Удалить"
            };
            item.Click += (sender, e) => {
                DeleteSelection();
            };
            m_flyout.Items.Add(item);

            item = new MenuFlyoutItem()
            {
                Text = "Статичный"
            };
            item.Click += (sender, e) => {
                foreach (var it in m_selected_items.Where(x => x is Content.rMindRowContainer).Select(x => x as Content.rMindRowContainer))
                {
                    it.Static = !it.Static;
                }
            };
            m_flyout.Items.Add(item);
            

            m_canvas.ContextFlyout = m_flyout;
        }

        protected virtual void OnFlyout(object sender, object e)
        {
            m_items_state.ActionItem = null;
        }

        //
    }
}

