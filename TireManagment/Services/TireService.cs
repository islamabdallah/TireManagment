using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using TireManagment.DbModels;
using TireManagment.Models;
using TireManagment.Enums;

namespace TireManagment.Services
{
    public class TireService
    {
        public DbContext context;

        public TireService(DbContext _context)
        {
            context = _context;
        }
        public string GetTireCategoryImage(int tireid)
        {
           string trucknumber= context.TruckTire.Where(tr => tr.TireId == tireid).Select(tr => tr.TruckNumber).FirstOrDefault();
          string image=context.trucks.Where(tr => tr.TruckNumber == trucknumber).Include(tr => tr.Category).Select(tr => tr.Category.Image).FirstOrDefault();
            return image;
        }
        public Tire GetFirstTire()
        {
            return context.tires.Include(t=>t.Brand).FirstOrDefault();
        }
        public IEnumerable<MovementDetails> GetTireMovemnts(DateTime sdate, DateTime edate, int tireid,bool all)

        {
            List<MovementDetails> movements=null;
            if (all)
            {
                movements = context.MovementDetails.Include(m => m.tire).Include(md => md.TireMovement).ThenInclude(m => m.Tireman).Where(md => md.TireMovement.SubmitDate >= sdate && md.TireMovement.SubmitDate <= edate).Include(md => md.TireMovement.Tireman).OrderByDescending(m => m.TireMovement.SubmitDate).ToList();
            }
            else
            {

                movements = context.MovementDetails.Include(m => m.tire).Include(md => md.TireMovement).ThenInclude(m => m.Tireman).Where(md => md.TireId == tireid && md.TireMovement.SubmitDate >= sdate && md.TireMovement.SubmitDate <= edate).Include(md => md.TireMovement.Tireman).OrderByDescending(m => m.TireMovement.SubmitDate).ToList();
            }
            return movements;
        }
        //public IEnumerable<MovementDetails> GetallTireMovemnts(DateTime sdate, DateTime edate)
        //{

        //    var movements = context.MovementDetails.Include(m => m.tire).Include(md => md.TireMovement).ThenInclude(m => m.Tireman).Where(md =>md.TireMovement.SubmitDate >= sdate && md.TireMovement.SubmitDate <= edate).Include(md => md.TireMovement.Tireman).OrderByDescending(m => m.TireMovement.SubmitDate).ToList();
        //    return movements;
        //}
        public IEnumerable<Tire> GetAll()
        {
            var tires = context.tires.Include(t => t.Brand).ToList();
            return tires;
        }

        public IEnumerable<Tire> GetNewTires()
        {
            var tires = context.tires.Include(t => t.Brand).Where(r => r.TireStatus == TireStatus.New || r.TireStatus == TireStatus.Retread).ToList();
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
        public IEnumerable<tirewithserial> GetNewandRetreadtires()
        {
          var tires=  context.tires.Where(t => t.TireStatus == TireStatus.New || t.TireStatus == TireStatus.Retread).Select(t => new tirewithserial() { serial = t.Serial, tierid = (int)t.TireId, size = t.Size , brand = t.Brand.Name }).ToList();
            return tires;
        }
        public void Update(Tire entity)
        {
            var dbEntityEntry = context.Entry(entity);

            if (dbEntityEntry.State == EntityState.Detached)
            {
                context.Attach(entity);
            }
            dbEntityEntry.State = EntityState.Modified;
            //Commit();
        }
        public void updatetorunning(int tireid)
        {
          var tire=  context.tires.Where(t => t.TireId == tireid).FirstOrDefault();
            tire.TireStatus = TireStatus.Running;
            Update(tire);

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
            return context.tires.Select(t => new tirewithserial() { serial = t.Serial, tierid = t.ID }).ToList();
        }

        public Tire GetTireDetails(int id)
        {
            
            return context.tires.Include(t => t.Brand).Where(t => t.ID == id).FirstOrDefault();
        }
        public string GetTireSerial(int id)
        {

            return context.tires.Where(t => t.ID == id).Select(t=>t.Serial).FirstOrDefault();
        }
        public string GetTruckNumber(int id)
        {
            var trucknumber = context.TruckTire.Where(tr => tr.TireId == id).Select(tr => tr.TruckNumber).FirstOrDefault();
            return trucknumber;
        }

        public IEnumerable<MovementDetails> GetTireHistory(int tireid)
        {
            return context.MovementDetails.OrderByDescending(m=>m.TireMovement.SubmitDate).Include(m=>m.TireMovement).Include(t=>t.TireMovement.Tireman).Where(m => m.TireId == tireid).ToList();
        }
        public IEnumerable<Tire> Tires()
        {
            return context.tires.ToList();
        }
    }
}

