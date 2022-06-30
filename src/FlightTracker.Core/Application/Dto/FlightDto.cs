﻿using System;

namespace FlightTracker.Application.Dto
{
    public class FlightDto
    {
        public int FlightId { get; set; }
        public int OriginCityId { get; set; }
        
        public int DestinationCityId { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public int AirlineId { get; set; }
        public FlightStatus Status { get; set; }
    }
}