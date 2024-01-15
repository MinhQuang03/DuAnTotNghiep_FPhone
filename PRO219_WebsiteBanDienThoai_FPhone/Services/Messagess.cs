using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using PRO219_WebsiteBanDienThoai_FPhone.ViewModel;

namespace PRO219_WebsiteBanDienThoai_FPhone.Services
{
    public  class Messagess
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public Messagess(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public bool HasFlash()
        {
            if (_httpContextAccessor.HttpContext.Session.GetString("Message") == null)
            {
                return false;
            }
            return true;
        }

        public  void Set_Flash(string msg, string msgCss)
        {
            MessageModel ms = new MessageModel()
            {
               msg = msg,
               msgCss = msgCss
            };

            _httpContextAccessor.HttpContext.Session.SetString("Message", Newtonsoft.Json.JsonConvert.SerializeObject(ms));
        }

        public MessageModel GetFlash()
        {
            var storedMessage = _httpContextAccessor.HttpContext.Session.GetString("Message");
            _httpContextAccessor.HttpContext.Session.Remove("Message");

            return storedMessage != null
                ? Newtonsoft.Json.JsonConvert.DeserializeObject<MessageModel>(storedMessage)
                : null;
        }
    }
}
