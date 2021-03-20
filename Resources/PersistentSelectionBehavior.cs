using System.Collections;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Interactivity;

namespace OneTimetablePlus.Resources
{
    public class PersistentSelectionBehavior : Behavior<TabControl>
    {
        private static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register(
            nameof(ItemsSourceProperty),
            typeof(IEnumerable),
            typeof(PersistentSelectionBehavior),
            new PropertyMetadata(OnItemSourcePropertyChanged));

        private static readonly DependencyProperty SelectedIndexProperty = DependencyProperty.Register(
            nameof(SelectedIndexProperty),
            typeof(int),
            typeof(PersistentSelectionBehavior),
            new PropertyMetadata(OnSelectedIndexPropertyChanged));

        private bool isCleanedUp;
        private int lastSelectedIndex = 0;

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.Loaded += AssociatedObject_Loaded;
            AssociatedObject.Unloaded += AssociatedObject_Unloaded;
        }

        protected override void OnDetaching()
        {
            Cleanup();
            base.OnDetaching();
        }

        private void AssociatedObject_Loaded(object sender, RoutedEventArgs e)
        {
            BindingOperations.SetBinding(this, ItemsSourceProperty,
                new Binding("ItemsSource") {Source = AssociatedObject});

            BindingOperations.SetBinding(this, SelectedIndexProperty,
                new Binding("SelectedIndex") {Source = AssociatedObject});

        }

        private void AssociatedObject_Unloaded(object sender, RoutedEventArgs e)
        {
            Cleanup();
        }

        private void Cleanup()
        {
            if (isCleanedUp)
                return;
            AssociatedObject.Loaded -= AssociatedObject_Loaded;
            AssociatedObject.Unloaded -= AssociatedObject_Unloaded;
            BindingOperations.ClearBinding(this, ItemsSourceProperty);
            BindingOperations.ClearBinding(this, SelectedIndexProperty);
            isCleanedUp = true;
        }

        private static void OnSelectedIndexPropertyChanged(
            DependencyObject o,
            DependencyPropertyChangedEventArgs e)
        {
            if (!(o is PersistentSelectionBehavior selectionBehavior))
                return;
            selectionBehavior.OnSelectedIndexPropertyChanged(e.OldValue, e.NewValue); ;
        }
        private static void OnItemSourcePropertyChanged(
          DependencyObject o,
          DependencyPropertyChangedEventArgs e)
        {
            if (!(o is PersistentSelectionBehavior selectionBehavior))
                return;
            
            selectionBehavior.OnItemsSourceChanged();
        }

        public void OnSelectedIndexPropertyChanged(object oldValue, object newValue)
        {
            lastSelectedIndex = (int) newValue == -1 ? (int)oldValue : (int)newValue;
        }
        public void OnItemsSourceChanged()
        {
            if (lastSelectedIndex < AssociatedObject.Items.Count)
            {
                //AssociatedObject.SelectedItem = AssociatedObject.Items[lastSelectedIndex];
                AssociatedObject.SelectedIndex = lastSelectedIndex;

            }

        }

    }
}
