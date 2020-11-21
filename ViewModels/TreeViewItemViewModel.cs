using Prism.Mvvm;
using System.Collections.ObjectModel;

namespace DragDropTree.ViewModels
{
    public class TreeViewItemViewModel : BindableBase
    {
        private bool _IsSelected;
        public bool IsSelected
        {
            get { return _IsSelected; }
            set { SetProperty(ref _IsSelected, value); }
        }
        private bool _IsExpanded;
        public bool IsExpanded
        {
            get { return _IsExpanded; }
            set { SetProperty(ref _IsExpanded, value);
                if (IsExpanded && Parent != null && !Parent.IsExpanded)
                    Parent.IsExpanded = true;
            }
        }
        private string _Header;
        public string Header
        {
            get { return _Header; }
            set { SetProperty(ref _Header, value); }
        }
        private TreeViewItemViewModel _Parent;
        public TreeViewItemViewModel Parent
        {
            get { return _Parent; }
            set { SetProperty(ref _Parent, value); }
        }
        private ObservableCollection<TreeViewItemViewModel> _Childrens = new();
        public ObservableCollection<TreeViewItemViewModel> Childrens
        {
            get { return _Childrens; }
            set { SetProperty(ref _Childrens, value); }
        }
    }
}
