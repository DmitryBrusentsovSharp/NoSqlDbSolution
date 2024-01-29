using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using ApiDBUI.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace ApiDBUI.Pages
{
	public class IndexModel : PageModel
	{
		private readonly ILogger<IndexModel> _logger;
		private readonly IHttpClientFactory _httpClientFactory;

		public IndexModel(ILogger<IndexModel> logger, IHttpClientFactory httpClientFactory)
		{
			_logger = logger;
			_httpClientFactory = httpClientFactory;
		}

		public async Task OnGet()
		{
			await GetSWChar();
			//await CreateContact();
			await GetAllContacts();
		}

		private async Task CreateContact() {

			ContactModel contact = new ContactModel
			{
				FirstName = "Dima",
				LastName = "Brusentsov"
			};
			contact.EmailAddresses.Add(new EmailAddressModel { EmailAddress = "hisair@yandex.ru" });
			contact.PhoneNumbers.Add(new PhoneNumberModel { PhoneNumber = "+79275698481" });

			var _client = _httpClientFactory.CreateClient();
			var response = await _client.PostAsync(
				"https://localhost:44319/api/Contacts",
				new StringContent(System.Text.Json.JsonSerializer.Serialize(contact), Encoding.UTF8, "application/json"));
		}

		private async Task GetAllContacts() 
		{
			var _client = _httpClientFactory.CreateClient();
			var response = await _client.GetAsync("https://localhost:44319/api/Contacts");

			List<ContactModel> contacts;

			if (response.IsSuccessStatusCode)
			{
				var options = new JsonSerializerOptions
				{
					PropertyNameCaseInsensitive = true
				};
				string responseText = await response.Content.ReadAsStringAsync();
				contacts = System.Text.Json.JsonSerializer.Deserialize<List<ContactModel>>(responseText, options);
			}
			else 
			{
				throw new Exception(response.ReasonPhrase);
			}
		}

		private async Task GetSWChar()
		{
			var _client = _httpClientFactory.CreateClient();
			var response = await _client.GetAsync("https://swapi.dev/api/people/5/");

			StarWarsModel character = new StarWarsModel();

			if (response.IsSuccessStatusCode)
			{
/*				var options = new JsonSerializerOptions
				{
					PropertyNameCaseInsensitive = true
				};*/
				string responseText = await response.Content.ReadAsStringAsync();
				//character = JsonConvert.DeserializeObject<StarWarsModel>(responseText);
				character = System.Text.Json.JsonSerializer.Deserialize<StarWarsModel>(responseText);
			}
			else
			{
				throw new Exception(response.ReasonPhrase);
			}
		}
	}
}
