USE [DotnetCoreLogger]
GO

CREATE TABLE [dbo].[SqlLogs](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[UserId] [bigint] NULL,
	[ApplicationName] [varchar](50) NULL,
	[IpAddress] [varchar](50) NULL,
	[Version] [varchar](50) NULL,
	[Host] [varchar](50) NULL,
	[Url] [varchar](255) NULL,
	[Source] [varchar](255) NULL,
	[Scheme] [varchar](50) NULL,
	[TraceId] [varchar](50) NULL,
	[Protocol] [varchar](50) NULL,
	[UrlReferrer] [varchar](250) NULL,
	[Area] [varchar](50) NULL,
	[ControllerName] [varchar](50) NULL,
	[ActionName] [varchar](50) NULL,
	[ClassName] [varchar](50) NULL,
	[MethodName] [varchar](50) NULL,
	[QueryType] [varchar](50) NULL,
	[Query] [nvarchar](max) NULL,
	[Response] [nvarchar](max) NULL,
	[Duration] [nvarchar](max) NULL,
	[Message] [nvarchar](max) NULL,
	[Connection] [nvarchar](max) NULL,
	[Command] [nvarchar](max) NULL,
	[Event] [nvarchar](max) NULL,
	[CreatedDateUtc] [datetime2](7) NULL,
 CONSTRAINT [PK_SqlLogs] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


