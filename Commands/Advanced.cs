namespace SharedSource.Pixlr.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Linq;
    using System.Text;
    using Sitecore;
    using Sitecore.Configuration;
    using Sitecore.Data;
    using Sitecore.Data.Items;
    using Sitecore.Data.Managers;
    using Sitecore.Data.Templates;
    using Sitecore.Diagnostics;
    using Sitecore.Globalization;
    using Sitecore.Pipelines;
    using Sitecore.Resources;
    using Sitecore.Shell.Framework.Commands;
    using Sitecore.Text;
    using Sitecore.Web;
    using Sitecore.Web.UI.Framework.Scripts;
    using Sitecore.Web.UI.Sheer;

    public class Advanced : Command
    {
        public override void Execute(CommandContext context)
        {
            if (context.Items.Length == 1)
            {
                Item item = context.Items[0];
                UrlString str = new UrlString();
                str.Append("sc_content", item.Database.Name);
                str.Append("id", item.ID.ToString());
                str.Append("la", item.Language.ToString());
                str.Append("vs", item.Version.ToString());
                if (!string.IsNullOrEmpty(context.Parameters["frameName"]))
                {
                    str.Add("pfn", context.Parameters["frameName"]);
                }


                SheerResponse.Eval("window.open('/sitecore modules/pixlr/pixlr.aspx?" + str + "&mode=editor', 'MediaLibrary', 'location=0,resizable=1')");

            }
        }

        public override CommandState QueryState(CommandContext context)
        {
            if (WebUtil.GetQueryString("mo") == "preview")
            {
                return CommandState.Hidden;
            }
            if (context.Items.Length != 1)
            {
                return CommandState.Hidden;
            }
            Item item = context.Items[0];
            if ((item.TemplateID != TemplateIDs.VersionedImage) && (item.TemplateID != TemplateIDs.UnversionedImage))
            {
                Template template = TemplateManager.GetTemplate(item.TemplateID, item.Database);
                Assert.IsNotNull(template, typeof(Template));
                if (!template.DescendsFrom(TemplateIDs.UnversionedImage) && !template.DescendsFrom(TemplateIDs.VersionedImage))
                {
                    return CommandState.Disabled;
                }
            }
            if (!item.Access.CanWrite())
            {
                return CommandState.Disabled;
            }
            return base.QueryState(context);

        }
    }
}
