using System;
using System.IO;
using System.Linq;
using System.Net;
using DataAccessLibrary;
using DataAccessLibrary.Models;
using Microsoft.Extensions.Configuration;

namespace MongoDbUI
{
	internal class Program
	{
		private static MongoDBDataAccess db;
		private static readonly string tableName = "Contacts";
		private static readonly string tableName2 = "Addresses";
		static void Main(string[] args)
		{
			db = new MongoDBDataAccess("MongoContactsDB", GetConnectionString());

			ContactModel user = new ContactModel
			{
				FirstName = "Charity",
				LastName = "Hanson",
			};
			user.EmailAddresses.Add(new EmailAddressModel { EmailAddress = "noop@ya.ru"});
			user.EmailAddresses.Add(new EmailAddressModel { EmailAddress = "strangenerd@yandex.ru" });
			user.PhoneNumbers.Add(new PhoneNumberModel { PhoneNumber = "1111111" });
			user.PhoneNumbers.Add(new PhoneNumberModel { PhoneNumber = "2222222" });

			//CreateContact(user);

			//GetAllContacts();

			//GetContactById("5df87e09-1e50-46c8-ad1d-cf6d0f31e2dc");

			//UpdateContactsFirstName("Timmy", "5df87e09-1e50-46c8-ad1d-cf6d0f31e2dc");
			//GetAllContacts();

			//RemovePhoneNumberFromUser("4154322", "5df87e09-1e50-46c8-ad1d-cf6d0f31e2dc");
			//GetAllContacts();

			RemoveUser("5df87e09-1e50-46c8-ad1d-cf6d0f31e2dc");

			AddressesModel address = new AddressesModel
			{
				City = "Moscow",
				Street = "Kashirskoe shosse",
				HouseNumber = "1",
				BuildingNumber = "",
				ApartmentNumber = "1"
			};
			//CreateAddress(address);
			
			//GetAllAddresses();
			
			GetAddressById("2363e726-398f-403e-af12-dddd24741cce");

			Console.WriteLine("Done Processing MongoDB");
			Console.ReadLine();
		}

		public static void RemoveUser(string id) 
		{
			Guid guid = new Guid(id);
			db.DeleteRecord<ContactModel>(tableName, guid);
		}

		public static void RemovePhoneNumberFromUser(string phoneNumber, string id) 
		{
			Guid guid = new Guid(id);
			var contact = db.LoadRecordById<ContactModel>(tableName, guid);

			contact.PhoneNumbers = contact.PhoneNumbers.Where(x => x.PhoneNumber != phoneNumber).ToList();

			db.UpsertRecord(tableName, contact.Id, contact);
		}

		private static void UpdateContactsFirstName(string firstName, string id) 
		{
			Guid guid = new Guid(id);
			var contact = db.LoadRecordById<ContactModel>(tableName, guid);

			contact.FirstName = firstName;

			db.UpsertRecord(tableName, contact.Id, contact);
		}

		private static void GetAddressById(string id)
		{
			var address = db.LoadRecordById<AddressesModel>(tableName2, new Guid(id));

			Console.WriteLine($"{address.Id}: {address.City} {address.BuildingNumber} ");
		}
		private static void GetContactById(string id)
		{
			var contact = db.LoadRecordById<ContactModel>(tableName, new Guid(id));

			Console.WriteLine($"{contact.Id}: {contact.FirstName} {contact.LastName} ");
		}

		private static void GetAllAddresses()
		{
			var addresses = db.LoadRecords<AddressesModel>(tableName2);

			foreach (var address in addresses)
			{
				Console.WriteLine($"{address.Id}: {address.City} {address.BuildingNumber} ");
			}
		}

		private static void GetAllContacts() 
		{
			var contacts = db.LoadRecords<ContactModel>(tableName);

			foreach (var contact in contacts) 
			{
				Console.WriteLine($"{contact.Id}: {contact.FirstName} {contact.LastName} ");
			}
		}

		private static void CreateContact(ContactModel contact) 
		{
			db.UpsertRecord(tableName, contact.Id, contact);
		}

		private static void CreateAddress(AddressesModel address)
		{
			db.UpsertRecord(tableName2, address.Id, address);
		}

		private static string GetConnectionString(string connectionStringName = "Default") 
		{
			string output = "";

			var builder = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("settings.json");

			var config = builder.Build();

			output = config.GetConnectionString(connectionStringName);

			return output;
		}
	}
}
