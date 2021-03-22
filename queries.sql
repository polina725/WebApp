DELETE FROM [WebAppDb].[dbo].[Messages] WHERE Id = 7;

SELECT TOP (1000) *
  FROM [WebAppDb].[dbo].[Photos]

SELECT [t].[Id], [t].[AccessFailedCount], [t].[City], [t].[ConcurrencyStamp], [t].[Country], [t].[Created], [t].[DateOfBirth], [t].[Email], [t].[EmailConfirmed], [t].[Gender], [t].[Interests], [t].[Introduction], [t].[KnownAs], [t].[LastActive], [t].[LockoutEnabled], [t].[LockoutEnd], [t].[LookingFor], [t].[NormalizedEmail], [t].[NormalizedUserName], [t].[PasswordHash], [t].[PasswordSalt], [t].[PhoneNumber], [t].[PhoneNumberConfirmed], [t].[SecurityStamp], [t].[TwoFactorEnabled], [t].[UserName], [t0].[Id], [t0].[AppUserId], [t0].[IsApproved], [t0].[IsMain], [t0].[PublicId], [t0].[Url]
      FROM (
          SELECT TOP(2) [a].[Id], [a].[AccessFailedCount], [a].[City], [a].[ConcurrencyStamp], [a].[Country], [a].[Created], [a].[DateOfBirth], [a].[Email], [a].[EmailConfirmed], [a].[Gender], [a].[Interests], 
[a].[Introduction], [a].[KnownAs], [a].[LastActive], [a].[LockoutEnabled], [a].[LockoutEnd], [a].[LookingFor], [a].[NormalizedEmail], [a].[NormalizedUserName], [a].[PasswordHash], [a].[PasswordSalt], [a].[PhoneNumber], [a].[PhoneNumberConfirmed], [a].[SecurityStamp], [a].[TwoFactorEnabled], [a].[UserName]
          FROM [AspNetUsers] AS [a]
          WHERE [a].[UserName] = 'davis'
      ) AS [t]
      LEFT JOIN (
          SELECT [p].[Id], [p].[AppUserId], [p].[IsApproved], [p].[IsMain], [p].[PublicId], [p].[Url]
          FROM [Photos] AS [p]
          WHERE [p].[IsApproved] = CAST(1 AS bit)
      ) AS [t0] ON [t].[Id] = [t0].[AppUserId]
      ORDER BY [t].[Id], [t0].[Id]

SELECT * FROM [WebAppDb].[dbo].[Photos] WHERE [AppUserId] = 10