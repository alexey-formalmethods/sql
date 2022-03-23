-- input variables -----------------------------
	if (object_id('tempdb..#t_tmp_var') is not null) drop table #t_tmp_var;
	create table #t_tmp_var (name nvarchar(max), value nvarchar(max));
	insert into #t_tmp_var
	(
		name
	  , value
	)
	select
		 name, value
	from (values
		 ('@build_location', N'C:\storage\hdd01\proj\sql\bi_dev.sql.mssql.extensions')
		,('@microsoft_csharp_location', N'C:\Windows\Microsoft.NET\Framework64\v4.0.30319\microsoft.csharp.dll')
		,('@net_standart_location', N'C:\Windows\Microsoft.NET\Framework64\v4.0.30319\netstandard.dll')
		,('@system_runtime_location', N'C:\Windows\Microsoft.NET\Framework64\v4.0.30319\System.Runtime.dll')
		,('@system_runtime_compilerservices_unsafe_location', N'C:\storage\hdd01\proj\sql\bi_dev.sql.mssql.extensions\bi_dev.sql.mssql.extensions.string.csv\bin\Debug\system.runtime.compilerservices.unsafe.dll')
		,('@system_value_tuple_location', 'C:\Windows\Microsoft.NET\Framework64\v4.0.30319\System.ValueTuple.dll')
	) t (name, value);
	go
-- microsoft.csharp--------
declare @microsoft_csharp_location nvarchar(4000);
select @microsoft_csharp_location = value
from #t_tmp_var 
where name = '@microsoft_csharp_location'
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
-- netstandart--------
declare @net_standart_location nvarchar(4000);
select @net_standart_location = value
from #t_tmp_var
where name = '@net_standart_location';
if (not exists (select 1 from sys.assemblies where name = N'netstandard')) create ASSEMBLY [netstandard] from @net_standart_location with permission_set = unsafe;
else begin
	begin try
		alter ASSEMBLY [netstandard] from @net_standart_location with permission_set = unsafe;
	end try
	begin catch
		declare @error_message nvarchar(max) = error_message();
		if (@error_message not like '%identical to an assembly that is already registered under the name%') begin
			raiserror(@error_message, 16, 1);
		end
	end catch
end
go

-- system.runtime--------
declare @system_runtime_location nvarchar(4000);
select
	 @system_runtime_location = value
from #t_tmp_var
where name = '@system_runtime_location'
if (not exists (select 1 from sys.assemblies where name = N'system.runtime')) create ASSEMBLY [system.runtime] from @system_runtime_location with permission_set = unsafe;
else begin
	begin try
		alter ASSEMBLY [system.runtime] from @system_runtime_location with permission_set = unsafe;
	end try
	begin catch
		declare @error_message nvarchar(max) = error_message();
		if (@error_message not like '%identical to an assembly that is already registered under the name%') begin
			raiserror(@error_message, 16, 1);
		end
	end catch
end
go
-- system.runtime.compilerservices.unsafe--------
declare @system_runtime_compilerservices_unsafe_location nvarchar(4000);
select
	 @system_runtime_compilerservices_unsafe_location = value
from #t_tmp_var
where name = '@system_runtime_compilerservices_unsafe_location'
if (not exists (select 1 from sys.assemblies where name = N'system.runtime.compilerservices.unsafe')) create ASSEMBLY [system.runtime.compilerservices.unsafe] from @system_runtime_compilerservices_unsafe_location with permission_set = unsafe;
else begin
	begin try
		alter ASSEMBLY [System.ValueTuple] from @system_runtime_compilerservices_unsafe_location with permission_set = unsafe;
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
declare @system_value_tuple_location nvarchar(4000);
select
	 @system_value_tuple_location = value
from #t_tmp_var
where name = '@system_value_tuple_location';
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
declare @build_location nvarchar(max);
select
	 @build_location = value
from #t_tmp_var
where name = '@build_location'
-------------------------------
	declare @build_file_name nvarchar(4000) = @build_location + N'\bi_dev.sql.mssql.extensions.string.csv\bin\Debug\bi_dev.sql.mssql.extensions.@string.csv.dll';
-- drop existing --------
	if (object_id('.dbo.f_clr_csv_get_content') is not null) drop function dbo.f_clr_csv_get_content;
	if (object_id('.dbo.f_clr_json_to_csv') is not null) drop function dbo.f_clr_json_to_csv; 
	if (exists (select 1 from sys.assemblies where name = N'mssql.extensions.string.csv')) drop assembly [mssql.extensions.string.csv];
-- create new --------------------
	create assembly [mssql.extensions.string.csv] from @build_file_name with permission_set = unsafe;
	go
	create function dbo.f_clr_csv_get_content(@value nvarchar(max), @delimiter nvarchar(max), @is_first_row_with_column_names bit, @null_when_error bit) returns nvarchar(max) with execute as owner as external name [mssql.extensions.string.csv].[bi_dev.sql.mssql.extensions.string.csv.Utils].GetCsvContent;
	go
	create function dbo.f_clr_json_to_csv(@json_object nvarchar(max), @delimiter nvarchar(max), @datetime_format nvarchar(max), @null_when_error bit) returns nvarchar(max) with execute as owner as external name [mssql.extensions.string.csv].[bi_dev.sql.mssql.extensions.string.csv.Utils].JsonToCsv;
	go
