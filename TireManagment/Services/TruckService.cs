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

  
        public IQueryable<TruckViewModel> GetAll()
        {
            var trucks = context.trucks.Include(truck => truck.Category).Select(c=>new TruckViewModel() { TruckId=c.ID, AxleCount=c.AxleCount,TruckName=c.TruckName,TruckNumber=c.TruckNumber, Category=c.Category.Category, Chassis=c.Chassis, Engine=c.Engine, Manufacturer=c.Manufacturer, Registeration=c.Registeration, Size=c.Size, Unit=c.Unit, TruckCompany=c.Company, VehichleModelNo=c.VehichleModelNo, Type=c.Type, TruckYear=c.Year, Status=c.Status});
            return trucks;
        }
        public int GetTruckCount()
        {
            var trucks = context.trucks.Count();
            return trucks;
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
        public IEnumerable<trucwithknumber> GetAllTruckNumbers()
        {
            return context.trucks.Select(t => new trucwithknumber { Id = t.ID, TruckNumber = t.TruckNumber });
        }
        public Truck GetTruckDetails(int id)
        {
            return context.trucks.Include(t=>t.Category).Where(t => t.ID == id).FirstOrDefault();
        }
        //public IActionResult Excel()
        //{
        //    var trucks=GetAll();
        //    using (var workbook = new XLWorkbook())
        //    {
        //        Truck t;
        //        var worksheet = workbook.Worksheets.Add("Trucks");
        //        var currentRow = 1;
        //        worksheet.Cell(currentRow, 1).Value = "Id";
        //        worksheet.Cell(currentRow, 2).Value = "TruckNumber";
        //        worksheet.Cell(currentRow, 2).Value ="TruckName" ;
        //        worksheet.Cell(currentRow, 2).Value = "Type";
        //        worksheet.Cell(currentRow, 2).Value = "Unit";
        //        worksheet.Cell(currentRow, 2).Value = "VehichleModelNo";
        //        worksheet.Cell(currentRow, 2).Value = "Active";
        //        worksheet.Cell(currentRow, 2).Value = "AxleCount";
        //        worksheet.Cell(currentRow, 2).Value = "Category";
        //        worksheet.Cell(currentRow, 2).Value = "Company";
        //        worksheet.Cell(currentRow, 2).Value = "Registeration";
              
        //        worksheet.Cell(currentRow, 2).Value = "Size";
        //        worksheet.Cell(currentRow, 2).Value = "Status";
        //        worksheet.Cell(currentRow, 2).Value = "Engine";
        //        worksheet.Cell(currentRow, 2).Value = "Chassis";
        //        foreach (var truck in trucks)
        //        {
        //            currentRow++;
        //            worksheet.Cell(currentRow, 1).Value = truck.TruckId;
        //            worksheet.Cell(currentRow, 2).Value = truck.TruckNumber;
        //            worksheet.Cell(currentRow, 3).Value = truck.TruckName;
        //            worksheet.Cell(currentRow, 4).Value = truck.Type;
        //            worksheet.Cell(currentRow, 5).Value = truck.Unit;
        //            worksheet.Cell(currentRow, 6).Value = truck.VehichleModelNo;
        //            worksheet.Cell(currentRow, 7).Value = truck.Active;
        //            worksheet.Cell(currentRow, 8).Value = truck.AxleCount;
        //            worksheet.Cell(currentRow, 9).Value = truck.Category;
        //            worksheet.Cell(currentRow, 10).Value = truck.TruckCompany;
        //            worksheet.Cell(currentRow, 11).Value = truck.Registeration;
                 
        //            worksheet.Cell(currentRow, 12).Value = truck.Size;
        //            worksheet.Cell(currentRow, 13).Value = truck.Status;
        //            worksheet.Cell(currentRow, 14).Value = truck.Engine;
        //            worksheet.Cell(currentRow, 15).Value = truck.Chassis;
        //        }

        //        using (var stream = new MemoryStream())
        //        {
        //            workbook.SaveAs(stream);
        //            var content = stream.ToArray();
                   
        //            return File(content,
        //                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
        //                "trucks.xlsx");
        //        }
        //    }
        //}
    }
}

