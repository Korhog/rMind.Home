using System.Xml.Linq;

namespace rMind.Content.Row
{
    using Storage;

    public partial class rMindRow : IStorageObject
    {
        #region Serialize
        public virtual XElement Serialize()
        {
            var node = new XElement("row");

            node.Add(new XAttribute("rtype", RowType));
            node.Add(new XAttribute("inode", InputNodeType));
            node.Add(new XAttribute("onode", OutputNodeType));

            return node;
        }        
        #endregion

        #region Deserialize
        public virtual void Deserialize(XElement node)
        {
            //OutputNodeType = 
        }

        #endregion
    }
}
