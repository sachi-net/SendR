namespace SimplySoft.Core.SendR.GlobalProperties
{
    internal class Messages
    {
        #region Generic
        internal const string CONFIGURATION_ERROR = "App configuration not defined.";
        internal const string SERVICE_NOT_CONFIGURED = "Service '[S]' is not configured as a dependency.";
        internal const string DUPLICATE_TEMPLATE = "Template of type [T] is already defined with name of '[N]'.";
        #endregion

        #region Email
        internal const string EMAIL_TEMPLATE_NAME_REQUIRED = "Email template name cannot be null or empty.";
        internal const string EMAIL_TEMPLATE_NOT_DEFINED = "Email template ('[T]') not defined.";
        internal const string EMAIL_TEMPLATE_PATH_NOT_EXIST = "Email template reference path ('[P]') does not exist.";
        #endregion
    }
}
