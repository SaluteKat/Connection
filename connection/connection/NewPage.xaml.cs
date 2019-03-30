using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Storage.Pickers;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Storage.AccessCache;
using Windows.ApplicationModel.Activation;
using Windows.UI.Core;
using Windows.UI.Popups;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Diagnostics;
using SQLitePCL;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace connection
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class NewPage : Page
    {
        DataTransferManager dataTransferManager = DataTransferManager.GetForCurrentView();
        private string s_title;
        private string s_detail;
        private StorageFile s_img;
        ViewModels.LinkManViewModel ViewModel = ViewModels.LinkManViewModel.getinstance();
        string pictrue_temp = "minion.jpg";
        bool flag_for_flag = false;
        public NewPage()
        {
            this.InitializeComponent();
            if (ViewModel.SelectedItem == null)
            {
                createButton.Content = "创建联系人";
                name.Text = "";
                phone.Text = "";
                Email.Text = "";
                address.Text = "";
                birth.Date = DateTime.Today;
            }
            else
            {
                createButton.Content = "更新信息";
                name.Text = ViewModel.SelectedItem.name;
                phone.Text = ViewModel.SelectedItem.tel;
                Email.Text = ViewModel.SelectedItem.email;
                address.Text = ViewModel.SelectedItem.address;
                birth.Date = ViewModel.SelectedItem.birthday;
                image.ImageSource = ViewModel.SelectedItem.picture;
                int len = ViewModel.SelectedItem.music.Length;
                musicName.Text = ViewModel.SelectedItem.music.Substring(28, len - 28);
                //var messageDialog1 = new MessageDialog(ViewModel.SelectedItem.music).ShowAsync();
                player.Source = new Uri(ViewModel.SelectedItem.music);
            }
        }

        void start_Click(object sender, RoutedEventArgs args)
        {
            player.Play();
        }
        void pause_Click(object sender, RoutedEventArgs args)
        {
            player.Pause();
        }
        void stop_Click(object sender, RoutedEventArgs args)
        {
            player.Stop();
        }
        private async void add_Click(object sender, RoutedEventArgs args)
        {
            await SetLocalMedia();
        }

        async private System.Threading.Tasks.Task SetLocalMedia()
        {
            var openPicker = new Windows.Storage.Pickers.FileOpenPicker();

            openPicker.FileTypeFilter.Add(".wmv");
            openPicker.FileTypeFilter.Add(".mp4");
            openPicker.FileTypeFilter.Add(".wma");
            openPicker.FileTypeFilter.Add(".mp3");

            var file = await openPicker.PickSingleFileAsync();

            // mediaPlayer is a MediaPlayerElement defined in XAML
            if (file != null)
            {
                var stream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);
                player.SetSource(stream, file.ContentType);
                
                string music_temp = file.Path.Substring(file.Path.LastIndexOf('\\') + 1);
                musicName.Text = music_temp;
                music_temp = "ms-resource:///Files/Assets/" + music_temp;
                
                player.Source = new Uri(music_temp);
                player.Play();
            }
        }

        public bool IsHandset(string str_handset)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(str_handset, @"^[1]+\d{10}");
        }

        private async void getLocation(object sender, RoutedEventArgs args)
        {
            if(!IsHandset(phone.Text))
            {
                var mes = new MessageDialog("电话号码格式错误").ShowAsync();
                return;
            }
            string myLocation = await findLocation.GetLoc(phone.Text);
            if(myLocation == null)
            {
                var message = new MessageDialog("该号码不存在").ShowAsync();
            }
            else
            {
                location.Text = myLocation;
            }
        }

        private async void share_Click(object sender, RoutedEventArgs args)
        {
            s_title = name.Text;
            s_detail = phone.Text;

            s_img = await Package.Current.InstalledLocation.GetFileAsync("Assets\\minion.jpg");

            DataTransferManager.ShowShareUI();
        }
        private void OnDataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            DataRequest request = args.Request;
            DataPackage requestData = request.Data;
            requestData.Properties.Title = s_title;
            request.Data.Properties.Description = s_detail;
            requestData.SetText(s_detail);

            DataRequestDeferral deferral = request.GetDeferral();

            requestData.SetBitmap(RandomAccessStreamReference.CreateFromFile(s_img));

            deferral.Complete();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            dataTransferManager.DataRequested += OnDataRequested;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            dataTransferManager.DataRequested += OnDataRequested;
        }

        private void createButton_Click(object sender, RoutedEventArgs e)
        {
            if(createButton.Content.ToString() == "创建联系人")
            {
                if (name.Text.Equals(""))
                {
                    var messageDialog1 = new MessageDialog("请输入联系人姓名\n").ShowAsync();
                    return;
                }
                if (Email.Text.Equals(""))
                {
                    var messageDialog2 = new MessageDialog("请输入联系人邮箱\n").ShowAsync();
                    return;
                }
                if (phone.Text.Equals(""))
                {
                    var messageDialog3 = new MessageDialog("请输入联系人电话\n").ShowAsync();
                    return;
                }

                var messageDialog = new MessageDialog("Create successfully!\n").ShowAsync();

                var db = App.conn;
                string sql = @"INSERT INTO connection (name, tel, address, email, birth, music, pictrue) VALUES (?,?,?,?,?,?,?)";
                using (var res = db.Prepare(sql))
                {
                    res.Bind(1, name.Text.Trim());
                    res.Bind(2, phone.Text.Trim());
                    res.Bind(3, address.Text.Trim());
                    res.Bind(4, Email.Text.Trim());
                    res.Bind(5, birth.Date.DateTime.ToString());
                    res.Bind(6, player.Source.ToString());
                    res.Bind(7, pictrue_temp);
                    res.Step();
                }
                BitmapImage result = image.ImageSource as BitmapImage;
                ViewModel.AddLinkMan(db.LastInsertRowId(), name.Text, phone.Text, Email.Text, birth.Date.DateTime, address.Text, result, player.Source.ToString());
                Frame.Navigate(typeof(MainPage));
            }
            else
            {
                if (name.Text.Equals(""))
                {
                    var messageDialog4 = new MessageDialog("请输入联系人姓名\n").ShowAsync();
                    return;
                }
                if (Email.Text.Equals(""))
                {
                    var messageDialog5 = new MessageDialog("请输入联系人邮箱\n").ShowAsync();
                    return;
                }
                if (phone.Text.Equals(""))
                {
                    var messageDialog6 = new MessageDialog("请输入联系人电话\n").ShowAsync();
                    return;
                }

                var db = App.conn;
                string sql = @"UPDATE connection SET name = ?, tel = ?, address = ?, email = ?, birth = ?, music = ? WHERE ID = ?";
                if (flag_for_flag)
                    sql = @"UPDATE connection SET name = ?, tel = ?, address = ?, email = ?, birth = ?, music = ?, pictrue = ? WHERE ID = ?";
                using (var res = db.Prepare(sql))
                {
                    res.Bind(1, name.Text.Trim());
                    res.Bind(2, phone.Text.Trim());
                    res.Bind(3, address.Text.Trim());
                    res.Bind(4, Email.Text.Trim());
                    res.Bind(5, birth.Date.DateTime.ToString());
                    res.Bind(6, player.Source.ToString());
                    //res.Bind(6, player.Source.ToString());
                    if (flag_for_flag)
                    {
                        res.Bind(7, pictrue_temp);
                        res.Bind(8, ViewModel.SelectedItem.get_id());
                    }
                    else
                    {
                        res.Bind(7, ViewModel.SelectedItem.get_id());
                    }
                    res.Step();
                }

                //viewmodel中还要更改数据 生日
                BitmapImage result = image.ImageSource as BitmapImage;
                ViewModel.UpdateLinkMan(ViewModel.SelectedItem.get_id(), name.Text, phone.Text, Email.Text, birth.Date.DateTime, address.Text, result, player.Source.ToString());
                ViewModel.SelectedItem = null;
                var messageDialog = new MessageDialog("Update successfully!\n").ShowAsync();
                //Tile(ViewModel);
                Frame.Navigate(typeof(MainPage));
            }
            name.Text = "";
            phone.Text = "";
            Email.Text = "";
            address.Text = "";
            birth.Date = DateTime.Today;
        }

        private void delete_ele(object sender, RoutedEventArgs e)
        {
            if (ViewModel.SelectedItem != null)
            {
                var db = App.conn;
                string sql = @"DELETE FROM connection WHERE ID = ?";
                try
                {
                    using (var res = db.Prepare(sql))
                    {
                        res.Bind(1, ViewModel.SelectedItem.get_id());
                        res.Step();
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    throw;
                }

                ViewModel.RemoveLinkMan(ViewModel.SelectedItem.get_id());
                Frame.Navigate(typeof(MainPage));
            }
        }

        private async void select_click(object sender, RoutedEventArgs e)
        {
            //文件选择器  
            FileOpenPicker openPicker = new FileOpenPicker();
            //选择视图模式  
            openPicker.ViewMode = PickerViewMode.Thumbnail;
            //openPicker.ViewMode = PickerViewMode.List;  
            //初始位置  
            openPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            //添加文件类型  
            openPicker.FileTypeFilter.Add(".jpg");
            openPicker.FileTypeFilter.Add(".jpeg");
            openPicker.FileTypeFilter.Add(".png");

            StorageFile file = await openPicker.PickSingleFileAsync();

            if (file != null)
            {
                ApplicationData.Current.LocalSettings.Values["TempImage"] = StorageApplicationPermissions.FutureAccessList.Add(file);
                using (IRandomAccessStream stream = await file.OpenAsync(FileAccessMode.Read))
                {
                    var srcImage = new BitmapImage();
                    await srcImage.SetSourceAsync(stream);
                    image.ImageSource = srcImage;
                    pictrue_temp = file.Path.Substring(file.Path.LastIndexOf('\\') + 1);
                    flag_for_flag = true;
                }
            }
        }

        private void cancelButton_click(object sender, RoutedEventArgs e)
        {
            name.Text = "";
            phone.Text = "";
            Email.Text = "";
            address.Text = "";
            birth.Date = DateTime.Today;
        }
    }

}