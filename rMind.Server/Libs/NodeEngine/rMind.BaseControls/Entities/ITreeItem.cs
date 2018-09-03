using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Linq;

namespace rMind.BaseControls.Entities
{
    /// <summary> Элемент дерева </summary>
    public interface ITreeItem
    {
        string Name { get; set; }

        bool Folder { get; }

        ITreeItem Parent { get; set; }

        ObservableCollection<ITreeItem> Children { get; }
    }

    public abstract class TreeFolderBase : ITreeItem
    {
        [JsonProperty]
        public string Name { get; set; }

        [JsonProperty]
        public ObservableCollection<TreeFolderBase> Folders { get; set; }

        [JsonProperty]
        public ObservableCollection<ITreeItem> Items { get; set; }

        public ObservableCollection<ITreeItem> Children { get; set; }

        public bool Folder { get { return true; } }
        public ITreeItem Parent { get; set; }
        

        public TreeFolderBase AddFolder(TreeFolderBase folder)
        {
            if (Folders == null)
                Folders = new ObservableCollection<TreeFolderBase>();
            Folders.Add(folder);

            folder.Parent = this;

            var idx = Children
                .IndexOf(Children.Where(x => x is TreeFolderBase).LastOrDefault()) + 1;
            Children.Insert(idx, folder);

            return folder;
        }

        public void AddItem(ITreeItem item)
        {
            if (Items == null)
                Items = new ObservableCollection<ITreeItem>();

            Items.Add(item);
            Children.Add(item);
        }

        public void Remove(ITreeItem item)
        {
            if (item is TreeFolderBase)
            {
                Folders.Remove(item as TreeFolderBase);
                Children.Remove(item);
            }
            else if (item is ITreeItem)
            {
                Items.Remove(item);
                Children.Remove(item);
            }
        }
    }

    public abstract class TreeItemBase : ITreeItem
    {
        public string Name { get; set; }
        public bool Folder { get { return false; } }
        public ITreeItem Parent { get; set; }
        public ObservableCollection<ITreeItem> Children { get { return null; } }
    }
}
