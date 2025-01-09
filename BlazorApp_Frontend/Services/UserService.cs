using System.Net.Http.Json;
using Models;

namespace BlazorApp_Frontend.Services
{
    public class UserService
    {
        private readonly HttpClient _httpClient;

        public UserService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Users>> GetAllUsersAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/Users/GetAllUsers");

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<List<Users>>() ?? new List<Users>();
                }

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    // Log or handle 404
                    Console.WriteLine("No users found.");
                    return new List<Users>();
                }

                throw new HttpRequestException($"Error: {response.StatusCode}, {await response.Content.ReadAsStringAsync()}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in GetAllUsersAsync: {ex.Message}");
                return new List<Users>();
            }
        }

        public async Task<Users?> GetUserByIdAsync(int? id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/Users/GetUserByID?ID={id}");

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<Users>();
                }

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    Console.WriteLine($"User with ID {id} not found.");
                    return null;
                }

                throw new HttpRequestException($"Error: {response.StatusCode}, {await response.Content.ReadAsStringAsync()}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in GetUserByIdAsync: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> DeleteUserAsync(int? id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/Users/DeleteUser?ID={id}");

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    Console.WriteLine($"User with ID {id} not found for deletion.");
                    return false;
                }

                throw new HttpRequestException($"Error: {response.StatusCode}, {await response.Content.ReadAsStringAsync()}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in DeleteUserAsync: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> AddUserAsync(Users user)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/Users/InsertUser", user);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                throw new HttpRequestException($"Error: {response.StatusCode}, {await response.Content.ReadAsStringAsync()}");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in AddUserAsync: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> UpdateUserAsync(Users user)
        {
            try
            {
                if (user.UserID != null)
                {

                    var response = await _httpClient.PutAsJsonAsync($"api/Users/UpdateUser", user);

                    if (response.IsSuccessStatusCode)
                    {
                        return true;
                    }

                    if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        Console.WriteLine($"User with ID {user.UserID} not found for update.");
                        return false;
                    }

                    throw new HttpRequestException($"Error: {response.StatusCode}, {await response.Content.ReadAsStringAsync()}");
                }
                throw new HttpRequestException($"No UserID Found.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in UpdateUserAsync: {ex.Message}");
                return false;
            }
        }
    }
}
