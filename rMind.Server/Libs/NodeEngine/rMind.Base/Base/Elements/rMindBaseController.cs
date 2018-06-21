using System.Linq;
using System.Collections.Generic;
using Windows.UI.Xaml.Controls;
using Newtonsoft.Json;

namespace rMind.Elements
{
    using Types;
    using Nodes;
    using CanvasEx;

    public struct rMindControllerState
    {        
        public rMindBaseElement DragedItem;
        public Vector2 StartPosition;
        public Vector2 StartPointerPosition;
        public bool IsDrag() { return DragedItem != null; }

        public rMindBaseNode OveredNode;
        public rMindBaseNode MagnetNode;

        public rMindBaseWireDot DragedWireDot;
        public bool IsDragDot() { return DragedWireDot != null; }

        public rMindItemUI ActionItem;

        // View state
        public float ZoomFactor;
        public double HorizontalOffset;
        public double VerticalOffset;
        public bool Saved;
    }

    /// <summary>
    /// Base scheme controller
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public partial class rMindBaseController : Storage.IStorageObject
    {
        /// <summary>
        /// Контейнер со всем блоками контроллера.
        /// </summary>
        protected List<rMindBaseElement> m_items;
        [JsonProperty]
        public List<rMindBaseElement> Items { get { return m_items; } }


        protected List<rMindBaseWire> m_wire_list;
        [JsonProperty]
        public List<rMindBaseElement> Wires { get { return m_items; } }

        protected rMindCanvasController m_parent;

        public rMindCanvasController CanvasController { get { return m_parent; } }
        public void SetParent(rMindCanvasController parent) { m_parent = parent; }

        protected bool m_subscribed;

        // Graphics
        Canvas m_canvas;
        ScrollViewer m_scroll;

        // Controls
        rMindControllerState m_items_state;
        protected List<rMindBaseElement> m_selected_items;
        public List<rMindBaseElement> SelectedItems { get { return m_selected_items; } }

        rMindBaseElement m_overed_item;

        // Menu
        MenuFlyout m_flyout;
        public MenuFlyout Flyout { get { return m_flyout; } }

        // Ext
        rMindMagnet m_magnet;

        // Test
        string json;

        public rMindBaseController(rMindCanvasController parent)
        {

            Name = "root"; 
            m_parent = parent;

            m_items_state = new rMindControllerState()
            {
                DragedItem = null,
                ZoomFactor = 1,
                Saved = false
            };

            m_magnet = new rMindMagnet();
            m_items = new List<rMindBaseElement>();
            m_wire_list = new List<rMindBaseWire>();
            m_selected_items = new List<rMindBaseElement>();
        }  

        /// <summary>
        /// Subscribe to canvas
        /// </summary>
        public virtual void Subscribe(Canvas canvas, ScrollViewer scroll = null)
        {
            if (m_subscribed)
                Unsubscribe();

            m_canvas = canvas;
            m_scroll = scroll;   

            m_subscribed = true;

            m_magnet.Draw(m_canvas);
            SubscribeInput();
            InitMenu();
            DrawElements();
            ResroteControllerState();
        }

        protected virtual void ResroteControllerState()
        {
            // пока смотрим в центр
            if (!m_items_state.Saved)
                onLoad(null, null);
            else {
                m_scroll?.ChangeView(
                    m_items_state.HorizontalOffset,
                    m_items_state.VerticalOffset,
                    m_items_state.ZoomFactor,
                    true
                );
            }
        }

        protected virtual void SaveControllerState()
        {
            m_items_state.Saved = true;
            if (m_scroll != null)
            {
                m_items_state.ZoomFactor = m_scroll.ZoomFactor;
                m_items_state.HorizontalOffset = m_scroll.HorizontalOffset;
                m_items_state.VerticalOffset = m_scroll.VerticalOffset;
            }
        }

        /// <summary>
        /// Unsubscribe from canvas
        /// </summary>
        public void Unsubscribe()
        {            
            if (m_subscribed)
            {
                SaveControllerState();
                m_canvas.Children.Clear();
                // events                
                UnsubscribeInput();

                m_canvas = null;
                m_scroll = null;
            }            
            m_subscribed = false;            
        }

        protected virtual void Draw(rMindBaseElement item)
        {
            if (m_subscribed && !m_canvas.Children.Contains(item.Template))
            {
                m_canvas.Children.Add(item.Template);
                item.Template.UpdateLayout();
                foreach (var node in item.Nodes)
                    node.Update();
            }
        }

        /// <summary> Отпускаем нод, который тянем </summary>
        public void DropWireDot()
        {
            var attachNode = m_items_state.OveredNode ?? m_items_state.MagnetNode;

            if (attachNode == null)
            {
                m_items_state.DragedWireDot.Wire.Delete();
                m_items_state.DragedWireDot = null;
            }
            else
            {
                attachNode.Attach(m_items_state.DragedWireDot);
                m_items_state.DragedWireDot.Wire.SetEnabledHitTest(true);
            }

            m_magnet.Hide();
        }

        public void TranslateWireDot(Vector2 translation)
        {
            var item = m_items_state.DragedWireDot;
            item.Translate(translation);

            m_items_state.MagnetNode = BakedNodes
                .Where(pair => Vector2.Length(pair.Key - item.Position) < (100 / (m_scroll == null ? 1 : m_scroll.ZoomFactor)))
                .OrderBy(pair => Vector2.Length(pair.Key - item.Position))
                .Select(pair => pair.Value)
                .FirstOrDefault();

            if (m_items_state.MagnetNode == null)
            {
                m_magnet.Hide();
            }
            else
            {
                m_magnet.Show();
                m_magnet.Magnet(item.Position, m_items_state.MagnetNode.GetOffset());
            }   
        }
        
        public void TranslateContainer(rMindBaseElement container, Vector2 translation)
        {
            if (SelectedItems.Contains(container))
            {
                foreach (var selection in SelectedItems)
                    selection.Translate(translation);
                return;
            }

            container.Translate(translation);
        }

        // controller reset
        public virtual void Reset()
        {
            if (m_subscribed)
            {
                m_canvas.Children.Clear();
            }

            while (m_wire_list.Count > 0)
                m_wire_list[0].Delete();

            foreach (var item in m_items.Where(x => x.Storable))
            {
                item.Reset();
            }

            m_items.Clear();
        }

        public void TryCopy()
        {
            if ((m_selected_items?.Count ?? 0) > 0)
            {
                json = Newtonsoft.Json.JsonConvert.SerializeObject(m_selected_items);
            }
        }

        public void TryPaste()
        {
            if (!string.IsNullOrEmpty(json))
            {
                rMindBaseElement element = null; ;

                var result = new List<rMindBaseElement>();
                var list = JsonConvert.DeserializeObject<List<object>>(json);

                foreach (var item in list)
                {
                    var s = item.ToString();
                    var baseObject = JsonConvert.DeserializeObject<rMindBaseElement>(s);
                    var type = baseObject?.ElementType ?? rElementType.RET_NONE;

                    switch (type)
                    {
                        case rElementType.RET_NONE:
                            element = JsonConvert.DeserializeObject<rMindBaseElement>(s);
                            element?.Translate(new Vector2(20, 20));
                            break;
                    }
                    if (element != null)
                        AddElement(element);                    
                }
            }
        }
    }
}

