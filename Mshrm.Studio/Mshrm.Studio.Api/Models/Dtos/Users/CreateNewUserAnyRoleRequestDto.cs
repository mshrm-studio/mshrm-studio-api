﻿using Mshrm.Studio.Api.Clients;
using Mshrm.Studio.Api.Clients.Auth;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Mshrm.Studio.Api.Models.Dtos.User
{
    /// <summary>
    /// Create a new user with any role specified
    /// </summary>
    public class CreateNewUserAnyRoleRequestDto
    {
        /// <summary>
        /// The user to registered email address this cannot be updated (used for identification). 
        /// Example: test@test.com
        /// </summary>
        [StringLength(256, ErrorMessage = "{0} must have less than {1} characters")]
        [Required(ErrorMessage = "MissingBindRequiredValueAccessor")]
        [RegularExpression(@"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*" + "@" + @"((([\-\w]+\.)+[a-zA-Z]{2,50})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$")]
        [JsonProperty("email")]
        public required string Email { get; set; }

        /// <summary>
        /// The users role. 
        /// Example: Member
        /// </summary>
        [Required(ErrorMessage = "MissingBindRequiredValueAccessor")]
        [JsonProperty("role")]
        public required RoleType Role { get; set; }

        /// <summary>
        /// The users first name (optional). 
        /// Example: John
        /// </summary>
        [JsonProperty("firstName")]
        [Required(ErrorMessage = "MissingBindRequiredValueAccessor")]
        [StringLength(256, ErrorMessage = "{0} must have less than {1} characters")]
        public required string FirstName { get; set; }

        /// <summary>
        /// The users last name (optional). 
        /// Example: Smith
        /// </summary>
        [JsonProperty("lastName")]
        [Required(ErrorMessage = "MissingBindRequiredValueAccessor")]
        [StringLength(256, ErrorMessage = "{0} must have less than {1} characters")]
        public required string LastName { get; set; }

        /// <summary>
        /// The password to set for new user. 
        /// Example: Pajhdy66_oRd
        /// </summary>
        [JsonProperty("password")]
        [Required(ErrorMessage = "MissingBindRequiredValueAccessor")]
        [StringLength(256, ErrorMessage = "{0} must have less than {1} characters")]
        [RegularExpression("^((?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])|(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[^a-zA-Z0-9])|(?=.*?[A-Z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])|(?=.*?[a-z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])).{10,}$", ErrorMessage = "Passwords must be at least 10 characters and contain all of the following: upper case (A-Z), lower case (a-z), number (0-9) and special character (e.g. !@#$%^&*)")]
        public required string Password { get; set; }
    }
}
