using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using MQTTCloud.Models;

namespace MQTTCloud.Services
{
    
    public abstract class BaseService<T>
    where T: DbEntity
    {
        private readonly IServiceProvider _provider;
        private readonly IConfiguration _config;

        public BaseService(IServiceProvider provider, IConfiguration config)
        {
            _provider = provider;
            _config = config;
        }
        
        public MessageContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<MessageContext>();
            optionsBuilder.UseNpgsql("User ID =lora;Password=loradbheslo/123;Server=lora.mucka.sk;Port=5432;Database=loradb;Integrated Security=true;Pooling=true;");

            return new MessageContext(optionsBuilder.Options);
        }
        
        
        public virtual T Add(T data)
        {
            var context = CreateDbContext(new string[]{});
            context.Add(data);
            context.SaveChanges();

            return data;
        }

        protected virtual T Add(T data, DbContext context)
        {
            //var context = CreateDbContext(new string[]{});
            context.Add(data);
            context.SaveChanges();

            return data;
        }
        
        public virtual T Update(T data)
        {
            var context = CreateDbContext(new string[]{});
            context.Update(data);                
            context.SaveChangesAsync();

            return data;
        }
        
        public virtual T Find(long id)
        {
            using var context = CreateDbContext(new string[] { });
            return context.Find<T>(id);
        }
        
        public virtual List<T> List()
        {
            using var context = CreateDbContext(new string[] { });
            return context.Set<T>().OrderByDescending(o => o.Id).Take(200).ToList();
        }

        public virtual T Delete(long id)
        {
            var context = CreateDbContext(new string[] { });
            
            var message = context.Find<T>(id);
            if (message == null)
            {
                throw new Exception("Not found");
            }
            
            context.Set<T>().Remove(message);
            context.SaveChanges();

            return message;
        }
    }
}