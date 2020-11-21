using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace DragDropTree.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private string _title = "Prism Application";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }
        private ObservableCollection<TreeViewItemViewModel> _TreeViewItems = new();
        public ObservableCollection<TreeViewItemViewModel> TreeViewItems
        {
            get { return _TreeViewItems; }
            set { SetProperty(ref _TreeViewItems, value); }
        }
        private Point _startPoint;
        public MainWindowViewModel()
        {
            for (int i = 0; i < 3; i++)
            {
                var rootItem = new TreeViewItemViewModel() { Header = i.ToString() };
                for (int j = 0; j < 3; j++)
                {
                    var innerItem = new TreeViewItemViewModel() { Header = i.ToString() + j.ToString(), Parent = rootItem };
                    rootItem.Childrens.Add(innerItem);
                }
                TreeViewItems.Add(rootItem);
            }
        }
        public void Tree_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _startPoint = e.GetPosition(null);
        }
        public void Tree_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                var diff = _startPoint - e.GetPosition(null);
                if (Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance
                    || Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance)
                {
                    var treeViewItem = FindAnchestor<TreeViewItem>((DependencyObject)e.OriginalSource) as TreeViewItem;
                    var dragData = new DataObject(treeViewItem.DataContext);
                    ItemsControl parent = GetSelectedTreeViewItemParent(treeViewItem);
                    dragData.SetData("ParentViewModel",parent.DataContext);
                    DragDrop.DoDragDrop(treeViewItem, dragData, DragDropEffects.Move);
                }
            }
        }
        public void DropTree_DragEnter(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(typeof(TreeViewItemViewModel)))
            {
                e.Effects = DragDropEffects.None;
            }
        }

        public void DropTree_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(TreeViewItemViewModel)))
            {
                var draggedItemViewModel = e.Data.GetData(typeof(TreeViewItemViewModel)) as TreeViewItemViewModel;
                
                var treeViewItem = FindAnchestor<TreeViewItem>((DependencyObject)e.OriginalSource) as TreeViewItem;
                var dropTarget = treeViewItem.DataContext as TreeViewItemViewModel;

                dropTarget.Childrens.Add(draggedItemViewModel);
                if (true)//check here is it copy or move
                {
                    if (e.Data.GetData("ParentViewModel") is TreeViewItemViewModel ParentViewModel)
                    {
                        ParentViewModel.Childrens.Remove(draggedItemViewModel);
                    }
                }
            }
        }

        private static object FindAnchestor<T>(DependencyObject current) where T : DependencyObject
        {
            do
            {
                if (current is T t)
                {
                    return t;
                }
                current = VisualTreeHelper.GetParent(current);
            }
            while (current != null);
            return null;
        }
        public static ItemsControl GetSelectedTreeViewItemParent(TreeViewItem item)
        {
            DependencyObject parent = VisualTreeHelper.GetParent(item);
            while (!(parent is TreeViewItem || parent is TreeView))
            {
                parent = VisualTreeHelper.GetParent(parent);
            }
            return parent as ItemsControl;
        }
    }
}
