using AppData.FPhoneDbContexts;
using AppData.IRepositories;
using AppData.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.Repositories
{
    public class BlogRepository:IBlogRepository
    {
        public readonly FPhoneDbContext _dbContext;
        public BlogRepository(FPhoneDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Blog> Add(Blog obj)
        {
            await _dbContext.Blogs.AddAsync(obj);
            await _dbContext.SaveChangesAsync();
            return obj;
        }

        public async Task Delete(Guid id)
        {
            var a = await _dbContext.Blogs.FindAsync(id);
            _dbContext.Blogs.Remove(a);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<Blog>> GetAll()
        {
            return await _dbContext.Blogs.ToListAsync();
        }

        public async Task<Blog> GetById(Guid id)
        {
            return await _dbContext.Blogs.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Blog> Update(Blog obj)
        {
            var a = await _dbContext.Blogs.FindAsync(obj.Id);
            a.Title = obj.Title;
            a.Content = obj.Content;
            a.CreatedDate = obj.CreatedDate;
            a.Status = obj.Status;
            _dbContext.Blogs.Update(a);
            await _dbContext.SaveChangesAsync();
            return obj;
        }
    }
}
