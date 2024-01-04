using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppData.Models;
using AppData.ViewModels.Options;

namespace AppData.IServices
{
    public interface IBlogService
    {
        public Blog Add(Blog obj);
        public Blog Update(Blog obj);
        public Blog Delete(Guid id);
        public Blog Details(Guid id);   
        public List<Blog> GetAll(Blog searchData,ListOptions listOptions);
            
    }
}
