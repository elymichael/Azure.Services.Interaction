namespace Azure.Services.Interaction.B2C.Commands
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// 
    /// </summary>
    public class CreateUserInfoCommand
    {    
        [Required]
        public string Name { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; }
        [Required]
        public string CustomerId { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
