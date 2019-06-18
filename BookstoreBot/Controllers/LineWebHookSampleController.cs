using BookstoreBot.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BookstoreBot.Controllers
{
    public class LineBotWebHookController : isRock.LineBot.LineWebHookControllerBase
    {
        const string channelAccessToken = "24FHi6i+Q6yR3MH7yFxl4CE2aIMms24CnLMYCp94wiNH75kjx9tDOCcGFPn5bXDEFHKhT6WY7J6zApwhd+lOzqfwIIuepOjsDr4Nn7R9Tn0WBEgLvm8foyCIwS4LCAcgYfXts19QSSZbik35L3JaWQdB04t89/1O/w1cDnyilFU=";
        const string AdminUserId = "U5bc6a706ccb8b54ede79b56408dda53b";

        [Route("api/LineWebHookSample")]
        [HttpPost]
        public IHttpActionResult POST()
        {
            try
            {
                //設定ChannelAccessToken(或抓取Web.Config)
                this.ChannelAccessToken = channelAccessToken;
                //取得Line Event(範例，只取第一個)
                var LineEvent = this.ReceivedMessage.events.FirstOrDefault();
                //配合Line verify 
                if (LineEvent.replyToken == "00000000000000000000000000000000") return Ok();
                //回覆訊息
                if (LineEvent.type == "message")
                {
                    var UserInfo = this.GetUserInfo(LineEvent.source.userId);
                    /**/
                    if (LineEvent.message.type == "text" && LineEvent.message.text.Contains("訂單查詢")) //收到文字 + 訂單查詢
                    {
                        this.PushMessage(AdminUserId, "使用者的UserID為：" + LineEvent.source.userId + "\r\n回報使用者問題：" + LineEvent.message.text);
                        this.ReplyMessage(LineEvent.replyToken, "哈囉，" + UserInfo.displayName + "\r\n如果您想查詢訂單，請照範例格式輸入您的帳號與指令。\r\nEX:查詢訂單,您的帳號");
                        
                    }


                    if (LineEvent.message.text.Contains("查詢訂單,"))
                    {
                        string[] texts = LineEvent.message.text.Split(',');

                        CustomersRepository _custrepo = new CustomersRepository();
                        var cust = _custrepo.SelectCustomerDetail(texts[1]);

                        if (cust != null)
                        {
                            OrderRepository _odrepo = new OrderRepository();
                            var od_status = _odrepo.GetOrderStatusByNotop4(texts[1]);
                            if (LineEvent.message.text.Contains(","))
                            {
                                ButtonsTemplateMessageForOrderStatus(LineEvent.replyToken, texts[1]);
                            }
                            else
                            {
                                this.ReplyMessage(LineEvent.replyToken, "哈囉，" + UserInfo.displayName + "\r\n您輸入的格式可能錯誤或沒有此帳號呦!");
                            }

                            if (od_status == null) { this.ReplyMessage(LineEvent.replyToken, "查無此筆訂單"); }
                        }
                    }
                    

                    if (LineEvent.message.type == "text" && LineEvent.message.text.Contains("AS") ) //使用者帳號為資料庫的會員帳號
                    {
                        OrderRepository _odrepo = new OrderRepository();
                        var od_status = _odrepo.GetOrderStatusByNo(LineEvent.message.text);
                        if (od_status == null) { this.ReplyMessage(LineEvent.replyToken, "查無此筆訂單"); }
                        else
                        {
                            string status;
                            if (od_status.TransactionComplete == null) { status = "已完成交易，等待確認"; }
                            else { status = "已完成交易"; }
                            if (od_status.CompletePickup == null) { status = "貨物已送達，完成取貨"; }
                            if (od_status.PickUp == null) { status = "貨物已送達，尚未取貨"; }
                            if (od_status.Delivery == null) { status = "貨物正在運送中"; }
                            if (od_status.Preparation == null) { status = "訂單尚在處理中，準備出貨"; }
                            this.ReplyMessage(LineEvent.replyToken, "哈囉，" + UserInfo.displayName + "\r\n您的訂單" + od_status.OrderNo + "\r\n" +
                                "狀態為：" + status + "\r\n若要查看詳細資料\r\n請到網站的會員專區查詢\r\n" + "https://mvcprojecttest20190614051921.azurewebsites.net/Customer/CustomerIndex");
                        }
                    }

                    /**/



                    //if (LineEvent.message.type == "text" && LineEvent.message.text.Contains("訂單")) //收到文字 + 訂單查詢
                    //{
                    //    this.PushMessage(AdminUserId, "使用者的UserID為：" + LineEvent.source.userId + "\r\n回報使用者問題：" + LineEvent.message.text);
                    //    this.ReplyMessage(LineEvent.replyToken, "哈囉，" + UserInfo.displayName + "\r\n如果您想查詢訂單，請照範例格式輸入您的帳號與要查的訂單編號。\r\nEX:您的帳號,訂單編號");
                    //}
                    if (LineEvent.message.type == "text" && LineEvent.message.text.Contains("推薦書本")) 
                    {
                        CarouselTemplateMessage(LineEvent.replyToken);
                    }
                    if (LineEvent.message.type == "sticker") //收到貼圖
                        this.ReplyMessage(LineEvent.replyToken, 1, 2);
                    
                    
                    if (LineEvent.message.type == "text" && LineEvent.message.text.Contains("查書") ) //使用者帳號為資料庫的會員帳號
                    {
                        if (LineEvent.message.text.Contains(","))
                        {
                            string[] texts = LineEvent.message.text.Split(',');
                            string bookname = texts[1];
                            ButtonsTemplateMessage(LineEvent.replyToken, bookname);
                        }
                        else
                        {
                            this.ReplyMessage(LineEvent.replyToken, "哈囉，" + UserInfo.displayName + "\r\n如果您想查詢書本，請照範例格式輸入您的帳號與要查的書名。\r\nEX:查書,書名");
                        }
                                            
                    }

                    if (LineEvent.message.type == "text" && LineEvent.message.text.Contains("服務")) //收到文字 + 訂單查詢
                    {
                        this.PushMessage(AdminUserId, "使用者的UserID為：" + LineEvent.source.userId + "\r\n回報使用者問題：" + LineEvent.message.text);
                        this.ReplyMessage(LineEvent.replyToken, "哈囉，" + UserInfo.displayName + "\r\n還滿意我們的服務嗎？\r\n如果不滿意或有任何疑問，\r\n都可以輸入 客訴 由專人為您服務");
                    }

                    if (LineEvent.message.type == "text" && LineEvent.message.text.Contains("客訴")) //收到文字 + 訂單查詢
                    {
                        this.PushMessage(AdminUserId, "使用者的UserID為：" + LineEvent.source.userId + "\r\n回報使用者問題：" + LineEvent.message.text);
                        this.ReplyMessage(LineEvent.replyToken, "哈囉，" + UserInfo.displayName + "\r\n非常抱歉，我們專人都在忙線中，\r\n如果不滿意或有任何疑問，明天請早。");
                    }
                }
                //response OK
                return Ok();
            }
            catch (Exception ex)
            {
                //如果發生錯誤，傳訊息給Admin
                this.PushMessage(AdminUserId, "發生錯誤:\n" + ex.Message);
                //response OK
                return Ok();
            }
        }

        public IHttpActionResult CarouselTemplateMessage(string replyToken)
        {
            try
            {
                this.ChannelAccessToken = channelAccessToken;                
                BooksRepository _repo = new BooksRepository();
                var msg = new isRock.LineBot.CarouselTemplate();
                var books = _repo.SelectTopBooks();
                List<isRock.LineBot.Column> columns = new List<isRock.LineBot.Column>();
                foreach (var book in books)
                {
                    var actions = new List<isRock.LineBot.TemplateActionBase>();
                    actions.Add(new isRock.LineBot.UriAction()
                    {
                        label = "前往購買",
                        uri = new Uri("https://mvcprojecttest20190614051921.azurewebsites.net/Book/BookDetail/" + book.BookId)
                    });
                    columns.Add(new isRock.LineBot.Column
                    {
                        text = (book.UnitPrice*(1-book.Discount)).ToString("0")+"元",
                        title = book.BooksName,
                        //設定圖片  
                        thumbnailImageUrl =
                    new Uri(book.ImgurUri),
                        actions = actions //設定回覆動作  
                    });
                }
                foreach (var column in columns)
                {
                    msg.columns.Add(column);
                }
                this.ReplyMessage(replyToken, msg);
                return Ok();
            }
            catch (Exception ex)
            {
                //如果發生錯誤，傳訊息給Admin
                this.PushMessage(AdminUserId, "發生錯誤:\n" + ex.Message);
                //response OK
                return Ok();
            }
        }

        public IHttpActionResult ButtonsTemplateMessage(string replyToken,string bookname)
        {
            try
            {
                this.ChannelAccessToken = channelAccessToken;
                BooksRepository _repo = new BooksRepository();
                //var allbooks = _repo.GetAllBook();

                //var book = allbooks.Where(o => o.BooksName.Contains(bookname)).FirstOrDefault();
                var book = _repo.GetOneBook(bookname);
                if (book != null)
                {
                    var actions = new List<isRock.LineBot.TemplateActionBase>();
                    actions.Add(new isRock.LineBot.UriAction() { label = "前往購買", uri = new Uri("https://mvcprojecttest20190614051921.azurewebsites.net/Book/BookDetail/" + book.BookId) });
                    string msgtext;
                    if (book.Discount == 0)
                    {
                        msgtext = "作者:" + book.AuthorName + ",價格:" + book.UnitPrice.ToString("0") + "元,庫存:" + book.InStock + "本";
                    }
                    else
                    {
                        msgtext = "作者:" + book.AuthorName + ",價格:" + book.UnitPrice.ToString("0") + "元,特價"+ (book.UnitPrice * (1 - book.Discount)).ToString("0") + "元,庫存:" + book.InStock + "本";
                    }


                    //單一Button Template Message
                    var ButtonTemplate = new isRock.LineBot.ButtonsTemplate()
                    {
                        
                        text = msgtext,
                        title = book.BooksName,
                        //設定圖片
                        thumbnailImageUrl = new Uri(book.ImgurUri),
                        actions = actions //設定回覆動作
                    };
                    //發送
                    this.ReplyMessage(replyToken, ButtonTemplate);

                }
                else
                {
                    this.ReplyMessage(replyToken, "抱歉，沒有找到這本書!");
                }
                                                                          
                //發送
                return Ok();
            }
            catch (Exception ex)
            {
                //如果發生錯誤，傳訊息給Admin
                this.PushMessage(AdminUserId, "發生錯誤:\n" + ex.Message);
                //response OK
                return Ok();
            }
        }

        public IHttpActionResult ButtonsTemplateMessageForOrderStatus(string replyToken, string customerAccount)
        {

            try
            {
                OrderRepository _odrepo = new OrderRepository();
                var orders = _odrepo.GetOrderStatusByNotop4(customerAccount);
                if (orders != null)
                {
                    var actions = new List<isRock.LineBot.TemplateActionBase>();
                    foreach (var order in orders)
                    {
                        actions.Add(new isRock.LineBot.MessageAction()
                        {
                            label = "訂單編號:" + order.OrderNo,
                            text = order.OrderNo
                        });
                    }


                    //單一Button Template Message
                    var ButtonTemplate = new isRock.LineBot.ButtonsTemplate()
                    {

                        text = "請選擇您要查詢的訂單編號，越上面為越近期的訂單。",
                        title = "以下是您尚未完成的訂單",
                        actions = actions //設定回覆動作
                    };
                    //發送
                    this.ReplyMessage(replyToken, ButtonTemplate);
                }
                else
                {
                    this.ReplyMessage(replyToken, "抱歉，沒有找到這本書!");
                }

                return Ok();

            }
            catch (Exception ex)
            {
                //如果發生錯誤，傳訊息給Admin
                this.PushMessage(AdminUserId, "發生錯誤:\n" + ex.Message);
                //response OK
                return Ok();
            }

        }
    }
}
