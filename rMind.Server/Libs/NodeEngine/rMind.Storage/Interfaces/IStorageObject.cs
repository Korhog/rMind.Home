using System.Xml;
using System.Xml.Linq;

namespace rMind.Storage
{
    public interface IStorageObject
    {
        XElement Serialize();
        void Deserialize(XElement node);
    }
}
