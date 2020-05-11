using System;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using System.Linq;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace LambdaFunction
{
    public class Function
    {
        
        // (Required if adding other constructors. Otherwise, optional.) A default constructor
        // called by Lambda. If you are adding your custom constructors,
        // default constructor with no parameters must be added
        public Function(){ }

        // (Optional) An example of injecting a service. As a default constructor is called by Lambda
        // this constructor has to be called from default constructor

        
        /// <summary>
        /// (Required) Entry method of your Lambda function.
        /// </summary>
        /// <param name="lambdaEvent">Type returned from CodeMash</param>
        /// <param name="context">Context data of a function (function config)</param>
        /// <returns></returns>
        public async Task Handler()
        {         
            if (Environment.GetEnvironmentVariable("apiKey") != null)
                CodeMashSettings.ApiKey = Environment.GetEnvironmentVariable("apiKey");

            if (CodeMashSettings.ApiKey == null)
                throw new Exception("ApiKey not set");

            var repo = new Repo();

            var users = await repo.GetAllUsers();
            var userIds = users.Select(x=>x.Id).ToList();

            var cars = await repo.GetAllCars();
            var carsIds = cars.Select(x=>x.Id).ToList();

            var repairs = await repo.GetAllRepairs();
            var shippings = await repo.GetAllShippings();
            var summaries = await repo.GetAllSummaries();
            var trackings = await repo.GetAllTrackings();
            var passresets = await repo.GetAllPasswordResets();

            foreach(var car in cars)
            {
                if(!userIds.Contains(car.User))
                    await repo.DeleteCar(car.Id);          
            }
            
            foreach(var repair in repairs)
            {
                if(!carsIds.Contains(repair.Car))
                    await repo.DeleteRepair(repair.Car);          
            }

            foreach(var shipping in shippings)
            {
                if(!carsIds.Contains(shipping.Car))
                    await repo.DeleteCarShipping(shipping.Car);          
            }

            foreach(var summary in summaries)
            {
                if(!carsIds.Contains(summary.Car))
                    await repo.DeleteCarSummary(summary.Car);          
            }

            foreach(var tracking in trackings)
            {
                if(!carsIds.Contains(tracking.Car))
                    await repo.DeleteCarTracking(tracking.Car);          
            }

            foreach(var passReset in passresets)
            {
                if(passReset.ValidUntil <= DateTime.Now)
                    await repo.DeletePassReset(passReset.Id);          
            }
        }
    }
}
