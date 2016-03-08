using System;

/// <summary>
/// Summary description for Class1
/// </summary>
namespace Photography
{
    public partial class Photo
    {
        public string PhotoName
        {
            get
            {
                return PhotoId + ".jpg";
            }
        }
    }
}