using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

                    //1= add new recored to movement table (truck number,date,operationtype,etc)
                    TireMovement tireMovment = new TireMovement() { TruckNumber = truckMovement.TruckNumber, SubmitDate = DateTime.Now, MovementType = truckMovement.MovementType,TireManId=truckMovement.UserId };
                    
                    
                    context.TireMovement.Add(tireMovment);
                    //2=add movement details to table movement detail 2 record each, which add changed positions with updated tires.
                    List<MovementDetails> movementDetails = new List<MovementDetails>() {
                new MovementDetails() { CurrentTireDepth=truckMovement.tireWithPoitionViewModels[0].CurrentTireDepth, Position=truckMovement.tireWithPoitionViewModels[0].Position,TireId=truckMovement.tireWithPoitionViewModels[0].TireId, TireMovement=tireMovment, STDthreadDepth=truckMovement.tireWithPoitionViewModels[0].STDThreadDepth , KMWhileChange=truckMovement.tireWithPoitionViewModels[0].KMWhileChange },
                new MovementDetails(){ CurrentTireDepth=truckMovement.tireWithPoitionViewModels[1].CurrentTireDepth, Position=truckMovement.tireWithPoitionViewModels[1].Position,TireId=truckMovement.tireWithPoitionViewModels[1].TireId, TireMovement=tireMovment, STDthreadDepth=truckMovement.tireWithPoitionViewModels[1].STDThreadDepth, KMWhileChange=truckMovement.tireWithPoitionViewModels[1].KMWhileChange  }
                };
                    context.AddRange(movementDetails);
                    context.SaveChanges();
                    //  throw new Exception();
                    //   context.SaveChanges();
                    //1-get changed tires with their new positions(rotation or replacement);
                    var tire1 = truckMovement.tireWithPoitionViewModels[0];
                    var tir2 = truckMovement.tireWithPoitionViewModels[1];
                    //2-get these positions from trucktire table and try to update the tireid  found in these positions with new tireids
                    var trucktires = context.TruckTire.Where(tr =>( tr.TruckNumber == truckMovement.TruckNumber));
                    if (truckMovement.MovementType == MovmentType.Rotation)
                    {
                        //
                        var changedtire2 = trucktires.Where(tr => tr.TirePosition == tire1.Position).FirstOrDefault();
                        //compare position with position and update the attached tire id;
                        changedtire2.TireId = tire1.TireId;
                        changedtire2.LastUdateDate = DateTime.Now;
                        var changedtire1 = trucktires.Where(tr => tr.TirePosition == tir2.Position).FirstOrDefault();
                        changedtire1.TireId = tir2.TireId;
                        changedtire1.LastUdateDate = DateTime.Now;
                        Update(changedtire1);
                        Update(changedtire2);
                    }
                    //in case of replacement
                    //the new  tireid with its position are in the first object of list;
                    else if (truckMovement.MovementType == MovmentType.Replacement)
                    {
                        //get changed position from the trucktire table;
                        var tirewithposition = trucktires.Where(tr => tr.TirePosition == truckMovement.tireWithPoitionViewModels[0].Position).FirstOrDefault();
                        //gets the newtireid  
                        var newtierid = truckMovement.tireWithPoitionViewModels[0].TireId;
                        
                        //try to update postion with the new tireid
                        tirewithposition.TireId = newtierid;
                        tirewithposition.LastUdateDate = DateTime.Now;
                        //set newtire status with running status
                        var newtire = context.tires.Where(t => t.ID == newtierid).FirstOrDefault();
                        newtire.TireStatus = TireStatus.Running;

                        context.tires.Update(newtire);
                        var oldtireId = truckMovement.tireWithPoitionViewModels[1].TireId;
                      //gets old tire in this position and try tochange its status /damgaed/retread
                        var oldtire = context.tires.Where(t => t.ID == oldtireId).FirstOrDefault();
                        oldtire.TireStatus = truckMovement.tireWithPoitionViewModels[1].Position;

                        context.tires.Update(oldtire);
                    }


                    context.SaveChanges();
                    transaction.Commit();
                    var Newtires = context.tires.Where(t => t.TireStatus == TireStatus.New).Count();
                    var Runningtires = context.tires.Where(t => t.TireStatus == TireStatus.Running).Count();
                    var Damagedtires = context.tires.Where(t => t.TireStatus == TireStatus.Damaged).Count();
                    var Retreadtires = context.tires.Where(t => t.TireStatus == TireStatus.ReTread).Count();
                    var alltires = Runningtires + Damagedtires + Retreadtires;
                    hubContext.Clients.All.SendAsync("ReciveNewTransaction", new {alltires=alltires, newtires=Newtires,runningtires=Runningtires,damagedtires=Damagedtires,retreadtires=Retreadtires, id = tireMovment.Id, operation = tireMovment.MovementType, trucknumber = truckMovement.TruckNumber, movmentdate = tireMovment.SubmitDate });
                    return true;
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
            return context.TireMovement.Where(tm => tm.Id == id).Include(tm=>tm.MovementDetails).FirstOrDefault();
        }
        public IEnumerable<TireMovement> GetAllMovements()
        {
            return context.TireMovement.Where(tm => tm.IsRead == false).Include(tm=>tm.Tireman).ToList();
        }
        public IEnumerable<TireMovement> GetTruckMovements(string trucknumber)

        {
         return context.TireMovement.Include(tm=>tm.Tireman).Include(tm => tm.MovementDetails).Where(tm => tm.TruckNumber == trucknumber).ToList();
        }
    }
}
