using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFWebRazor.Migrations
{
    /// <inheritdoc />
    public partial class SeedUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

         for(int i =0; i< 150; i++)
         {

            migrationBuilder.InsertData(
                "Users",
                columns : new[]
                {
                    "Id",
                    "UserName",
                    "Email",
                    "SecurityStamp",
                    "EmailConfirmed",
                    "PhoneNumberConfirmed",
                    "TwoFactorEnabled",
                    "LockoutEnabled",
                    "AccessFailedCount",
                    "HomeAddrss"
                },
                values : new object[]{
                    Guid.NewGuid().ToString(),
                    "User-"+i.ToString("D3"),
                    $"email{i.ToString("D3")}@example.com",
                    Guid.NewGuid().ToString(),
                    true,
                    false,
                    false,
                    false,
                    0,
                    "...@#%..."



                }
            );
        }}

/*
  ,[UserName]
      ,[NormalizedUserName]
      ,[Email]
      ,[NormalizedEmail]
      ,[EmailConfirmed]
      ,[PasswordHash]
      ,[SecurityStamp]
      ,[ConcurrencyStamp]
      ,[PhoneNumber]
      ,[PhoneNumberConfirmed]
      ,[TwoFactorEnabled]
      ,[LockoutEnd]
      ,[LockoutEnabled]
      ,[AccessFailedCount]
      ,[HomeAddrss]
      ,[BrithDate]
*/
        

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}

