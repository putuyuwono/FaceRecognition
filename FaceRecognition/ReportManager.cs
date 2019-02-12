using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace OpenScreenProjectFaceRS
{
    public class ReportManager
    {
        public static async Task<String> PostAsync(String uri, String data)
        {
            String result = String.Empty;
            var httpClient = new HttpClient();
            try
            {
                var content = new StringContent(data, Encoding.UTF8, "application/x-www-form-urlencoded");
                var response = await httpClient.PostAsync(uri, content);
                response.EnsureSuccessStatusCode();
                result = await response.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException ex)
            {
                throw ex;
            }
            httpClient.Dispose();
            return result;
        }
    }
}
