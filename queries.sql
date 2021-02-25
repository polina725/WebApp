INSERT INTO [WebAppDb].[dbo].[Photos] ([AppUserId], [Url], [IsMain]) VALUES (1, 'https://randomuser.me/api/portraits/women/54.jpg', 1)

SELECT TOP (1000) [Id]
      ,[Url]
      ,[IsMain]
      ,[PublicId]
      ,[AppUserId]
  FROM [WebAppDb].[dbo].[Photos]