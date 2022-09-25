using System;
using System.Diagnostics.CodeAnalysis;

namespace SimplySoft.Core.SendR.Email.Models
{
    /// <summary>
    /// Define a custom email template with dynamic data binding ability.
    /// </summary>
    public class EmailTemplate : IEquatable<EmailTemplate>
    {
        /// <summary>
        /// Name of the custom template. Name must be unique.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Email subject associated with this email template.
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Absolute path reference to the template file.
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Determine whether this template is equal to the provided <see cref="EmailTemplate"/>.
        /// </summary>
        /// <param name="other">Other <see cref="EmailTemplate"/> object to be compared with.</param>
        /// <returns>Returns <see langword="true"/> if the name of these templates are same or otherwise <see langword="false"/>.</returns>
        public bool Equals([AllowNull] EmailTemplate other)
        {
            return other != null && Name.Equals(other.Name, StringComparison.OrdinalIgnoreCase);
        }
    }
}
