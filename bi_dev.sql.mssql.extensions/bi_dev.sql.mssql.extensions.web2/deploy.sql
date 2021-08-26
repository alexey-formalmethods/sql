-- assembiles -------------
-- System.Runtime.Serialization -------------
declare @system_runtime_serialization_location nvarchar(4000) = 'C:\Windows\Microsoft.NET\Framework64\v4.0.30319\System.Runtime.Serialization.dll';
if (not exists (select 1 from sys.assemblies where name = N'System.Runtime.Serialization')) create ASSEMBLY [System.Runtime.Serialization] from @system_runtime_serialization_location with permission_set = unsafe;
else begin
	begin try
		alter ASSEMBLY [System.Net.Http] from @system_runtime_serialization_location with permission_set = unsafe;
	end try
	begin catch
		declare @error_message nvarchar(max) = error_message();
		if (@error_message not like '%identical to an assembly that is already registered under the name%') begin
			raiserror(@error_message, 16, 1);
		end
	end catch
end
go
-- Newtonsoft.Json ---
declare @newtonsoft_json_location nvarchar(4000) = N'C:\storage\ssd01\source\sql\bi_dev.sql.mssql.extensions\bi_dev.sql.mssql.extensions.web2\bin\Debug\Newtonsoft.Json.dll';
if (not exists (select 1 from sys.assemblies where name = N'Newtonsoft.Json')) create ASSEMBLY [Newtonsoft.Json] from @newtonsoft_json_location with permission_set = unsafe;
else begin
	begin try
		alter ASSEMBLY [Newtonsoft.Json] from @newtonsoft_json_location with permission_set = unsafe;
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
-- INPUT --
-- determin project-location
declare @build_location nvarchar(max) = N'C:\storage\ssd01\source\sql\bi_dev.sql.mssql.extensions';
declare @build_file_name nvarchar(4000) = @build_location + N'\bi_dev.sql.mssql.extensions.web2\bin\Debug\bi_dev.sql.mssql.extensions.web2.dll';
-- drop existing --------
if (object_id('.dbo.f_clr_web_process_request') is not null) drop function dbo.f_clr_web_process_request;
if (exists (select 1 from sys.assemblies where name = N'mssql.extensions.web2')) drop ASSEMBLY [mssql.extensions.web2];
-- create new --------------------
create assembly [mssql.extensions.web2] from @build_file_name with permission_set = unsafe;
go

CREATE FUNCTION dbo.f_clr_web_process_request (@json_argument nvarchar(max), @ignore_response_errors bit) returns nvarchar(max) 
with execute as owner 
as external name [mssql.extensions.web2].[bi_dev.sql.mssql.extensions.web2.Utils].[WebProcessRequest];
