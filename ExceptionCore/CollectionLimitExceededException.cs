using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExceptionCore
{
    public class CollectionLimitExceededException : Bussiness
    {
        private const string message = "The limit of the wallpaper collection exceeded its limit.";
        public CollectionLimitExceededException() : base(message)
        { }
    }
}
