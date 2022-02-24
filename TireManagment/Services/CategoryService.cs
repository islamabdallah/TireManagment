using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TireManagment.DbModels;

namespace TireManagment.Services
{
    public class CategoryService
    {
        public DbContext context;

        public CategoryService(DbContext _context)
        {
            context = _context;
        }
  

        public IQueryable<TruckCategory> GetAll()
        {
            var categories = context.categories;
            return categories;
        }

        public TruckCategory GetById(int entityId)
        {
            return context.categories.Find(entityId);

        }

        public bool Insert(TruckCategory entity)
        {
            var category = context.Entry(entity);
            if (category.State != EntityState.Detached)
            {
                category.State = EntityState.Added;
            }
            else
            {
                context.categories.Add(entity);
            }
            Commit();
            return true;
        }



        public void Update(TruckCategory entity)
        {
            var dbEntityEntry = context.Entry(entity);

            if (dbEntityEntry.State == EntityState.Detached)
            {
                context.Attach(entity);
            }
            dbEntityEntry.State = EntityState.Modified;
            Commit();
        }
        public void Commit()
        {
            context.SaveChanges();
        }
    
        public void InsertList(List<TruckCategory> entityList)
        {
            context.categories.AddRange(entityList);
            Commit();
        }
    }
}

