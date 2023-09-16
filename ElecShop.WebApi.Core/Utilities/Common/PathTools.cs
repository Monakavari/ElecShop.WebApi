using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElecShop.Core.Utilities.Common
{
    public class PathTools
    {
        #region Domain

        public static string Domain = "https://localhost:44381";

        #endregion Domain

        #region Product

        public static string ProductImagePath = "/Images/Products/Origin/";
        public static string ProductServerImagePath =Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Products/Origin/") ;

        #endregion Product
    }
}
