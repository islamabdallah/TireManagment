using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TireManagment.DbModels;
using TireManagment.Models;

namespace TireManagment.Services
{
    public class TruckService
    {
        public DbContext context;

        public TruckService(DbContext _context)
        {
            context = _context;
        }

  
        public IQueryable<TruckViewModel> GetAll()
        {
            var trucks = context.trucks.Include(truck => truck.Category).Select(c=>new TruckViewModel() { TruckId=c.ID, AxleCount=c.AxleCount,TruckName=c.TruckName,TruckNumber=c.TruckNumber, Category=c.Category.Category, Chassis=c.Chassis, Engine=c.Engine, Manufacturer=c.Manufacturer, Registeration=c.Registeration, Size=c.Size, Unit=c.Unit, TruckCompany=c.Company, VehichleModelNo=c.VehichleModelNo, Type=c.Type, TruckYear=c.Year, Status=c.Status});
            return trucks;
        }
     

        public Truck GetById(int entityId)
        {
            return context.trucks.Find(entityId);

        }

        public bool Insert(Truck entity)
        {
            var category = context.Entry(entity);
            if (category.State != EntityState.Detached)
            {
                category.State = EntityState.Added;
            }
            else
            {
                context.trucks.Add(entity);
            }
        
            return true;
        }



        public void Update(Truck entity)
        {
            var dbEntityEntry = context.Entry(entity);

            if (dbEntityEntry.State == EntityState.Detached)
            {
                context.Attach(entity);
            }
            dbEntityEntry.State = EntityState.Modified;
            Commit();
        }
       
        public IQueryable GetTruckTires(string TruckNumber)
        {
         
            var res= context.TruckTire.Where(tr=>tr.TruckNumber==TruckNumber).Include(t => t.tire).Select(t => new TruckTireViewModel() { Id = t.Id, LastUpdateTime = t.LastUdateDate, Position = t.TirePosition, TireSerial = t.tire.Serial,Tirebrand=t.tire.Brand.Name, TruckNumber = t.TruckNumber,TireStatus=t.tire.TireStatus });

            return res;
        }
        public void Commit()
        {
            context.SaveChanges();
        }
  
        public void InsertList(List<TruckCategory> entityList)
        {
            throw new NotImplementedException();
        }
      public bool  checknumberExists(string number)
        {
            var x = context.trucks.Where(t => t.TruckNumber == number);
          var res= context.trucks.Any(t => t.TruckNumber == number);
            return res;
        }
    }
}

