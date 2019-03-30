
using connection.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Notifications;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using SQLitePCL;

namespace connection.ViewModels
{
    class LinkManViewModel
    {
        private ObservableCollection<Models.LinkMan> allItems = new ObservableCollection<Models.LinkMan>();
        public ObservableCollection<Models.LinkMan> AllItems { get { return this.allItems; } }

        private Models.LinkMan selectedItem = default(Models.LinkMan);
        public Models.LinkMan SelectedItem { get { return selectedItem; } set { this.selectedItem = value; } }

        int count = 0;
        private static LinkManViewModel instance;
        public static LinkManViewModel getinstance()
        {
            if (instance == null)
            {
                instance = new LinkManViewModel();
            }
            return instance;
        }

        public LinkManViewModel()
        {
            try
            {
                var sql = "SELECT ID, name, tel, address, email, birth, music, pictrue FROM connection";
                var db = App.conn;
                using (var statement = db.Prepare(sql))
                {
                    while (SQLiteResult.ROW == statement.Step())
                    {
                        var s = statement[5].ToString();
                        s = s.Substring(0, s.IndexOf(' '));
                        long ID = (long)statement[0];
                        string name = (string)statement[1];
                        string tel = (string)statement[2];
                        string address = (string)statement[3];
                        string email = (string)statement[4];
                        DateTime birthday = new DateTime(int.Parse(s.Split('/')[0]), int.Parse(s.Split('/')[1]), int.Parse(s.Split('/')[2]));
                        string music = (string)statement[6];
                        string pictrue = (string)statement[7];
                        pictrue = "ms-appx:///Assets/" + pictrue;
                        BitmapImage temp = new BitmapImage(new Uri(pictrue));
                        this.AddLinkMan(ID, name, tel, email, birthday, address, temp, music);
                        count++;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            //磁贴+toast
            var updater = TileUpdateManager.CreateTileUpdaterForApplication();
            updater.Clear();
            updater.EnableNotificationQueue(true);
            int ii;
            for (ii = 0; ii < AllItems.Count; ii++)
            {
                if(AllItems[ii].birthday.Month==DateTime.Today.Month && AllItems[ii].birthday.Day == DateTime.Today.Day)
                {
                    var xmlDoc = TileService.CreateTiles(AllItems[ii]);
                    TileNotification notification = new TileNotification(xmlDoc);
                    updater.Update(notification);

                    var toastXml = ToastServices.CreateToast(AllItems[ii]);
                    ToastNotification notification2 = new ToastNotification(toastXml);
                    ToastNotificationManager.CreateToastNotifier().Show(notification2);
                }

            }
        }

        public int get_count()
        {
            return count;
        }

        public void AddLinkMan(long id, string name, string tel, string email, DateTime birthday, string address, BitmapImage picture, string music)
        {
            this.allItems.Add(new Models.LinkMan(id, name, tel, email, birthday, address, picture, music));
            count++;
        }

        public void RemoveLinkMan(long id)
        {
            int i = 0;
            for (i = 0; i < count; i++)
            {
                if (allItems[i].get_id() == id)
                    break;
            }
            this.allItems.Remove(allItems[i]);
            count--;
            this.selectedItem = null;
        }

        public void UpdateLinkMan(long id, string name, string tel, string email, DateTime birthday, string address, BitmapImage picture, string music)
        {
            int i = 0;
            for (i = 0; i < allItems.Count; i++)
            {
                if (allItems[i].get_id() == id)
                {
                    allItems[i].rewrite_name(name);
                    allItems[i].rewrite_tel(tel);
                    allItems[i].rewrite_email(email);
                    allItems[i].rewrite_birthday(birthday);
                    allItems[i].rewrite_address(address);
                    allItems[i].rewrite_pic(picture);
                    allItems[i].rewrite_music(music);
                }
            }
            this.selectedItem = null;
        }

    }
}