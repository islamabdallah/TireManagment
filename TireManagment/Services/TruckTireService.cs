using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using TireManagment.DbModels;
using TireManagment.Enums;
using TireManagment.Hubs;
using TireManagment.Models;

namespace TireManagment.Services
{
    public class TruckTireService
    {
        public DbContext context;
        public IHubContext<notifyHub> hubContext;

        public TruckTireService(DbContext _context, IHubContext<notifyHub> _hubContext)
        {
            hubContext = _hubContext;
            context = _context;
            
        }
        public bool AddNewMovement(TruckMovementViewModel truckMovement)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    if (truckMovement != null && truckMovement.TirePositionViewModel.Count == 2)
                    {
                        //Step#1 : [Table : Movement] : Add
                        
                        TireMovement tireMovment = new()
                        {
                            TruckNumber = truckMovement.TruckNumber,
                            MovementType = truckMovement.MovementType,
                            TireManId = truckMovement.UserId,
                            SubmitDate = DateTime.Now
                        };
                        context.TireMovement.Add(tireMovment);


                        //Step#2 : [Table : MovementDetails] : Add Details

                        List<MovementDetails> movementDetails = new();
                        foreach (var item in truckMovement.TirePositionViewModel)
                        {
                            var _tireMovement = new MovementDetails()
                            {
                                TireId = item.TireId,
                                CurrentTireDepth = item.CurrentTireDepth,
                                STDthreadDepth = item.STDThreadDepth,
                                KMWhileChange = item.KMWhileChange,
                                Position = item.Position,
                                TireMovement = tireMovment
                            };
                            movementDetails.Add(_tireMovement);
                        }
                        context.AddRange(movementDetails);
                        context.SaveChanges();


                        //Step#3 : [Table : TruckTire] : Update truck with the new tires

                        var _tire1 = truckMovement.TirePositionViewModel[0];
                        var _tire2 = truckMovement.TirePositionViewModel[1];
                        var _trucktires = context.TruckTire.Where(tr => (tr.TruckNumber == truckMovement.TruckNumber));

                        //Rotation
                        if (truckMovement.MovementType == MovmentType.Rotation)
                        {
                            //Position#1
                            var _updatePosition1 = _trucktires.Where(tr => tr.TirePosition == _tire1.Position).FirstOrDefault();
                            _updatePosition1.TireId = _tire1.TireId;
                            _updatePosition1.LastUdateDate = DateTime.Now;

                            //Position#2
                            var _updatePosition2 = _trucktires.Where(tr => tr.TirePosition == _tire2.Position).FirstOrDefault();
                            _updatePosition2.TireId = _tire2.TireId;
                            _updatePosition2.LastUdateDate = DateTime.Now;
                            Update(_updatePosition1);
                            Update(_updatePosition2);
                        }
                        //Replacement
                        else if (truckMovement.MovementType == MovmentType.Replacement)
                        {
                            var _tirePosition = _trucktires.Where(tr => tr.TirePosition == truckMovement.TirePositionViewModel[0].Position).FirstOrDefault();
                            _tirePosition.TireId = truckMovement.TirePositionViewModel[0].TireId;
                            _tirePosition.LastUdateDate = DateTime.Now;

                            //set newtire status with running status
                            var updateTireStatus = context.tires.Where(t => t.ID == _tirePosition.TireId).FirstOrDefault();
                            updateTireStatus.TireStatus = TireStatus.Running;
                            context.tires.Update(updateTireStatus);

                            //gets old tire in this position and try tochange its status /damgaed/retread
                            var _oldTire = context.tires.Where(t => t.ID == truckMovement.TirePositionViewModel[1].TireId).FirstOrDefault();
                            _oldTire.TireStatus = truckMovement.TirePositionViewModel[1].Position;
                            context.tires.Update(_oldTire);
                        }

                        context.SaveChanges();
                        transaction.Commit();
                        var Newtires = context.tires.Where(t => t.TireStatus == TireStatus.New).Count();
                        var Runningtires = context.tires.Where(t => t.TireStatus == TireStatus.Running).Count();
                        var Damagedtires = context.tires.Where(t => t.TireStatus == TireStatus.Damaged).Count();
                        var Retreadtires = context.tires.Where(t => t.TireStatus == TireStatus.Retread).Count();
                        var alltires = Runningtires + Damagedtires + Retreadtires;
                        hubContext.Clients.All.SendAsync("ReciveNewTransaction", new { alltires = alltires, newtires = Newtires, runningtires = Runningtires, damagedtires = Damagedtires, retreadtires = Retreadtires, id = tireMovment.Id, operation = tireMovment.MovementType, trucknumber = truckMovement.TruckNumber, movmentdate = tireMovment.SubmitDate });
                     
                        return true;
                    }
                    return false;
                }
                catch (Exception e)
                {
                    return false;
                }
            }
        }
        public void Update(TruckTire entity)
        {
            var dbEntityEntry = context.Entry(entity);

            if (dbEntityEntry.State == EntityState.Detached)
            {
                context.Attach(entity);
            }
            dbEntityEntry.State = EntityState.Modified;
            
        }
        public void UpdateMovement(TireMovement entity)
        {
            var dbEntityEntry = context.Entry(entity);

            if (dbEntityEntry.State == EntityState.Detached)
            {
                context.Attach(entity);
            }
            dbEntityEntry.State = EntityState.Modified;
            context.SaveChanges();

        }
        public bool Insert(TireMovement entity)
        {
            var category = context.Entry(entity);
            if (category.State != EntityState.Detached)
            {
                category.State = EntityState.Added;
            }
            else
            {
                context.TireMovement.Add(entity);
            }
            context.SaveChanges();
            return true;
        }
        public IEnumerable<TireMovement> GetMovments()
        {
           var res= context.TireMovement.Where(m=>m.IsRead==false).OrderByDescending(m=>m.SubmitDate).Take(5).ToList();
            return res;
        }
        public IEnumerable<TireMovement> GetTopMovments()
        {
            var res = context.TireMovement.OrderByDescending(m => m.SubmitDate).Take(5).ToList();
            return res;
        }
        public int GetMovmentsCount()
        {
            return context.TireMovement.Where(m => m.IsRead == false).Count();
        }
        public TireMovement GetMovementDetail(int id)
        {
            return context.TireMovement.Where(tm => tm.Id == id).Include(tm=>tm.MovementDetails).Include(m=>m.Tireman).FirstOrDefault();
        }
        public IEnumerable<TireMovement> GetAllMovements()
        {
            return context.TireMovement.Where(tm => tm.IsRead == false).Include(tm=>tm.Tireman).ToList();
        }
        public string GetCategoryImage(string trucknumber)
        {
            string image = context.trucks.Include(t => t.Category).Where(t => t.TruckNumber == trucknumber).Select(t => t.Category.Image).FirstOrDefault();
            return image;
        }
        public IEnumerable<TireMovement> GetTruckMovements(string trucknumber)

        {
         return context.TireMovement.Include(tm=>tm.Tireman).Include(tm => tm.MovementDetails).Where(tm => tm.TruckNumber == trucknumber).ToList();
        }
    }
}
