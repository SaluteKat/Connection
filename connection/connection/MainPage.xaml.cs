using connection.ViewModels;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Notifications;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace connection
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        ViewModels.LinkManViewModel ViewModel = ViewModels.LinkManViewModel.getinstance();
        public MainPage()
        {

            this.InitializeComponent();
            ViewModel.SelectedItem = null;
        }




        async void searchButtonClick(object sender, RoutedEventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            using (var statement = App.conn.Prepare("SELECT name, tel, address, email, birth FROM connection WHERE name LIKE ? OR tel LIKE ? OR address LIKE ? OR email LIKE ? OR birth LIKE ?"))
            {
                statement.Bind(1, "%" + search.Text + "%");
                statement.Bind(2, "%" + search.Text + "%");
                statement.Bind(3, "%" + search.Text + "%");
                statement.Bind(4, "%" + search.Text + "%");
                statement.Bind(5, "%" + search.Text + "%");
                while (SQLiteResult.DONE != statement.Step())
                {
                    sb.AppendFormat("name:{0} tel:{1} address:{2} email:{3} birth:{4}\n", statement[0], statement[1], statement[2], statement[3], statement[4]);
                }
            }
            var dialog = new MessageDialog(sb.ToString());
            dialog.Commands.Add(new UICommand("确定", cmd => { }, commandId: 0));
            var result = await dialog.ShowAsync();
        }

        void AddButtonClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(NewPage));
        }

        private void ItemClicked(object sender, ItemClickEventArgs e)
        {
            ViewModel.SelectedItem = (Models.LinkMan)(e.ClickedItem);
            Frame.Navigate(typeof(NewPage));
        }
    }
}
