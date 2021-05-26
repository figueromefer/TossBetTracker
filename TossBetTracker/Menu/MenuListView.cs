using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace TossBetTracker
{
    public class MenuListView : ListView
    {
        public MenuListView()
        {
            this.SeparatorColor = Color.Transparent;
            this.RowHeight = 38;
            List<MenuItem> data = new MenuListData();
            ItemsSource = data;
            //VerticalOptions = LayoutOptions.FillAndExpand;
            BackgroundColor = Color.Transparent;
            var cell = new DataTemplate(typeof(MenuCell));
            cell.SetBinding(MenuCell.TextProperty, "Titulo");
            cell.SetBinding(MenuCell.ImageSourceProperty, "Icon");
            cell.SetValue(RowHeightProperty, 15);

            ItemTemplate = cell;
        }
    }
}

