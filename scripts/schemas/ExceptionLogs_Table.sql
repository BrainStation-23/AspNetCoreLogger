USE [DotnetCoreLogger]

CREATE TABLE [dbo].[ExceptionLogs](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[UserId] [bigint] NULL,
	[ApplicationName] [varchar](50) NULL,
	[IpAddress] [varchar](50) NULL,
	[Version] [varchar](50) NULL,
	[Host] [varchar](50) NULL,
	[Url] [varchar](255) NULL,
	[Source] [varchar](255) NULL,
	[Form] [nvarchar](max) NULL,
	[Body] [nvarchar](max) NULL,
	[Response] [nvarchar](max) NULL,
	[RequestHeaders] [varchar](max) NULL,
	[ResponseHeaders] [varchar](max) NULL,
	[ErrorCode] [varchar](50) NULL,
	[Scheme] [varchar](50) NULL,
	[TraceId] [varchar](50) NULL,
	[Protocol] [varchar](50) NULL,
	[Errors] [nvarchar](max) NULL,
	[StatusCode] [varchar](50) NULL,
	[AppStatusCode] [varchar](50) NULL,
	[Message] [nvarchar](255) NULL,
	[MessageDetails] [nvarchar](max) NULL,
	[StackTrace] [nvarchar](max) NULL,
	[CreatedDateUtc] [datetime2](7) NULL,
 CONSTRAINT [PK_ExceptionLogs] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


