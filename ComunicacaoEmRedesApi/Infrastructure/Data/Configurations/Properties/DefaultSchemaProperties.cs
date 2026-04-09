namespace ComunicacaoEmRedesApi.Infrastructure.Data.Configurations.Properties;

public struct DefaultSchemaProperties
{
    public struct TableNames
    {
        public const string UserTable = "users";
        public const string ChatTable = "chats";
        public const string MessageTable = "messages";
        public const string UserChatTable = "user_chats";
        public const string TokenSessionTable = "session_tokens";
    }
    
    public struct ColumnNames
    {
        // Default columns
        public const string IdColumnName = "id";
        public const string ActiveColumnName = "is_active";
        
        // User columns
        public const string EmailColumnName = "email";
        public const string PasswordColumnName = "password_hash";
        
        // Message columns
        public const string ContentColumnName = "content";
        public const string ChatIdColumnName = "chat_id";
        public const string UserIdColumnName = "user_id";
        
        // Chat columns
        public const string TitleColumnName = "title";
        
        public const string TokenValueColumnName = "hash_token";
        public const string IsRevokedColumnName = "is_revoked";
        public const string ExpirationColumnName = "expires_at";
    }

    public struct ColumnProperties
    {
        // User column properties
        public const int EmailMaxLength = 252;
        public const int PasswordHashMaxLength = 60;
        
        // Message column properties
        public const int ContentMaxLength = 2000;
        
        // Chat column properties
        public const int TitleMaxLength = 30;
        
        public const int TokenValueMaxLength = 70;
    }
}