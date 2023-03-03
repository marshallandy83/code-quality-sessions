using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;

namespace OCP
{
	internal class MultiSelectDataGrid : DataGrid
	{
		public static readonly DependencyProperty SelectedItemsProperty = DependencyProperty.Register(nameof(SelectedItems), typeof(IList), typeof(MultiSelectDataGrid), new PropertyMetadata(default(IList), OnSelectedItemsPropertyChanged));

		protected override void OnSelectionChanged(SelectionChangedEventArgs e)
		{
			base.OnSelectionChanged(e);
			SetValue(SelectedItemsProperty, base.SelectedItems);
		}

		public new IList SelectedItems
		{
			get => (IList)GetValue(SelectedItemsProperty);
			set => throw new Exception("This property is read-only. To bind to it you must use 'Mode=OneWayToSource'.");
		}

		private static void OnSelectedItemsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((MultiSelectDataGrid)d).OnSelectedItemsChanged((IList)e.OldValue, (IList)e.NewValue);
		}

		protected virtual void OnSelectedItemsChanged(IList oldSelectedItems, IList newSelectedItems)
		{
		}
	}
}
