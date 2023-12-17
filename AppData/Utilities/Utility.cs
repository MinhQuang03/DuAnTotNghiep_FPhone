using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using AppData.Models;
using AppData.ViewModels;
using Newtonsoft.Json;

namespace AppData.Utilities
{

    public static class Utility
    {
        
        public static DataError GetDataErrror(Exception ex)
        {
            // 1. trả về DataError false không có msg nếu không có exception
            if (ex == null) return new DataError() { Success = false };

            // 2. in ra nội dung lỗi từ Exception bao gồm Message và InnerException Message
            StringBuilder content = new StringBuilder();
            content.Append(ex.Message);
            if (ex.InnerException != null)
            {
                content.Append($". {ex.InnerException.Message}");
            }
            if (ex.StackTrace != null)
            {
                content.Append($". {ex.StackTrace.ToString()}");
            }

            return new DataError()
            {
                Success = false,
                Msg = content.ToString()
            };
        }
        public static string ConvertObjectToJson(object obj)
        {
            if (obj == null) return null;
            try
            {
                return JsonConvert.SerializeObject(obj);
            }
            catch (Exception)
            {
                return null;
            }
        }
        public static T ConvertJsonToObject<T>(string json)
        {
            if (string.IsNullOrEmpty(json)) return default;
            try
            {
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch
            {
                return default;
                throw;
            }
        }

      public static string EmailCreateAccountTemplate(string fullName, string username)  
        {
            return $@"
        <!DOCTYPE html>
        <html lang='en'>
        <head>
            <meta charset='UTF-8'>
            <meta name='viewport' content='width=device-width, initial-scale=1.0'>
            <style>
                body {{
                    font-family: 'Arial', sans-serif;
                    background-color: #f4f4f4;
                    margin: 0;
                    padding: 0;
                }}
                .container {{
                    max-width: 600px;
                    margin: 20px auto;
                    padding: 20px;
                    border: 1px solid #ddd;
                    border-radius: 8px;
                    background-color: #fff;
                    box-shadow: 0 0 10px rgba(0,0,0,0.1);
                }}
                h1 {{
                    color: #007BFF;
                    margin-bottom: 20px;
                }}
                p {{
                    margin-bottom: 15px;
                    line-height: 1.6;
                    color: #555;
                }}
                strong {{
                    font-weight: bold;
                }}
                ul {{
                    list-style: none;
                    padding: 0;
                }}
                li {{
                    margin-bottom: 8px;
                }}
                a {{
                    color: #007BFF;
                    text-decoration: none;
                    font-weight: bold;
                }}
                footer {{
                    margin-top: 20px;
                    text-align: center;
                    color: #777;
                }}
            </style>
        </head>
        <body>
            <div class='container'>
                <h1>{fullName} thân mến,</h1>
                <p>Chúng tôi rất vui thông báo rằng tài khoản của Quý khách đã được tạo thành công trên hệ thống của <strong> FPHONE STORE. </strong></p>
                <p>Thông tin tài khoản của Quý khách như sau:</p>
                <ul>
                    <li><strong>Tên đăng nhập:</strong> {username}</li>
                </ul>
                <p>Nếu Quý khách có bất kỳ câu hỏi hoặc cần hỗ trợ, vui lòng liên hệ với chúng tôi qua email <a href='mailto:support@fphonestore.com'>support@fphonestore.com</a> hoặc gọi số điện thoại hỗ trợ khách hàng: <strong>0123-456-789</strong>.</p>
                <p>Chúc Quý khách có những trải nghiệm tốt nhất với hệ thống của chúng tôi!</p>
                <footer>
                    Trân trọng,<br>
                    FPHONE STORE
                </footer>
            </div>
        </body>
        </html>";
        }

    }
   
    public class ObjectEmailInput
    {
        /// <summary>
        /// Tên người dùng
        /// </summary>
        public string FullName { get; set; }
        /// <summary>
        /// Tên đăng nhập
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// Gửi đến
        /// </summary>
        public string SendTo { get; set; }
        /// <summary>
        /// Thông báo
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// Tiêu đề
        /// </summary>
        public string Subject { get; set; }
        /// <summary>
        /// Chữ ký
        /// </summary>
        public string Signature { get; set; }

    }

    public class ObjectEmailOutput
    {
        public bool Success { get; set; } = false;
        public string Message { get; set; }     
    }   

    public class Security
    {   
        /// <summary>
        /// Mã hoá thành chuỗi hash
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public string Encrypt(string key, string data)
        {
            string encData = null;
            var keys = GetHashKeys(key);

            try
            {
                encData = EncryptStringToBytes_Aes(data, keys[0], keys[1]);
            }
            catch (CryptographicException)
            {
            }
            catch (ArgumentNullException)
            {
            }

            return encData;
        }

        /// <summary>
        /// Giải mã chuỗi hash
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public string Decrypt(string key, string data)
        {
            string decData = null;
            var keys = GetHashKeys(key);

            try
            {
                decData = DecryptStringFromBytes_Aes(data, keys[0], keys[1]);
            }
            catch (CryptographicException)
            {
            }
            catch (ArgumentNullException)
            {
            }

            return decData;
        }

        private byte[][] GetHashKeys(string key)
        {
            var result = new byte[2][];
            var enc = Encoding.UTF8;

            SHA256 sha2 = new SHA256CryptoServiceProvider();

            var rawKey = enc.GetBytes(key);
            var rawIV = enc.GetBytes(key);

            var hashKey = sha2.ComputeHash(rawKey);
            var hashIV = sha2.ComputeHash(rawIV);

            Array.Resize(ref hashIV, 16);

            result[0] = hashKey;
            result[1] = hashIV;

            return result;
        }

        private static string EncryptStringToBytes_Aes(string plainText, byte[] Key, byte[] IV)
        {
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");

            byte[] encrypted;

            using (var aesAlg = new AesManaged())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (var msEncrypt = new MemoryStream())
                {
                    using (var csEncrypt =
                           new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }

                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            return Convert.ToBase64String(encrypted);
        }

        private static string DecryptStringFromBytes_Aes(string cipherTextString, byte[] Key, byte[] IV)
        {
            var cipherText = Convert.FromBase64String(cipherTextString);

            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");

            string plaintext = null;

            using (var aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (var msDecrypt = new MemoryStream(cipherText))
                {
                    using (var csDecrypt =
                           new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (var srDecrypt = new StreamReader(csDecrypt))
                        {
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }

            return plaintext;
        }
    }
}
