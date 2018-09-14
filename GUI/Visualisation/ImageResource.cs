using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GUI.Visualisation
{
    public static class ImageResource
    {
        public static BitmapSource DrawShareLogo1 { get; set; } = LoadBitmap(ResourcePictures.DrawShareLogo);

        public static ImageBrush TrashGuy { get; set; } = new ImageBrush() { ImageSource = LoadBitmap(ResourcePictures.trashguy) };
        public static ImageBrush Trash1 { get; set; } = new ImageBrush() { ImageSource = LoadBitmap(ResourcePictures.trash1) };
        public static ImageBrush Trash1_Small { get; set; } = new ImageBrush() { ImageSource = LoadBitmap(ResourcePictures.trash1_small) };
        public static ImageBrush Trash2 { get; set; } = new ImageBrush() { ImageSource = LoadBitmap(ResourcePictures.trash2) };
        public static ImageBrush PadlockOpen { get; set; } = new ImageBrush() { ImageSource = LoadBitmap(ResourcePictures.padlock_open) };
        public static ImageBrush PadlockClosed { get; set; } = new ImageBrush() { ImageSource = LoadBitmap(ResourcePictures.padlock_closed) };
        public static ImageBrush PadlockOpen2 { get; set; } = new ImageBrush() { ImageSource = LoadBitmap(ResourcePictures.padlock_open2) };
        public static ImageBrush PadlockClosed2 { get; set; } = new ImageBrush() { ImageSource = LoadBitmap(ResourcePictures.padlock_closed2) };
        public static ImageBrush PadlockOpen3 { get; set; } = new ImageBrush() { ImageSource = LoadBitmap(ResourcePictures.padlock_open3) };
        public static ImageBrush PadlockClosed3 { get; set; } = new ImageBrush() { ImageSource = LoadBitmap(ResourcePictures.padlock_closed3) };
        public static ImageBrush LogoFacebook { get; set; } = new ImageBrush() { ImageSource = LoadBitmap(ResourcePictures.Facebook_Logo_100) };
        public static ImageBrush LogoInstagram { get; set; } = new ImageBrush() { ImageSource = LoadBitmap(ResourcePictures.Instagram_Logo_100) };
        public static ImageBrush LogoTwitter { get; set; } = new ImageBrush() { ImageSource = LoadBitmap(ResourcePictures.Twitter_Logo_100) };
        public static ImageBrush LogoGithub { get; set; } = new ImageBrush() { ImageSource = LoadBitmap(ResourcePictures.Github_Logo_100) };
        public static ImageBrush UserCount { get; set; } = new ImageBrush() { ImageSource = LoadBitmap(ResourcePictures.User_50) };





        public static BitmapSource LoadBitmap(System.Drawing.Bitmap source)
        {
            return System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(source.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty,
                   System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());
        }
    }
}
