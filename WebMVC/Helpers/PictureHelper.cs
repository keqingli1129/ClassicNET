using System;
using System.Web;
using System.Web.Mvc;

namespace WebMVC.Helpers
{
    /// <summary>
    /// Provides HtmlHelper extensions for rendering Northwind category pictures.
    /// Northwind seed images are stored as OLE objects with a 78-byte header that
    /// must be stripped before the raw image bytes can be displayed.
    /// </summary>
    public static class PictureHelper
    {
        private const int OleHeaderLength = 78;

        /// <summary>
        /// Returns a base64-encoded data URI for a category picture byte array,
        /// stripping the OLE header when present. Returns null if the picture is empty.
        /// </summary>
        public static string GetPictureDataUri(this HtmlHelper html, byte[] picture)
        {
            if (picture == null || picture.Length == 0)
                return null;

            byte[] imageData;
            if (picture.Length > OleHeaderLength && picture[0] == 0x15 && picture[1] == 0x1C)
            {
                imageData = new byte[picture.Length - OleHeaderLength];
                Array.Copy(picture, OleHeaderLength, imageData, 0, imageData.Length);
            }
            else
            {
                imageData = picture;
            }

            return "data:image/bmp;base64," + Convert.ToBase64String(imageData);
        }
    }
}
