using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

namespace rMind.BaseControls.Tree
{
    using rMind.BaseControls.Entities;
    using Windows.ApplicationModel.DataTransfer;
    using Windows.UI.Xaml.Media.Animation;

    public delegate void OnCreateButtonDelegate(ITreeItem folder);
    public delegate void OnSelectItemDelegate(ITreeItem item);

    public class MyTemplateSelector : DataTemplateSelector
    {
        public DataTemplate DefaultTemplate { get; set; }
        public DataTemplate ItemTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            if (item is TreeFolderBase)
                return DefaultTemplate;

            return ItemTemplate;
        }
    }  

    public sealed class TreeSelector : Control
    {
        ITreeItem root;
        ITreeItem buffer;

        public ITreeItem Buffer { get { return buffer; } }

        ListView items;
        ListView subItems;
        Button backButton;
        Button addButton;
        TranslateTransform slideTransform;
        TranslateTransform slideSubTransform;



        public Button AddButton { get { return addButton; } }

        DataTemplate defaultTemplate = null;
        public DataTemplate DefaultTemplate { get => defaultTemplate; set => defaultTemplate = value; }

        DataTemplate itemTemplate = null;
        public DataTemplate ItemTemplate { get => itemTemplate; set => itemTemplate = value; }

        object itemsSource;
        public object ItemsSource
        {
            get => itemsSource;
            set
            {
                itemsSource = value;
                if (items == null) return;
                items.ItemsSource = itemsSource;
            }
        }

        Border bucket;

        public event OnCreateButtonDelegate OnCreateButton;
        public event OnSelectItemDelegate OnSelectItem;

        public TreeSelector()
        {
            this.DefaultStyleKey = typeof(TreeSelector);
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            items = GetTemplateChild("PART_Items") as ListView;

            if (itemsSource != null)
                items.ItemsSource = itemsSource;

            var selector = items.ItemTemplateSelector as MyTemplateSelector;
            if (selector != null)
            {
                if (itemTemplate != null)
                    selector.ItemTemplate = itemTemplate;

                if (defaultTemplate != null)
                    selector.DefaultTemplate = defaultTemplate;
            }

            slideTransform = GetTemplateChild("PART_SlideTransform") as TranslateTransform;
            slideSubTransform = GetTemplateChild("PART_SlideSubTransform") as TranslateTransform;

            bucket = GetTemplateChild("PART_Bucked") as Border;
            bucket.DragOver += (s, e) =>
            {
                e.AcceptedOperation = DataPackageOperation.Move;
            };

            bucket.Drop += async (s, e) =>
            {
                if (buffer == null)
                    return;

                TreeFolderBase folder = this.DataContext as TreeFolderBase;
                if (folder == null)
                    return;

                folder.Remove(buffer);
            };


            subItems = GetTemplateChild("PART_SlideItems") as ListView;


            var adder = GetTemplateChild("PART_AddButton") as Button;
            addButton = adder;
            adder.Click += (s, e) =>
            {
                ITreeItem folder = (DataContext as ITreeItem) ?? root;
                OnCreateButton?.Invoke(folder);
            };

            items.ItemClick += (s, e) =>
            {
                var f = e.ClickedItem as TreeFolderBase;
                if (f != null)
                {
                    backButton.Visibility = Visibility.Visible;
                    subItems.ItemsSource = f.Parent.Children;
                    DataContext = f;
                    Slide(true);
                    return;
                }

                var item = e.ClickedItem as ITreeItem;
                if (item != null)
                {
                    OnSelectItem?.Invoke(item);
                }
            };

            items.DataContextChanged += (s, e) =>
            {
                var a = e;
                if (itemsSource != null)
                    items.ItemsSource = itemsSource;
            };

            items.DragItemsStarting += (s, e) =>
            {
                e.Data.RequestedOperation = DataPackageOperation.Move;
                buffer = e.Items.First() as ITreeItem;
            };

            backButton = GetTemplateChild("PART_BackButton") as Button;
            backButton.Click += (s, e) =>
            {
                Button sender = s as Button;
                ITreeItem folder = DataContext as ITreeItem;
                if (folder == null)
                {
                    return;
                }

                subItems.ItemsSource = folder.Children;
                if (folder.Parent == root)
                    sender.Visibility = Visibility.Collapsed;

                DataContext = folder.Parent;
                Slide(false);
            };
        }

        void Slide(bool forward)
        {
            Storyboard storyboard = new Storyboard();
            DoubleAnimation slideAnimation = new DoubleAnimation();

            var size = ActualWidth;

            if (forward)
            {
                slideAnimation.From = size;
                slideAnimation.To = 0;
            }
            else
            {
                slideAnimation.From = -size;
                slideAnimation.To = 0;
            }
            slideAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(250));

            Storyboard.SetTarget(slideAnimation, slideTransform);
            Storyboard.SetTargetProperty(slideAnimation, "X");

            storyboard.Children.Add(slideAnimation);

            slideAnimation = new DoubleAnimation();

            if (forward)
            {
                slideAnimation.From = 0;
                slideAnimation.To = -size;
            }
            else
            {
                slideAnimation.From = 0;
                slideAnimation.To = size;
            }
            slideAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(250));

            Storyboard.SetTarget(slideAnimation, slideSubTransform);
            Storyboard.SetTargetProperty(slideAnimation, "X");

            storyboard.Children.Add(slideAnimation);

            storyboard.Begin();
        }

        public void SetRoot(ITreeItem target)
        {
            root = target;
            DataContext = target;
        }

        public string Header
        {
            get { return (string)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(
            "Header",
            typeof(string),
            typeof(TreeSelector),
            new PropertyMetadata("Library")
        );

        public Visibility EditControlsVisibility
        {
            get { return (Visibility)GetValue(EditControlsVisibilityProperty); }
            set { SetValue(EditControlsVisibilityProperty, value); }
        }

        public static readonly DependencyProperty EditControlsVisibilityProperty = DependencyProperty.Register(
            "EditControlsVisibility",
            typeof(Visibility),
            typeof(TreeSelector),
            new PropertyMetadata(true, (s, e) => {
                if ((Visibility)e.NewValue == Visibility.Visible)
                {

                }
            })
        );
    }
}
