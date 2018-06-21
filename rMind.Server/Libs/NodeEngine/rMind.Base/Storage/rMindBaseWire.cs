using System.Xml.Linq;

namespace rMind.Elements
{
    using Storage;
    using Draw;
    using Types;
    /// <summary> Wire to connect nodes </summary>
    public partial class rMindBaseWire : IDrawElement, IStorageObject
    {
        #region Serialize
        public virtual XElement Serialize()
        {
            var node = new XElement("wire");

            if (A.Node != null)
            {
                var dotA = new XElement("dota");
                dotA.Add(new XAttribute("item", A.Node.Parent.Index));
                dotA.Add(new XAttribute("node", A.Node.Index));
                node.Add(dotA);
            }

            if (B.Node != null)
            {
                var dotB = new XElement("dotb");
                dotB.Add(new XAttribute("item", B.Node.Parent.Index));
                dotB.Add(new XAttribute("node", B.Node.Index));
                node.Add(dotB);
            }

            return node;
        }
        #endregion

        #region Deserialize
        protected bool TryAttachDot(rMindBaseWireDot dot, XElement options)
        {
            if (options == null)
                return false;

            int itemIdx = 0;
            int nodeIdx = 0;

            if (int.TryParse(options.Attribute("item").Value, out itemIdx) && int.TryParse(options.Attribute("node").Value, out nodeIdx))
            {
                if (itemIdx == -1 || nodeIdx == -1)
                    return false;

                var controller = GetController();
                if (controller == null)
                    return false;

                var node = controller.GetNodeByIndexPair(itemIdx, nodeIdx);
                node.Attach(dot);

                return true;
            }

            return false;
        }

        public virtual void Deserialize(XElement node)
        {
            TryAttachDot(A, node.Element("dota"));
            TryAttachDot(B, node.Element("dotb"));
            SetEnabledHitTest(true);
        }

        #endregion        
    }
}
