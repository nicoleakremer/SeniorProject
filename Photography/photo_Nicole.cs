using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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