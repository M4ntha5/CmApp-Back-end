using CmApp.Contracts.Domains;
using CmApp.Contracts.Entities;
using CmApp.Contracts.Interfaces.Repositories;
using CmApp.Contracts.Interfaces.Services;
using image4ioDotNetSDK.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CmApp.BusinessLogic.Services
{
    public class CarService : ICarService
    {
        private readonly ICarRepository CarRepository;
        private readonly IScraperService WebScraper;
        private readonly IFileRepository FileRepository;
        private readonly ITrackingRepository TrackingRepository;

        public CarService(ICarRepository carRepository, IScraperService webScraper, 
            IFileRepository fileRepository, ITrackingRepository trackingRepository)
        {
            CarRepository = carRepository;
            WebScraper = webScraper;
            FileRepository = fileRepository;
            TrackingRepository = trackingRepository;
        }

        public Task DeleteCar(int userId, int carId)
        {
            throw new NotImplementedException();
        }

        public Task DeleteImages(int carId, List<string> images)
        {
            throw new NotImplementedException();
        }

        public Task<Car> InsertCar(Car car)
        {
            throw new NotImplementedException();
        }

        public Task<List<string>> InsertImages(int carId, List<string> images)
        {
            throw new NotImplementedException();
        }

        public Task UpdateCar(int userId, int carId, Car car)
        {
            throw new NotImplementedException();
        }

        public Task UpdateImages(int carId, Images images)
        {
            throw new NotImplementedException();
        }

        /* private async Task<Car> InsertCarDetailsFromScraper(Car car)
         {
             if (car == null || car.Vin == "" || car.Vin == null)
                 throw new BusinessException("Vin number cannot be null or empty!");
             if (car.Make.Name != "BMW" && car.Make.Name != "Mercedes-benz")
                 throw new BusinessException("Bad vin for this make!");

             //matching parameters to entity
             var parResults = WebScraper.GetVehicleInfo(car.Vin, car.Make.Name);

             Car carEntity = new Car
             {
                 Make = car.Make,                //default for this scraper
             };

             foreach (var param in parResults)
             {
                 if (param.Key == "Prod. Date" || param.Key == "Production Date")
                     carEntity.ManufactureDate = Convert.ToDateTime(param.Value);
                 else if (param.Key == "Type" || param.Key == "Model")
                     carEntity.Model.Name = param.Value;
                 else if (param.Key == "Series")
                     carEntity.Series = param.Value;
                 else if (param.Key == "Body Type")
                     carEntity.BodyType = param.Value;
                 else if (param.Key == "Steering")
                     carEntity.Steering = param.Value;
                 else if (param.Key == "Engine")
                     carEntity.Engine = param.Value;
                 else if (param.Key == "Displacement")
                     carEntity.Displacement = Double.Parse(param.Value);
                 else if (param.Key == "Power")
                     carEntity.Power = param.Value;
                 else if (param.Key == "Drive")
                     carEntity.Drive = param.Value;
                 else if (param.Key == "Transmission")
                     carEntity.Transmission = param.Value;
                 else if (param.Key == "Colour")
                     carEntity.Color = param.Value;
                 else if (param.Key == "Upholstery")
                     carEntity.Interior = param.Value;
             }
             if (carEntity.Drive == "HECK")
                 carEntity.Drive = "Rear wheel drive";
             else if (carEntity.Drive == "ALLR")
                 carEntity.Drive = "All wheel drive";
             //making first letter upper
             if (carEntity.BodyType.ToLower().Contains("Coup".ToLower()))
                 carEntity.BodyType = "Coupe";
             else if (carEntity.BodyType == "SAV")
                 carEntity.BodyType = "SUV";
             else if (carEntity.BodyType == "LIM")
                 carEntity.BodyType = "Limousine";

             var eqResults = WebScraper.GetVehicleEquipment(car.Vin, car.Make.Name);
             var equipment = new List<Contracts.Entities.Equipment>();

             foreach (var eq in eqResults)
                 equipment.Add(new Contracts.Entities.Equipment() { Code = eq.Key, Name = eq.Value });

             carEntity.Equipment = equipment;
             carEntity.Vin = car.Vin.ToUpper();

             //inserting vehicle data
             var insertedCar = await CarRepository.InsertCar(carEntity);

             await InsertImages(insertedCar.ID, car.Base64images);

             //inserts empty tracking 
             await TrackingRepository.InsertTracking(new Tracking { Car = insertedCar });
             return insertedCar;

         }

         private async Task<Car> InsertOtherCar(Car car)
         {
             if (car == null)
                 throw new BusinessException("Can not insert car, because car is empty!");

             car.Vin = car.Vin.ToUpper();

             //inserting vehicle data
             var insertedCar = await CarRepository.InsertCar(car);

             await InsertImages(insertedCar.ID, car.Base64images);

             //inserts empty tracking 
             await TrackingRepository.InsertTracking(new Tracking { Car = insertedCar });
             return insertedCar;
         }

         public async Task<Car> InsertCar(Car car)
         {
             var userCars = await CarRepository.GetAllUserCars(1);//car.User);
             var userVins = userCars.Select(x => x.Vin).ToList();

             if (car.Make == null || car.Make.Name == "")
                 throw new BusinessException("Make not defined");

             if (userVins.Contains(car.Vin))
                 throw new BusinessException("There is already a car with this VIN number");

             if ((car.Make.Name == "BMW" || car.Make.Name == "Mercedes-benz") && car.Model.Name == "")
                 return await InsertCarDetailsFromScraper(car);
             else
                 return await InsertOtherCar(car);
         }
        */
        /*  public async Task DeleteCar(int userId, int carId)
          {
              var car = await CarRepository.GetCarById(carId);
             // if (car.User != userId)
             //     throw new BusinessException("Car does not exist");

              var tracking = await TrackingRepository.GetTrackingByCar(carId);
              await CarRepository.DeleteCar(car.ID);
              await FileRepository.DeleteFolder("/cars/" + carId);

              await FileRepository.DeleteFolder("/tracking/" + tracking.ID);
          }

          public async Task UpdateCar(int userId, int carId, Car car)
          {
             // if (car.User != userId)
             //     throw new BusinessException("Car does not exist");

              var list = car.Equipment.Select(x => x.Code).ToList().Distinct().Count();
              if (car.Equipment.Count != list)
                  throw new BusinessException("Car cannot have multiple equipment with the same code!");

              await CarRepository.UpdateCar(carId, car);
          }

          public async Task<List<string>> InsertImages(int carId, List<string> images)
          {
              if (images != null && images.Count > 0)
              {
                  //deletes form cloud
                  await FileRepository.DeleteFolder("/cars/" + carId);
                  //deletes from db
                  await CarRepository.DeleteAllCarImages(carId);

                  var imgsList = new List<UploadImageRequest.File>();

                  int count = 1;
                  images.ForEach(x => imgsList.Add(
                      new UploadImageRequest.File()
                      {
                          FileName = count++ + ".jpeg",
                          Data = new MemoryStream(FileRepository.Base64ToByteArray(x.Split(',')[1]))
                      }
                  ));

                  //inserts to cloud 
                  var insertedUrls = await FileRepository.InsertCarImages(carId, imgsList);
                  //inserts to db
                  await CarRepository.UploadImageToCar(carId, insertedUrls);
                  return insertedUrls;
              }
              else
              {
                  //deletes form cloud
                  await FileRepository.DeleteFolder("/cars/" + carId);
                  //deletes from db
                  await CarRepository.DeleteAllCarImages(carId);
                  return null;
              }
          }

          public async Task DeleteImages(int carId, List<string> images)
          {
              if (images != null && images.Count > 0)
              {
                  foreach (var img in images)
                  {
                      var path = img.Split("cmapp")[1];
                      await FileRepository.DeleteImage(path);
                  }
              }
          }

          public async Task UpdateImages(int carId, Images images)
          {
              //deleting images from cloud
              await DeleteImages(carId, images.Deleted);

              List<string> urls = new List<string>();
              List<string> base64 = new List<string>();
              //images.All.ForEach(x =>
              //{
              //    if (x.Url.StartsWith("http"))
              //        urls.Add(x.Url);
              //    else
              //        base64.Add(x.Url);
              //});

              if (base64.Count > 0)
              {
                  var imgsList = new List<UploadImageRequest.File>();
                  int count = 1;
                  base64.ForEach(x => imgsList.Add(
                      new UploadImageRequest.File()
                      {
                          FileName = count++ + ".jpeg",
                          Data = new MemoryStream(FileRepository.Base64ToByteArray(x.Split(',')[1]))
                      }
                  ));

                  //inserts to cloud 
                  var insertedUrls = await FileRepository.InsertCarImages(carId, imgsList);
                  if (insertedUrls != null)
                  {
                      insertedUrls.ForEach(x => urls.Add(x));
                  }
              }
              await CarRepository.UploadImageToCar(carId, urls);
          }*/
    }
}
