using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TireManagment.DbModels;

namespace TireManagment.Services
{
    public class BrandService
    {

        public DbContext context;

        public BrandService(DbContext _context)
        {
            context = _context;
        }


        public IQueryable<TireBrand> GetAll()
        {
            var brands = context.brands;
            return brands;
        }

        public TireBrand GetById(int entityId)
        {
            return context.brands.Find(entityId);

        }

        public bool Insert(TireBrand entity)
        {
            var brand = context.Entry(entity);
            if (brand.State != EntityState.Detached)
            {
                brand.State = EntityState.Added;
            }
            else
            {
                context.brands.Add(entity);
            }
            Commit();
            return true;
        }



        public void Update(TireBrand entity)
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

        public void InsertList(List<TireBrand> entityList)
        {
            throw new NotImplementedException();
        }
    }
}
