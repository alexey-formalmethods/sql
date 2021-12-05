-- microsoft.csharp--------
declare @microsoft_csharp_location nvarchar(4000) = 'C:\Windows\Microsoft.NET\Framework64\v4.0.30319\microsoft.csharp.dll';
if (not exists (select 1 from sys.assemblies where name = N'microsoft.csharp')) create ASSEMBLY [microsoft.csharp] from @microsoft_csharp_location with permission_set = unsafe;
else begin
	begin try
		alter ASSEMBLY [microsoft.csharp] from @microsoft_csharp_location with permission_set = unsafe;
	end try
	begin catch
		declare @error_message nvarchar(max) = error_message();
		if (@error_message not like '%identical to an assembly that is already registered under the name%') begin
			raiserror(@error_message, 16, 1);
		end
	end catch
end
go
-- System.ValueTuple--------
declare @system_value_tuple_location nvarchar(4000) = 'C:\Windows\Microsoft.NET\Framework64\v4.0.30319\System.ValueTuple.dll';
if (not exists (select 1 from sys.assemblies where name = N'System.ValueTuple')) create ASSEMBLY [System.ValueTuple] from @system_value_tuple_location with permission_set = unsafe;
else begin
	begin try
		alter ASSEMBLY [System.ValueTuple] from @system_value_tuple_location with permission_set = unsafe;
	end try
	begin catch
		declare @error_message nvarchar(max) = error_message();
		if (@error_message not like '%identical to an assembly that is already registered under the name%') begin
			raiserror(@error_message, 16, 1);
		end
	end catch
end
go
----------------------------
-- INPUT --
-- determin project-location
declare @build_location nvarchar(max) = N'C:\storage\ssd01\app\sql\bi_dev.sql.mssql.extensions';
-------------------------------
	declare @build_file_name nvarchar(4000) = @build_location + N'\bi_dev.sql.mssql.extensions.string.csv\bin\Debug\bi_dev.sql.mssql.extensions.@string.csv.dll';
-- drop existing --------
	if (object_id('.dbo.f_clr_csv_get_content') is not null) drop function dbo.f_clr_csv_get_content;
	if (object_id('.dbo.f_clr_parse_csv') is not null) drop function dbo.f_clr_parse_csv; 
	if (object_id('.dbo.f_clr_json_to_csv') is not null) drop function dbo.f_clr_json_to_csv; 
	if (exists (select 1 from sys.assemblies where name = N'mssql.extensions.string.csv')) drop assembly [mssql.extensions.string.csv];
-- create new --------------------
	create assembly [mssql.extensions.string.csv] from @build_file_name with permission_set = unsafe;
	go
	create function dbo.f_clr_csv_get_content(@value nvarchar(max), @delimiter nvarchar(max), @is_first_row_with_column_names bit, @null_when_error bit) returns nvarchar(max) with execute as owner as external name [mssql.extensions.string.csv].[bi_dev.sql.mssql.extensions.string.csv.Utils].GetCsvContent;
	go
	create function dbo.f_clr_parse_csv(@value nvarchar(max), @delimiter nvarchar(max), @null_when_error bit) 
	returns table([row_number] int, column_index int, row_value nvarchar(max))
	external name [mssql.extensions.string.csv].[bi_dev.sql.mssql.extensions.string.csv.Utils].ParseCsv;
	go
	create function dbo.f_clr_json_to_csv(@json_object nvarchar(max), @delimiter nvarchar(max), @datetime_format nvarchar(max), @null_when_error bit) returns nvarchar(max) with execute as owner as external name [mssql.extensions.string.csv].[bi_dev.sql.mssql.extensions.string.csv.Utils].JsonToCsv;
	go
