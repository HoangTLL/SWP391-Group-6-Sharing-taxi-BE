﻿using Microsoft.EntityFrameworkCore;
using PMS.Repository.Base;
using STP.Repository.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STP.Repository
{
    public class TripRepository : GenericRepository<Trip>
    {
        public TripRepository(ShareTaxiContext context) : base(context)
        {
        }

        // Add a new trip asynchronously
        public async Task AddAsync(Trip trip)
        {
            await _context.Trips.AddAsync(trip);
            await _context.SaveChangesAsync(); // Save changes to the database
        }

        // Get all trips with related entities
        public async Task<List<Trip>> GetAllTripsWithDetailsAsync()
        {
            return await _context.Trips
                .Include(t => t.PickUpLocation)
                .Include(t => t.DropOffLocation)
                .Include(t => t.TripType) // Removed t.ToArea
                .ToListAsync();
        }

        // Get a specific trip with related entities
        public async Task<Trip> GetTripByIdWithDetailsAsync(int id)
        {
            return await _context.Trips
                .Include(t => t.PickUpLocation)
                .Include(t => t.DropOffLocation)
                .Include(t => t.TripType) // Removed t.ToArea
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        // Get trips by date range
        public async Task<List<Trip>> GetTripsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.Trips
                .Where(t => t.BookingDate >= startDate && t.BookingDate <= endDate)
                .ToListAsync();
        }

        // Get available trips (trips with available seats)
        public async Task<List<Trip>> GetAvailableTripsAsync()
        {
            return await _context.Trips
                .Where(t => t.BookingDate > DateTime.Now && t.MaxPerson > t.Bookings.Count)
                .Include(t => t.PickUpLocation)
                .Include(t => t.DropOffLocation)
                .ToListAsync();
        }

        // Update trip details
        public async Task UpdateTripDetailsAsync(Trip trip)
        {
            _context.Entry(trip).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        // Delete a trip and its related bookings
        public async Task DeleteTripAndBookingsAsync(int tripId)
        {
            var trip = await _context.Trips
                .Include(t => t.Bookings)
                .FirstOrDefaultAsync(t => t.Id == tripId);

            if (trip != null)
            {
                _context.Bookings.RemoveRange(trip.Bookings);
                _context.Trips.Remove(trip);
                await _context.SaveChangesAsync();
            }
        }

        // Get trips by TripType
        public async Task<List<Trip>> GetTripsByTypeAsync(int tripTypeId)
        {
            return await _context.Trips
                .Where(t => t.TripTypeId == tripTypeId)
                .Include(t => t.PickUpLocation)
                .Include(t => t.DropOffLocation)
                .ToListAsync();
        }

        // Search trips by location names
        public async Task<List<Trip>> SearchTripsByLocationAsync(string locationName)
        {
            return await _context.Trips
                .Where(t => t.PickUpLocation.Name.Contains(locationName) ||
                            t.DropOffLocation.Name.Contains(locationName))
                .Include(t => t.PickUpLocation)
                .Include(t => t.DropOffLocation)
                .ToListAsync();
        }
    }
}