using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Linq;
using System.Windows.Forms;

namespace CipherUnlockPro
{
    public class DebugUserManager
    {
        private static readonly string API_URL = "https://www.cipherunlock.xyz/debug_api.php";
        private static DebugUserManager _instance;
        private static readonly object _lock = new object();

        public static DebugUserManager Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                        _instance = new DebugUserManager();
                }
                return _instance;
            }
        }

        private DebugUserManager() { }

        public async Task<UserRegistrationResult> RegisterUserAsync(string username, string email, string password)
        {
            var debugInfo = new StringBuilder();

            try
            {
                debugInfo.AppendLine($"=== DEBUG INFO ===");
                debugInfo.AppendLine($"Usuario: {username}");
                debugInfo.AppendLine($"Email: {email}");
                debugInfo.AppendLine($"URL: {API_URL}");
                debugInfo.AppendLine($"Timestamp: {DateTime.Now}");
                debugInfo.AppendLine();

                using (HttpClient client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromSeconds(30);

                    // Headers de navegador
                    client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36");
                    client.DefaultRequestHeaders.Add("Accept", "application/json, text/plain, */*");

                    var requestData = new
                    {
                        username = username,
                        email = email,
                        password = password
                    };

                    string json = JsonConvert.SerializeObject(requestData);
                    debugInfo.AppendLine($"JSON enviado: {json}");
                    debugInfo.AppendLine();

                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    var response = await client.PostAsync(API_URL, content);

                    debugInfo.AppendLine($"Status Code: {response.StatusCode} ({(int)response.StatusCode})");
                    debugInfo.AppendLine($"Status: {response.ReasonPhrase}");

                    if (response.Content.Headers.ContentType != null)
                    {
                        debugInfo.AppendLine($"Content-Type: {response.Content.Headers.ContentType}");
                    }

                    // Headers de respuesta
                    debugInfo.AppendLine("Response Headers:");
                    foreach (var header in response.Headers)
                    {
                        debugInfo.AppendLine($"  {header.Key}: {string.Join(", ", header.Value)}");
                    }
                    debugInfo.AppendLine();

                    string responseText = await response.Content.ReadAsStringAsync();

                    debugInfo.AppendLine($"Response Length: {responseText.Length} chars");
                    debugInfo.AppendLine($"Response Preview: {responseText.Substring(0, Math.Min(100, responseText.Length))}...");
                    debugInfo.AppendLine();

                    // Verificar tipo de contenido
                    if (responseText.TrimStart().StartsWith("<"))
                    {
                        debugInfo.AppendLine("⚠️ PROBLEMA: Recibiendo HTML!");
                        debugInfo.AppendLine("Response completa:");
                        debugInfo.AppendLine(responseText);

                        // Mostrar debug info antes de fallar
                        MessageBox.Show(debugInfo.ToString(), "DEBUG - HTML Recibido", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                        return new UserRegistrationResult
                        {
                            IsSuccess = false,
                            Message = "Server returned HTML instead of JSON",
                            Username = username,
                            Email = email
                        };
                    }
                    else if (responseText.TrimStart().StartsWith("{"))
                    {
                        debugInfo.AppendLine("✅ Recibiendo JSON válido");
                        debugInfo.AppendLine($"JSON completo: {responseText}");

                        // Mostrar debug info de éxito
                        MessageBox.Show(debugInfo.ToString(), "DEBUG - JSON Recibido", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        debugInfo.AppendLine("⚠️ Formato desconocido:");
                        debugInfo.AppendLine(responseText);

                        MessageBox.Show(debugInfo.ToString(), "DEBUG - Formato Desconocido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }

                    var result = JsonConvert.DeserializeObject<SimpleApiResponse>(responseText);

                    return new UserRegistrationResult
                    {
                        IsSuccess = result?.Success ?? false,
                        Message = result?.Message ?? "Error desconocido",
                        Username = username,
                        Email = email
                    };
                }
            }
            catch (HttpRequestException httpEx)
            {
                debugInfo.AppendLine($"HTTP Error: {httpEx.Message}");
                MessageBox.Show(debugInfo.ToString(), "DEBUG - HTTP Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return new UserRegistrationResult
                {
                    IsSuccess = false,
                    Message = $"HTTP Error: {httpEx.Message}",
                    Username = username,
                    Email = email
                };
            }
            catch (TaskCanceledException timeoutEx)
            {
                debugInfo.AppendLine($"Timeout: {timeoutEx.Message}");
                MessageBox.Show(debugInfo.ToString(), "DEBUG - Timeout", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return new UserRegistrationResult
                {
                    IsSuccess = false,
                    Message = "Timeout",
                    Username = username,
                    Email = email
                };
            }
            catch (Exception ex)
            {
                debugInfo.AppendLine($"Exception: {ex.GetType().Name}");
                debugInfo.AppendLine($"Message: {ex.Message}");
                debugInfo.AppendLine($"StackTrace: {ex.StackTrace}");

                MessageBox.Show(debugInfo.ToString(), "DEBUG - Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return new UserRegistrationResult
                {
                    IsSuccess = false,
                    Message = $"Error: {ex.Message}",
                    Username = username,
                    Email = email
                };
            }
        }

        public async Task<bool> TestConnectionAsync()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromSeconds(10);
                    client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36");

                    var testData = new { test = "connectivity_check" };
                    string json = JsonConvert.SerializeObject(testData);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    var response = await client.PostAsync(API_URL, content);

                    MessageBox.Show($"Connection Test:\nStatus: {response.StatusCode}\nSuccess: {response.IsSuccessStatusCode}", "Connection Test", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    return response.IsSuccessStatusCode;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Connection Test Failed:\n{ex.Message}", "Connection Test", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
    }

    public class SimpleApiResponse
    {
        [JsonProperty("Success")]
        public bool Success { get; set; }

        [JsonProperty("Message")]
        public string Message { get; set; }

        [JsonProperty("Data")]
        public object Data { get; set; }

        [JsonProperty("Error")]
        public string Error { get; set; }

        [JsonProperty("Debug_Timestamp")]
        public string DebugTimestamp { get; set; }
    }

    public class UserRegistrationResult
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
    }
}