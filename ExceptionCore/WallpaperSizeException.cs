using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExceptionCore
{
    public class WallpaperSizeException : Bussiness
    {
        private const string message = "The image file is too big.";
        public WallpaperSizeException() 
            : base(message)
        { }
    }
}
