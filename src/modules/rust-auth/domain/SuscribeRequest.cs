public class SuscribeRequest {

    public string service { get; set; }
    public string pass_key { get; set; }
    public SymmetricKey symetric_key { get; set; }
    public string host { get; set; }
    public string end_point_status { get; set; }
    public string end_point_key { get; set; }

    public SuscribeRequest(string service, string pass_key, SymmetricKey symetric_key, string host, string end_point_status, string end_point_key) {
        this.service = service;
        this.pass_key = pass_key;
        this.symetric_key = symetric_key;
        this.host = host;
        this.end_point_status = end_point_status;
        this.end_point_key = end_point_key;
    }

}