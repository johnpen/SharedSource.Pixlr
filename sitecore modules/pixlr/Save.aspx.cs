using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Web.UI.Sheer;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Publishing;
using Sitecore.Configuration;
using System.IO;
using System.Drawing;
using System.Collections.Specialized;
using SharedSource.Pixlr.Utils;
using System.Net;
using Sitecore.Resources.Media;

namespace SharedSource.Pixlr.sitecore_modules
{


    public partial class Save : System.Web.UI.Page
    {

        internal Sitecore.Data.Database master;


        protected void Page_Load(object sender, EventArgs e)
        {
            master = Factory.GetDatabase("master");
            MediaItem media = master.GetItem(new ID(Request.QueryString["title"]));
            var item = MediaManager.GetMedia(media);


            HttpWebRequest httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(Request.QueryString["image"]);
            HttpWebResponse httpWebReponse = (HttpWebResponse)httpWebRequest.GetResponse();

            byte[] b = null;
            using (Stream stream = httpWebReponse.GetResponseStream())
            using (MemoryStream ms = new MemoryStream())
            {
                int count = 0;
                do
                {
                    byte[] buf = new byte[1024];
                    count = stream.Read(buf, 0, 1024);
                    ms.Write(buf, 0, count);
                } while (stream.CanRead && count > 0);
                b = ms.ToArray();

                item.SetStream(ms, Request.QueryString["type"]);
            }

            

            httpWebReponse.Close();
            
        }




    }
}
