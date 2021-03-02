DELETE FROM [WebAppDb].[dbo].[Users] WHERE Id = 12;

SELECT TOP (1000) *
  FROM [WebAppDb].[dbo].[Users] WHERE [WebAppDb].[dbo].[Users].[Gender] = 'male'