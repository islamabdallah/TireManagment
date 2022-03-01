﻿using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TireManagment.DbModels;
using TireManagment.Models;

namespace TireManagment.Services
{
    public class TireService
    {
        public DbContext  context;

        public TireService(DbContext _context)
        {
            context = _context;
        }

        public IQueryable<Tire> GetAll()
        {
            var tires = context.tires.Include(t=>t.Brand);
            return tires;
        }

        public Tire GetById(int entityId)
        {
            return context.tires.Find(entityId);

        }

        public void Insert(Tire entity)
        {
            var category = context.Entry(entity);
            if (category.State != EntityState.Detached)
            {
                category.State = EntityState.Added;
            }
            else
            {
                context.tires.Add(entity);
            }
          Commit();


            //if (entity.TireId == 0)
            //{

            //    entity.TireId = entity.ID;
            //    Update(entity);
            //}
       
          
        }



        public void Update(Tire entity)
        {
            var dbEntityEntry = context.Entry(entity);

            if (dbEntityEntry.State == EntityState.Detached)
            {
                context.Attach(entity);
            }
            dbEntityEntry.State = EntityState.Modified;
            Commit();
            
        }
      public bool checkserialExists(string serial)
        {
          return context.tires.Any(t => t.Serial == serial);
        }
        public void Commit()
        {
           
                context.SaveChanges();
          
        }
    
        public void InsertList(List<TruckCategory> entityList)
        {
            throw new NotImplementedException();
        }
        public IEnumerable<tirewithserial> gettireserials()
        {
            return context.tires.Select(t => new tirewithserial() { serial = t.Serial, tierid = t.ID });
        }
        public Tire GetTireDetails(int id)
        {
            
            return context.tires.Include(t => t.Brand).Where(t => t.ID == id).FirstOrDefault();
        }
        public string GetTruckNumber(int id)
        {
            var trucknumber = context.TruckTire.Where(tr => tr.TireId == id).Select(tr => tr.TruckNumber).FirstOrDefault();
            return trucknumber;
        }
        public IEnumerable<MovementDetails> GetTireHistory(int tireid)
        {
            return context.MovementDetails.Include(m=>m.TireMovement).Include(t=>t.TireMovement.Tireman).Where(m => m.TireId == tireid);
        }
    }
}

