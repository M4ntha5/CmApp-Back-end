using CmApp.Contracts;
using CmApp.Domains;
using CmApp.Entities;
using System;
using System.Collections.Generic;
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
        public IExternalAPIService ExternalAPIService { get; set; }
        public IShippingRepository ShippingRepository { get; set; }

        private async Task<CarEntity> InsertCarDetailsFromScraper(CarEntity car)
        {
            if (car == null || car.Vin == "" || car.Vin == null)
                throw new BusinessException("Vin number cannot be null or empty!");
            if (car.Make != "BMW" && car.Make != "Mercedes-benz")
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
                if (param.Key == "Prod. Date" || param.Key == "Production Date")
                    carEntity.ManufactureDate = Convert.ToDateTime(param.Value);
                else if (param.Key == "Type" || param.Key == "Model")
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

                    count++;
                }
            }
            //inserts empty tracking 
            await TrackingRepository.InsertTracking(new TrackingEntity { Car = insertedCar.Id });
            return insertedCar;

        }

        private async Task<CarEntity> InsertOtherCar(CarEntity car)
        {
            if (car == null)
                throw new BusinessException("Can not insert car, because car is empty!");

            car.Vin = car.Vin.ToUpper();

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
                    count++;
                }
            }

            //inserts empty tracking 
            await TrackingRepository.InsertTracking(new TrackingEntity { Car = insertedCar.Id });
            return insertedCar;
        }

        public async Task<CarEntity> InsertCar(CarEntity car)
        {
            var userCars = await CarRepository.GetAllUserCars(car.User);
            var userVins = userCars.Select(x => x.Vin).ToList();

            if (car.Make == null || car.Make == "")
                throw new BusinessException("Make not defined");

            if (userVins.Contains(car.Vin))
                throw new BusinessException("There is already a car with this VIN number");

            if ((car.Make == "BMW" || car.Make == "Mercedes-benz") && car.Model == "")
                return await InsertCarDetailsFromScraper(car);
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

        public async Task UpdateCar(string userId, string carId, CarEntity car)
        {
            if (car.User != userId)
                throw new BusinessException("Car does not exist");

            var list = car.Equipment.Select(x => x.Code).ToList().Distinct().Count();
            if (car.Equipment.Count != list)
                throw new BusinessException("Car cannot have multiple equipment with the same code!");

            await CarRepository.UpdateCar(carId, car);

            if (car.Base64images.Count > 0)
            {
                await CarRepository.DeleteAllCarImages(carId);
                int counter = 1;
                foreach (var img in car.Base64images)
                {
                    var image = img.Split(",")[1];
                    var type = img.Split(",")[0].Split("/")[1].Split(";")[0];
                    var bytes = FileRepository.Base64ToByteArray(image);
                    var imageName = carId + "_image" + counter + "." + type;
                    await CarRepository.UploadImageToCar(carId, bytes, imageName);
                    counter++;
                }
            }
            else
                await CarRepository.DeleteAllCarImages(carId);
        }

        //summary
        public async Task UpdateSoldSummary(string carId, SummaryEntity summary)
        {
            summary.Car = carId;
            summary.SoldDate = DateTime.Now;
            var time = summary.SoldDate.Subtract(summary.CreatedAt);
            string message;
            if (time.Days > 0)
                if (time.Days == 1)
                    message = $"Sold within {time.Days} day";
                else
                    message = $"Sold within {time.Days} days";
            else
                if (time.Hours == 1)
                message = $"Sold within {time.Hours} hour";
            else
                message = $"Sold within {time.Hours} hours";

            summary.SoldWithin = message;
            var oldSummary = await SummaryRepository.GetSummaryByCarId(carId);
            summary.Id = oldSummary.Id;
            await SummaryRepository.UpdateCarSoldSummary(summary);
        }
        public async Task<SummaryEntity> InsertCarSummary(string carId, SummaryEntity summary)
        {
            if (summary.SelectedCurrency == "" || summary.BaseCurrency == "")
                throw new BusinessException("Currency not set");

            summary.Car = carId;
            summary.Total = summary.BoughtPrice;
            if (summary.SelectedCurrency != summary.BaseCurrency)
            {
                var input = new ExchangeInput
                {
                    Amount = summary.BoughtPrice,
                    From = summary.SelectedCurrency,
                    To = summary.BaseCurrency
                };
                var convertedPrice = await ExternalAPIService.CalculateResult(input);
                summary.Total = Math.Round(convertedPrice, 2);
                summary.BoughtPrice = Math.Round(convertedPrice, 2);
            }

            var newSummary = await SummaryRepository.InsertSummary(summary);
            return newSummary;
        }

        //shipping
        public async Task UpdateShipping(string carId, ShippingEntity shipping)
        {
            shipping.Car = carId;
            var oldShipping = await ShippingRepository.GetShippingByCar(carId);
            await ShippingRepository.UpdateCarShipping(oldShipping.Id, shipping);
        }
        public async Task<ShippingEntity> InsertShipping(string carId, ShippingEntity shipping)
        {
            if (string.IsNullOrEmpty(shipping.AuctionFeeCurrency) ||
                string.IsNullOrEmpty(shipping.CustomsCurrency) ||
                string.IsNullOrEmpty(shipping.TransportationFeeCurrency) ||
                string.IsNullOrEmpty(shipping.TransferFeeCurrency))
                throw new BusinessException("Shipping data currencies not set");

            shipping.Car = carId;

            if (shipping.TransferFeeCurrency != shipping.BaseCurrency)
            {
                var input = new ExchangeInput
                {
                    Amount = shipping.TransferFee,
                    From = shipping.TransferFeeCurrency,
                    To = shipping.BaseCurrency
                };
                var convertedPrice = await ExternalAPIService.CalculateResult(input);
                shipping.TransferFee = Math.Round(convertedPrice, 2);
            }
            if (shipping.TransportationFeeCurrency != shipping.BaseCurrency)
            {
                var input = new ExchangeInput
                {
                    Amount = shipping.TransportationFee,
                    From = shipping.TransportationFeeCurrency,
                    To = shipping.BaseCurrency
                };
                var convertedPrice = await ExternalAPIService.CalculateResult(input);
                shipping.TransportationFee = Math.Round(convertedPrice, 2);
            }
            if (shipping.AuctionFeeCurrency != shipping.BaseCurrency)
            {
                var input = new ExchangeInput
                {
                    Amount = shipping.AuctionFee,
                    From = shipping.AuctionFeeCurrency,
                    To = shipping.BaseCurrency
                };
                var convertedPrice = await ExternalAPIService.CalculateResult(input);
                shipping.AuctionFee = Math.Round(convertedPrice, 2);
            }
            if (shipping.CustomsCurrency != shipping.BaseCurrency)
            {
                var input = new ExchangeInput
                {
                    Amount = shipping.Customs,
                    From = shipping.CustomsCurrency,
                    To = shipping.BaseCurrency
                };
                var convertedPrice = await ExternalAPIService.CalculateResult(input);
                shipping.Customs = Math.Round(convertedPrice, 2);
            }

            var newShipping = await ShippingRepository.InsertShipping(shipping);

            double totalPrice = newShipping.Customs + newShipping.AuctionFee +
                newShipping.TransferFee + newShipping.TransportationFee;

            var summary = await SummaryRepository.GetSummaryByCarId(carId);
            await SummaryRepository.InsertTotalByCar(summary.Id, summary.Total + totalPrice);

            return newShipping;
        }
    }
}
