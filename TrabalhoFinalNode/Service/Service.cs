using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MvcCoreTutorial.Models.Domain;
using Newtonsoft.Json;
using NuGet.Common;
using NuGet.Protocol.Plugins;
using System.Drawing;
using System.Net.Http.Headers;
using System.Text;
using TrabalhoFinalNode.Models.Domain;

namespace MvcCoreTutorial.Service
{
    public class Retorno
    {
        public string message { get; set; }
    }
    
    public static class Service
    {
        const string _urlBase = @"https://trabalhofinalnode.lucasluis3.repl.co/";

        public async static Task<List<Aparelho>> GetAllAparelhos(string token)
        {
            using (var client = new HttpClient())
            {
                string RequestUri = _urlBase + "aparelhos";

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await client.GetAsync(RequestUri);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string responseString = await response.Content.ReadAsStringAsync();
                    var aparelhos = JsonConvert.DeserializeObject<List<Aparelho>>(responseString);
                    return aparelhos;
                }  
                else
                {
                    throw new InvalidOperationException("Erro na autenticação ao buscar token");
                }
            }
        }

        public async static Task<bool> AddAparelho(Aparelho aparelho, string token)
        {
            using (var client = new HttpClient())
            {
                string RequestUri = _urlBase + "aparelhos";

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var jsonAparelho = JsonConvert.SerializeObject(aparelho);
                HttpContent content = new StringContent(jsonAparelho, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(RequestUri, content);
                string responseString = await response.Content.ReadAsStringAsync();                

                if (response.StatusCode == System.Net.HttpStatusCode.Created)
                {                    
                    return true;
                }
                else
                {
                    var retorno = JsonConvert.DeserializeObject<string>(responseString);
                    throw new InvalidOperationException(retorno);
                }
            }
        }

        public async static Task<Aparelho> GetAparelhoById(int id, string token)
        {
            using (var client = new HttpClient())
            {
                string RequestUri = _urlBase + "aparelhos/" + id;

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await client.GetAsync(RequestUri);
                string responseString = await response.Content.ReadAsStringAsync();
                

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var aparelho = JsonConvert.DeserializeObject<Aparelho>(responseString);
                    return aparelho;
                }
                else
                {
                    var retorno = JsonConvert.DeserializeObject<Retorno>(responseString);
                    throw new InvalidOperationException(retorno.message);
                }
            }
        }

        public async static Task<bool> DeleteAparelhoById(int id, string token)
        {
            using (var client = new HttpClient())
            {
                string RequestUri = _urlBase + "aparelhos/" + id;

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await client.DeleteAsync(RequestUri);
                string responseString = await response.Content.ReadAsStringAsync();


                if (response.StatusCode == System.Net.HttpStatusCode.Accepted)
                {                    
                    return true;
                }
                else
                {
                    var retorno = JsonConvert.DeserializeObject<Retorno>(responseString);
                    throw new InvalidOperationException(retorno.message);
                }
            }
        }

        public async static Task<bool> UpdateAparelhoById(Aparelho aparelho, string token)
        {
            using (var client = new HttpClient())
            {
                string RequestUri = _urlBase + "aparelhos/" + aparelho.id;

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var jsonAparelho = JsonConvert.SerializeObject(aparelho);
                HttpContent content = new StringContent(jsonAparelho, Encoding.UTF8, "application/json");

                var response = await client.PutAsync(RequestUri, content);
                string responseString = await response.Content.ReadAsStringAsync();


                if (response.StatusCode == System.Net.HttpStatusCode.Created)
                {
                    var aparelhoUpdated = JsonConvert.DeserializeObject<List<Aparelho>>(responseString);
                    return true;
                }
                else
                {
                    var retorno = JsonConvert.DeserializeObject<Retorno>(responseString);
                    throw new InvalidOperationException(retorno.message);
                }
            }
        }

        public async static Task<WebToken> Login(Login login)
        {
            try
            {
                WebToken token;

                using (var httpClient = new HttpClient())
                {
                    var jsonLogin = JsonConvert.SerializeObject(login);
                    HttpContent content = new StringContent(jsonLogin, Encoding.UTF8, "application/json");

                    string RequestUri = _urlBase + "seguranca/login";

                    using (var response = await httpClient.PostAsync(RequestUri, content))
                    {
                        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            string responseString = await response.Content.ReadAsStringAsync();
                            token = JsonConvert.DeserializeObject<WebToken>(responseString);
                        }
                        else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                        {
                            throw new InvalidOperationException("Email/Senha inválidos");
                        }
                        else
                        {
                            throw new InvalidOperationException("Erro na autenticação ao buscar token");
                        }
                    }
                }
                return token;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
