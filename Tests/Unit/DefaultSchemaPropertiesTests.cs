using ComunicacaoEmRedesApi.Infrastructure.Data.Configurations.Properties;

namespace Tests.Unit;

public abstract class DefaultSchemaPropertiesTests
{
    public class TableNameTests
    {
        [Fact]
        internal void Should_GetUserTable_When_Called()
        {
            Assert.Equal("users", DefaultSchemaProperties.TableNames.UserTable);
        }
        
        [Fact]
        internal void Should_GetChatTable_When_Called()
        {
            Assert.Equal("chats", DefaultSchemaProperties.TableNames.ChatTable);
        }
        
        [Fact]
        internal void Should_GetMessageTable_When_Called()
        {
            Assert.Equal("messages", DefaultSchemaProperties.TableNames.MessageTable);
        }
        
        [Fact]
        internal void Should_GetUserChatTable_When_Called()
        {
            Assert.Equal("user_chats", DefaultSchemaProperties.TableNames.UserChatTable);
        }
    }

    public class ColumnPropertiesTests
    {
        [Fact]
        internal void Should_GetEmailMaxLength_When_Called()
        {
            Assert.Equal(252, DefaultSchemaProperties.ColumnProperties.EmailMaxLength);
        }

        [Fact]
        internal void Should_GetPasswordHashMaxLength_When_Called()
        {
            Assert.Equal(60, DefaultSchemaProperties.ColumnProperties.PasswordHashMaxLength);
        }

        [Fact]
        internal void Should_GetContentMaxLength_When_Called()
        {
            Assert.Equal(2000, DefaultSchemaProperties.ColumnProperties.ContentMaxLength);
        }

        [Fact]
        internal void Should_GetTitleMaxLength_When_Called()
        {
            Assert.Equal(30, DefaultSchemaProperties.ColumnProperties.TitleMaxLength);
        }
    }

    public class ColumnNameTests
    {
        [Fact]
        internal void Should_GetIdColumnName_When_Called()
        {
            Assert.Equal("id", DefaultSchemaProperties.ColumnNames.IdColumnName);
        }
        
        [Fact]
        internal void Should_GetActiveColumnName_When_Called()
        {
            Assert.Equal("is_active", DefaultSchemaProperties.ColumnNames.ActiveColumnName);
        }
        
        [Fact]
        internal void Should_GetEmailColumnName_When_Called()
        {
            Assert.Equal("email", DefaultSchemaProperties.ColumnNames.EmailColumnName);
        }
        
        [Fact]
        internal void Should_GetPasswordColumnName_When_Called()
        {
            Assert.Equal("password_hash", DefaultSchemaProperties.ColumnNames.PasswordColumnName);
        }
        
        [Fact]
        internal void Should_GetContentColumnName_When_Called()
        {
            Assert.Equal("content", DefaultSchemaProperties.ColumnNames.ContentColumnName);
        }
        
        [Fact]
        internal void Should_GetChatIdColumnName_When_Called()
        {
            Assert.Equal("chat_id", DefaultSchemaProperties.ColumnNames.ChatIdColumnName);
        }

        [Fact]
        internal void Should_GetUserIdColumnName_When_Called()
        {
            Assert.Equal("user_id", DefaultSchemaProperties.ColumnNames.UserIdColumnName);
        }

        [Fact]
        internal void Should_GetTitleColumnName_When_Called()
        {
            Assert.Equal("title", DefaultSchemaProperties.ColumnNames.TitleColumnName);
        }
    }
}