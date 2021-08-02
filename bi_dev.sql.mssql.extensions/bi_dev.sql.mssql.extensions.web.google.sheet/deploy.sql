-- global assembiles -------------
-- System.Runtime.Serialization---
declare @system_runtime_serialization_location nvarchar(4000) = 'C:\Windows\Microsoft.NET\Framework64\v4.0.30319\System.Runtime.Serialization.dll';
if (not exists (select 1 from sys.assemblies where name = N'System.Runtime.Serialization')) create ASSEMBLY [System.Runtime.Serialization] from @system_runtime_serialization_location with permission_set = unsafe;
else begin
	begin try
		alter ASSEMBLY [System.Runtime.Serialization] from @system_runtime_serialization_location with permission_set = unsafe;
	end try
	begin catch
		declare @error_message nvarchar(max) = error_message();
		if (@error_message not like '%identical to an assembly that is already registered under the name%') begin
			raiserror(@error_message, 16, 1);
		end
	end catch
end
go
--------------------------------
-- System.Net.Http -------------
declare @system_net_http_location nvarchar(4000) = 'C:\Windows\Microsoft.NET\Framework64\v4.0.30319\System.Net.Http.dll';
if (not exists (select 1 from sys.assemblies where name = N'System.Net.Http')) create ASSEMBLY [System.Net.Http] from @system_net_http_location with permission_set = unsafe;
else begin
	begin try
		alter ASSEMBLY [System.Net.Http] from @system_net_http_location with permission_set = unsafe;
	end try
	begin catch
		declare @error_message nvarchar(max) = error_message();
		if (@error_message not like '%identical to an assembly that is already registered under the name%') begin
			raiserror(@error_message, 16, 1);
		end
	end catch
end
go
---------------------------------
-- INPUT ------------------------
-- determin project-location ----
declare @build_location nvarchar(max) = N'C:\storage\ssd01\app\sql\bi_dev.sql.mssql.extensions';
-------------------------------
declare @build_file_name nvarchar(4000) = @build_location + N'\bi_dev.sql.mssql.extensions.web.google.sheet\bin\Debug\bi_dev.sql.mssql.extensions.web.google.sheet.dll';
-- drop existing --------
if (object_id('.dbo.f_clr_gs_clear_range') is not null) drop function dbo.f_clr_gs_clear_range;
if (object_id('.dbo.f_clr_gs_update_range') is not null) drop function dbo.f_clr_gs_update_range;
if (object_id('.dbo.f_clr_gs_get_range') is not null) drop function dbo.f_clr_gs_get_range;
if (exists (select 1 from sys.assemblies where name = N'mssql.extensions.web.google.sheet')) drop ASSEMBLY [mssql.extensions.web.google.sheet];
-- create new --------------------
create assembly [mssql.extensions.web.google.sheet] from @build_file_name with permission_set = unsafe;
go
CREATE FUNCTION dbo.f_clr_gs_update_range (
	@access_token nvarchar(max),
	@spreadsheet_id nvarchar(max),
	@sheet_name nvarchar(max),
	@range_from nvarchar(max),
	@json_object nvarchar(max),
	@include_column_names bit,
	@clean_before_update bit,
	@false_when_error bit
)  
RETURNS bit
with execute as owner 
as external name [mssql.extensions.web.google.sheet].[bi_dev.sql.mssql.extensions.web.google.sheet.Utils].[UpdateRange];
go
create function dbo.f_clr_gs_clear_range (
	@access_token nvarchar(max),
	@spreadsheet_id nvarchar(max),
	@sheet_name nvarchar(max),
	@range nvarchar(max),
	@false_when_error bit
) returns bit
with execute as owner
as external name [mssql.extensions.web.google.sheet].[bi_dev.sql.mssql.extensions.web.google.sheet.Utils].[ClearRange];
go
create function dbo.f_clr_gs_get_range (
	@access_token nvarchar(max),
	@spreadsheet_id nvarchar(max),
	@sheet_name nvarchar(max),
	@range nvarchar(max),
	@null_when_error bit
) returns nvarchar(max)
with execute as owner
as external name [mssql.extensions.web.google.sheet].[bi_dev.sql.mssql.extensions.web.google.sheet.Utils].[GetRange];
go