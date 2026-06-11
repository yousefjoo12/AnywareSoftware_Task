using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Repository.Data
{
    public static class StoreContextSeed
    {
        public async static Task SeedAsync(StoreContext _dbcontext)
        { 
            //var deliverysData = File.ReadAllText("../Talabat.Repository/Data/DataSeeding/delivery.json");
            //var DeliveryMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(deliverysData);
            //if (DeliveryMethods.Count() > 0)
            //{
            //    if (_dbcontext.DeliveryMethods.Count() == 0)
            //    {
            //        foreach (var Methods in DeliveryMethods)
            //        {
            //            _dbcontext.Set<DeliveryMethod>().Add(Methods);
            //        }
            //        await _dbcontext.SaveChangesAsync();
            //    }
            //}
        }
    }
}
