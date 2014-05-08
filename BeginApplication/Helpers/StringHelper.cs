using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace BeginApplication.Helpers
{
    public static class StringHelper
    {
        public static bool RemoteFileExists(this string url)
        {
            if (url == null) return false;

            try
            {
                //Creating the HttpWebRequest
                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                //Setting the Request method HEAD, you can also use GET too.
                request.Method = "HEAD";
                //Getting the Web Response.
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                //Returns TRUE if the Status code == 200
                return (response.StatusCode == HttpStatusCode.OK);
            }
            catch
            {
                //Any exception will returns false.
                return false;
            }
        }
    }
}