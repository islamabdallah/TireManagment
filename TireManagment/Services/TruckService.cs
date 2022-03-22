using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
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

   
        public async Task<IEnumerable<TruckViewModel>> GetAll()
        {
            var trucks = context.trucks.Include(truck => truck.Category).Select(c=>new TruckViewModel() { TruckId=c.ID, AxleCount=c.AxleCount,TruckName=c.TruckName,TruckNumber=c.TruckNumber, Category=c.Category.Category, Chassis=c.Chassis, Engine=c.Engine, Manufacturer=c.Manufacturer, Registeration=c.Registeration, Size=c.Size, Unit=c.Unit, TruckCompany=c.Company, VehichleModelNo=c.VehichleModelNo, Type=c.Type, TruckYear=c.Year, Status=c.Status}).ToList();
            return trucks;
        }
        public int GetTruckCount()
        {
            var trucks = context.trucks.Count();
            return trucks;
        }
        public Truck GetFirstTruck()
        {
            return  context.trucks.Include(t=>t.Category).FirstOrDefault();
        }
        public IEnumerable<TireMovement> GetTruckMovemnts(DateTime sdate,DateTime edate,string number)
        {

         var movements=context.TireMovement.Where(m => m.TruckNumber == number && m.SubmitDate>=sdate && m.SubmitDate <= edate).Include(m=>m.MovementDetails).ThenInclude(m=>m.tire).Include(m=>m.Tireman).OrderByDescending(m=>m.SubmitDate).ToList();
            return movements;
        }
        public Truck GetById(int entityId)
        {
            return context.trucks.Find(entityId);

        }

        public bool Insert(Truck entity)
        {
            try
            {
                var truckpositions = context.tiredsitributions.Include(td=>td.Category).Where(td => td.Category.Id == entity.CategoryId).ToList();
                List<TruckTire> truckTires = new();
                foreach (var position in truckpositions)
                {
                    truckTires.Add(new TruckTire() { LastUdateDate = DateTime.Now, TirePosition = position.Position, TruckNumber = entity.TruckNumber });
                }

                var category = context.Entry(entity);
                if (category.State != EntityState.Detached)
                {
                    category.State = EntityState.Added;
                }
                else
                {
                    context.trucks.Add(entity);
                }
                context.TruckTire.AddRange(truckTires);
                Commit();
                return true;
            }
            catch(Exception)
            {
                return false;
            }
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
       
        public IEnumerable<TruckTireViewModel>  GetTruckTires(string TruckNumber)
        {
         
            var res= context.TruckTire.Where(tr=>tr.TruckNumber==TruckNumber).Include(t => t.tire).Select(t => new TruckTireViewModel() { Id = t.Id, LastUpdateTime = t.LastUdateDate, TirePosition = t.TirePosition,TireId= (int)t.TireId, TireSerial = t.tire.Serial,Tirebrand=t.tire.Brand.Name, TruckNumber = t.TruckNumber,TireStatus=t.tire.TireStatus }).ToList();

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
        public IEnumerable<trucwithknumber> GetAllTruckNumbers()
        {
            return context.trucks.Select(t => new trucwithknumber { Id = t.ID, TruckNumber = t.TruckNumber }).ToList();
        }
        public Truck GetTruckDetails(int id)
        {
            return context.trucks.Include(t=>t.Category).Where(t => t.ID == id).FirstOrDefault();
        }
        public List<Truck> gettrucks(string[]trucks)
        {
           var x= context.trucks.Where(t => trucks.Contains(t.TruckNumber)).ToList();
            return x;
        }
        public Truck GeTruck(string trucknumber)
        {
            return context.trucks.Where(t => t.TruckNumber == trucknumber).FirstOrDefault();
        }
       
    }
}

