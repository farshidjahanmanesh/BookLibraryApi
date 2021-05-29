using AutoMapper;
using BookLibrary.Application.UseCases.Commands.CreateBookCommand;
using BookLibrary.Application.UseCases.Queries.CheckIsBookInDbQuery;
using BookLibrary.Domain.Domains.Books;
using BookLibrary.Domain.Dtos.Author;
using BookLibrary.Domain.Dtos.Book;
using BookLibrary.Domain.Interfaces.ReadRepositories.Book;
using BookLibrary.Domain.Interfaces.WriteRepositories.Book;
using MediatR;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using static BookLibrary.Application.Services.OpenLibraryApiBookDetail;

namespace BookLibrary.Application.Services
{

    public class OpenLibraryApiSearchTemplate
    {
        public int NumFound { get; set; }
        public List<OpenLibraryApiBook> Docs { get; set; }

        public class OpenLibraryApiBook
        {
            public string Key { get; set; }
            public string Type { get; set; }
            public string Title { get; set; }
            public int first_publish_year { get; set; }
            public List<string> Author_key { get; set; }
        }
    }

    public class OpenLibraryApiBookDetail
    {
        public DescriptionInfo Description { get; set; }
        public List<LinkInfo> Links { get; set; }

        public class LinkInfo
        {
            public string Title { get; set; }
            public string Url { get; set; }
        }
        public class DescriptionInfo
        {
            public string Value { get; set; }

        }
    }

    public class OpenLibraryApiAuthorDetail
    {
        public string Key { get; set; }
        public string Fuller_name { get; set; }
        public string WikiPedia { get; set; }
        public BioInfo Bio { get; set; }
        public string birth_date { get; set; }
        public class BioInfo
        {
            public string Value { get; set; }
        }
    }

    public class OpenLibraryApi : IReadDataFromApi
    {
        const string _siteName = "OpenLibrary";
        const string _searchForBook = "http://openlibrary.org/search.json?title=";
        const string _searchWithBookId = "http://openlibrary.org";
        const string _searchWithAuthorId = "http://openlibrary.org/authors/";
        private readonly IMediator mediator;
        private readonly IMapper mapper;

        public string SiteName { get => _siteName; }
        public string BookUrl { get => _searchWithBookId; }
        public string AuthorUrl { get => _searchWithAuthorId; }
        public string SearchForBookWithNameUrl => _searchForBook;

        public OpenLibraryApi(IMediator mediator, IMapper mapper)
        {
            this.mediator = mediator;
            this.mapper = mapper;
        }
        public Task StartGetDataAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> StartToSearchForBookWith(string bookName)
        {
            if (await CheckIsBookInDataBase(bookName))
            {
                return false;
            }
            var escapeBookName = Uri.EscapeDataString(bookName);
            HttpClient httpClient = new HttpClient();
            var listOfBook = await GetListOfBookWith(escapeBookName, httpClient);
            if (listOfBook.NumFound == 0)
                return false;
            var firstBookInformation = listOfBook.Docs.First();
            var firstBookDetail = await GetBookDetailWith(firstBookInformation.Key, httpClient);
            var authorsDetails = await GetAuthorsOfBookWithAuthorsKeys(firstBookInformation.Author_key, httpClient);

            var bookItemObject =
          mapper.Map<OpenLibraryApiBookDetail, InsertBookDto>(firstBookDetail,
          mapper.Map<InsertBookDto>(listOfBook.Docs.First()));
            try
            {
                bookItemObject.Authors = mapper.Map<List<OpenLibraryApiAuthorDetail>, List<InsertAuthorDto>>(authorsDetails);
            }
            catch (Exception ex)
            {

                throw;
            }
            
            await mediator.Send(new CreateBookCommand(bookItemObject));
            return true;
        }
        private async Task<bool> CheckIsBookInDataBase(string bookName)
        {
            return await mediator.Send(new CheckIsBookInDbQuery(bookName));
        }
        private async Task<OpenLibraryApiSearchTemplate> GetListOfBookWith(string bookName, HttpClient client)
        {
            var listOfBookHttpResponse = await client.GetAsync(SearchForBookWithNameUrl + bookName);
            listOfBookHttpResponse.EnsureSuccessStatusCode();
            var listOfBookWithStringFormat = await listOfBookHttpResponse.Content.ReadAsStringAsync();
            var ListOfBook = JsonConvert.DeserializeObject<OpenLibraryApiSearchTemplate>(listOfBookWithStringFormat);
            return ListOfBook;
        }
        private async Task<OpenLibraryApiBookDetail> GetBookDetailWith(string key, HttpClient client)
        {
            var firstBookOfTheListHttpResponse = await client.GetAsync(_searchWithBookId + key + ".json");
            var firstBookOfTheListStringFormat = await firstBookOfTheListHttpResponse.Content.ReadAsStringAsync();
            var firstBookDetail = JsonConvert.DeserializeObject<OpenLibraryApiBookDetail>(firstBookOfTheListStringFormat);
            return firstBookDetail;
        }
        private async Task<List<OpenLibraryApiAuthorDetail>> GetAuthorsOfBookWithAuthorsKeys(List<string> keys, HttpClient client)
        {
            List<OpenLibraryApiAuthorDetail> authorsDetail = new();
            List<Task<HttpResponseMessage>> responseTasks = new();
            foreach (var author in keys)
            {
                responseTasks.Add(client.GetAsync(_searchWithAuthorId + author + ".json"));
            }
            Task.WaitAll(responseTasks.ToArray());
            foreach (var task in responseTasks)
            {
                var contentStringType = await task.Result.Content.ReadAsStringAsync();
                var contentDeserialize = JsonConvert.DeserializeObject<OpenLibraryApiAuthorDetail>(
                   contentStringType);

                if (string.IsNullOrEmpty(contentDeserialize.Fuller_name))
                {
                    var jObj = JObject.Parse(contentStringType);
                    contentDeserialize.Fuller_name = (string)jObj["name"];
                }
                if (string.IsNullOrEmpty(contentDeserialize.WikiPedia))
                {
                    contentDeserialize.WikiPedia = "";
                }
                authorsDetail.Add(contentDeserialize);
            }

            return authorsDetail;
        }
    }
}
