using CmApp.Contracts;
using CmApp.Domains;
using CmApp.Entities;
using CmApp.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CmApp.Services
{
    public class CarService : ICarService
    {
        public ICarRepository CarRepository { get; set; }
        public IScraperService WebScraper { get; set; }
        public ISummaryRepository SummaryRepository { get; set; }
        public IFileRepository FileRepository { get; set; }
        public ITrackingRepository TrackingRepository { get; set; }

        private async Task<CarEntity> InsertCarDetailsFromScraperBMW(CarEntity car)
        {
            if (car == null || car.Vin == "" || car.Vin == null)
                throw new BusinessException("Vin number cannot be null or empty!");
            if (car.Make != "BMW")
                throw new BusinessException("Bad vin for this make!");

            //matching parameters to entity
            var parResults = WebScraper.GetVehicleInfo(car.Vin, car.Make);

            CarEntity carEntity = new CarEntity
            {
                Make = car.Make,                //default for this scraper
                User = car.User
            };

            foreach (var param in parResults)
            {
                if (param.Key == "Prod. Date")
                    carEntity.ManufactureDate = Convert.ToDateTime(param.Value);
                else if (param.Key == "Type")
                    carEntity.Model = param.Value;
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


            var eqResults = WebScraper.GetVehicleEquipment(car.Vin, car.Make);
            var equipment = new List<Equipment>();

            foreach (var eq in eqResults)
                equipment.Add(new Equipment() { Code = eq.Key, Name = eq.Value });

            carEntity.Equipment = equipment;
            carEntity.Vin = car.Vin.ToUpper();
            carEntity.MainImageUrl = Settings.DefaultImageUrl;

            //inserting vehicle data
            var insertedCar = await CarRepository.InsertCar(carEntity);

            //image upload here
            if (car.Base64images != null && car.Base64images.Count > 0)
            {
                int count = 1;
                foreach (var image in car.Base64images)
                {
                    //spliting base64 front and getting image format and base64 string 
                    var split = image.Split(';');
                    var imageType = split[0].Split('/')[1];
                    var base64 = split[1].Split(',')[1];
                    var imgName = insertedCar.Id + "_image" + count + "." + imageType;

                    var bytes = FileRepository.Base64ToByteArray(base64);
                    var res = await CarRepository.UploadImageToCar(insertedCar.Id, bytes, imgName);
                    if (count == 1)
                    {
                        var url = await FileRepository.GetFileUrl(res.Key);
                        await CarRepository.UpdateCarMainImg(insertedCar.Id, url);
                    }
                    
                    count++;
                }
            }

           /* else
            {
                var defaultImg = await FileRepository.GetFile(Settings.DefaultImage);
                using MemoryStream ms = new MemoryStream();
                defaultImg.CopyTo(ms);
                var bytes = ms.ToArray();
                var imgName = "Default-image.jpg";
                await CarRepository.UploadImageToCar(insertedCar.Id, bytes, imgName);
            }*/
            //inserts empty tracking 
            await TrackingRepository.InsertTracking(new TrackingEntity { Car = insertedCar.Id });
            return insertedCar;

        }

        private async Task<CarEntity> InsertCarDetailsFromScraperMB(CarEntity car)
        {
            if (car == null || car.Vin == "" || car.Vin == null)
                throw new BusinessException("Vin number cannot be null or empty!");
            if (car.Make != "Mercedes-benz")
                throw new BusinessException("Bad vin for this make!");

            //matching parameters to entity
            var parResults = WebScraper.GetVehicleInfo(car.Vin, car.Make);

            CarEntity carEntity = new CarEntity
            {
                //default for this scraper
                Make = car.Make,
                User = car.User
            };

            foreach (var param in parResults)
            {
                if (param.Key == "Production Date")
                    carEntity.ManufactureDate = Convert.ToDateTime(param.Value);
                else if (param.Key == "Model")
                    carEntity.Model = param.Value;
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
            if (carEntity.BodyType.ToLower().Contains("coup"))
                carEntity.BodyType = "Coupe";
            else if (carEntity.BodyType == "SAV")
                carEntity.BodyType = "SUV";
            else if (carEntity.BodyType.ToLower().Contains("lim"))
                carEntity.BodyType = "Limousine";

            //matching equipment to entity
            var eqResults = WebScraper.GetVehicleEquipment(car.Vin, car.Make);
            var equipment = new List<Equipment>();

            foreach (var eq in eqResults)
                equipment.Add(new Equipment() { Code = eq.Key, Name = eq.Value });

            carEntity.Equipment = equipment;
            carEntity.Vin = car.Vin.ToUpper();

            carEntity.MainImageUrl = Settings.DefaultImageUrl;
            //inserting vehicle data
            var insertedCar = await CarRepository.InsertCar(carEntity);

            //image upload here
            if (car.Base64images != null && car.Base64images.Count > 0)
            {
                int count = 1;
                foreach (var image in car.Base64images)
                {
                    //spliting base64 front and getting image format and base64 string 
                    var split = image.Split(';');
                    var imageType = split[0].Split('/')[1];
                    var base64 = split[1].Split(',')[1];
                    var imgName = insertedCar.Id + "_image" + count + "." + imageType;

                    var bytes = FileRepository.Base64ToByteArray(base64);
                    var res = await CarRepository.UploadImageToCar(insertedCar.Id, bytes, imgName);
                    if (count == 1)
                    {
                        var url = await FileRepository.GetFileUrl(res.Key);
                        await CarRepository.UpdateCarMainImg(insertedCar.Id, url);
                    }
                    count++;
                }
            }
           /* else
            {
                var defaultImg = await FileRepository.GetFile(Settings.DefaultImage);
                using MemoryStream ms = new MemoryStream();
                defaultImg.CopyTo(ms);
                var bytes = ms.ToArray();
                var imgName = "Default-image.jpg";
                await CarRepository.UploadImageToCar(insertedCar.Id, bytes, imgName);
            }*/
            //inserts empty tracking 
            await TrackingRepository.InsertTracking(new TrackingEntity { Car = insertedCar.Id });
            return insertedCar;

        }
        private async Task<CarEntity> InsertOtherCar(CarEntity car)
        {
            if (car == null)
                throw new BusinessException("Can not insert car, because car is empty!");

            car.Vin = car.Vin.ToUpper();
            string mainImg = "";
            if (car.Base64images.Count > 0)
                mainImg = car.Base64images[0];
            else
                mainImg = Settings.DefaultImageUrl;
            car.MainImageUrl = mainImg;
            //inserting vehicle data
            var insertedCar = await CarRepository.InsertCar(car);

            //image upload here
            if (car.Base64images != null && car.Base64images.Count > 0)
            {
                int count = 1;
                foreach (var image in car.Base64images)
                {
                    //spliting base64 begining and getting image format and base64 string 
                    var split = image.Split(';');
                    var imageType = split[0].Split('/')[1];
                    var base64 = split[1].Split(',')[1];
                    var imgName = insertedCar.Id + "_image" + count + "." + imageType;

                    var bytes = FileRepository.Base64ToByteArray(base64);
                    var res = await CarRepository.UploadImageToCar(insertedCar.Id, bytes, imgName);
                    if (count == 1)
                    {
                        var url = await FileRepository.GetFileUrl(res.Key);
                        await CarRepository.UpdateCarMainImg(insertedCar.Id, url);
                    }
                    count++;
                }
            }
           /* else
            {
                var defaultImg = await FileRepository.GetFile(Settings.DefaultImage);
                using MemoryStream ms = new MemoryStream();
                defaultImg.CopyTo(ms);
                var bytes = ms.ToArray();
                var imgName = "Default-image.jpg";
                await CarRepository.UploadImageToCar(insertedCar.Id, bytes, imgName);
            }*/
            //inserts empty tracking 
            await TrackingRepository.InsertTracking(new TrackingEntity { Car = insertedCar.Id });
            return insertedCar;
        }

        public async Task<CarEntity> InsertCar(CarEntity car)
        {
            var userCars = await GetAllUserCars(car.User);
            var userVins = userCars.Select(x => x.Vin).ToList();
            if (userVins.Contains(car.Vin))
                throw new BusinessException("There is already a car with this VIN number");

            if (car.Make == null || car.Make == "")
                throw new BusinessException("Make not defined");
            if (car.Make == "BMW" && car.Model == "")
                return await InsertCarDetailsFromScraperBMW(car);
            else if (car.Make == "Mercedes-benz" && car.Model == "")
                return await InsertCarDetailsFromScraperMB(car);
            else
                return await InsertOtherCar(car);
        }

        public async Task DeleteCar(string userId, string carId)
        {
            var car = await CarRepository.GetCarById(carId);
            if (car.User != userId)
                throw new BusinessException("Car does not exist");

            await CarRepository.DeleteCar(car.Id);
        }
        public async Task<List<CarDisplay>> GetAllUserCars(string userid)
        {
            var cars = await CarRepository.GetAllUserCars(userid);
            if (cars.Count == 0)
                return cars;

            foreach (var car in cars)
            {
                if (car.Images.Count != 0)
                {
                    var fileInfo = FileRepository.GetFileId(car.Images[0]);
                    var fileId = fileInfo.Item1;

                    var url = await FileRepository.GetFileUrl(fileId);

                    car.MainImageUrl = url;
                }
                else
                {
                    var tracking = await TrackingRepository.GetTrackingByCar(car.Id);
                    if(tracking.AuctionImages.Count != 0 && tracking.ShowImages)
                    {
                        var fileInfo = FileRepository.GetFileId(tracking.AuctionImages[0]);
                        var fileId = fileInfo.Item1;

                        var url = await FileRepository.GetFileUrl(fileId);

                        car.MainImageUrl = url;
                    }
                    else
                        car.MainImageUrl = Settings.DefaultImageUrl;
                }
                    
            }
            return cars;
            
        }

        public async Task UpdateCar(string userId, string carId, CarEntity car)
        {
            if (car.User != userId)
                throw new BusinessException("Car does not exist");
            await CarRepository.UpdateCar(carId, car);
            await CarRepository.DeleteAllCarImages(carId);

            if(car.Base64images.Count > 0)
            {
                int counter = 1;
                foreach(var img in car.Base64images)
                {
                    var image = img.Split(",")[1];
                    var bytes = FileRepository.Base64ToByteArray(image);
                    var imageName = carId + "_image" + counter + ".png";
                    await CarRepository.UploadImageToCar(carId, bytes, imageName);
                    counter++;
                }
            }         
        }
        public async Task<List<CarEntity>> GetAllCars()
        {
            var cars = await CarRepository.GetAllCars();
            if (cars.Count == 0)
                throw new BusinessException("You do not have any cars yet!");
            return cars;
        }
        public async Task<CarEntity> GetCarById(string id)
        {
            var car = await CarRepository.GetCarById(id);
            if (car == null)
                throw new BusinessException("Car with provided id does not exists!");
            car.MainImageUrl = Settings.DefaultImageUrl;

            return car;
        }

    }
}
