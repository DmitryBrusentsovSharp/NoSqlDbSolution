using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson.Serialization.Attributes;

namespace DataAccessLibrary.Models
{
	public class AddressesModel
	{
		[BsonId]
		public Guid Id { get; set; } = Guid.NewGuid();

		public string City;
		public string Street;
		public string HouseNumber;
		public string BuildingNumber;
		public string ApartmentNumber;
	}
}
