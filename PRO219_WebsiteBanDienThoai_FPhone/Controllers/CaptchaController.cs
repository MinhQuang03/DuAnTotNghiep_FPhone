using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PRO219_WebsiteBanDienThoai_FPhone.Services;

namespace PRO219_WebsiteBanDienThoai_FPhone.Controllers
{
    public class CaptchaController : Controller
    {
        private IHttpContextAccessor _accessor;
    
        public CaptchaController(IHttpContextAccessor accessor)
        {
            this._accessor = accessor;
        }
        [AllowAnonymous]
        public ActionResult Index(string id)
        {

            bool noisy = true;
            var rand = new Random((int)DateTime.Now.Ticks);
          
            var captcha = GetRandomString();

            var bytes = Encoding.UTF8.GetBytes(captcha);
            SetSessionValue("_captcha_value", captcha);
            FileContentResult img = null;
            
            string capcha1;
            if (bytes != null && bytes.Count() > 0) capcha1 = Encoding.UTF8.GetString(bytes);
            using (var mem = new MemoryStream())
            using (var bmp = new Bitmap(130, 30))
            using (var gfx = Graphics.FromImage((Image)bmp))
            {
                gfx.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
                gfx.SmoothingMode = SmoothingMode.AntiAlias;
                gfx.FillRectangle(Brushes.White, new Rectangle(0, 0, bmp.Width, bmp.Height));
              
                int nextX = 2;
                var array = captcha.ToCharArray();
                for (int i = 0; i < array.Length; i++)
                {

                    gfx.DrawString(array[i].ToString(), new Font("Tahoma", rand.Next(14, 20)), Brushes.Gray, nextX, rand.Next(3, 5));
                    nextX = nextX + 20;
                }
                
                if (noisy)
                {
                    int i, r, x, y;
                    var pen = new Pen(Color.Yellow);
                    for (i = 1; i < 10; i++)
                    {
                        pen.Color = Color.FromArgb(
                            (rand.Next(0, 255)),
                            (rand.Next(0, 255)),
                            (rand.Next(0, 255)));

                        r = rand.Next(0, (130 / 3));
                        x = rand.Next(0, 130);
                        y = rand.Next(0, 30);

                        gfx.DrawEllipse(pen, x - r, y - r, r, r);

                    }
                }

              
                bmp.Save(mem, System.Drawing.Imaging.ImageFormat.Jpeg);

                img = this.File(mem.GetBuffer(), "image/Jpeg");
            }
            return img;
        }



        public void SetSessionValue(string key, string value)
        {
            if (_accessor != null && _accessor.HttpContext.Session != null)
            {
                _accessor.HttpContext.Session.LoadAsync().GetAwaiter().GetResult();
                if (!string.IsNullOrEmpty(value))
                {
                    var bytes = Encoding.UTF8.GetBytes(value);
                    _accessor.HttpContext.Session.Set(key, bytes);
                }
                else
                {
                    if (_accessor.HttpContext.Session.Keys.Contains(key))
                    {
                        _accessor.HttpContext.Session.Remove(key);
                    }
                }
            }
        }

        public string GetSessionValue(string key)
        {
            if (_accessor != null && _accessor.HttpContext.Session != null)
            {
                // await _accessor.HttpContext.Session.LoadAsync();
                //return _accessor.HttpContext.Session.GetString(key);

                string content = string.Empty;
                var bytes = default(byte[]);
                _accessor.HttpContext.Session.TryGetValue(key, out bytes);
                if (bytes != null && bytes.Count() > 0) content = Encoding.UTF8.GetString(bytes);
                return content;
            }
            else return null;
        }

        public static String GetRandomString()
        {
            var allowedChars = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789";
            var length = 5;

            var chars = new char[length];
            var rd = new Random();

            for (var i = 0; i < length; i++)
            {
                chars[i] = allowedChars[rd.Next(0, allowedChars.Length)];
            }

            return new String(chars);
        }
        // GET: Captcha
        //public void Index_(string id)
        //{
        //    // Get a random integer from RandomNumber(int min, int max) method
        //    //Random random = new Random();
        //    //int ImageNum = RandomNumber(1, 5); // (1-max number of background images)

        //    // Select Background Image
        //    string filename = Server.MapPath("/Content/Images/b1.jpg");

        //    // Load image
        //    Bitmap bitMapImage = new System.Drawing.Bitmap(filename);
        //    Graphics graphicImage = Graphics.FromImage(bitMapImage);

        //    // Get random string from RandomString(int sizeOfString) method
        //    string textToWrite = GenerateRandomCode();// RandomString(5);

        //    // Write text on image
        //    graphicImage.DrawString(textToWrite, new Font("Times New Roman", 16, FontStyle.Regular),
        //       SystemBrushes.WindowText, new Point(0, 5));

        //    // Encode random string and save the cipher text to a session variable
        //    //MD5 md5 = new MD5CryptoServiceProvider();
        //    //byte[] pass = Encoding.UTF8.GetBytes(textToWrite);
        //    //Session[type + "_captcha_value"] = Encoding.UTF8.GetString(md5.ComputeHash(pass));
        //    //Session[id + "_captcha_value"] = textToWrite;
        //    Session.Add(id + "_captcha_value", textToWrite);


        //    System.IO.MemoryStream mstr = new System.IO.MemoryStream();
        //    // Save the image
        //    bitMapImage.Save(mstr, ImageFormat.Jpeg);

        //    // Set the content type
        //    Response.ContentType = "image/jpeg";
        //    // Write image
        //    Response.BinaryWrite((byte[])(mstr).ToArray());

        //    //Clean
        //    graphicImage.Dispose();
        //    bitMapImage.Dispose();
        //}
        private int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max); // max is exclusive
        }
        // Function to generate random string with Random class.
        private string GenerateRandomCode()
        {
            Random r = new Random();
            string s = "";
            for (int j = 0; j < 5; j++)
            {
                int i = r.Next(3);
                int ch;
                string sTmp = "";
                switch (i)
                {
                    case 1:
                        ch = r.Next(0, 9);
                        sTmp = ch.ToString();
                        break;
                    case 2:
                        ch = r.Next(65, 90);
                        sTmp = Convert.ToChar(ch).ToString();
                        break;
                    case 3:
                        ch = r.Next(97, 122);
                        sTmp = Convert.ToChar(ch).ToString();
                        break;
                    default:
                        ch = r.Next(97, 122);
                        sTmp = Convert.ToChar(ch).ToString();
                        break;
                }
                if (sTmp == "0" || sTmp == "o" || sTmp == "O" || sTmp == "1" || sTmp == "i" || sTmp == "I")
                {
                    sTmp = "9";
                }
                s = s + sTmp;
                r.NextDouble();
                r.Next(100, 1999);
            }
            return s;
        }
    }
}
