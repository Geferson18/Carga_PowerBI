namespace Billycock_MS_Reusable.DTO.Utils
{
    public class DescriptionValidationRequest
    {
        public string tipo { get; set; }
        public List<string> descriptions {  get; set; }
        public string descriptiontoValidate { get; set; }

        public DescriptionValidationRequest()
        {
            descriptions = new List<string>();
        }
    }
}
