using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using System.Collections.Generic;
using System.Text.Json;
using Javeriana.Api.DTO;
using System.IO;
using System.Text;
using ApplicationCore.DTO;

namespace IntegrationTest
{
    public class HttpClientTest
    {
        private readonly ITestOutputHelper output;

        private static readonly HttpClient client = new HttpClient();

        public HttpClientTest(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Theory]
        [InlineData("https://api.github.com/orgs/dotnet/repos")]
        [InlineData("https://api.github.com/orgs/dotnet/repo")]
        public async Task ShouldConsumeEndpoint(string url)
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
            client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");
            try
            {
                var stringTask = client.GetStringAsync(url);
                var msg = await stringTask;
                output.WriteLine(msg);
                Assert.NotNull(msg);
            } catch (HttpRequestException ex)
            {
                output.WriteLine("Message :{0} ", ex.Message);
                Assert.Contains("404", ex.Message);
            }  
        }

        [Theory]
        [InlineData("https://api.github.com/orgs/dotnet/repos")]
        [InlineData("https://api.github.com/orgs/dotnet/repo")]
        public async Task ShouldConsumeEndpointWithDetails(string url)
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
            client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");
            try
            {
                var response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                var msg = await response.Content.ReadAsStringAsync();
                
                foreach (var header in response.Headers)
                {
                    output.WriteLine("{0} : {1}", header.Key, string.Join(" ", header.Value));
                }
                Assert.NotNull(msg);
                Assert.True(HttpStatusCode.OK == response.StatusCode);
            }
            catch (HttpRequestException ex)
            {
                output.WriteLine("Message :{0} ", ex.Message);
                Assert.Contains("404", ex.Message);
            }
        }

        [Fact]
        public async Task ShouldDeserializeJSON()
        {
            string json = "{\"id\":200,\"name\":\"tarea200\",\"isComplete\":true}";
            MemoryStream jsonStream = new MemoryStream(Encoding.UTF8.GetBytes(json));
            Tarea tarea = null;
            tarea = await JsonSerializer.DeserializeAsync<Tarea>(jsonStream);

            Assert.NotNull(tarea);
            output.WriteLine("Id :{0}\nName : {1}\nIsComplete : {2}", tarea.Id, tarea.Name, tarea.IsComplete);
            Assert.True(200L == tarea.Id);
            Assert.Equal("tarea200", tarea.Name);
            Assert.True(tarea.IsComplete);
        }

        [Fact]
        public async Task ShouldIgnoreUknownProperties()
        {
            string json = "{\"property1\":200,\"property2\":\"tarea200\",\"property3\":false}";
            MemoryStream jsonStream = new MemoryStream(Encoding.UTF8.GetBytes(json));
            Tarea tarea = null;
            tarea = await JsonSerializer.DeserializeAsync<Tarea>(jsonStream);

            Assert.NotNull(tarea);
            output.WriteLine("Id :{0}\nName : {1}\nIsComplete : {2}", tarea.Id, tarea.Name, tarea.IsComplete);

            Assert.True(0 == tarea.Id);
            Assert.Null(tarea.Name);
            Assert.False(tarea.IsComplete);
        }

        [Fact]
        public async Task ShouldThrowExceptionOnDeserializeInvalidJSON()
        {
            string json = "{#\"id\":200,\"name\":\"tarea200\",\"isComplete\":false}";
            MemoryStream jsonStream = new MemoryStream(Encoding.UTF8.GetBytes(json));
            await Assert.ThrowsAsync<JsonException>(async () => await JsonSerializer.DeserializeAsync<Tarea>(jsonStream));
        }

        [Fact]
        public void ShouldBuildRequestPath()
        {
            string basePath = "https://localhost:5001";
            string endpoint = "tareas/{0}/{1}/{2}";
            Peticion<Tarea> peticion = new Peticion<Tarea>(endpoint);
            peticion.PathVariables.Add("var1");
            peticion.PathVariables.Add("var2");
            peticion.PathVariables.Add("var3");
            string[] vars = ((List<string>)peticion.PathVariables).ToArray();
            string formatedEndpoint = string.Format(peticion.Endpoint, vars);
            string fullUrl = string.Format("{0}/{1}", basePath, formatedEndpoint);

            Assert.Equal("https://localhost:5001/tareas/var1/var2/var3", fullUrl);
        }

        [Fact]
        public void ShouldBuildRequestPathWithQueryParams()
        {
            string basePath = "https://localhost:5001";
            string endpoint = "tareas/{0}/{1}/{2}";
            Peticion<Tarea> peticion = new Peticion<Tarea>(endpoint);
            peticion.PathVariables.Add("var1");
            peticion.PathVariables.Add("var2");
            peticion.PathVariables.Add("var3");

            peticion.Params.Add("param1", "string1");
            peticion.Params.Add("param2", "string2");
            string[] vars = ((List<string>)peticion.PathVariables).ToArray();
            string formatedEndpoint = string.Format(peticion.Endpoint, vars);
            string fullUrl = string.Format("{0}/{1}", basePath, formatedEndpoint);
            StringBuilder stringBuilder = new StringBuilder(fullUrl);
            if (peticion.Params.Count > 0)
            {
                int index = 0;
                foreach (var param in peticion.Params)
                {
                    stringBuilder.Append((index == 0) ? "?" : "&");
                    stringBuilder.Append($"{param.Key}={param.Value}");
                    index++;
                }
            }

            Assert.Equal("https://localhost:5001/tareas/var1/var2/var3?param1=string1&param2=string2", stringBuilder.ToString());
        }

        [Fact]
        public void ShouldBuildRequestPathWithQueryParamsFromPeticion()
        {
            string basePath = "https://localhost:5001";
            string endpoint = "tareas/{0}/{1}/{2}";
            Peticion<Tarea> peticion = new Peticion<Tarea>($"{basePath}/{endpoint}");
            peticion.PathVariables.Add("var1");
            peticion.PathVariables.Add("var2");
            peticion.PathVariables.Add("var3");

            peticion.Params.Add("param1", "string1");
            peticion.Params.Add("param2", "string2");

            Assert.Equal("https://localhost:5001/tareas/var1/var2/var3?param1=string1&param2=string2", peticion.ResolverRequestURL());
        }
    }
}
