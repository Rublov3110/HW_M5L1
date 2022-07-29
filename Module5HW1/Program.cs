using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Net;
using Module5HW1.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Module5HW1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var getUsersTask = Task.Run(() => GetData<User>("users"));
            var getSingleUserTask = Task.Run(() => GetSingleData<User>("users", 2));
            var getResourcesTask = Task.Run(() => GetData<Resource>("unknown"));
            var getSingleResourceTask = Task.Run(() => GetSingleData<Resource>("unknown", 2));
            var createUserTask = Task.Run(() => CreateUser("morpheus", "leader"));
            var updateUserTask = Task.Run(() => UpdateUser("morpheus", "zion resident", 2));
            var patchUserTask = Task.Run(() => PatchUser("morpheus", "zion resident", 2));
            var deleteUserTask = Task.Run(() => DeleteUser(2));
            var registerUserTask = Task.Run(() => RegisterUser("eve.holt@reqres.in", "pistol"));
            var registerUserByEmailTask = Task.Run(() => RegisterUser("sydney@fife"));
            var loginTask = Task.Run(() => Login("eve.holt@reqres.in", "cityslicka"));
            var loginByEmailTask = Task.Run(() => Login("peter@klaven"));
            var getUsersWithDelayTask = Task.Run(GetUsers);

            Task.WhenAll(
                getUsersTask,
                getSingleUserTask,
                getResourcesTask,
                getSingleResourceTask,
                createUserTask,
                updateUserTask,
                patchUserTask,
                deleteUserTask,
                registerUserTask,
                registerUserByEmailTask,
                loginTask,
                loginByEmailTask,
                getUsersWithDelayTask);

            /*var users = getUsersTask.Result;
            var user = getSingleUserTask.Result;
            var resources = getResourcesTask.Result;
            var resource = getSingleResourceTask.Result;
            createUserTask.GetAwaiter().GetResult();
            updateUserTask.GetAwaiter().GetResult();
            patchUserTask.GetAwaiter().GetResult();
            deleteUserTask.GetAwaiter().GetResult();
            registerUserTask.GetAwaiter().GetResult();
            registerUserByEmailTask.GetAwaiter().GetResult();
            loginTask.GetAwaiter().GetResult();
            loginByEmailTask.GetAwaiter().GetResult();
            var delayedUsers = getUsersWithDelayTask.Result;*/
        }

        public static async Task<List<T>> GetData<T>(string path)
            where T : class
        {
            using var httpClient = new HttpClient();
            var result = await httpClient.GetAsync($@"http://reqres.in/api/{path}");

            if (result.StatusCode == HttpStatusCode.OK)
            {
                var content = await result.Content.ReadAsStringAsync();
                var response = JsonConvert.DeserializeObject<Response<T>>(content);
                return response.Data;
            }

            return null;
        }

        public static async Task<T> GetSingleData<T>(string path, int id)
            where T : class
        {
            using var httpClient = new HttpClient();
            var result = await httpClient.GetAsync($@"http://reqres.in/api/{path}/{id}");

            if (result.StatusCode == HttpStatusCode.OK)
            {
                var content = await result.Content.ReadAsStringAsync();
                var response = JsonConvert.DeserializeObject<DataResponse<T>>(content);
                return response.Data;
            }

            return null;
        }

        public static async Task CreateUser(string name, string job)
        {
            using var httpClient = new HttpClient();
            var user = new { Name = name, Job = job };

            var httpContent = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");

            var httpMessage = new HttpRequestMessage();
            httpMessage.Content = httpContent;
            httpMessage.RequestUri = new Uri(@"http://reqres.in/api/users");
            httpMessage.Method = HttpMethod.Post;
            var result = await httpClient.SendAsync(httpMessage);

            if (result.StatusCode == HttpStatusCode.Created)
            {
                var content = await result.Content.ReadAsStringAsync();
            }
        }

        public static async Task UpdateUser(string name, string job, int id)
        {
            using var httpClient = new HttpClient();
            var user = new { Name = name, Job = job };

            var httpContent = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");

            var httpMessage = new HttpRequestMessage();
            httpMessage.Content = httpContent;
            httpMessage.RequestUri = new Uri($@"http://reqres.in/api/users/{id}");
            httpMessage.Method = HttpMethod.Put;
            var result = await httpClient.SendAsync(httpMessage);

            if (result.StatusCode == HttpStatusCode.OK)
            {
                var content = await result.Content.ReadAsStringAsync();
            }
        }

        public static async Task PatchUser(string name, string job, int id)
        {
            using var httpClient = new HttpClient();
            var user = new { Name = name, Job = job };

            var httpContent = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");

            var httpMessage = new HttpRequestMessage();
            httpMessage.Content = httpContent;
            httpMessage.RequestUri = new Uri($@"http://reqres.in/api/users/{id}");
            httpMessage.Method = HttpMethod.Patch;

            var result = await httpClient.SendAsync(httpMessage);

            if (result.StatusCode == HttpStatusCode.OK)
            {
                var content = await result.Content.ReadAsStringAsync();
            }
        }

        public static async Task DeleteUser(int id)
        {
            using var httpClient = new HttpClient();

            var httpMessage = new HttpRequestMessage();
            httpMessage.RequestUri = new Uri($@"http://reqres.in/api/users/{id}");
            httpMessage.Method = HttpMethod.Delete;

            var result = await httpClient.SendAsync(httpMessage);

            if (result.StatusCode == HttpStatusCode.NoContent)
            {
                var content = await result.Content.ReadAsStringAsync();
            }
        }

        public static async Task RegisterUser(string email, string password)
        {
            using var httpClient = new HttpClient();
            var user = new { Email = email, Password = password };

            var httpContent = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");

            var httpMessage = new HttpRequestMessage();
            httpMessage.Content = httpContent;
            httpMessage.RequestUri = new Uri(@"http://reqres.in/api/register");
            httpMessage.Method = HttpMethod.Post;
            var result = await httpClient.SendAsync(httpMessage);

            if (result.StatusCode == HttpStatusCode.OK)
            {
                var content = await result.Content.ReadAsStringAsync();
            }
        }

        public static async Task RegisterUser(string email)
        {
            using var httpClient = new HttpClient();
            var user = new { Email = email };

            var httpContent = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");

            var httpMessage = new HttpRequestMessage();
            httpMessage.Content = httpContent;
            httpMessage.RequestUri = new Uri(@"http://reqres.in/api/register");
            httpMessage.Method = HttpMethod.Post;
            var result = await httpClient.SendAsync(httpMessage);

            if (result.StatusCode == HttpStatusCode.BadRequest)
            {
                var content = await result.Content.ReadAsStringAsync();
            }
        }

        public static async Task Login(string email, string password)
        {
            using var httpClient = new HttpClient();
            var user = new { Email = email, Password = password };

            var httpContent = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");

            var httpMessage = new HttpRequestMessage();
            httpMessage.Content = httpContent;
            httpMessage.RequestUri = new Uri(@"http://reqres.in/api/login");
            httpMessage.Method = HttpMethod.Post;
            var result = await httpClient.SendAsync(httpMessage);

            if (result.StatusCode == HttpStatusCode.OK)
            {
                var content = await result.Content.ReadAsStringAsync();
            }
        }

        public static async Task Login(string email)
        {
            using var httpClient = new HttpClient();
            var user = new { Email = email };

            var httpContent = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");

            var httpMessage = new HttpRequestMessage();
            httpMessage.Content = httpContent;
            httpMessage.RequestUri = new Uri(@"http://reqres.in/api/login");
            httpMessage.Method = HttpMethod.Post;
            var result = await httpClient.SendAsync(httpMessage);

            if (result.StatusCode == HttpStatusCode.BadRequest)
            {
                var content = await result.Content.ReadAsStringAsync();
            }
        }

        public static async Task<List<User>> GetUsers()
        {
            using var httpClient = new HttpClient();
            var result = await httpClient.GetAsync($@"http://reqres.in/api/users?delay=3");

            if (result.StatusCode == HttpStatusCode.OK)
            {
                var content = await result.Content.ReadAsStringAsync();
                var response = JsonConvert.DeserializeObject<Response<User>>(content);
                return response.Data;
            }

            return null;
        }
    }
}
