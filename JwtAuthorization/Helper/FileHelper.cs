using System.Text;
using System.Text.Json;

namespace JwtAuthorization.Helpers
{
    public static class FileHelper
    {
        /// <summary>
        /// 讀取文字檔案
        /// </summary>
        /// <param name="filePath">檔案路徑</param>
        /// <param name="encoding">編碼格式，默認為UTF-8</param>
        /// <returns>檔案內容</returns>
        /// <exception cref="FileNotFoundException">檔案不存在時拋出異常</exception>
        /// <exception cref="UnauthorizedAccessException">沒有讀取權限時拋出異常</exception>
        public static string ReadTextFile(string filePath, Encoding? encoding = null)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("檔案路徑不能為空", nameof(filePath));

            if (!File.Exists(filePath))
                throw new FileNotFoundException($"檔案不存在: {filePath}");

            encoding ??= Encoding.UTF8;

            try
            {
                return File.ReadAllText(filePath, encoding);
            }
            catch (Exception ex)
            {
                throw new IOException($"讀取檔案失敗: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 非同步讀取文字檔案
        /// </summary>
        /// <param name="filePath">檔案路徑</param>
        /// <param name="encoding">編碼格式，默認為UTF-8</param>
        /// <returns>檔案內容</returns>
        public static async Task<string> ReadTextFileAsync(string filePath, Encoding? encoding = null)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("檔案路徑不能為空", nameof(filePath));

            if (!File.Exists(filePath))
                throw new FileNotFoundException($"檔案不存在: {filePath}");

            encoding ??= Encoding.UTF8;

            try
            {
                return await File.ReadAllTextAsync(filePath, encoding);
            }
            catch (Exception ex)
            {
                throw new IOException($"讀取檔案失敗: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 寫入文字檔案
        /// </summary>
        /// <param name="filePath">檔案路徑</param>
        /// <param name="content">要寫入的內容</param>
        /// <param name="encoding">編碼格式，默認為UTF-8</param>
        /// <param name="append">是否追加模式，默認為false（覆蓋）</param>
        /// <exception cref="UnauthorizedAccessException">沒有寫入權限時拋出異常</exception>
        public static void WriteTextFile(string filePath, string content, Encoding? encoding = null, bool append = false)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("檔案路徑不能為空", nameof(filePath));

            content ??= string.Empty;
            encoding ??= Encoding.UTF8;

            try
            {
                // 確保目錄存在
                var directory = Path.GetDirectoryName(filePath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                if (append)
                {
                    File.AppendAllText(filePath, content, encoding);
                }
                else
                {
                    File.WriteAllText(filePath, content, encoding);
                }
            }
            catch (Exception ex)
            {
                throw new IOException($"寫入檔案失敗: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 非同步寫入文字檔案
        /// </summary>
        /// <param name="filePath">檔案路徑</param>
        /// <param name="content">要寫入的內容</param>
        /// <param name="encoding">編碼格式，默認為UTF-8</param>
        /// <param name="append">是否追加模式，默認為false（覆蓋）</param>
        public static async Task WriteTextFileAsync(string filePath, string content, Encoding? encoding = null, bool append = false)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("檔案路徑不能為空", nameof(filePath));

            content ??= string.Empty;
            encoding ??= Encoding.UTF8;

            try
            {
                // 確保目錄存在
                var directory = Path.GetDirectoryName(filePath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                if (append)
                {
                    await File.AppendAllTextAsync(filePath, content, encoding);
                }
                else
                {
                    await File.WriteAllTextAsync(filePath, content, encoding);
                }
            }
            catch (Exception ex)
            {
                throw new IOException($"寫入檔案失敗: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 讀取JSON檔案並轉換為指定類型的物件
        /// </summary>
        /// <typeparam name="T">要轉換的物件類型</typeparam>
        /// <param name="filePath">JSON檔案路徑</param>
        /// <param name="options">JSON序列化選項</param>
        /// <returns>轉換後的物件</returns>
        /// <exception cref="FileNotFoundException">檔案不存在時拋出異常</exception>
        /// <exception cref="JsonException">JSON格式錯誤時拋出異常</exception>
        public static T ReadJsonFile<T>(string filePath, JsonSerializerOptions? options = null)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("檔案路徑不能為空", nameof(filePath));

            if (!File.Exists(filePath))
                throw new FileNotFoundException($"檔案不存在: {filePath}");

            try
            {
                var jsonContent = File.ReadAllText(filePath, Encoding.UTF8);

                if (string.IsNullOrWhiteSpace(jsonContent))
                    throw new InvalidDataException("JSON檔案內容為空");

                options ??= new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    ReadCommentHandling = JsonCommentHandling.Skip,
                    AllowTrailingCommas = true
                };

                var result = JsonSerializer.Deserialize<T>(jsonContent, options);
                return result ?? throw new InvalidDataException("JSON反序列化結果為null");
            }
            catch (JsonException ex)
            {
                throw new JsonException($"JSON格式錯誤: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new IOException($"讀取JSON檔案失敗: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 非同步讀取JSON檔案並轉換為指定類型的物件
        /// </summary>
        /// <typeparam name="T">要轉換的物件類型</typeparam>
        /// <param name="filePath">JSON檔案路徑</param>
        /// <param name="options">JSON序列化選項</param>
        /// <returns>轉換後的物件</returns>
        public static async Task<T> ReadJsonFileAsync<T>(string filePath, JsonSerializerOptions? options = null)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("檔案路徑不能為空", nameof(filePath));

            if (!File.Exists(filePath))
                throw new FileNotFoundException($"檔案不存在: {filePath}");

            try
            {
                var jsonContent = await File.ReadAllTextAsync(filePath, Encoding.UTF8);

                if (string.IsNullOrWhiteSpace(jsonContent))
                    throw new InvalidDataException("JSON檔案內容為空");

                options ??= new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    ReadCommentHandling = JsonCommentHandling.Skip,
                    AllowTrailingCommas = true
                };

                var result = JsonSerializer.Deserialize<T>(jsonContent, options);
                return result ?? throw new InvalidDataException("JSON反序列化結果為null");
            }
            catch (JsonException ex)
            {
                throw new JsonException($"JSON格式錯誤: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new IOException($"讀取JSON檔案失敗: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 將物件序列化為JSON並寫入檔案
        /// </summary>
        /// <typeparam name="T">物件類型</typeparam>
        /// <param name="filePath">檔案路徑</param>
        /// <param name="obj">要序列化的物件</param>
        /// <param name="options">JSON序列化選項</param>
        public static void WriteJsonFile<T>(string filePath, T obj, JsonSerializerOptions? options = null)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("檔案路徑不能為空", nameof(filePath));

            if (obj == null)
                throw new ArgumentNullException(nameof(obj), "要序列化的物件不能為null");

            try
            {
                options ??= new JsonSerializerOptions
                {
                    WriteIndented = true,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                };

                var jsonContent = JsonSerializer.Serialize(obj, options);
                WriteTextFile(filePath, jsonContent);
            }
            catch (Exception ex)
            {
                throw new IOException($"寫入JSON檔案失敗: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 非同步將物件序列化為JSON並寫入檔案
        /// </summary>
        /// <typeparam name="T">物件類型</typeparam>
        /// <param name="filePath">檔案路徑</param>
        /// <param name="obj">要序列化的物件</param>
        /// <param name="options">JSON序列化選項</param>
        public static async Task WriteJsonFileAsync<T>(string filePath, T obj, JsonSerializerOptions? options = null)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("檔案路徑不能為空", nameof(filePath));

            if (obj == null)
                throw new ArgumentNullException(nameof(obj), "要序列化的物件不能為null");

            try
            {
                options ??= new JsonSerializerOptions
                {
                    WriteIndented = true,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                };

                var jsonContent = JsonSerializer.Serialize(obj, options);
                await WriteTextFileAsync(filePath, jsonContent);
            }
            catch (Exception ex)
            {
                throw new IOException($"寫入JSON檔案失敗: {ex.Message}", ex);
            }
        }
    }
}