using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace connection.Models
{
    public class LinkMan
    {
        private long id;
        //the name of the linkman
        public string name { get; set; }
        //the telephone number of the linkman
        public string tel { get; set; }
        //the email-address of the linkman
        public string email { get; set; }
        //the birthday of the link man
        public DateTime birthday { get; set; }
        //the address of the linkman
        public string address { get; set; }
        //the head picture of the linkman
        public BitmapImage picture { get; set; }
        public string picture_string { get; set; }
        //the background music of the linkman
        public string music { get; set; }



        public LinkMan(long id, string name, string tel, string email, DateTime birthday, string address, BitmapImage picture, string music)
        {
            this.id = id;
            this.name = name;
            this.tel = tel;
            this.email = email;
            this.birthday = birthday;
            this.address = address;
            //这里记得加上默认图片和默认背景乐
            this.picture = (picture == null ? new BitmapImage(new Uri("ms-appx:///Assets/minion.jpg")) : picture);
            //this.picture_string = picture.ToString().Substring(picture.ToString().LastIndexOf('/') + 1);
            this.music = (music == null ? "something" : music);
        }

        public long get_id()
        {
            return this.id;
        }

        /*
         public string get_name()
        {
            return this.name;
        }
        */

        //(long id,string name, string tel, string email, DateTime birthday, string address, BitmapImage picture, string music)
        public void rewrite_name(string name)
        {
            this.name = name;
        }

        public void rewrite_tel(string tel)
        {
            this.tel = tel;
        }

        public void rewrite_email(string email)
        {
            this.email = email;
        }

        public void rewrite_birthday(DateTime birthday)
        {
            this.birthday = birthday;
        }

        public void rewrite_address(string address)
        {
            this.address = address;
        }

        public void rewrite_pic(BitmapImage picture)
        {
            this.picture = picture;
        }

        public void rewrite_music(string music)
        {
            this.music = music;
        }
    }
}