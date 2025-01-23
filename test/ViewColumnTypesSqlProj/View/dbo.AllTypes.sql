CREATE VIEW [dbo].[AllTypes]
AS

SELECT 
 CAST( 0x1  AS bit 				  ) col_bit 						
,CAST( 0x2  AS tinyint			  ) col_tinyint			  
,CAST( 0x3  AS smallint			  ) col_smallint		  
,CAST( 0x4  AS int				  ) col_int				  
,CAST( 0x5  AS bigint			  ) col_bigint			  
,CAST( 6  AS decimal 			  ) col_decimal 		
,CAST( 7  AS numeric 			  ) col_numeric 		
,CAST( 8  AS money				  ) col_money			
,CAST( 9  AS smallmoney		  ) col_smallmoney		  
,CAST( 10 AS float				  ) col_float			  
,CAST( 11  AS real				  ) col_real			  
,CAST( '12/12/12'  AS date				  ) col_date			
,CAST( '13:13'  AS time				  ) col_time			
,CAST( '12/14/2014 14:14' AS datetime2		  ) col_datetime2		
,CAST( '12/15/2015 15:15'  AS datetimeoffset	  ) col_datetimeoffset	  
,CAST( '12/16/2016 16:16'  AS datetime			  ) col_datetime		
,CAST( '12/17/17'  AS smalldatetime		  ) col_smalldatetime
,CAST( 0x18  AS varchar(1)			  ) col_varchar			
,CAST( '19'  AS text				  ) col_text			
,CAST( 0x20  AS nchar				  ) col_nchar			
,CAST( 0x21 AS nvarchar(1)		  ) col_nvarchar		  
,CAST( '22'  AS ntext				  ) col_ntext			
,CAST( 0x23  AS binary			  ) col_binary			  
,CAST( 0x24 AS varbinary(1)			  ) col_varbinary
,CAST( 0x25 AS image				  ) col_image			
,CAST( 0x26  AS rowversion		  ) col_rowversion		  
,CAST( 0x27 AS uniqueidentifier	  ) col_uniqueidentifier  
,CAST( '<root>28</root>'  AS xml				  ) col_xml				  

GO 

CREATE TYPE [dbo].[AllTypes] AS TABLE (
	[col_bit] [bit] NULL,
	[col_tinyint] [tinyint] NULL,
	[col_smallint] [smallint] NULL,
	[col_int] [int] NULL,
	[col_bigint] [bigint] NULL,
	[col_decimal] [decimal](18, 0) NULL,
	[col_numeric] [numeric](18, 0) NULL,
	[col_money] [money] NULL,
	[col_smallmoney] [smallmoney] NULL,
	[col_float] [float] NULL,
	[col_real] [real] NULL,
	[col_date] [date] NULL,
	[col_time] [time](7) NULL,
	[col_datetime2] [datetime2](7) NULL,
	[col_datetimeoffset] [datetimeoffset](7) NULL,
	[col_datetime] [datetime] NULL,
	[col_smalldatetime] [smalldatetime] NULL,
	[col_varchar] [varchar](1) NULL,
	[col_text] [text] NULL,
	[col_nchar] [nchar](30) NULL,
	[col_nvarchar] [nvarchar](1) NULL,
	[col_ntext] [ntext] NULL,
	[col_binary] [binary](30) NULL,
	[col_varbinary] [varbinary](1) NULL,
	[col_image] [image] NULL,
	[col_rowversion] [timestamp] NULL,
	[col_uniqueidentifier] [uniqueidentifier] NULL,
	[col_xml] [xml] NULL
) 
GO