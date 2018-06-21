using Newtonsoft.Json;

namespace rMind.Elements
{
    using Draw;
    using Types;

    [JsonObject(MemberSerialization.OptIn)]
    public class rMindBaseItem : rMindItemUI, IDrawElement
    {
        protected rMindBaseController m_parent;
        public rMindBaseController Parent { get { return m_parent; } }

        public rMindBaseItem(rMindBaseController parent) : base()
        {
            m_parent = parent;
        }
        
        public virtual void Init()
        {

        }

        public rMindBaseController GetController()
        {
            return m_parent;
        }

        public virtual Vector2 GetOffset()
        {
            return new Vector2(0, 0);
        }
    }
}
