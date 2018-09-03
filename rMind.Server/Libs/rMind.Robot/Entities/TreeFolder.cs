using Newtonsoft.Json;
using rMind.BaseControls.Entities;
using System.Collections.ObjectModel;
using System.Linq;

namespace rMind.Robot.Entities
{
    [JsonObject(MemberSerialization.OptIn)]
    public class TreeFolder : TreeFolderBase
    {
        public void Update()
        {
            Children.Clear();

            foreach (var folder in (Folders ?? new ObservableCollection<TreeFolderBase>()).Where(x => x is TreeFolder).Select(x => x as TreeFolder))
            {
                folder.Parent = this;
                folder.Update();
                Children.Add(folder);
            }

            foreach (var drv in (Items ?? new ObservableCollection<ITreeItem>()))
            {
                drv.Parent = this;
                Children.Add(drv);
            }
        }

        public TreeFolder()
        {
            Children = new ObservableCollection<ITreeItem>();
        }
    }
}
