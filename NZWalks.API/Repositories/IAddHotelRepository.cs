using Booking.Com_Clone_API.Models.Domain;
using Booking.Com_Clone_API.Models.DTO;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace Booking.Com_Clone_API.Repositories
{
    public interface IAddHotelRepository
    {
        ///<summary>
        ///method to get all hotels
        /// </summary>
        IEnumerable<HotelDto> GetAllHotels();

        ///<summary>
        ///method to get hotel details based on id
        /// </summary>
        IEnumerable<HotelDto> GetHotelById(Guid? userId, Guid? hotelId);

        ///<summary>
        ///method to add hotels
        /// </summary>
        /// 
        Task<Guid> AddHotelAsync(HotelDto hotelDto);

        ///<summary>
        ///method to update hotl record
        /// </summary>
        void UpdateHotel(Guid id, HotelDto hotelDto);

        ///<summary>
        ///method to delete hotel record
        /// </summary>
        void DeleteHotel(Guid id);
    
    }
}
