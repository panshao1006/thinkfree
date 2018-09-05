using System.IO;

namespace Core.Common.JWTIdentity
{
    internal class JwtClaimTypes
    {
        public static string Audience
        {
            get
            {
                return "Audience" ;
            }
        }

        public static string Id
        {
            get
            {
                return "Id";
            }
        }

        public static string Name
        {
            get
            {
                return "Name";
            }
        }

        public static string Email
        {
            get
            {
                return "Email";
            }
        }

        public static string Phone
        {
            get
            {
                return "Phone";
            }
        }
    }
}