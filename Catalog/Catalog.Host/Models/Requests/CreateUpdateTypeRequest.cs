namespace Catalog.Host.Models.Requests
{
    public class CreateUpdateTypeRequest
    {
        [Required]
        [StringLength(50, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 2)]
        public string Type { get; set; } = null!;
    }
}
