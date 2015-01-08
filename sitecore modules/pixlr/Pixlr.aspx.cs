using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sitecore.Web.UI.Sheer;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Publishing;
using Sitecore.Configuration;
using System.IO;
using System.Drawing;
using System.Collections.Specialized;
using SharedSource.Pixlr.Utils;

namespace SharedSource.Pixlr.sitecore_modules
{


    public partial class Pixlr : System.Web.UI.Page
    {

        internal Sitecore.Data.Database master;


        protected void Page_Load(object sender, EventArgs e)
        {
            master = Factory.GetDatabase("master");
            MediaItem media = master.GetItem(new ID(Request.QueryString["id"]));

            Stream mediaStream = media.GetMediaStream();

            var filename = media.DisplayName + "." + media.Extension;
            var filenamepath = Path.Combine(Server.MapPath("~/App_Data"), filename);

            String strUrl = HttpContext.Current.Request.Url.AbsoluteUri.Replace(HttpContext.Current.Request.Url.PathAndQuery, "/");
            strUrl = strUrl +  "/sitecore modules/pixlr/";
            

            SaveStreamToFile(filenamepath, mediaStream);
            
            UploadFile[] files = new UploadFile[] 
            { 
                new UploadFile(filenamepath)
            }; 

            
            NameValueCollection nv = new NameValueCollection();
            nv.Add("target", strUrl + "save.aspx");
            nv.Add("exit", strUrl + "exit.aspx");
            nv.Add("method", "get");
            nv.Add("title", media.ID.ToString().Replace("{", "").Replace("}", ""));
            nv.Add("locktarget", "true");
            nv.Add("locktitle", "true");
            nv.Add("referrer", "Sitecore");

            var response = Utils.HttpUploadHelper.Upload("http://apps.pixlr.com/" + Request.QueryString["mode"] + "/",files , nv);

            File.Delete(filenamepath);

            Response.Redirect(response, true);
            Response.End();
        }


        public void SaveStreamToFile(string fileFullPath, Stream stream)
        {
            if (stream.Length == 0) return;

            // Create a FileStream object to write a stream to a file
            using (FileStream fileStream = System.IO.File.Create(fileFullPath, (int)stream.Length))
            {
                // Fill the bytes[] array with the stream data
                byte[] bytesInStream = new byte[stream.Length];
                stream.Read(bytesInStream, 0, (int)bytesInStream.Length);

                // Use FileStream object to write to the specified file
                fileStream.Write(bytesInStream, 0, bytesInStream.Length);
            }
        }



    }
}
