
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

    public async Task<Result<string, LogApiException>> Suscribe(SuscribePayload request) {
        using HttpClient client = new();

        string json = JsonSerializer.Serialize(request);
        StringContent body = new(json, Encoding.UTF8, "application/json");

        HttpResponseMessage response = await client.PostAsync($"{this.host}/subscribe", body);

        if (!response.IsSuccessStatusCode) {
            var exception = new LogApiException(500, "", "Could not get node public key.");
            return Result<string, LogApiException>.ERR(exception);
        }

        string token = await response.Content.ReadAsStringAsync();

        return Result<string, LogApiException>.OK(token);
    }

}