namespace Billycock_MS_Reusable.DTO.Utils
{
    public class RegisterExceptionRequest
    {
        public string integration { get; set; }
        public string target { get; set; }
        public string ex { get; set; }
        public string method { get; set; }
        public string input { get; set; }
    }
}
