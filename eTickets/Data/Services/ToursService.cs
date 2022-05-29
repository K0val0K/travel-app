using eTickets.Data.Base;
using eTickets.Data.ViewModels;
using eTickets.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace eTickets.Data.Services
{
    public class ToursService : EntityBaseRepository<Tour>, IToursService
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        public ToursService(AppDbContext context, IWebHostEnvironment hostEnvironment) : base(context)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        public async Task AddNewTourAsync(NewTourVM data)
        {
            string imagePath = "/Files/" + data.Image.FileName;
            using (var fileStream = new FileStream(_hostEnvironment.WebRootPath + imagePath, FileMode.Create))
            {
                await data.Image.CopyToAsync(fileStream);
            }
            var newTour = new Tour()
            {
                Name = data.Name,
                Description = data.Description,
                Price = data.Price,
                ImageURL = imagePath,
                TravelAgencyId = data.TravelAgencyId,
                StartDate = data.StartDate,
                EndDate = data.EndDate,
                TourCategory = data.TourCategory,
            };
            await _context.Tours.AddAsync(newTour);
            await _context.SaveChangesAsync();

            //Add Tour Countries
            foreach (var countryId in data.CountryIds)
            {
                var newCountryTour = new Country_Tour()
                {
                    TourId = newTour.Id,
                    CountryId = countryId
                };
                await _context.Countries_Tours.AddAsync(newCountryTour);
            }
            await _context.SaveChangesAsync();
        }

        public async Task<Tour> GetTourByIdAsync(int id)
        {
            var TourDetails = await _context.Tours
                .Include(c => c.TravelAgency)
                .Include(am => am.Countries_Tours).ThenInclude(a => a.Country)
                .FirstOrDefaultAsync(n => n.Id == id);

            return TourDetails;
        }

        public async Task<NewTourDropdownsVM> GetNewTourDropdownsValues()
        {
            var response = new NewTourDropdownsVM()
            {
                Countries = await _context.Countries.OrderBy(n => n.CountryName).ToListAsync(),
                TravelAgencies = await _context.TravelAgencies.OrderBy(n => n.Name).ToListAsync(),
            };

            return response;
        }

        public async Task UpdateTourAsync(NewTourVM data)
        {
            var dbTour = await _context.Tours.FirstOrDefaultAsync(n => n.Id == data.Id);
            string imagePath = "";
            if(data.Image != null)
            {
                imagePath = "/Files/" + data.Image.FileName;
                using (var fileStream = new FileStream(_hostEnvironment.WebRootPath + imagePath, FileMode.Create))
                {
                    await data.Image.CopyToAsync(fileStream);
                }
            }
            if (dbTour != null)
            {
                dbTour.Name = data.Name;
                dbTour.Description = data.Description;
                dbTour.Price = data.Price;
                dbTour.TravelAgencyId = data.TravelAgencyId;
                dbTour.StartDate = data.StartDate;
                dbTour.EndDate = data.EndDate;
                dbTour.TourCategory = data.TourCategory;

                if(!string.IsNullOrEmpty(imagePath))
                {
                    dbTour.ImageURL = imagePath;
                }

                await _context.SaveChangesAsync();
            }

            //Remove existing countries
            var existingCountriesDb = _context.Countries_Tours.Where(n => n.TourId == data.Id).ToList();
            _context.Countries_Tours.RemoveRange(existingCountriesDb);
            await _context.SaveChangesAsync();

            //Add Tour Countries
            foreach (var countryId in data.CountryIds)
            {
                var CountryTour = new Country_Tour()
                {
                    TourId = data.Id,
                    CountryId = countryId
                };
                await _context.Countries_Tours.AddAsync(CountryTour);
            }
            await _context.SaveChangesAsync();
        }
    }
}
