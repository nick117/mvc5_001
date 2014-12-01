using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace appraisal.Models
{
    public class SessionHelper
    {
        //Session variable constants
        public const string COUNTERVAR = "Counter";
        public const string TEXTVAR = "RealName";
        public const string TEXTVAR1 = "UserGroup";
        public const string TEXTVAR2 = "Readed";

        public static T Read<T>(string variable)
        {
            object value = HttpContext.Current.Session[variable];
            if (value == null)
                return default(T);
            else
                return ((T)value);
        }

        public static void Write(string variable, object value)
        {
            HttpContext.Current.Session[variable] = value;
        }

        public static int Counter
        {
            get
            {
                return Read<int>(COUNTERVAR);
            }
            set
            {
                Write(COUNTERVAR, value);
            }
        }

        public static string RealName
        {
            get
            {
                return Read<string>(TEXTVAR);
            }
            set
            {
                Write(TEXTVAR, value);
            }
        }

        public static string UserGroup
        {
            get
            {
                return Read<string>(TEXTVAR1);
            }
            set
            {
                Write(TEXTVAR1, value);
            }
        }

        public static string Readed
        {
            get
            {
                return Read<string>(TEXTVAR2);
            }
            set
            {
                Write(TEXTVAR2, value);
            }
        }
    }
}