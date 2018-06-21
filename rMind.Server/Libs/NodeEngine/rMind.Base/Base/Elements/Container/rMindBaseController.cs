using System.Collections.Generic;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

using rMind.Types;

namespace rMind.Elements
{
    using Types;
    using Nodes;

    /// <summary>
    /// Base scheme controller
    /// </summary>
    public partial class rMindBaseController : Storage.IStorageObject
    {
        public long GetIndexOfElement(rMindBaseElement element)
        {
            if (m_items.Contains(element))
                return m_items.IndexOf(element);
            else return -1;
        }

        public string Name { get; set; }

        public bool CheckIsOvered(rMindBaseElement item)
        {
            return m_overed_item == item;
        }

        public bool CheckIsDraged(rMindBaseElement item)
        {
            return m_items_state.DragedItem == item;
        }

        public bool CheckIsDraggedDot(rMindBaseWireDot dot)
        {
            return m_items_state.DragedWireDot == dot;
        }

        /// <summary>
        /// Add new element
        /// </summary>
        public virtual rMindBaseElement AddElement(rMindBaseElement item, bool silent = false)
        {
            m_items.Add(item);

            if (m_subscribed && !silent)
            {
                Draw(item);
            }

            return item;
        }

        /// <summary>
        /// Remove item from canvas
        /// </summary>
        public virtual void RemoveElement(rMindBaseElement item)
        {
            if (CheckIsOvered(item))
                m_overed_item = null;

            if (m_items.Contains(item))
            {
                if (m_selected_items.Contains(item))
                    m_selected_items.Remove(item);

                m_canvas.Children.Remove(item.Template);
                m_items.Remove(item);
            }
        }

        public void SetSelectedItem(rMindBaseElement item, bool multi = false)
        {
            if (item == null)
            {
                foreach (var selection in SelectedItems)
                    selection.SetSelected(false);
                SelectedItems.Clear();
                return;
            }

            if (multi)
            {
                if (!SelectedItems.Contains(item))
                {
                    item.SetSelected(true);
                    SelectedItems.Add(item);
                }
            }
            else
            {
                if (!SelectedItems.Contains(item))
                {
                    foreach (var selection in SelectedItems)
                    {
                        selection.SetSelected(false);
                    }
                    SelectedItems.Clear();                    
                    item.SetSelected(true);
                    SelectedItems.Add(item);
                } 
            }
            /*
            if (item == null)
            {
                foreach (var it in m_selected_items)
                {
                    it.SetSelected(false);
                }
                m_selected_items.Clear();
                return;
            }
            if (!multi && !m_selected_items.Contains(item))
            {
                foreach (var it in m_selected_items)
                {
                    if (it == item)
                        continue;
                    it.SetSelected(false);
                }
                m_selected_items.Clear();
                m_selected_items.Add(item);
            }
            if (!m_selected_items.Contains(item))
            {
                item.SetSelected(true);
                m_selected_items.Add(item);
            }
            */
        }

        public void SetOveredItem(rMindBaseElement item)
        {
            m_overed_item = item;
        }

        public void SetOveredNode(rMindBaseNode node)
        {
            m_items_state.OveredNode = node;
        }

        public void SetDragItem(rMindBaseElement item, PointerRoutedEventArgs e)
        {
            m_items_state.DragedItem = item;
            if (item == null)
                return;

            var p = e.GetCurrentPoint(m_canvas);
            m_items_state.StartPointerPosition = new Vector2(p.Position.X, p.Position.Y);
            m_items_state.StartPosition = item.Position;
        }

        protected void DragContainer(PointerRoutedEventArgs e)
        {
            var p = e.GetCurrentPoint(m_canvas);
            Vector2 offset = new Vector2(p.Position.X, p.Position.Y) - m_items_state.StartPointerPosition;
            var item = m_items_state.DragedItem;

            var translation = item.SetPosition(m_items_state.StartPosition + offset);
            if (m_selected_items.Contains(item))
            {
                foreach (var it in m_selected_items)
                {
                    if (it == item)
                        continue;

                    it.Translate(translation);
                }

            }
        }

        protected virtual void DrawElements()
        {
            if (!m_subscribed)
                return;

            foreach (var e in m_items)                
                Draw(e);
            

            foreach (var w in m_wire_list)
                Draw(w);
        }

        public virtual Vector2 GetScreenCenter(rMindBaseElement item = null)
        {
            if (!m_subscribed)
                return new Vector2(0, 0);

            if (m_scroll == null) {
                return new Vector2(40, 40);
            }

            return new Vector2(
                (m_scroll.HorizontalOffset + m_scroll.ActualWidth / 2 - (item == null ? 0 : item.Width / 2)) / m_scroll.ZoomFactor,
                (m_scroll.VerticalOffset + m_scroll.ActualHeight / 2 - (item == null ? 0 : item.Height / 2)) / m_scroll.ZoomFactor
            );
        }

        public virtual rMindBaseElement CreateElementByElementType(rElementType type, object createParams, bool silent = false)
        {
            rMindBaseElement element = null;
            switch (type)
            {
                case rElementType.RET_NONE:
                    element = new Content.rMindHeaderRowContainer(this);                    
                    break;
                case rElementType.RET_DEVICE_OUTPUT:
                    element = new Content.rMindDeviceOutput(this);
                    break;
            }            
            AddElement(element, silent);

            return element;
        }
    }
}

