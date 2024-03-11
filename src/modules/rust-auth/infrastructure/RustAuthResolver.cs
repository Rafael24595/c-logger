
using System.Text;
using System.Text.Json;

class RustAuthResolver {
    
    private readonly string host;

    public RustAuthResolver(string host) {
        this.host = host;
    }

    public async Task<Result<PubKey, LogApiException>> PubKey() {
        using HttpClient client = new();

        HttpResponseMessage response = await client.GetAsync($"{this.host}/nodekey");

        if (!response.IsSuccessStatusCode) {
            var exception = new LogApiException(500, "", "Could not get node public key.");
            return Result<PubKey, LogApiException>.ERR(exception);
        }

        string content = await response.Content.ReadAsStringAsync();
        PubKey pubkey = JsonSerializer.Deserialize<PubKey>(content);

        if (pubkey == null) {
            var exception = new LogApiException(422, "", "Could not get node public key.");
            return Result<PubKey, LogApiException>.ERR(exception);
        }

        return Result<PubKey, LogApiException>.OK(pubkey);
    }

    public async Task<Result<SuscribeResponse, LogApiException>> Suscribe(string request) {
        using HttpClient client = new();

        StringContent body = new(request, Encoding.UTF8, "text/plain");

        HttpResponseMessage response = await client.PostAsync($"{this.host}/nodekey", body);

        if (!response.IsSuccessStatusCode) {
            var exception = new LogApiException(500, "", "Could not get node public key.");
            return Result<SuscribeResponse, LogApiException>.ERR(exception);
        }

        string content = await response.Content.ReadAsStringAsync();
        SuscribeResponse token = JsonSerializer.Deserialize<SuscribeResponse>(content);

        if (token == null) {
            var exception = new LogApiException(422, "", "Could not get node public key.");
            return Result<SuscribeResponse, LogApiException>.ERR(exception);
        }

        return Result<SuscribeResponse, LogApiException>.OK(token);

    }

}