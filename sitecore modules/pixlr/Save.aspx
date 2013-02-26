<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Save.aspx.cs" Inherits="SharedSource.Pixlr.sitecore_modules.Save" %>
<!DOCTYPE html>

<html lang="en" xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta charset="utf-8" />
    <title></title>
    <script type="text/javascript">

        function refreshandclose()
        {
            window.opener.scForm.postRequest("","","","item:load(id={<%=Request.QueryString["title"]%>})");
            self.close();
        }
    </script>
</head>
<body onload="refreshandclose()">

</body>
</html>