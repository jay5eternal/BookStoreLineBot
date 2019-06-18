using isRock.LineBot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BookstoreBot
{
    public partial class _default : System.Web.UI.Page
    {
        const string channelAccessToken = "24FHi6i+Q6yR3MH7yFxl4CE2aIMms24CnLMYCp94wiNH75kjx9tDOCcGFPn5bXDEFHKhT6WY7J6zApwhd+lOzqfwIIuepOjsDr4Nn7R9Tn0WBEgLvm8foyCIwS4LCAcgYfXts19QSSZbik35L3JaWQdB04t89/1O/w1cDnyilFU=";
        const string AdminUserId= "U5bc6a706ccb8b54ede79b56408dda53b";

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            var bot = new Bot(channelAccessToken);
            bot.PushMessage(AdminUserId, $"測試 {DateTime.Now.ToString()} ! ");
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            var bot = new Bot(channelAccessToken);
            bot.PushMessage(AdminUserId, 1,2);
        }
    }
}